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
    public class NEController : Controller
    {
        SaronaRepository repository;
        public NEController(SaronaRepository repo) => repository = repo;
        // GET: api/<controller>
        [HttpGet("abb/{abb}/{type?}")]
        public IActionResult Get(string abb, NeType? type)
        {
            if(type is null)
            {
                var q = repository.NetworkElements.Where(x => x.Exchange.Abb == abb).OrderBy(x => x.Name).Select(x=> new { x.Name, x.Model,x.Manufacturer,x.Id}).ToList();
                return Json(q);
            }
                
            return Json(repository.NetworkElements.Where(x => x.Exchange.Abb == abb && x.NetworkType == type).OrderBy(x=>x.Name).Select(x => new { x.Name, x.Model, x.Manufacturer, x.Id }));
        }
        

        
    }
}
