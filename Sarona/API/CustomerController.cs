using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        SaronaRepository repository;
        public CustomerController(SaronaRepository repo) => repository = repo;
        // GET: api/<controller>
        [HttpGet("Name/{name}")]
        public IEnumerable<Customer> GetName(string name)
        {
            return repository.Customers.Where(x => EF.Functions.Like(x.Name,$"%{name}%")).OrderBy(x=>x.Name).ToList();
        }

        // GET api/<controller>/5
        [HttpGet("Abb/{abb}")]
        public Customer GetAbb(string abb)
        {
            return repository.Customers.Where(x => x.Abb == abb).FirstOrDefault();
        }
    }
}
