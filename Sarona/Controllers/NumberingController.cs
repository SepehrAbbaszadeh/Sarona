using Microsoft.AspNetCore.Mvc;
using Sarona.Models;
using Sarona.ViewModels;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.Controllers
{
    public class NumberingController : Controller
    {
        private SaronaRepository repository;
        public NumberingController(SaronaRepository rep)
        {
            repository = rep;
        }
        public IActionResult Pool(string prefix = "", int page = 1)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = "";
            }

            var model = new NumberingPoolViewModel()
            {
                Prefix = prefix,
                Miscs = repository.Miscs.OrderBy(x => x.Name).ToArray(),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = Settings.PageSize,
                    From = (page - 1) * Settings.PageSize,
                    To = (page - 1) * Settings.PageSize + Settings.PageSize,
                    TotalItems = repository.NumberingPools
                                .Where(x => x.Prefix.StartsWith(prefix))
                                .Count()
                },
                Prefixes = repository.GetNps()
                            .Where(x => x.Prefix.StartsWith(prefix))
                            .Skip((page - 1) * Settings.PageSize)
                            .Take(Settings.PageSize)
                            .ToArray()
            };
            return View("Pool", model);
        }

        [HttpPost]
        public IActionResult InsertFreeCodes([Bind(Prefix = nameof(NumberingPoolViewModel.FreeCode))]InsertNumberingPool freeCode, string prefix, int page)
        {
            if (ModelState.IsValid)
            {
                if (repository.InsertFreeCodes(freeCode, User.Identity.Name, out string error, out int addNo))
                {
                    TempData["message"] = $"\"{freeCode.From}\" to \"{freeCode.To}\" ({addNo}) added successfully.";
                    return RedirectToAction(nameof(Pool), new { prefix, page });
                }
                ModelState.AddModelError("overlap", error);
            }

            return Pool(prefix, page);
        }

        public NumberingPool Prefix(long id)
        {
            return repository.NumberingPools.Where(x => x.Id == id).First();
        }

        [HttpPost]
        public IActionResult DeletePrefix(string selectedPrefix, string prefix, int page)
        {
            if (ModelState.IsValid)
            {
                var prefixes = selectedPrefix.Split(',');
                repository.DeleteNumberingPool(prefixes);
                TempData["message"] = $"{string.Join(',',prefixes)} deleted successfully.";
                return RedirectToAction(nameof(Pool), new { prefix });
            }
            return Pool(prefix);
        }

        [HttpPost]
        public IActionResult EditPrefix([Bind(Prefix = nameof(NumberingPoolViewModel.EditPrefix))]NumberingPool editPrefix, string prefix, int page)
        {
            if (ModelState.IsValid)
            {
                editPrefix.Username = User.Identity.Name;
                repository.EditNumberingPool(editPrefix);
                TempData["message"] = $"{editPrefix.Prefix} edited successfully.";
                return RedirectToAction(nameof(Pool), new { prefix });
            }
            return Pool(prefix);
        }

        [HttpPost]
        public IActionResult AddPrefix([Bind(Prefix = nameof(NumberingPoolViewModel.NewPrefix))]NumberingPool np, bool isReservedPrefix, string prefix, int page)
        {
            if (ModelState.IsValid)
            {
                np.Username = User.Identity.Name;
                repository.AddNumberingPool(np, isReservedPrefix);
                TempData["message"] = $"{np.Prefix} added successfully.";
                return RedirectToAction(nameof(Pool), new { np.Prefix });
            }
            return Pool(prefix, page);
        }

        public IActionResult PrefixValidation(string prefix, string oldPrefix)
        {
            var q = repository.PrefixValidationSearch(prefix);
            if(!string.IsNullOrEmpty(oldPrefix))
            {
                if (oldPrefix.StartsWith(prefix) || prefix.StartsWith(oldPrefix))
                    return Json(q.Count() - 1);
            }
            return Json(q.LongCount());
        }




        public IEnumerable<NumberingPool> GetPrefixes(string prefix)
        {
            var q = repository.NumberingPools
                .Where(x => x.Prefix.StartsWith(prefix) && ((x.Status == NumberingStatus.Free || x.Status == NumberingStatus.Reserved) || x.IsFloat))
                .OrderBy(x => x.Prefix).ToArray();
            return q;
        }

        [HttpPost]
        public IActionResult ReserveCodes(Area? area, LinkType? link, string[] prefixes, byte min, byte max, bool changeMinMax)
        {
            if (ModelState.IsValid)
            {
                repository.ReserveCodes(prefixes, area, link, min, max,changeMinMax, User.Identity.Name);
                return Json(true);
            }
            return BadRequest();
        }

        public IActionResult GetSubscribers(string name)
        {
            name = name.Replace('ي', 'ی').Replace('ك', 'ک');
            var subs = repository.NumberingPools.Where(x => x.NormalizedSubscriberName.Contains(name.Replace(" ",""))).Select(x => new { x.Id, x.SubscriberName, x.Abb }).OrderBy(x => x.SubscriberName).GroupBy(x => new { x.SubscriberName, x.Abb }).Select(x => x.First()).ToList();
            return Json(subs);
        }
        [HttpPost]
        public IActionResult AssignPrefix(string selectedPrefix, long customerId, string prefix, int page, LinkType link, LinkDirection direction)
        {
            if (ModelState.IsValid)
            {
                var prefixes = selectedPrefix.Split(',');
                repository.AssignPrefix(prefixes, customerId, User.Identity.Name, link, direction);
                return RedirectToAction(nameof(Pool), new { prefix, page });
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult AssignPrefixWithNewCustomer(string selectedPrefix, string abb, string name, string prefix, int page, LinkType link, LinkDirection direction)
        {
            if (ModelState.IsValid)
            {
                var prefixes = selectedPrefix.Split(',');
                repository.AssignPrefix(prefixes, abb, name, User.Identity.Name, link, direction);
                return RedirectToAction(nameof(Pool), new { prefix, page });
            }
            return BadRequest();
        }
        public IActionResult RemovePrefix(string selectedPrefix, string prefix, int page)
        {
            if (ModelState.IsValid)
            {
                var prefixes = selectedPrefix.Split(',');
                repository.RemovePrefix( User.Identity.Name, prefixes);
                return RedirectToAction(nameof(Pool), new { prefix, page });
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult AttachPrefix(string selectedPrefix, long neId, string prefix, int page)
        {
            if (ModelState.IsValid)
            {
                var prefixes = selectedPrefix.Split(',');
                repository.AttachNumberingPool(neId, User.Identity.Name, prefixes);
                return RedirectToAction(nameof(Pool), new { prefix, page });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
