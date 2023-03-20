using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> logger;

        public BaseController(ILogger<BaseController> logger)
        {
            this.logger = logger;
        }

        protected IActionResult HandleException(Exception ex)
        {
            logger.LogError(ex, "An error occurred");
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred. Please try again later." });
        }
    }

}
