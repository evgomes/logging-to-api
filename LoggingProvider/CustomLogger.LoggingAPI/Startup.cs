using CustomLogger.Data.MongoDB.Contexts;
using CustomLogger.Data.MongoDB.Contexts.Options;
using CustomLogger.Data.MongoDB.Repositories;
using CustomLogger.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CustomLogger.LoggingAPI
{
    public class Startup
    {
        private const string CORS_ORIGINS = "CORS_ORIGINS";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_ORIGINS,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin();
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyHeader();
                                  });
            });

            services.Configure<ConnectionOptions>(Configuration.GetSection(nameof(ConnectionOptions)));
            services.AddSingleton<IMongoDbContext, LogDbContext>();
            services.AddSingleton<ILogRepository, LogRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Logging API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomLogger.LoggingAPI v1"));

            app.UseRouting();

            app.UseCors(CORS_ORIGINS);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
