using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Owin;
using Owin;

[assembly:OwinStartup(typeof(WinFormHub.Startup))]
namespace WinFormHub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) 
        {
            app.UseCors(CorsOptions.AllowAll);
            /* use default end point /signalr */
            app.MapSignalR();
        }
    }
}
