using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HubServer
{
    public class WriteLogThreadSafe
    {
        public async static Task WriteToFileThreadSafe(string text, string path)
        {
            try
            {
                // Append text to the file
                using (StreamWriter sw = File.AppendText(path))
                {
                    var tw = StreamWriter.Synchronized(sw);
                    await tw.WriteLineAsync(text);
                    tw.Close();
                }
            }
            catch(Exception exe)
            {
                throw;
            }      
        }
    }
}
