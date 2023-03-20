using RouletteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Services
{
 
public interface IRouletteService
    {
        Task<int> PlaceBet(Bet bet);
        Task<Spin> Spin();
        Task<int> Payout(string winningNumber);
        Task<IEnumerable<Spin>> ShowPreviousSpins(int count);
    }

}
