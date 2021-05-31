using AutoMapper;
using CustomLogger.Domain.Models;
using CustomLogger.Domain.Repositories;
using CustomLogger.LoggingAPI.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLogger.LoggingAPI.Controllers
{
    [Route("/api/logs")]
    [ApiController]
    public class LogController : Controller
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;

        public LogController(ILogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateLogResource resource)
        {
            var log = _mapper.Map<Log>(resource);
            await _logRepository.AddAsync(log);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Log>))]
        public async Task<IActionResult> ListAsync()
        {
            return Ok(await _logRepository.ListAsync());
        }
    }
}