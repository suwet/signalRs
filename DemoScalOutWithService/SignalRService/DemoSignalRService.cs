using SignalRService.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SignalRService
{
    public partial class DemoSignalRService : ServiceBase
    {
        private HubServer hubserver;
        public DemoSignalRService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                hubserver = new HubServer();
                hubserver.StartSignalRServer();
                hubserver.Start_Table_Dependency();

                WriteLog.WriteWindowsEventLog("Start SignalR hub successfully.");
                WriteLog.WriteWindowsEventLog("Start Sql Dependency successfully.");
            }
            catch (Exception ex)
            {
                WriteLog.WriteWindowsEventLog(ex.Message);
                return;
            }
           
            
        }

        protected override void OnStop()
        {
            if (hubserver != null)
            {
                hubserver.Stop_Table_Dependency();
                WriteLog.WriteWindowsEventLog("stop table Dependency and signalR successfully");
            }

            hubserver = null;
        }
    }
}
