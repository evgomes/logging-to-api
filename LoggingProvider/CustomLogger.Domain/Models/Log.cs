﻿using Microsoft.Extensions.Logging;
using System;

namespace CustomLogger.Domain.Models
{
    public class Log
    {
        public LogLevel Level { get; set; }
        public string Application { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public string User { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
