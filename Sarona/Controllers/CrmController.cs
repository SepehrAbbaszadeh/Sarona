using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;
using Sarona.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.Controllers
{
    public class CrmController : Controller
    {
        SaronaRepository repository;
        public CrmController(SaronaRepository repo) => repository = repo;
        
        public IActionResult Customers()
        {
            var model = new CustomersViewModel()
            {
                NewCustomer = new Customer(),
                Customers = repository.Customers.ToList()
            };
            return View("Customers", model);
        }
        public IActionResult AddCustomer([Bind(Prefix=nameof(CustomersViewModel.NewCustomer))] Customer newCustomer)
        {
            if(ModelState.IsValid)
            {
                repository.AddCustomer(newCustomer);
                TempData["message"] = $"{newCustomer.Name} ({newCustomer.Abb}) added succesfully.";
                return RedirectToAction(nameof(Customers));
            }
            return Customers();
        }
    }
}
