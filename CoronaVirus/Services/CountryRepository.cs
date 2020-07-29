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
    public class CountryRepository : ICountryService
    {
        public IConfiguration Configuration { get; set; }
        private readonly int countryPort;
        private readonly string countryServer;
        private readonly int continentPort;
        private readonly string continentServer;

        public CountryRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            countryPort = int.Parse(Configuration["CountryApi.Port"]);
            countryServer = Configuration["CountryApi.Server"];
            continentPort = int.Parse(Configuration["ContinentApi.Port"]);
            continentServer = Configuration["ContinentApi.Server"];
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            var countriesList = new List<Country>();
            var contientList = new List<Continent>();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{countryServer}:{countryPort}/api/Countries"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    countriesList = JsonConvert.DeserializeObject<List<Country>>(apiResponse);
                }

                using (var response = await client.GetAsync($"http://{continentServer}:{continentPort}/api/Continents"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    contientList = JsonConvert.DeserializeObject<List<Continent>>(apiResponse);
                }

                foreach (var country in countriesList)
                {
                    country.Continent = contientList.FirstOrDefault(a => a.Id.Equals(country.ContinentId));
                }
            }

            return countriesList;
        }

        public async Task<Country> GetCountryAsync(int id)
        {
            var country = new Country();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{countryServer}:{countryPort}/api/Countries/{id}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    country = JsonConvert.DeserializeObject<Country>(apiResponse);
                }

                using (var response = await client.GetAsync($"http://{continentServer}:{continentPort}/api/Continents/{country.ContinentId}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    country.Continent = JsonConvert.DeserializeObject<Continent>(apiResponse);
                }
            }

            return country;
        }

        public async Task<Country> GetCountryAsync(string name)
        {
            var country = new Country();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{countryServer}:{countryPort}/api/Countries/GetCountryByName/{name}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    country = JsonConvert.DeserializeObject<Country>(apiResponse);
                }

                using (var response = await client.GetAsync($"http://{continentServer}:{continentPort}/api/Continents/{country.ContinentId}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    country.Continent = JsonConvert.DeserializeObject<Continent>(apiResponse);
                }
            }

            return country;
        }

        public async Task<bool> AddCountryAsync(Country country)
        {
            var isSucceeded = false;
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(country), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync($"http://{countryServer}:{countryPort}/api/Countries", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        isSucceeded = true;
                    }
                }
            }

            return isSucceeded;
        }

        public async Task<bool> UpdateCountryAsync(Country country)
        {
            var isSucceeded = false;
            using (var httpClient = new HttpClient())
            {
                StringContent newContent = new StringContent(JsonConvert.SerializeObject(country), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"http://{countryServer}:{countryPort}/api/Countries/{country.Id}", newContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        isSucceeded = true;
                    }
                }
            }

            return isSucceeded;
        }

        public async Task<bool> DeleteCountryAsync(Country country)
        {
            var isSucceeded = false;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"http://{countryServer}:{countryPort}/api/Countries/{country.Id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        isSucceeded = true;
                    }
                }
            }

            return isSucceeded;
        }
    }
}
