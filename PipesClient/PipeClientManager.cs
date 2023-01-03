using Helper;
using Models.LogsModels;
using PipeClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipesClient
{
    public class PipeClientManager
    {
        public void Send(MessageType messageType)
        {
            string pipeName = GetPipeName();
            int processId = ProcessHelper.GetProcessId();
            
            PipesHelper pipesHelper = new();
            MessageLog message = new MessageLog(senderProcessId: processId, messageType: messageType);
            var serializedMessage = JsonSerializer.Serialize(message);

            pipesHelper.Send(pipeName: pipeName, message: serializedMessage);
        }

        public string GetInitiatingElectionMessage()
        {
            string message = Program.Configuration.GetSection("mySettings:initiatingElectionMessage").Value.ToString();
            string timeIntervalText = Program.Configuration.GetSection("mySettings:timeIntervalText").Value.ToString();

            string messageTextToBeShown = string.Format(message, timeIntervalText);
            return messageTextToBeShown;
        }

        public Entity Deserialize<Entity>(string message)
        {
            return JsonSerializer.Deserialize<Entity>(message);
        }

        public string GetPipeName()
        {
            return Program.Configuration.GetSection("mySettings:pipeName").Value.ToString();
        }

        public string FormatMessageLog(MessageLog messageLog, bool isCurrentProcessTheSender)
        {
            string message = String.Empty;

            switch (messageLog.MessageType)
            {
                case MessageType.Election:
                    {
                        message = isCurrentProcessTheSender
                            ? Program.Configuration.GetSection("mySettings:currentProcessElectionLogText").Value.ToString()
                            : Program.Configuration.GetSection("mySettings:initiatingElectionMessage").Value.ToString();
                        break;
                    }
                case MessageType.Coordinator:
                    {
                        message = isCurrentProcessTheSender
                            ? Program.Configuration.GetSection("mySettings:currentProcessCoordinatorLogText").Value.ToString()
                            : Program.Configuration.GetSection("mySettings:coordinatorLogText").Value.ToString();
                        break;
                    }
                case MessageType.Terminate:
                    {
                        message = Program.Configuration.GetSection("mySettings:currentProcessTerminationText").Value.ToString();
                        break;
                    }
            }

            return string.Format(message, messageLog.SenderProcessId, DateTime.Now);
        }

        /// <summary>
        /// To check if the time is gone and needs to initiate an election or not
        /// </summary>
        /// <returns>decision</returns>
        public bool IsTimeHasBeenExceededAndShouldInitiateElection()
        {
            double intervalToDecideToInitiateElectionInSeconds 
                = double.Parse(Program.Configuration.GetSection("mySettings:intervalToDecideToInitiateElectionInSeconds").Value.ToString());

            var deltaStamp = DateTime.Now.Subtract(PipeProperties.LastMessageReceivedStamp);

            return deltaStamp.TotalSeconds > intervalToDecideToInitiateElectionInSeconds;
        }
    }
}
