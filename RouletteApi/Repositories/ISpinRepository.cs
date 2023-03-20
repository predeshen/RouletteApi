using RouletteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApi.Repositories
{
    public interface ISpinRepository
    {
        Task<int> Add(Spin spin);
        Task<Spin> Get(int id);
        Task<IEnumerable<Spin>> GetAll();
    }
}
