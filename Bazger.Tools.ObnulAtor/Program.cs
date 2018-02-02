using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bazger.Tools.ObnulAtor
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //TODO: Add close processes of AV
            //TODO: Add loading from licence
            //TODO: Add logging
            //CloseProcesses();

            var krt = new KasperskyResetTrialVersion5("KRT", "Kaspersky_Reset_Trial_5.1.0.29");
            if (!krt.Open())
            {
                return;
            }
            krt.ResetActivation();
        }

        private static void CloseProcesses()
        {
            var processes = Process.GetProcessesByName("avpui");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }
    }
}
