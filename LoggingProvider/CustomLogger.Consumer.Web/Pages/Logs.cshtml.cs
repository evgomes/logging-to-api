using CustomLogger.Consumer.ApiClient.HttpClients;
using CustomLogger.Consumer.ApiClient.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLogger.Consumer.Web.Pages
{
    public class LogsModel : PageModel
    {
        private readonly ILoggerApiClient _loggerApiClient;

        public List<LogResource> Logs { get; set; }

        public LogsModel(ILoggerApiClient loggerApiClient)
        {
            _loggerApiClient = loggerApiClient;
        }

        public async Task<IActionResult> OnGet()
        {
            Logs = await _loggerApiClient.ListAsync();
            return Page();
        }
    }
}
