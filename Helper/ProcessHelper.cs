using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class ProcessHelper
    {
        public static int GetProcessId()
        {
            return Environment.ProcessId;
        }

        /// <summary>
        /// Kill the current process
        /// </summary>
        public static void KillCurrentProcess()
        {
            try
            {
                int currentProcessId = ProcessHelper.GetProcessId();
                Process processToKill = Process.GetProcessById(currentProcessId);
                processToKill.Kill();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }
    }
}
