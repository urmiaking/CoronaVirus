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
        private readonly int continentPort;
        private readonly string continentServer;
        private readonly int countryPort;
        private readonly string countryServer;

        public ContinentRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            continentPort = int.Parse(Configuration["ContinentApi.Port"]);
            continentServer = Configuration["ContinentApi.Server"];
            countryPort = int.Parse(Configuration["CountryApi.Port"]);
            countryServer = Configuration["CountryApi.Server"];
        }

        public async Task<List<Continent>> GetAllContinentsAsync()
        {
            var continentsList = new List<Continent>();
            var countriesList = new List<Country>();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{continentServer}:{continentPort}/api/Continents"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    continentsList = JsonConvert.DeserializeObject<List<Continent>>(apiResponse);
                }

                using (var response = await client.GetAsync($"http://{countryServer}:{countryPort}/api/Countries"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    countriesList = JsonConvert.DeserializeObject<List<Country>>(apiResponse);
                }

                foreach (var continent in continentsList)
                {
                    continent.Countries = countriesList.Where(a => a.ContinentId.Equals(continent.Id)).ToList();
                }
            }

            return continentsList;
        }

        public async Task<Continent> GetContinentAsync(int id)
        {
            var continent = new Continent();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{continentServer}:{continentPort}/api/Continents/{id}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    continent = JsonConvert.DeserializeObject<Continent>(apiResponse);
                }

                using (var response = await client.GetAsync($"http://{countryServer}:{countryPort}/api/Countries"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var countries = JsonConvert.DeserializeObject<List<Country>>(apiResponse);
                    continent.Countries = countries;
                }
            }

            return continent;
        }

        public async Task<Continent> GetContinentAsync(string name)
        {
            var continent = new Continent();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{continentServer}:{continentPort}/api/Continents/GetContinentByName/{name}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    continent = JsonConvert.DeserializeObject<Continent>(apiResponse);
                }

                using (var response = await client.GetAsync($"http://{countryServer}:{countryPort}/api/Countries"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var countries = JsonConvert.DeserializeObject<List<Country>>(apiResponse);
                    continent.Countries = countries;
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
                using (var response = await client.PostAsync($"http://{continentServer}:{continentPort}/api/Continents", content))
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

                using (var response = await httpClient.PutAsync($"http://{continentServer}:{continentPort}/api/Continents/{continent.Id}", newContent))
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
                using (var response = await httpClient.DeleteAsync($"http://{continentServer}:{continentPort}/api/Continents/{continent.Id}"))
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
