using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.LogsModels
{
    public class MessageLog
    {
        public MessageLog(int senderProcessId, MessageType messageType)
        {
            SenderProcessId = senderProcessId;
            MessageType = messageType;
        }

        public int SenderProcessId { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        Election = 1,
        Coordinator = 2,
        Terminate = 3
    }
}