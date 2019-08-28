using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;
using Sarona.ViewModels;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private SaronaRepository repository;
        private IUrlHelperFactory urlHelperFactory;
        public SearchController(SaronaRepository repo, IUrlHelperFactory url)
        {
            repository = repo;
            urlHelperFactory = url;
        }
        public async Task<IActionResult> Index(string query)
        {
            var exchanges = repository.Exchanges
                .Where(x => x.Name.Contains(query) || x.Abb.Contains(query))
                .Select(x => new SearchRecord
                {
                    Type = SearchRecordType.Exchange,
                    Text = $"{x.Name}({x.Abb})",
                    HtmlLink = Url.Action("Network", "Exchange", new { exchange = x.Name, district = x.Area })
                });
            var nes = repository.NetworkElements
                .Where(x => x.Name.Contains(query))
                .Select(x => new SearchRecord
                {
                    Type = SearchRecordType.NE,
                    Text = $"{x.Name}({x.Model}-{x.Manufacturer})",
                    HtmlLink = Url.Action("Specification", "Network", new { district=x.Exchange.Area, exchange=x.Exchange.Name, ne=x.Name})
                });
            var numbers = repository.NumberingPools
                .Where(x => x.Prefix.Contains(query) || x.SubscriberName.Contains(query) || x.Abb.Contains(query))
                .Select(x => new SearchRecord
                {
                    Type = SearchRecordType.Number,
                    Text = $"{x.Prefix} ({x.Min}-{x.Max}) Subscriber: {x.SubscriberName} Abb: {x.Abb}",
                    HtmlLink = Url.Action("Pool", "Numbering", new { prefix = x.Prefix })
                });

            var model = new SearchViewModel()
            {
                Query = query,
                Records = await exchanges.Union(nes.Union(numbers)).AsNoTracking().ToArrayAsync()
            };
            return View(model);
        }
    }
}
