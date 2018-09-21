using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sarona.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        SaronaRepository repository;
        public HomeController(SaronaRepository repo) => repository = repo;
        public IActionResult Index()
        {
            var list = new List<NetworkElement>();
            for (int i = 0; i < 1000; i++)
            {
                var ne = new NetworkElement()
                {
                    ExchangeId = 1,
                    InstalledCapacity = 1000,
                    Manufacturer = "fwefw" + i,
                    Model = "fwvfb" + i,
                    Name = "CoreTest" + i,
                    NetworkType = NeType.Core,
                    //ParentId = 2,
                    Remark = "sepehr",
                    Type = "type",
                    Owner = "owner",
                    Username = "username",
                    UsedCapacity = 0,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now


                };
                list.Add(ne);
            }

            for (int i = 0; i < 1000; i++)
            {
                var ne = new NetworkElement()
                {
                    ExchangeId = 1,
                    InstalledCapacity = 1000,
                    Manufacturer = "fwefw" + i,
                    Model = "fwvfb" + i,
                    Name = "RemoteTest" + i,
                    NetworkType = NeType.Remote,
                    ParentId = 2,
                    Remark = "sepehr",
                    Type = "type",
                    Owner = "owner",
                    Username = "username",
                    UsedCapacity = 0,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now


                };
                list.Add(ne);
            }

            for (int i = 0; i < 1000; i++)
            {
                var ne = new NetworkElement()
                {
                    ExchangeId = 1,
                    InstalledCapacity = 1000,
                    Manufacturer = "fwefw" + i,
                    Model = "fwvfb" + i,
                    Name = "AccessTest" + i,
                    NetworkType = NeType.Access,
                    ParentId = 2,
                    Remark = "sepehr",
                    Type = "type",
                    Owner = "owner",
                    Username = "username",
                    UsedCapacity = 0,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now


                };
                list.Add(ne);
            }

            for (int i = 0; i < 1000; i++)
            {
                var ne = new NetworkElement()
                {
                    ExchangeId = 1,
                    InstalledCapacity = 1000,
                    Manufacturer = "fwefw" + i,
                    Model = "fwvfb" + i,
                    Name = "pbxTest" + i,
                    NetworkType = NeType.PBX,
                    //ParentId = 2,
                    Remark = "sepehr",
                    Type = "type",
                    Owner = "owner",
                    Username = "username",
                    UsedCapacity = 0,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now


                };
                list.Add(ne);
            }

            for (int i = 0; i < 1000; i++)
            {
                var ne = new NetworkElement()
                {
                    ExchangeId = 1,
                    InstalledCapacity = 1000,
                    Manufacturer = "fwefw" + i,
                    Model = "fwvfb" + i,
                    Name = "ippbxTest" + i,
                    NetworkType = NeType.IP_PBX,
                    //ParentId = 2,
                    Remark = "sepehr",
                    Type = "type",
                    Owner = "owner",
                    Username = "username",
                    UsedCapacity = 0,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };
                list.Add(ne);
            }

            //repository.AddRangeNetworkElement(list);


            return View();
        }
    }
}
