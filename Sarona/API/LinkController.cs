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
    public class LinkController : Controller
    {
        SaronaRepository repository;
        public LinkController(SaronaRepository repo) => repository = repo;
        [HttpGet("{id}")]
        public Link GetLink(int id)
        {
            return repository.Links.Where(x => x.Id == id).First();
        }
    }
}
