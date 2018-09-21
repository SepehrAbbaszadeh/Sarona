using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;
using Sarona.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Sarona.Controllers
{
    [Authorize]
    public class ReportController:Controller
    {
        SaronaRepository repository;
        private IHostingEnvironment hostingEnvironment;

        public ReportController(SaronaRepository repo, IHostingEnvironment env)
        {
            repository = repo;
            hostingEnvironment = env;
        }


        public IActionResult Shenasname()
        {
            return View();
        }

        public async Task<IActionResult> ShenasnameExcel(Area area)
        {
            string tempFolder = Path.GetTempPath();
            var q = await repository.Exchanges.Where(x => x.Area == area).Include(x => x.NetworkElements).ThenInclude(x => x.Parent).ToListAsync();
            var fileName = $"{q.First().Area} (Shenasname) {Settings.GetDateTimeNowFile()}.xlsx";
            string path = Path.Combine(tempFolder, fileName);
            Infrastructure.ReportGenerator report = new Infrastructure.ReportGenerator(User.Identity.Name);
            report.ShenasnameByArea(q, path);
            var memory = new MemoryStream();    
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public async Task<IActionResult> Links(string name)
        {
            string tempFolder = Path.GetTempPath();
            var fileName = $"{name} (Links) {Settings.GetDateTimeNowFile()}.xlsx";

            string path = Path.Combine(tempFolder, fileName);
            Infrastructure.ReportGenerator report = new Infrastructure.ReportGenerator(User.Identity.Name);
            report.LinksReport(repository.GetLinks(name), path);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

       


    }
}
