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
        private readonly int port;
        private readonly string server;

        public CountryRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            port = int.Parse(Configuration["Api.Port"]);
            server = Configuration["Api.Server"];
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            var countriesList = new List<Country>();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://{server}:{port}/api/Countries"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    countriesList = JsonConvert.DeserializeObject<List<Country>>(apiResponse);
                }
            }

            return countriesList;
        }

        public async Task<Country> GetCountryAsync(int id)
        {
            var country = new Country();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://{server}:{port}/api/Countries/{id}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    country = JsonConvert.DeserializeObject<Country>(apiResponse);
                }
            }

            return country;
        }

        public async Task<Country> GetCountryAsync(string name)
        {
            var country = new Country();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://{server}:{port}/api/Countries/GetCountryByName/{name}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    country = JsonConvert.DeserializeObject<Country>(apiResponse);
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
                using (var response = await client.PostAsync($"https://{server}:{port}/api/Countries", content))
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

                using (var response = await httpClient.PutAsync($"https://{server}:{port}/api/Countries/{country.Id}", newContent))
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
                using (var response = await httpClient.DeleteAsync($"https://{server}:{port}/api/Countries/{country.Id}"))
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
