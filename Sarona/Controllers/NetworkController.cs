using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sarona.Models;
using Sarona.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.Controllers
{
    public class NetworkController : Controller
    {
        SaronaRepository repository;

        public NetworkController(SaronaRepository repo)
        {
            repository = repo;
        }
        // GET: /<controller>/
        public IActionResult District(Area district = Area.A2)
        {
            
            var model = new DistrictViewModel
            {
                Exchanges = repository.Abbreviations.Where(x => x.Type == AbbType.Exchange && x.Area == district).OrderBy(x=>x.Abb),
                NewAbbreviation = new Abbreviation(),
                SelectedDistrict = district
            };


            return View("District",model);
        }
        [HttpPost]
        public IActionResult AddExchange(Abbreviation newAbbreviation)
        {

            if (ModelState.IsValid)
                repository.AddAbbreviation(newAbbreviation);
            return RedirectToAction(nameof(District), new { district = newAbbreviation.Area ?? Area.A2 });
        }
        [HttpPost]
        public IActionResult RemoveExchange(Area district, int id)
        {
            if (ModelState.IsValid)
                repository.RemoveAbbreviation(id);
            return RedirectToAction(nameof(District),new { district});
        }
    }
}
