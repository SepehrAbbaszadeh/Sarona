using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;
using Sarona.ViewModels;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.Controllers
{
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
            var networkElement = repository.NetworkElements.Where(x => x.Name == ne && x.Exchange.Abb == exchange && x.Exchange.Area == district)
                .Select(x => new NetworkElement()
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    ExchangeId = x.ExchangeId,
                    Exchange = new Exchange() { Name = x.Exchange.Name },
                    InstalledCapacity = x.InstalledCapacity,
                    UsedCapacity = x.UsedCapacity,
                    Manufacturer = x.Manufacturer,
                    Model = x.Model,
                    Name = x.Name,
                    Type = x.Type,
                    Remark = x.Remark,
                    ParentId = x.ParentId,
                    LinksOnEnd1 = x.LinksOnEnd1.OrderBy(z => z.End2.Name).Select(y => new Link()
                    {
                        Channels = y.Channels,
                        Type = y.Type,
                        CreatedOn = y.CreatedOn,
                        Direction = y.Direction,
                        Remark = y.Remark,
                        End2Id = y.End2Id,
                        End2 = new NetworkElement() { Name = y.End2.Name, NetworkType = y.End2.NetworkType },
                        Id = y.Id
                    }),
                }).FirstOrDefault();
            var model = new LinksViewModel()
            {
                Misc = repository.Miscs.ToArray(),
                NE = networkElement
            };

            return View("Links", model);
        }
        [HttpGet]
        public IActionResult NetworkElement(Area district, string exchange, string ne)
        {

            var networkElement = repository.NetworkElements.Where(x => x.Name == ne && x.Exchange.Abb == exchange && x.Exchange.Area == district)
                .Select(x => new NetworkElement()
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    ExchangeId = x.ExchangeId,
                    Exchange = new Exchange() { Name = x.Exchange.Name },
                    InstalledCapacity = x.InstalledCapacity,
                    UsedCapacity = x.UsedCapacity,
                    Manufacturer = x.Manufacturer,
                    Model = x.Model,
                    Name = x.Name,
                    Type = x.Type,
                    Remark = x.Remark,
                    ParentId = x.ParentId,
                    NetworkElements = x.NetworkElements.Select(z => z),
                    NumberingPoolNetworkElements = x.NumberingPoolNetworkElements.OrderBy(a => a.Numbering.From).Select(z => new NumberingPoolNetworkElement()
                    {
                        Numbering = z.Numbering
                    }),
                    LinksOnEnd1 = x.LinksOnEnd1.OrderBy(z => z.End2.Name).Select(y => new Link()
                    {
                        Channels = y.Channels,
                        Type = y.Type,
                        CreatedOn = y.CreatedOn,
                        Direction = y.Direction,
                        Remark = y.Remark,
                        End2Id = y.End2Id,
                        End2 = new NetworkElement() { Name = y.End2.Name, NetworkType = y.End2.NetworkType },
                        Id = y.Id
                    }).ToList(),
                    Customer = x.Customer,
                }).FirstOrDefault();


            if (networkElement is null)
                throw new System.Exception("No NE.");
            switch (networkElement.NetworkType)
            {
                case NeType.Core:
                    return CoreElement(networkElement);
                case NeType.Access:
                    return AccessElement(networkElement);
                case NeType.Remote:
                    return RemoteElement(networkElement);
                case NeType.PBX:
                    return PbxElement(networkElement);
                case NeType.IP_PBX:
                    return CoreElement(networkElement);
                default:
                    throw new System.Exception("No page found!");
            }
        }
        private IActionResult CoreElement(NetworkElement ne)
        {
            var model = new CoreViewModel()
            {
                NE = ne,
                Miscs = repository.Miscs.OrderBy(x => x.Name).ToArray()
            };
            return View("Core", model);
        }
        private IActionResult RemoteElement(NetworkElement ne)
        {
            return View("Core");
        }
        private IActionResult AccessElement(NetworkElement ne)
        {
            return View("Core");
        }
        private IActionResult PbxElement(NetworkElement ne)
        {
            return View("Core");
        }

        public IActionResult DeleteNe(int id)
        {
            var ne = repository.NetworkElements.Where(x => x.Id == id).Include(x => x.Exchange).First();
            repository.DeleteNetworkElement(ne);
            return RedirectToAction(nameof(Exchange), new { exchange = ne.Exchange.Abb, district = ne.Exchange.Area });
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
                return RedirectToAction(nameof(NetworkElement), new { district = exch.Area, exchange = exch.Abb, ne = ne.Name });
            }
            else
                return NetworkElement(exch.Area, exch.Abb, ne.Name);
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
    }
}
