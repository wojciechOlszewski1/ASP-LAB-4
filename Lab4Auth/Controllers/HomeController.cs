using Lab4Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Lab4Auth.ViewModels;

namespace Lab4Auth.Controllers
{
    [Authorize] //dodanie atrybutu przed kontrolerem blokuje wszystkie metody
    public class HomeController : Controller
    {
        private ApplicationDbContext _db; 

        public HomeController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing) 
        {
            _db.Dispose();
        }

        [AllowAnonymous] // dodanie tego atrybutu w przypadku kiedy nad kontrolerem jest atrybut [Authorize], pozwala niezalogowanym użytkownikom na dostęp 
        public ActionResult Index()
        {
            return View(_db.Customers.Include(x => x.City).ToList());
        }

        
        public ActionResult CreateCustomer() 
        {
            var cities = _db.Cities.ToList();
            CustomersWithCityViewModel customersWithCityView = new CustomersWithCityViewModel()
            {
                Cities = cities,
                Customer = new Customer()
            };
            return View("CustomerForm", customersWithCityView);
        }

        public ActionResult EditCustomer(int id) 
        {
            var customer = _db.Customers.SingleOrDefault(x => x.Id == id);
            if (customer == null)
                return HttpNotFound();

            CustomersWithCityViewModel customersWithCityViewModel = new CustomersWithCityViewModel()
            {
                Cities = _db.Cities.ToList(),
                Customer = customer
            };

            return View("CustomerForm", customersWithCityViewModel);
        }

        [HttpPost]
        public ActionResult AddCustomer(Customer customer) 
        {
            if (ModelState.IsValid)
            {
                if (customer.Id == 0)
                {
                    _db.Customers.Add(customer);
                    _db.SaveChanges();
                }
                else
                {
                    var customerToUpdate = _db.Customers.Single(x => x.Id == customer.Id);
                    customerToUpdate.Name = customer.Name;
                    customerToUpdate.Surname = customer.Surname;
                    customerToUpdate.Phone = customer.Phone;
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                var cities = _db.Cities.ToList();
                CustomersWithCityViewModel customersWithCityView = new CustomersWithCityViewModel()
                {
                    Customer = customer,
                    Cities = cities
                };
                return View("CustomerForm", customersWithCityView);
            }
        }
    }
}