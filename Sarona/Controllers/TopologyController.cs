using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;
using Sarona.ViewModels;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarona.Controllers
{
    [Authorize]
    public class TopologyController : Controller
    {
        private SaronaRepository repository;
        private IHostingEnvironment hostingEnvironment;
        private SwitchContext switchContext;

        public TopologyController(SaronaRepository repo, IHostingEnvironment env, SwitchContext swcntx)
        {
            repository = repo;
            hostingEnvironment = env;
            switchContext = swcntx;
        }


        public IActionResult Index()
        {
            var q = repository.Exchanges.Where(x => x.Area == Area.A8).Include(x => x.NetworkElements).ThenInclude(x => x.LinksOnEnd1).ThenInclude(x => x.End2).ToList();
            Infrastructure.TopologyDrawing drawing = new Infrastructure.TopologyDrawing();
            drawing.Create(q);
            return View();
        }

    }
}
