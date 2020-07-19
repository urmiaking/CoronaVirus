using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaVirus.Models;

namespace CoronaVirus.ViewModels
{
    public class ContinentCountryViewModel
    {
        public ContinentCountryViewModel()
        {
            Continents = new List<Continent>();
            Countries = new List<Country>();
        }

        public ContinentCountryViewModel(List<Continent> continents = null, List<Country> countries = null)
        {
            Continents = continents ?? new List<Continent>();
            Countries = countries ?? new List<Country>();
        }

        public List<Continent> Continents { get; set; }
        public List<Country> Countries { get; set; }
    }
}
