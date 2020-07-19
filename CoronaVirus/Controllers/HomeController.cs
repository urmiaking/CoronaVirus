using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoronaVirus.Models;
using CoronaVirus.Services;
using CoronaVirus.ViewModels;

namespace CoronaVirus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IContinentService _continentService;
        private readonly ICountryService _countryService;

        public HomeController(ILogger<HomeController> logger, IContinentService continentService, ICountryService countryService)
        {
            _logger = logger;
            _continentService = continentService;
            _countryService = countryService;
        }

        public async Task<IActionResult> Index()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            var continents = await _continentService.GetAllContinentsAsync();
            var vm = new ContinentCountryViewModel(
                continents.Take(5).OrderByDescending(a => a.InfectedNo).ToList(),
                countries.Take(5).OrderByDescending(a => a.InfectedNo).ToList()
                );
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
