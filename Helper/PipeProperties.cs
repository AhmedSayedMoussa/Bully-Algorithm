using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class PipeProperties
    {
        public static PipeStatus Status { get; set; }
        public static DateTime LastMessageReceivedStamp { get; set; }
        public static DateTime LastProcessingStamp { get; set; }
    }

    public enum PipeStatus
    {
        Started = 1,
        Sending = 2,
        Stopped = 3
    }
}
