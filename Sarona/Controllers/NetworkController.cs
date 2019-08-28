using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;
using Sarona.ViewModels;
using System;
using System.Linq;


namespace Sarona.Controllers
{
    [Authorize(Roles = "Admins, Traffic")]
    public class NetworkController : Controller
    {
        private SaronaRepository repository;

        public NetworkController(SaronaRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public IActionResult District(Area district = Area.A2)
        {
            var model = new DistrictViewModel
            {
                Exchanges = repository.Exchanges.Where(x => x.Area == district).OrderBy(x => x.Abb).GroupBy(x => x.Providence, x => x, (key, g) => new ProvidenceExchanges { Providence = key, Exchanges = g.ToList() }),
                NewExchange = new Exchange(),
                SelectedDistrict = district,
                Miscs = repository.Miscs.Where(x => x.Type == MiscType.Providence).OrderBy(x => x.Name).ToArray()
            };
            return View("District", model);
        }

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
            {
                return Exchange(district, exchange);
            }
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
                    IsSite = x.IsSite,
                    Username = x.Username,
                    Providence = x.Providence,
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
                        Parent = new NetworkElement() { Name = y.Parent.Name }
                    }).ToList()
                }).FirstOrDefault(),
                Miscs = repository.Miscs.OrderBy(x => x.Name).ToArray()
            };
            return View(nameof(Exchange), model);
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult AbbValidation(string abb, bool onlyExchanges = false)
        {
            var res = abb.ToUpperInvariant();
            int q = 0;
            if (onlyExchanges)
            {
                q = repository.Exchanges.Where(x => x.Abb == res).Count();
            }
            else
            {
                q = repository.Abbreviations.Where(x => x.Abb == abb).Count();
            }

            if (q == 0)
            {
                return Json(true);
            }
            else
            {
                return Json($"This abbreviation already used!");
            }
        }
        [HttpPost]
        public IActionResult Exchange(Area district, string exchange, [Bind(Prefix = nameof(ExchangeViewModel.NewNE))]NetworkElement newNe)
        {
            if (ModelState.IsValid)
            {
                newNe.Username = User.Identity.Name;
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
                Misc = repository.Miscs.OrderBy(x => x.Name).ToArray(),
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
            {
                return Json(true);
            }
            else
            {
                return Json($"Duplicate Name: {q.Name} in {q.Exchange}");
            }
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
            {
                return Specifications(exch.Area, exch.Abb, ne.Name);
            }
        }

        [HttpPost]
        public IActionResult Links(Area district, string exchange, string ne, [Bind(Prefix = nameof(LinksViewModel.NewLink))]Link newLink)
        {
            if (ModelState.IsValid)
            {
                newLink.Username = User.Identity.Name;
                repository.AddLink(newLink);
                TempData["message"] = $"New \"{newLink.Type}\" link with \"{newLink.Channels}\" channels added successfully.";
                return RedirectToAction(nameof(Links));
            }
            return Links(district, exchange, ne);
        }

        public IActionResult LinkChannelValidaton([Bind(Prefix = nameof(LinksViewModel.NewLink))]Link newLink)
        {
            if (newLink.Type == LinkType.ISUP && newLink.Channels % 31 != 0)
            {
                return Json("Number of channels for ISUP links must be N*31");
            }
            else
            {
                return Json(true);
            }
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
                .ThenInclude(x => x.Exchange)
                .Include(x => x.Exchange)
                .First()
            };
            return View("RemoteElement", model);
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
            {
                return Links(district, exchange, ne);
            }
        }
        public IActionResult DeleteNe(Area district, string exchange, string ne, int id)
        {
            var networkElement = repository.DeleteNetworkElement(id);
            TempData["message"] = $"{networkElement.Name} deleted successfuly.";
            return RedirectToAction(nameof(Exchange), new { district, exchange });
        }

        public IActionResult Remotes(Area district, string exchange, string ne)
        {
            var model = new RemotesViewModel()
            {
                NE = repository.NetworkElements
                .Where(x => x.Name == ne)
                .Include(x => x.Exchange)

                .First()

            };
            model.Remotes = repository.NetworkElements
                .Where(x => x.ParentId == model.NE.Id && x.NetworkType == NeType.Remote)
                .Include(x => x.Exchange)
                .Include(x => x.NumberingPoolNetworkElements)
                .ThenInclude(x => x.Numbering);
            return View("Remotes", model);
        }
        public IActionResult Accesses(Area district, string exchange, string ne)
        {
            var model = new RemotesViewModel()
            {
                NE = repository.NetworkElements
                .Where(x => x.Name == ne)
                .Include(x => x.Exchange)

                .First()

            };
            model.Remotes = repository.NetworkElements
                .Where(x => x.ParentId == model.NE.Id && x.NetworkType == NeType.Access)
                .Include(x => x.Exchange)
                .Include(x => x.NumberingPoolNetworkElements)
                .ThenInclude(x => x.Numbering);
            return View("Remotes", model);
        }

        public IActionResult Numbering(Area district, string exchange, string ne)
        {
            var model = repository.NetworkElements
                .Where(x => x.Name == ne)
                .Include(x => x.NumberingPoolNetworkElements)
                .ThenInclude(x => x.Numbering)
                .Include(x => x.Exchange)
                .First();
            return View("Numbering", model);
        }

        public IActionResult AttachPrefixes(Area district, string exchange, string ne, string prefixes)
        {
            var splits = prefixes.Split(',');
            long[] ids = new long[splits.Length];
            for (int i = 0; i < splits.Length; i++)
            {
                ids[i] = Convert.ToInt64(splits[i]);
            }
            repository.AttachNumberingPool(ne, User.Identity.Name, ids);
            return RedirectToAction(nameof(Numbering), new { district, exchange, ne });
        }

        public IActionResult DetachPrefix(Area district, string exchange, string ne, long neId, long prefixId)
        {
            if (ModelState.IsValid)
            {
                repository.DetachPrefix(neId, prefixId);
                return RedirectToAction(nameof(Numbering), new { district, exchange, ne });
            }
            return BadRequest();
        }
    }
}
