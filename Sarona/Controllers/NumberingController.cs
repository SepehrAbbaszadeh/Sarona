using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;
using Sarona.ViewModels;
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
            if (prefix is null)
                prefix = "";
            var model = new NumberingPoolViewModel()
            {
                Prefix = prefix,
                Miscs = repository.Miscs.ToArray(),
                //NewPrefix = new NumberingPool()
                Prefixes = repository.NumberingPools
                            .Where(x => x.Prefix.StartsWith(prefix))
                            .Include(x => x.NumberingPoolNetworkElements)
                            .ThenInclude(x => x.Element)
                            .ThenInclude(x => x.Customer)
                            .OrderBy(x => x.Prefix)
                            .Skip((page - 1) * Settings.PageSize)
                            .Take(Settings.PageSize)
                            .ToArray()
            };
            return View(model);
        }

        public NumberingPool Prefix(long id)
        {
            return repository.NumberingPools.Where(x => x.Id == id).First();
        }

        [HttpPost]
        public IActionResult DeletePrefix(long id,string prefix)
        {
            if(ModelState.IsValid)
            {
                var np = repository.DeleteNumberingPool(id);
                TempData["message"] = $"{np.Prefix} deleted successfully.";
                return RedirectToAction(nameof(Pool), new { prefix });
            }
            return Pool(prefix);
        }

        [HttpPost]
        public IActionResult EditPrefix([Bind(Prefix = nameof(NumberingPoolViewModel.NewPrefix))]NumberingPool np, string prefix)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = $"{np.Prefix} edited successfully.";
                np.Username = User.Identity.Name;
                repository.EditNumberingPool(np);
                return RedirectToAction(nameof(Pool), new { prefix });
            }
            return Pool(prefix);
        }

        [HttpPost]
        public IActionResult AddPrefix([Bind(Prefix = nameof(NumberingPoolViewModel.NewPrefix))]NumberingPool np, string prefix)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = $"{np.Prefix} added successfully.";
                np.Username = User.Identity.Name;
                repository.AddNumberingPool(np);
                return RedirectToAction(nameof(Pool), new { prefix});
            }
            return Pool(prefix);
        }
    }
}
