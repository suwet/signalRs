using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly:OwinStartup(typeof(WinFormHub.Startup))]
namespace WinFormHub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) 
        {
            app.UseCors(CorsOptions.AllowAll);
            //GlobalHost.ConnectionManager.GetHubContext<NotiHub>();
            /* use default end point /signalr */
            app.MapSignalR();
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
            //services.AddSignalR();
            //services.AddScoped<CallingSideClass>();
       // }
    }
}
