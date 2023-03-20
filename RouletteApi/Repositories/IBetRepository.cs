using RouletteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Repositories
{
    public interface IBetRepository
    {
        Task<int> Add(Bet bet);
        Task<Bet> Get(int id);
        Task<IEnumerable<Bet>> GetAll();
    }
}
