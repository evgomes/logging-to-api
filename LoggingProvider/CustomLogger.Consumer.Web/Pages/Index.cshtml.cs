using CustomLogger.Consumer.ApiClient.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomLogger.Consumer.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public LogLevel LogLevel { get; set; }
        
        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public bool GenerateExceptionData { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnPost()
        {
            Exception exception = null;
            if(GenerateExceptionData)
            {
                exception = new Exception("Sample exception payload.");
            }

            _logger.LogToApi(LogLevel, Message, exception, user: UserName);

            ViewData["ShowToast"] = true;
        }
    }
}
