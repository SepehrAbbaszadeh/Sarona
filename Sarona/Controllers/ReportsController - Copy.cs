using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class ReportController : Controller
    {
        private SaronaRepository repository;
        private IHostingEnvironment hostingEnvironment;
        private SwitchContext switchContext;

        public ReportController(SaronaRepository repo, IHostingEnvironment env, SwitchContext swcntx)
        {
            repository = repo;
            hostingEnvironment = env;
            switchContext = swcntx;
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
            Infrastructure.ReportGenerator report = new Infrastructure.ReportGenerator(User.Identity.Name, path);
            report.ShenasnameByArea(q);
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
            Infrastructure.ReportGenerator report = new Infrastructure.ReportGenerator(User.Identity.Name, path);
            report.LinksReport(repository.GetLinks(name));
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private void GetFullPageRecord(int i, FullPageRecord[] records, ILookup<string, NumberingPool> allPrefixes)
        {
            string query = i.ToString();

            var q = allPrefixes.Where(x => x.Key == i.ToString());
            var record = new FullPageRecord
            {
                //Row = Convert.ToInt32(query.Substring(0, 2)),
                Prefix = i.ToString()
            };
            if (q.Count() == 0)
            {
                record.Description = "";
                record.Color = Color.White;
                records[i - 2000] = record;
            }
            else
            {
                int capacity = 0;


                foreach (var number in q.First())
                {
                    switch (number.Prefix.Length)
                    {
                        case 4:
                            capacity += 10000;
                            break;
                        case 5:
                            capacity += 1000;
                            break;
                        case 6:
                            capacity += 100;
                            break;
                        case 7:
                            capacity += 10;
                            break;
                        case 8:
                            capacity += 1;
                            break;
                        default:
                            break;
                    }
                }
                if (capacity == 10000)
                {
                    record.Color = Color.LightGray;
                }
                else
                {
                    record.Color = Color.LightGreen;
                }

                var typeGroup = (from r in q.First()
                                 orderby r.Status
                                 group r by r.NumberType into grp
                                 select new { NumberType = grp.Key, grp }).ToArray();

                StringBuilder sb = new StringBuilder();
                foreach (var item in typeGroup)
                {
                    sb.Append($"<b>{item.NumberType}</b>:<br/>");
                    var statusGroup = from r in item.grp
                                      group r by r.Status into sgrp
                                      select new { Status = sgrp.Key, Count = sgrp.Count() };
                    foreach (var stat in statusGroup)
                    {
                        if (stat.Status == NumberingStatus.Used)
                        {
                            var linkTypeGroup = from r in item.grp
                                                where r.Status == stat.Status && r.Link != null
                                                group r by r.Link into lgrp
                                                select new { LinkType = lgrp.Key, Count = lgrp.Count() };
                            if (linkTypeGroup.Count() != 0)
                            {
                                sb.Append($"{stat.Status}: {stat.Count}(");


                                foreach (var link in linkTypeGroup)
                                {
                                    sb.Append($"{link.LinkType}: {link.Count}");
                                }

                                sb.Append(")<br/>");
                            }
                            else
                            {
                                sb.Append($"{stat.Status}: {stat.Count}");
                            }
                        }
                        else
                        {
                            sb.Append($"{stat.Status}: {stat.Count}<br/>");
                        }
                    }

                }
                record.Description = sb.ToString();
                records[i - 2000] = record;
            }
        }

        public IActionResult FullSheet()
        {

            var model = new FullPageRecord[7000];


            var allPrefixes = repository.NumberingPools
                .Where(x =>
            (x.Prefix.StartsWith('2') ||
            x.Prefix.StartsWith('3') ||
            x.Prefix.StartsWith('4') ||
            x.Prefix.StartsWith('5') ||
            x.Prefix.StartsWith('6') ||
            x.Prefix.StartsWith('7') ||
            x.Prefix.StartsWith('8')) && x.Prefix.Length > 3)
            .AsNoTracking()
            .ToLookup(x => x.Prefix.Substring(0, 4));

            Parallel.For(2000, 9000, (i) =>
            {
                GetFullPageRecord(i, model, allPrefixes);
            });

            var lessThan4Digit = repository.NumberingPools
                .Where(x =>
            (x.Prefix.StartsWith('2') ||
            x.Prefix.StartsWith('3') ||
            x.Prefix.StartsWith('4') ||
            x.Prefix.StartsWith('5') ||
            x.Prefix.StartsWith('6') ||
            x.Prefix.StartsWith('7') ||
            x.Prefix.StartsWith('8')) && x.Prefix.Length < 4)
            .AsNoTracking()
            .ToList();

            foreach (var np in lessThan4Digit)
            {
                int j = 0;
                switch (np.Prefix.Length)
                {
                    case 3:
                        j = 10;
                        break;
                    case 2:
                        j = 100;
                        break;
                    case 1:
                        j = 1000;
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < j; i++)
                {
                    if (!int.TryParse(np.Prefix, out int p))
                        continue;
                    p = p * j;
                    model[(p + i) - 2000].Color = Color.LightGray;
                    model[(p + i) - 2000].Description = $"Please refer to '{np.Prefix}' for detail.";
                }

            }




            return View(model);
        }



        private IQueryable<PRA_SIP_Sarona> GetRedFolderRecords(string abb, string code, QueryMode mode)
        {
            IQueryable<PRA_SIP_Sarona> trunks = switchContext.RedFolder.AsQueryable();
            if (!string.IsNullOrEmpty(abb))
            {
                trunks = trunks.Where(x => x.SubscriberAbb == abb);
            }

            if (!string.IsNullOrEmpty(code))
            {
                switch (mode)
                {
                    case QueryMode.RightMatch:
                        trunks = trunks.Where(x => x.SubscriberCode.StartsWith(code));
                        break;
                    case QueryMode.Exact:
                        trunks = trunks.Where(x => x.SubscriberCode == code);
                        break;
                    case QueryMode.Like:
                        trunks = trunks.Where(x => x.SubscriberCode.Contains(code));
                        break;
                    default:
                        break;
                }
            }
            return trunks;
        }

        public async Task<IActionResult> RedFolder(string abb, string code, QueryMode mode, int page = 1)
        {
            var trunks = GetRedFolderRecords(abb, code, mode);
            var model = new RedFolderViewModel()
            {
                Abb = abb,
                Code = code,
                Mode = mode,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = Settings.PageSize,
                    TotalItems = await trunks.CountAsync(),
                    From = (page - 1) * Settings.PageSize,
                    To = (page - 1) * Settings.PageSize + Settings.PageSize
                },
                Records = await trunks.Skip((page - 1) * Settings.PageSize).Take(Settings.PageSize).ToListAsync()
            };


            return View(model);
        }

        public async Task<IActionResult> RedFolderExcel(string abb, string code, QueryMode mode)
        {
            var trunks = GetRedFolderRecords(abb, code, mode);
            string tempFolder = Path.GetTempPath();
            var fileName = $"RedFolder({abb}_{code}) {Settings.GetDateTimeNowFile()}.xlsx";
            string path = Path.Combine(tempFolder, fileName);
            Infrastructure.ReportGenerator report = new Infrastructure.ReportGenerator(User.Identity.Name, path);
            report.RedFolder(trunks);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


        public async Task<IActionResult> NumberingExcel([Bind(Prefix = nameof(ViewModels.NumberingSearchViewModel.ExcelSearch))]NpSearch search)
        {

            var nps = NumberingQuery(search);
            string tempFolder = Path.GetTempPath();
            var fileName = $"Numbering {Settings.GetDateTimeNowFile()}.xlsx";
            string path = Path.Combine(tempFolder, fileName);
            Infrastructure.ReportGenerator report = new Infrastructure.ReportGenerator(User.Identity.Name, path);
            report.Numbering(nps);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private IQueryable<NumberingPool> NumberingQuery(NpSearch search)
        {
            var query = repository.GetNps();

            if (!string.IsNullOrEmpty(search.Prefix))
            {
                switch (search.PrefixMode)
                {
                    case QueryMode.RightMatch:
                        query = query.Where(x => x.Prefix.StartsWith(search.Prefix));
                        break;
                    case QueryMode.Exact:
                        query = query.Where(x => x.Prefix == search.Prefix);
                        break;
                    case QueryMode.Like:
                        query = query.Where(x => x.Prefix.Contains(search.Prefix));
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(search.Abb))
            {
                query = query.Where(x => x.Abb == search.Abb);
            }

            if (search.ChargingCases != null)
            {
                query = query.Where(x => search.ChargingCases.Contains(x.ChargingCase));
            }

            if (search.Directions != null)
            {
                query = query.Where(x => search.Directions.Contains(x.Direction.Value));
            }

            if (search.Max != null)
            {
                query = query.Where(x => x.Max == search.Max);
            }

            if (search.Min != null)
            {
                query = query.Where(x => x.Min == search.Min);
            }

            if (search.Owners != null)
            {
                query = query.Where(x => search.Owners.Contains(x.Owner));
            }

            if (search.RondTypes != null)
            {
                query = query.Where(x => search.RondTypes.Contains(x.Rond));
            }

            if (search.Statuses != null)
            {
                query = query.Where(x => search.Statuses.Contains(x.Status));
            }

            if (!string.IsNullOrEmpty(search.SubscriberName))
            {
                search.SubscriberName = search.SubscriberName.Replace('ي', 'ی').Replace('ك', 'ک');
                query = query.Where(x => x.NormalizedSubscriberName.Contains(search.SubscriberName.Replace(" ","")));
            }

            if (search.RondTypes != null)
            {
                query = query.Where(x => search.RondTypes.Contains(x.Rond));
            }

            if (search.NumberTypes != null)
            {
                query = query.Where(x => search.NumberTypes.Contains(x.NumberType));
            }
            if (search.LinkTypes != null)
            {
                query = query.Where(x => search.LinkTypes.Contains(x.Link.Value));
            }
            if (search.Areas != null)
            {
                query = query.Where(x => search.Areas.Contains(x.Area.Value));
            }

            return query;
        }

        public async Task<IActionResult> Numbering([Bind(Prefix = nameof(NumberingSearchViewModel.Search))]NpSearch search, int page = 1)
        {
            var query = NumberingQuery(search);
            var model = new NumberingSearchViewModel
            {
                PageInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = Settings.PageSize,
                    TotalItems = await query.CountAsync(),
                    From = (page - 1) * Settings.PageSize,
                    To = (page - 1) * Settings.PageSize + Settings.PageSize
                },
                Search = search,
                Miscs = await repository.Miscs.OrderBy(x => x.Name).ToArrayAsync(),
                ExcelSearch = search,
                Results = await query
                            .Skip((page - 1) * Settings.PageSize)
                            .Take(Settings.PageSize)
                            .Include(x => x.NumberingPoolNetworkElements)
                            .ThenInclude(x => x.Element).ToArrayAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> NeExcel(long neId)
        {
            var ne = repository.NetworkElements.Where(x => x.Id == neId).Include(x => x.Exchange).AsNoTracking();
            //Links
            ne = ne.Include(x => x.LinksOnEnd1)
                .ThenInclude(x => x.End2)
                .ThenInclude(x => x.Exchange);

            //Numberings
            ne = ne.Include(x => x.NumberingPoolNetworkElements)
                .ThenInclude(x => x.Numbering);
            //Childrens with Numbering
            ne = ne
                .Include(x => x.NetworkElements)
                .ThenInclude(x => x.NumberingPoolNetworkElements)
                .ThenInclude(x => x.Numbering)
                .Include(x => x.NetworkElements)
                .ThenInclude(x => x.Exchange);

            var res = await ne.FirstAsync();
            string tempFolder = Path.GetTempPath();
            var fileName = $"{res.Name} {Settings.GetDateTimeNowFile()}.xlsx";
            string path = Path.Combine(tempFolder, fileName);
            Infrastructure.ReportGenerator report = new Infrastructure.ReportGenerator(User.Identity.Name, path);
            report.NE(res);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public async Task<IActionResult> NE(long? neId)
        {
            if (neId.HasValue)
            {
                var ne = repository.NetworkElements.Where(x => x.Id == neId).AsNoTracking();
                //Links
                ne = ne.Include(x => x.LinksOnEnd1)
                    .ThenInclude(x => x.End2)
                    .ThenInclude(x => x.Exchange);

                //Numberings
                ne = ne.Include(x => x.NumberingPoolNetworkElements)
                    .ThenInclude(x => x.Numbering);
                //Childrens with Numbering
                ne = ne
                    .Include(x => x.NetworkElements)
                    .ThenInclude(x => x.NumberingPoolNetworkElements)
                    .ThenInclude(x => x.Numbering)
                    .Include(x => x.NetworkElements)
                    .ThenInclude(x => x.Exchange);

                return View(await ne.FirstAsync());
            }
            return View(null);

        }
        public IActionResult Rms()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Rms(IFormFile file)
        {

            var rmsAnalyzer = new Infrastructure.RmsAnalyzer(file.OpenReadStream(), repository.RmsMappings.ToArray());
            var result = rmsAnalyzer.Analyze();
            string tempFolder = Path.GetTempPath();
            var fileName = $"RMS {Settings.GetDateTimeNowFile()}.csv";
            string path = Path.Combine(tempFolder, fileName);

            return File(System.Text.Encoding.ASCII.GetBytes(result), "text/csv", fileName);

        }
    }
}
