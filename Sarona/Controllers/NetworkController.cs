using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sarona.Models;
using Sarona.ViewModels;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet]
        public IActionResult District(Area district = Area.A2)
        {
            var model = new DistrictViewModel
            {
                Exchanges = repository.Abbreviations.Where(x => x.Type == AbbType.Exchange && x.Area == district).OrderBy(x=>x.Abb),
                SelectedExchange = new Abbreviation(),
                SelectedDistrict = district
            };
            return View("District",model);
        }

        [HttpPost]
        public IActionResult AddExchange([Bind(Prefix =nameof(DistrictViewModel.SelectedExchange))]Abbreviation newAbb)
        {
            if (ModelState.IsValid)
            {
                repository.AddAbbreviation(newAbb);
                TempData["message"] = $"{newAbb.Name} added successfully.";
                return RedirectToAction(nameof(District), new { district = newAbb.Area ?? Area.A2 });
            }
            else
            {
                return District(newAbb.Area ?? Area.A2);
                //return RedirectToAction(nameof(District), new { district = newAbb.Area ?? Area.A2 });
            }
                
        }
        [HttpPost]
        public IActionResult DeleteExchange(Area district,string abbreviation)
        {
            if (ModelState.IsValid)
            {
                repository.RemoveAbbreviation(abbreviation);
                TempData["message"] = $"{abbreviation} deleted successfully.";
            }
            return RedirectToAction(nameof(District),new { district});
        }

        [HttpPost]
        public IActionResult EditExchange([Bind(Prefix = nameof(DistrictViewModel.SelectedExchange))]Abbreviation editAbb)
        {
            if(ModelState.IsValid)
            {
                repository.EditAbbreviation(editAbb);
                TempData["message"] = $"{editAbb.Name} edited successfully.";
                return RedirectToAction(nameof(District), new { district = editAbb.Area ?? Area.A2 });
            }
            else
            {
                return District(editAbb.Area.Value);
            }
        }


        public IActionResult Exchange(Area district, string exchange)
        {
            //var q = repository.Abbreviations.Where(x => x.Abb == exchange).Include(x => x.NetworkElements).OrderBy(x => x.NetworkElements.Select(y => y.Name)).FirstOrDefault();
            var model = new ExchangeViewModel()
            {
                SelectedDistrict = district,
                Exchanges = repository.Abbreviations.Where(x => x.Area == district).OrderBy(x => x.Abb).ToList(),
                SelectedExchange = repository.Abbreviations.Where(x => x.Abb == exchange).Include(x => x.NetworkElements).FirstOrDefault(),
                Miscs = repository.Miscs.ToList()
            };
            
            return View(nameof(Exchange),model);
        }
        //[AcceptVerbs("Get", "Post")]
        public IActionResult AbbValidation(string exchange, [Bind(Prefix = nameof(ExchangeViewModel.SelectedExchange))]Abbreviation arg)
        {
          var res = arg.Abb.ToUpperInvariant();
            var q = repository.Abbreviations.Where(x => x.Abb == res).Select(x=>x.Name).FirstOrDefault();
            if (q is null || exchange == res)
                return Json(true);
            else
                return Json($"Duplicate Abbreviation: {q}");
        }

        public IActionResult AddNe(Area district, string exchange, [Bind(Prefix =nameof(ExchangeViewModel.NewNE))]NetworkElement newNe)
        {
            if(ModelState.IsValid)
            {
                repository.AddNetworkElement(newNe);
                TempData["message"] = $"{newNe.Name} added succesfully.";
                return RedirectToAction(nameof(Exchange), new { district, exchange });
            }
            else
            {
                return Exchange(district, exchange);
            }
        }
    }
}
