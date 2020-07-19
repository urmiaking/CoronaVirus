using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaVirus.Models;
using CoronaVirus.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoronaVirus.Controllers
{
    public class ContinentController : Controller
    {
        private readonly IContinentService _continentService;

        public ContinentController(IContinentService continentService)
        {
            _continentService = continentService;
        }

        public async Task<IActionResult> Index()
        {
            var continents = await _continentService.GetAllContinentsAsync();
            return View(continents.OrderByDescending(a => a.InfectedNo));
        }

        [HttpPost]
        public async Task<IActionResult> AddContinent(string continentName)
        {
            var continent = new Continent()
            {
                Name = continentName
            };
            var exContinent = await _continentService.GetContinentAsync(continentName);
            if (exContinent != null)
            {
                TempData["Error"] = "مشکلی در درج قاره پیش آمد! قاره از قبل وجود دارد";
                return RedirectToAction("Index");
            }

            var isSucceeded = await _continentService.AddContinentAsync(continent);

            if (!isSucceeded)
            {
                TempData["Error"] = "مشکلی در درج قاره پیش آمد! لطفا اطلاعات وارد شده را بررسی کنید";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "قاره موردنظر با موفقیت ثبت گردید";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditContinent(string continentName, int id = 0)
        {
            if (id == 0)
            {
                TempData["Error"] = "مشکلی در ویرایش اطلاعات قاره پیش آمد! لطفا اطلاعات وارد شده را بررسی کنید";
                return RedirectToAction("Index");
            }

            var continent = await _continentService.GetContinentAsync(id);
            if (continent == null)
            {
                TempData["Error"] = "مشکلی در ویرایش اطلاعات قاره پیش آمد! لطفا اطلاعات وارد شده را بررسی کنید";
                return RedirectToAction("Index");
            }

            continent.Name = continentName;

            var isSucceeded = await _continentService.UpdateContinentAsync(continent);
            if (!isSucceeded)
            {
                TempData["Error"] = "مشکلی در ویرایش اطلاعات قاره پیش آمد! لطفا اطلاعات وارد شده را بررسی کنید";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "اطلاعات قاره موردنظر با موفقیت بروزرسانی شد";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteContinent(int id = 0)
        {
            if (id == 0)
            {
                return StatusCode(404);
            }

            var continent = await _continentService.GetContinentAsync(id);

            if (continent == null)
            {
                return StatusCode(404);
            }

            var isSucceeded = await _continentService.DeleteContinentAsync(continent);

            if (!isSucceeded)
            {
                return StatusCode(404);
            }

            return StatusCode(200);
        }
    }
}