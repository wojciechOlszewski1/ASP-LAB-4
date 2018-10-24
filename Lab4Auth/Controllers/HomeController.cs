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
    public class HomeController : Controller
    {
        private ApplicationDbContext _db; //

        public HomeController()
        {
            _db = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing) // niszczenie obiektu 
        {
            _db.Dispose();
        }

        public ActionResult Index()
        {
            return View(_db.Customers.Include(x => x.City).ToList());
        }

        [Authorize] // zabezpiecza dostęp w przypadku niezalogowanych użytkowników
        public ActionResult CreateCustomer() // formularz dla nowego użytkownika
        {
            var cities = _db.Cities.ToList();
            CustomersWithCityViewModel customersWithCityView = new CustomersWithCityViewModel()
            {
                Cities = cities,
                Customer = new Customer()
            };
            return View("CustomerForm", customersWithCityView);
        }

        public ActionResult EditCustomer(int id) // edycja istniejącego użytkownika
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
        public ActionResult AddCustomer(Customer customer) // dodawanie do bazy danych
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