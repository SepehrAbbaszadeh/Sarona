using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;
using Sarona.ViewModels;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.Controllers
{
    [Authorize]
    public class NetworkController : Controller
    {
        private SaronaRepository repository;
        
        public NetworkController(SaronaRepository repo)
        {
            repository = repo;
        }
        /// <summary>
        /// View Area page.[HttpGet]
        /// URL Example: Network/A8
        /// </summary>
        /// <param name="district">Name of area.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult District(Area district = Area.A2)
        {
            var model = new DistrictViewModel
            {
                Exchanges = repository.Exchanges.Where(x => x.Area == district).OrderBy(x => x.Abb),
                NewExchange = new Exchange(),
                SelectedDistrict = district
            };
            return View("District", model);
        }
        /// <summary>
        /// Add new exchange to an area. [HttpPost]
        /// URL Example: /Network/A8
        /// </summary>
        /// <param name="newExch"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult District([Bind(Prefix = nameof(DistrictViewModel.NewExchange))]Exchange newExch)
        {
            if (ModelState.IsValid)
            {
                newExch.Username = User.Identity.Name;
                repository.AddExchange(newExch);
                TempData["message"] = $"{newExch.Name} added successfully.";
                return RedirectToAction(nameof(District), new { district = newExch.Area });
            }
            else
            {
                return District(newExch.Area);
                //return RedirectToAction(nameof(District), new { district = newAbb.Area ?? Area.A2 });
            }

        }

        [HttpPost]
        public IActionResult DeleteExchange(Area district, string exchange)
        {
            if (ModelState.IsValid)
            {
                var exch = repository.RemoveExchange(exchange);
                TempData["message"] = $"{exch.Abb}({exch.Name}) deleted successfully.";
                return RedirectToAction(nameof(District), new { district });
            }
            else
                return Exchange(district, exchange);
        }
        [HttpPost]
        public IActionResult EditExchange([Bind(Prefix = nameof(ExchangeViewModel.SelectedExchange))]Exchange exch)
        {
            if (ModelState.IsValid)
            {
                exch.Username = User.Identity.Name;
                repository.EditExchange(exch);
                TempData["message"] = $"{exch.Name} edited successfully.";
                return RedirectToAction(nameof(Exchange), new { district = exch.Area, exchange = exch.Abb });
            }
            else
            {
                return Exchange(exch.Area, exch.Abb);
            }
        }
        /// <summary>
        /// Get exchange page.
        /// </summary>
        /// <param name="exchange">Exchange's abbreviation</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Exchange(Area district, string exchange)
        {
            var model = new ExchangeViewModel()
            {
                SelectedDistrict = district,
                Exchanges = repository.Exchanges.Where(x => x.Area == district).OrderBy(x => x.Abb).ToList(),
                SelectedExchange = repository.Exchanges.Where(x => x.Abb == exchange && x.Area == district).Select(x => new Exchange()
                {
                    Id = x.Id,
                    Abb = x.Abb,
                    Name = x.Name,
                    CreatedOn = x.CreatedOn,
                    ModifiedOn = x.ModifiedOn,
                    Area = x.Area,
                    Username = x.Username,
                    NetworkElements = x.NetworkElements.Select(y => new NetworkElement()
                    {
                        Model = y.Model,
                        Name = y.Name,
                        InstalledCapacity = y.InstalledCapacity,
                        Manufacturer = y.Manufacturer,
                        NetworkType = y.NetworkType,
                        Remark = y.Remark,
                        UsedCapacity = y.UsedCapacity,
                        Type = y.Type,
                        Owner = y.Owner,
                        Customer = new Customer() { Name = y.Customer.Name, Abb = y.Customer.Abb },
                        Parent = new NetworkElement() { Name = y.Parent.Name }
                    }).ToList()
                }).FirstOrDefault(),
                Miscs = repository.Miscs.OrderBy(x => x.Name).ToArray()
            };
            return View(nameof(Exchange), model);
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult AbbValidation(string abb)
        {
            var res = abb.ToUpperInvariant();
            var q = repository.Abbreviations.Where(x => x.Abb == res).Select(x => x.Name).FirstOrDefault();
            if (q is null)
                return Json(true);
            else
                return Json($"Duplicate Abbreviation: {q}");
        }
        [HttpPost]
        public IActionResult Exchange(Area district, string exchange, [Bind(Prefix = nameof(ExchangeViewModel.NewNE))]NetworkElement newNe)
        {
            if (ModelState.IsValid)
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


        public IActionResult Links(Area district, string exchange, string ne)
        {
            
            var model = new LinksViewModel()
            {
                Misc = repository.Miscs.ToArray(),
                NE = repository.GetLinks(ne)
            };

            return View("Links", model);
        }




        [AcceptVerbs("Get", "Post")]
        public IActionResult NeNameValidation(string name)
        {
            var res = name.ToUpperInvariant();
            var q = repository.NetworkElements.Where(x => x.Name == res).Select(x => new { x.Name, Exchange = x.Exchange.Name }).FirstOrDefault();
            if (q is null)
                return Json(true);
            else
                return Json($"Duplicate Name: {q.Name} in {q.Exchange}");
        }
        [HttpPost]
        public IActionResult EditNe([Bind(Prefix = nameof(CoreViewModel.NE))]NetworkElement ne)
        {
            var exch = repository.Exchanges.Where(x => x.Id == ne.ExchangeId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                repository.EditNetworkElement(ne);
                TempData["message"] = $"{ne.Name} edited successfully.";
                return RedirectToAction(nameof(Specifications), new { district = exch.Area, exchange = exch.Abb, ne = ne.Name });
            }
            else
                return Specifications(exch.Area, exch.Abb, ne.Name);
        }
        /// <summary>
        /// Add Link.
        /// </summary>
        /// <param name="district"></param>
        /// <param name="exchange"></param>
        /// <param name="ne"></param>
        /// <param name="newLink"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Links(Area district, string exchange, string ne, [Bind(Prefix = nameof(LinksViewModel.NewLink))]Link newLink)
        {
            if (ModelState.IsValid)
            {
                repository.AddLink(newLink);
                TempData["message"] = $"New \"{newLink.Type}\" link with \"{newLink.Channels}\" channels added successfully.";
                return RedirectToAction(nameof(Links));
            }
            return Links(district, exchange, ne);
        }

        public IActionResult LinkChannelValidaton([Bind(Prefix = nameof(LinksViewModel.NewLink))]Link newLink)
        {
            if (newLink.Type == LinkType.ISUP && newLink.Channels % 31 != 0)
                return Json("Number of channels for ISUP links must be N*31");
            else
                return Json(true);
        }
        public IActionResult Specifications(Area district, string exchange, string ne)
        {
            var q = repository.NetworkElements
                .Where(x => x.Name == ne)
                .Select(x => new NetworkElement()
                {
                    NetworkType = x.NetworkType,
                    Id = x.Id
                })
                .First();
            switch (q.NetworkType)
            {
                case NeType.Core:
                    return CoreElement(q);
                case NeType.Access:
                    return AccessElement(q);
                case NeType.Remote:
                    return RemoteElement(q);
                case NeType.PBX:
                    return PbxElement(q);
                case NeType.IP_PBX:
                    return PbxElement(q);
                default:
                    return BadRequest();
            }
        }
        private IActionResult CoreElement(NetworkElement ne)
        {
            var childrenCap = repository.NetworkElements
                .Where(x => x.ParentId == ne.Id)
                .GroupBy(x => x.NetworkType)
                .Select(x => new CapacitySpecs
                {
                    Type = x.Key,
                    Count = x.Count(),
                    SumInstalled = x.Sum(y => (long)y.InstalledCapacity),
                    SumUsed = x.Sum(y => (long)y.UsedCapacity)
                })
                .ToList();

            var linksSpecsByLinkType = repository.Links
                .Where(l => l.End1Id == ne.Id)
                .GroupBy(x => x.Type)
                .Select(x => new LinksSpecsByLinkType()
                {
                    Type = x.Key,
                    Count = x.Count(),
                    SumChannels = x.Sum(y => y.Channels)
                })
                .ToList();

            var linksSpecsByNeType = repository.Links
                .Where(l => l.End1Id == ne.Id)
                .GroupBy(x => x.End2.NetworkType)
                .Select(x => new LinksSpecsByNeType()
                {
                    Type = x.Key,
                    Count = x.Count(),
                    SumChannels = x.Sum(y => y.Channels)
                })
                .ToList();

            var model = new CoreViewModel()
            {
                NE = repository.NetworkElements.Where(x => x.Id == ne.Id).Include(x => x.Exchange).First(),
                Remotes = childrenCap.Where(x => x.Type == NeType.Remote).FirstOrDefault(),
                Accesses = childrenCap.Where(x => x.Type == NeType.Access).FirstOrDefault(),
                CoreLinks = linksSpecsByNeType.Where(x => x.Type == NeType.Core).FirstOrDefault(),
                PbxLinks = linksSpecsByNeType.Where(x => x.Type == NeType.PBX).FirstOrDefault(),
                IpPbxLinks = linksSpecsByNeType.Where(x => x.Type == NeType.IP_PBX).FirstOrDefault(),
                IsupLinks = linksSpecsByLinkType.Where(x => x.Type == LinkType.ISUP).FirstOrDefault(),
                SipLinks = linksSpecsByLinkType.Where(x => x.Type == LinkType.SIP).FirstOrDefault(),
                Miscs = repository.Miscs.OrderBy(x => x.Name).ToArray()
            };
            return View("Core", model);
        }
        private IActionResult RemoteElement(NetworkElement ne)
        {
            var model = new RemoteElementViewModel()
            {
                Miscs = repository.Miscs.OrderBy(x => x.Name).ToArray(),
                NE = repository.NetworkElements
                .Where(x => x.Id == ne.Id)
                .Include(x => x.Parent)
                .ThenInclude(x=>x.Exchange)
                .Include(x => x.Exchange)
                .First()
            };
            return View("RemoteElement",model);
        }
        private IActionResult AccessElement(NetworkElement ne)
        {
            var model = new RemoteElementViewModel()
            {
                Miscs = repository.Miscs.OrderBy(x => x.Name).ToArray(),
                NE = repository.NetworkElements
                .Where(x => x.Id == ne.Id)
                .Include(x => x.Parent)
                .ThenInclude(x => x.Exchange)
                .Include(x => x.Exchange)
                .First()
            };
            return View("AccessElement", model);
        }
        private IActionResult PbxElement(NetworkElement ne)
        {
            var model = new RemoteElementViewModel()
            {
                Miscs = repository.Miscs.OrderBy(x => x.Name).ToArray(),
                NE = repository.NetworkElements.Where(x => x.Id == ne.Id).Include(x => x.Parent).Include(x => x.Exchange).First()
            };
            return View("RemoteElement", model);
        }

        public IActionResult DeleteLink(Area district, string exchange, string ne, long id)
        {
            repository.DeleteLink(id);
            TempData["message"] = "Link deleted successfully.";
            return RedirectToAction(nameof(Links), new { district, exchange, ne });
        }
        [HttpPost]
        public IActionResult EditLink(Area district, string exchange, string ne, [Bind(Prefix = nameof(LinksViewModel.EditLink))]Link link)
        {
            if (ModelState.IsValid)
            {
                repository.EditLink(link);
                TempData["message"] = $"Link edited successfully.";
                return RedirectToAction(nameof(Links), new { district, exchange, ne });
            }
            else
                return Links(district, exchange, ne);
        }
        public IActionResult DeleteNe(Area district, string exchange, string ne, int id)
        {
            var networkElement = repository.NetworkElements.Where(x => x.Id == id).First();
            repository.DeleteNetworkElement(networkElement);
            TempData["message"] = $"{networkElement.Name} deleted successfuly.";
            return RedirectToAction(nameof(Exchange), new { district, exchange });
        }

        public IActionResult Remotes(Area district, string exchange, string ne)
        {
            var model = new RemotesViewModel()
            {
                NE = repository.NetworkElements.Where(x => x.Name == ne).Include(x=>x.Exchange).First()
                
            };
            model.Remotes = repository.NetworkElements.Where(x => x.ParentId == model.NE.Id && x.NetworkType == NeType.Remote).Include(x => x.Exchange);
            return View("Remotes", model);
        }
        public IActionResult Accesses(Area district, string exchange, string ne)
        {
            var model = new RemotesViewModel()
            {
                NE = repository.NetworkElements.Where(x => x.Name == ne).Include(x => x.Exchange).First()

            };
            model.Remotes = repository.NetworkElements.Where(x => x.ParentId == model.NE.Id && x.NetworkType == NeType.Access).Include(x => x.Exchange);
            return View("Remotes", model);
        }
    }
}
