using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SignalRService.Helper
{
    public class WriteLog
    {

        public static void WriteWindowsEventLog(string logMessage)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry(logMessage, EventLogEntryType.Information);
            }
        }

        public static void WriteWindowsEventLog(string logMessage,string source)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = source;
                eventLog.WriteEntry(logMessage, EventLogEntryType.Information);
            }
        }

        public static void WriteLogToFile(string logPath,string strLog)
        {
            string logDate = DateTime.Now.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
            using (StreamWriter file = new StreamWriter(logPath + @"\InterfaceToLine_" + logDate + ".txt", true))
            {
                var writer = StreamWriter.Synchronized(file);
                writer.WriteLine(DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss") + " : " + strLog);
                writer.Close();
            }
           
        }
    }
}
