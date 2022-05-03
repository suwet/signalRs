using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoDxGridControl.Helper
{
    public class WriteLogThreadSafe
    {
        private static bool enable_log = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["enable_log"]);
        public async static Task WriteToFile(string text, string path)
        {
            if (enable_log)
            {
                // Append text to the file
                using (StreamWriter sw = File.AppendText(path))
                {
                    var tw = StreamWriter.Synchronized(sw);
                    await tw.WriteLineAsync(text);
                    tw.Close();
                }
            }
        }
    }
}
