using Lab4Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab4Auth.ViewModels
{
    public class CustomersWithCityViewModel
    {
        public List<City> Cities { get; set; }
        public Customer Customer { get; set; }
    }
}