using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TableMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// // https://www.youtube.com/watch?v=5uXRrYE5q2s
        /// // http://sqltabledependency.somee.com/test
        /// //https://github.com/christiandelbianco/monitor-table-change-with-sqltabledependency
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.FrmHome());
        }
    }
}
