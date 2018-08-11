using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sarona.Models;

namespace Sarona
{

    public class Startup
    {
        public Startup(IConfiguration config) => Configuration = config;
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            string conString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<SaronaContext>(options => options.UseSqlServer(conString));
            services.AddTransient<SaronaRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(rt =>
            {
                rt.MapRoute(
                    name: "",
                    template: "Network/{district}/{action}",
                    defaults: new { controller = "Network", action = "District" });
                rt.MapRoute(
                    name: "",
                    template: "Network/{district}",
                    defaults: new { controller = "Network", action = "District" });
                rt.MapRoute(
                    name: "Network",
                    template: "Network/",
                    defaults: new { controller = "Network", action = "District" });
                
                
                rt.MapRoute(null, "{controller=Home}/{action=Index}");
            });
        }
    }
}
