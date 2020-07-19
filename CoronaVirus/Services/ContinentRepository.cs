using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoronaVirus.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CoronaVirus.Services
{
    public class ContinentRepository : IContinentService
    {
        public IConfiguration Configuration { get; set; }
        private readonly int port;
        private readonly string server;

        public ContinentRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            port = int.Parse(Configuration["Api.Port"]);
            server = Configuration["Api.Server"];
        }

        public async Task<List<Continent>> GetAllContinentsAsync()
        {
            var continentsList = new List<Continent>();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://{server}:{port}/api/Continents"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    continentsList = JsonConvert.DeserializeObject<List<Continent>>(apiResponse);
                }
            }

            return continentsList;
        }

        public async Task<Continent> GetContinentAsync(int id)
        {
            var continent = new Continent();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://{server}:{port}/api/Continents/{id}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    continent = JsonConvert.DeserializeObject<Continent>(apiResponse);
                }
            }

            return continent;
        }

        public async Task<Continent> GetContinentAsync(string name)
        {
            var continent = new Continent();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://{server}:{port}/api/Continents/GetContinentByName/{name}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    continent = JsonConvert.DeserializeObject<Continent>(apiResponse);
                }
            }

            return continent;
        }

        public async Task<bool> AddContinentAsync(Continent continent)
        {
            var isSucceeded = false;
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(continent), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync($"https://{server}:{port}/api/Continents", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        isSucceeded = true;
                    }
                }
            }

            return isSucceeded;
        }

        public async Task<bool> UpdateContinentAsync(Continent continent)
        {
            var isSucceeded = false;
            using (var httpClient = new HttpClient())
            {
                StringContent newContent = new StringContent(JsonConvert.SerializeObject(continent), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"https://{server}:{port}/api/Continents/{continent.Id}", newContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        isSucceeded = true;
                    }
                }
            }

            return isSucceeded;
        }

        public async Task<bool> DeleteContinentAsync(Continent continent)
        {
            var isSucceeded = false;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"https://{server}:{port}/api/Continents/{continent.Id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        isSucceeded = true;
                    }
                }
            }

            return isSucceeded;
        }

        public async Task<int> GetAllInfectedNoAsync()
        {
            var infectedCount = 0;

            foreach (var continent in await GetAllContinentsAsync())
            {
                foreach (var countryInfectedNo in continent.Countries.Select(a => a.InfectedNo))
                {
                    infectedCount += countryInfectedNo;
                }
            }

            return infectedCount;
        }

        public async Task<int> GetAllRecoveredNoAsync()
        {
            var recoveredCount = 0;

            foreach (var continent in await GetAllContinentsAsync())
            {
                foreach (var countryRecoveredCount in continent.Countries.Select(a => a.RecoveredNo))
                {
                    recoveredCount += countryRecoveredCount;
                }
            }

            return recoveredCount;
        }

        public async Task<int> GetAllDeathsNoAsync()
        {
            var deathsCount = 0;

            foreach (var continent in await GetAllContinentsAsync())
            {
                foreach (var countryDeathsCount in continent.Countries.Select(a => a.DeathNo))
                {
                    deathsCount += countryDeathsCount;
                }
            }

            return deathsCount;
        }
    }
}
