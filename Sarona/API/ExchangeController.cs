using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sarona.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : Controller
    {
        SaronaRepository repository;
        public ExchangeController(SaronaRepository repo) => repository = repo;
        // GET: api/<controller>/A2
        [HttpGet("{district}")]
        public IEnumerable<Exchange> GetExchanges(Area district)
        {
            var res =  repository.Exchanges.Where(x => x.Area == district).OrderBy(x=>x.Abb).ToArray();
            return res;
        }
    }
}
