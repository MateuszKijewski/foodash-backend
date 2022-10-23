using Dawn;
using FooDash.Application.Common.Interfaces.System;
using FooDash.Application.System.Dtos.Contracts;
using FooDash.Application.System.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FooDash.WebApi.Controllers.System
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SettingsController : ControllerBase
    {
        private readonly ISystemService _systemService;

        public SettingsController(ISystemService systemService)
        {
            _systemService = Guard.Argument(systemService).NotNull().Value;
        }

        [HttpGet]
        public async Task<ActionResult<GetSettingsResponse>> GetSystemSettings()
        {
            var systemSettings = await _systemService.GetSystemSettings();

            return Ok(systemSettings);
        }

        [HttpPut]
        public async Task<ActionResult> SetSystemSettings([FromBody] ChangeSettingsContract changeSettingsContract)
        {
            await _systemService.ChangeSystemSettings(changeSettingsContract);

            return Ok();
        }
    }
}