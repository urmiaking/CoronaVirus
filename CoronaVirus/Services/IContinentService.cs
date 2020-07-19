using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaVirus.Models;

namespace CoronaVirus.Services
{
    public interface IContinentService
    {
        Task<List<Continent>> GetAllContinentsAsync();

        Task<Continent> GetContinentAsync(int id);

        Task<Continent> GetContinentAsync(string name);

        Task<bool> AddContinentAsync(Continent continent);

        Task<bool> UpdateContinentAsync(Continent continent);

        Task<bool> DeleteContinentAsync(Continent continent);

        Task<int> GetAllInfectedNoAsync();

        Task<int> GetAllRecoveredNoAsync();

        Task<int> GetAllDeathsNoAsync();
    }
}
