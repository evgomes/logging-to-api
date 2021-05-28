using System.Collections.Generic;

namespace CustomLogger.Consumer.ApiClient.Options
{
	public class ApiLoggerOptions
	{
		public string ApiUrl { get; set; }
		public string ApplicationName { get; set; }
		public Dictionary<string, string> LogLevels { get; set; }
	}
}
