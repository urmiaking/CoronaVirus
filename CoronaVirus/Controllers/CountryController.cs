using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaVirus.Models;
using CoronaVirus.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoronaVirus.Controllers
{
    public class CountryController : Controller
    {

        private readonly ICountryService _countryService;
        private readonly IContinentService _continentService;


        public CountryController(ICountryService countryService, IContinentService continentService)
        {
            _countryService = countryService;
            _continentService = continentService;
        }

        public async Task<IActionResult> Index()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return View(countries);
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(string countryName, string continentName, int infectedNo, int recoveredNo, int deathNo)
        {
            var continent = await _continentService.GetContinentAsync(continentName);

            var country = new Country()
            {
                ContinentId = continent.Id,
                DeathNo = deathNo,
                InfectedNo = infectedNo,
                RecoveredNo = recoveredNo,
                RefreshDate = DateTime.Now,
                Name = countryName
            };

            var exCountry = await _countryService.GetCountryAsync(countryName);
            if (exCountry != null)
            {
                TempData["Error"] = "مشکلی در درج کشور پیش آمد! کشور از قبل وجود دارد";
                return RedirectToAction("Index");
            }

            var isSucceeded = await _countryService.AddCountryAsync(country);
            if (!isSucceeded)
            {
                TempData["Error"] = "مشکلی در درج اطلاعات کشور پیش آمد! لطفا اطلاعات وارد شده را بررسی کنید";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "اطلاعات کشور موردنظر با موفقیت ثبت گردید";
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> EditCountry(string countryName, string continentName, int infectedNo, int recoveredNo, int deathNo, int id = 0)
        {
            if (id == 0)
            {
                TempData["Error"] = "مشکلی در ویرایش اطلاعات کشور پیش آمد! لطفا اطلاعات وارد شده را بررسی کنید";
                return RedirectToAction("Index");
            }

            var country = await _countryService.GetCountryAsync(id);
            if (country == null)
            {
                TempData["Error"] = "مشکلی در ویرایش اطلاعات کشور پیش آمد! لطفا اطلاعات وارد شده را بررسی کنید";
                return RedirectToAction("Index");
            }

            country.InfectedNo = infectedNo;
            country.DeathNo = deathNo;
            country.RecoveredNo = recoveredNo;
            country.RefreshDate = DateTime.Now;
            country.Name = countryName;

            var continent = await _continentService.GetContinentAsync(continentName);

            country.ContinentId = continent.Id;

            var isSucceeded = await _countryService.UpdateCountryAsync(country);

            if (!isSucceeded)
            {
                TempData["Error"] = "مشکلی در ویرایش اطلاعات کشور پیش آمد! لطفا اطلاعات وارد شده را بررسی کنید";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "اطلاعات کشور موردنظر با موفقیت بروزرسانی شد";
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> DeleteCountry(int id = 0)
        {
            if (id == 0)
            {
                return StatusCode(404);
            }

            var country = await _countryService.GetCountryAsync(id);

            if (country == null)
            {
                return StatusCode(404);
            }

            var isSucceeded = await _countryService.DeleteCountryAsync(country);

            if (!isSucceeded)
            {
                return StatusCode(404);
            }

            return StatusCode(200);
        }
    }
}