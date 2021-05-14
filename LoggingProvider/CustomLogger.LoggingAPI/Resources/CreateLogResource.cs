using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomLogger.LoggingAPI.Resources
{
    public class CreateLogResource
    {
        [Range(1, 6)]
        public LogLevel Level { get; set; }
        
        [Required]
        public string Application { get; set; }
        
        public string Class { get; set; }
        
        public string Method { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        public string Stacktrace { get; set; }
        
        public string User { get; set; }
    }
}
