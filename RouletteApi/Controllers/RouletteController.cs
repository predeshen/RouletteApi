using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RouletteApi.Models;
using RouletteApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly IRouletteService rouletteService;

        public RouletteController(IRouletteService rouletteService)
        {
            this.rouletteService = rouletteService;
        }

        [HttpPost("placebet")]
        public async Task<ActionResult<int>> PlaceBet([FromBody] Bet bet)
        {
            int betId = await rouletteService.PlaceBet(bet);
            return Ok(betId);
        }

        [HttpPost("spin")]
        public async Task<ActionResult<Spin>> Spin()
        {
            Spin spin = await rouletteService.Spin();
            return Ok(spin);
        }

        [HttpPost("payout/{winningNumber}")]
        public async Task<ActionResult<int>> Payout(string winningNumber)
        {
            int rowsAffected = await rouletteService.Payout(winningNumber);
            return Ok(rowsAffected);
        }

        [HttpGet("spins")]
        public async Task<ActionResult<IEnumerable<Spin>>> ShowPreviousSpins([FromQuery] int count)
        {
            IEnumerable<Spin> spins = await rouletteService.ShowPreviousSpins(count);
            return Ok(spins);
        }
    }

}
