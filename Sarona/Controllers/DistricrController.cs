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
    public class ExchangeController : Controller
    {
        SaronaRepository repository;
        public ExchangeController(SaronaRepository repo) => repository = repo;
        
        public IActionResult Index()
        {
            return Json("Exchange");
        }
    }
}
