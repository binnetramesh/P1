using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P1.Models
{
    public class Inventory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }
        public string Country { get; set; }
        public string StartDate { get; set; }
        public List<Country> CountryList { get; set; }
    }
    public class Country
    {
        public string Id { get; set; }
        public string CountryName { get; set; }
    }
}
