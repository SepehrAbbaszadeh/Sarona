﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.Controllers
{
    public class HomeController : Controller
    {
        SaronaRepository repository;
        public HomeController(SaronaRepository repo) => repository = repo;
        public IActionResult Index()
        {
            return Redirect("https://localhost:44350/Network/A2/MF/SS2B");
        }
    }
}
