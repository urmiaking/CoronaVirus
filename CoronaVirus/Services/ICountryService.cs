using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaVirus.Models;

namespace CoronaVirus.Services
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllCountriesAsync();

        Task<Country> GetCountryAsync(int id);

        Task<Country> GetCountryAsync(string name);

        Task<bool> AddCountryAsync(Country country);

        Task<bool> UpdateCountryAsync(Country country);

        Task<bool> DeleteCountryAsync(Country country);
    }
}
