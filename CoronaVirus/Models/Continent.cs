using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaVirus.Models
{
    public class Continent
    {
        public int Id { get; set; }

        [Display(Name = "نام قاره")]
        public string Name { get; set; }

        [Display(Name = "تعداد مبتلایان")]
        public int InfectedNo
        {
            get
            {
                var totalNo = 0;
                if (Countries == null)
                {
                    return totalNo;
                }
                foreach (var country in Countries)
                {
                    totalNo += country.InfectedNo;
                }

                return totalNo;
            }
        }

        [Display(Name = "تعداد بهبود یافتگان")]
        public int RecoveredNo
        {
            get
            {
                var totalNo = 0;
                if (Countries == null)
                {
                    return totalNo;
                }
                foreach (var country in Countries)
                {
                    totalNo += country.RecoveredNo;
                }

                return totalNo;
            }
        }

        [Display(Name = "تعداد فوت شده ها")]
        public int DeathNo
        {
            get
            {
                var totalNo = 0;
                if (Countries == null)
                {
                    return totalNo;
                }
                foreach (var country in Countries)
                {
                    totalNo += country.DeathNo;
                }

                return totalNo;
            }
        }

        [Display(Name = "تاریخ به روز رسانی")]
        public DateTime RefreshDate
        {
            get
            {
                DateTime latestRefreshDate;
                if (Countries == null)
                {
                    latestRefreshDate = DateTime.MinValue;
                    return latestRefreshDate;
                }
                var lastUpdatedCountry = Countries.OrderByDescending(a => a.RefreshDate).FirstOrDefault();
                if (lastUpdatedCountry == null)
                {
                    latestRefreshDate = DateTime.MinValue;
                }
                else
                {
                    latestRefreshDate = lastUpdatedCountry.RefreshDate;
                }

                return latestRefreshDate;
            }
        }


        public List<Country> Countries { get; set; }
    }
}
