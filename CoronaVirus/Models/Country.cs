using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaVirus.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name = "نام کشور")]
        public string Name { get; set; }

        [Display(Name = "تعداد مبتلایان")]
        public int InfectedNo { get; set; }

        [Display(Name = "تعداد بهبود یافتگان")]
        public int RecoveredNo { get; set; }

        [Display(Name = "تعداد فوت شده ها")]
        public int DeathNo { get; set; }

        [Display(Name = "تاریخ به روز رسانی")]
        public DateTime RefreshDate { get; set; }

        public int ContinentId { get; set; }

        public Continent Continent { get; set; }
    }
}
