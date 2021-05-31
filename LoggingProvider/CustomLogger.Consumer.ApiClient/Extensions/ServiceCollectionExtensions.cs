using CustomLogger.Consumer.ApiClient.HttpClients;
using CustomLogger.Consumer.ApiClient.Options;
using CustomLogger.Consumer.ApiClient.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CustomLogger.Consumer.ApiClient.Extensions
{
    public static class ServiceCollectionExtensions
    {
		/// <summary>
		/// Adds the API logger to the application.This method reads an "ApiLogger" section from appsettings.json or from 
		/// environment variables, depending on which one you are using.
		/// </summary>
		/// <param name="services">Service collection.</param>
		public static void AddApiLogger(this IServiceCollection services, IConfiguration configuration)
		{
			var loggingSection = configuration.GetSection("Logging");
			var apiLoggerSection = loggingSection.GetSection("ApiLogger");

			var logLevelsDictionary = loggingSection.GetSection("LogLevel").Get<Dictionary<string, string>>();

			services.Configure<ApiLoggerOptions>(opt =>
			{
				opt.ApiUrl = apiLoggerSection["ApiUrl"];
				opt.ApplicationName = apiLoggerSection["ApplicationName"];
				opt.LogLevels = logLevelsDictionary;
			});

			AddApiClientAndProvider(services);
		}

		/// <summary>
		/// Adds the API logger to the application. This method allows passing the API logger options manually.
		/// <param name="services">Service collection.</param>
		/// <param name="configuration">Configuration properties.</param>
		/// <param name="options">API logger options.</param>
		public static void AddApiLogger(this IServiceCollection services, IConfiguration configuration, ApiLoggerOptions options)
		{
			services.Configure<ApiLoggerOptions>(opt =>
			{
				opt.ApiUrl = options.ApiUrl;
				opt.ApplicationName = options.ApplicationName;
				opt.LogLevels = options.LogLevels;
			});

			AddApiClientAndProvider(services);
		}

		/// <summary>
		/// Configures the API logger provider using the current logger factory.
		/// </summary>
		/// <param name="loggerFactory">Logger factory.</param>
		/// <param name="serviceProvider">Application service provider.</param>
		public static void UseApiLoggerProvider(this ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
		{
			var provider = serviceProvider.GetService<ApiLoggerProvider>();
			loggerFactory.AddProvider(provider);
		}

		/// <summary>
		/// Adds the HttpClient and ApiLoggerProviders to the service collection.
		/// </summary>
		/// <param name="services"></param>
		private static void AddApiClientAndProvider(IServiceCollection services)
		{
			services.AddHttpClient<ILoggerApiClient, LoggerApiClient>(LoggerApiClient.API_CLIENT_NAME, client =>
			{
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.Timeout = TimeSpan.FromSeconds(30);
			}).AddPolicyHandler(GetRetryPolicy()); ;

			services.AddTransient<ApiLoggerProvider>();
		}

		/// <summary>
		/// Creates a retry policy for the logger API client, to handle scenarios where the API is not immediately available.
		/// </summary>
		/// <returns>Retry policy.</returns>
		private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
		}
	}
}
