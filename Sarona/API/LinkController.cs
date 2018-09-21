using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [HttpGet("History/{id}")]
        public string History(int id)
        {
            var history = repository.LinkHistories.Where(x => x.LinkId == id).ToList();
            string result = "";
            foreach (var lh in history)
            { 
                result += ($"<tr><td>{lh.Channels}</td><td>{lh.Type}</td><td>{lh.Direction}</td><td>{lh.ModifiedOn.GetPersianDate()}</td><td>{lh.Username}</td><td>{lh.Remark}</td><tr>");
            }
            return result;

        }
    }
}
