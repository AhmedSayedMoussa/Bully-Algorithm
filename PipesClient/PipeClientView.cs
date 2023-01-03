using Helper;
using Microsoft.VisualBasic.Logging;
using Models.LogsModels;
using PipesClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Timer = System.Windows.Forms.Timer;

namespace PipeClient
{
    public partial class PipeClientView : Form
    {
        public delegate void NewMessageDelegate(string NewMessage);

        PipesHelper pipesHelper;
        PipeClientManager pipeClientManager;
        int processId;

        public PipeClientView()
        {
            pipesHelper = new();
            pipeClientManager = new();

            InitializeComponent();
            ShowProcessId();
            SetPipeProperties();

            lstBox_messagesLogs.Items.Add("The Process has been started...");

            pipesHelper.PipeMessage += new DelegateMessage(PipesMessageHandler);
            pipesHelper.Listen(pipeName: pipeClientManager.GetPipeName());
            lstBox_messagesLogs.Items.Add("The Process is listening...");
        }

        void SetPipeProperties()
        {
            PipeProperties.Status = PipeStatus.Started;
        }

        private void ShowProcessId()
        {
            processId = ProcessHelper.GetProcessId();
            Lbl_processNumber.Text = processId.ToString();
        }

        private void PipesMessageHandler(string message)
        {
            try
            {
                // eleminate the following segment
                //TODO: eleminate the unneeded chars using regular expression
                message = message.Replace("\0", "");
                if (this.InvokeRequired)
                {
                    this.Invoke(new NewMessageDelegate(PipesMessageHandler), message);
                }
                else
                {
                    DoReceivingMessageWorkFlow(message: message);
                }
            }
            catch (Exception ex)
            {
                lstBox_messagesLogs.Items.Add(ex.Message);
            }
        }

        void DoReceivingMessageWorkFlow(string message)
        {
            PipeProperties.LastMessageReceivedStamp = DateTime.Now;

            var messageLogModel = DeserializeReceivedMessage(message: message);
            int senderProcessId = messageLogModel.SenderProcessId;
            bool isCurrentProcessTheSender = IsCurrentProcessTheSender(senderProcessId);
            LogTransaction(messageLog: messageLogModel, isCurrentProcessTheSender: isCurrentProcessTheSender, messageType: messageLogModel.MessageType);
            
            if (! isCurrentProcessTheSender)
                if (senderProcessId > processId)
                {
                    // I'm not a coordinator anymore and must stop sending 
                    PipeProperties.Status = PipeStatus.Stopped;
                    lstBox_messagesLogs.Items.Add($"I'm no longer a coordinator as there is a process has a higher processId than me which is the following process: {senderProcessId}...");
                }
                else
                {
                    // keep sending
                    PipeProperties.Status = PipeStatus.Sending;
                }
        }

        MessageLog DeserializeReceivedMessage(string message) => pipeClientManager.Deserialize<MessageLog>(message);

        bool IsCurrentProcessTheSender(int senderProcessId) => senderProcessId == processId;

        private void Btn_close_Click(object sender, EventArgs e)
        {
            TerminateCurrentProcess();
        }

        private void PipeClientView_FormClosing(object sender, FormClosingEventArgs e)
        {
            TerminateCurrentProcess();
        }

        void TerminateCurrentProcess()
        {
            LogTransaction(messageType: MessageType.Terminate);
            ProcessHelper.KillCurrentProcess();
            pipesHelper.PipeMessage -= new DelegateMessage(PipesMessageHandler);
        }

        private void PipeClientView_Load(object sender, EventArgs e)
        {
            int reprocessingTheWorkflowEachInSeconds
                = int.Parse(Program.Configuration.GetSection("mySettings:reprocessingTheWorkflowEachInSeconds").Value.ToString());

            Timer MyTimer = new();
            MyTimer.Interval = (reprocessingTheWorkflowEachInSeconds * 1000); // in seconds
            MyTimer.Tick += new EventHandler(DoWorkFlowBasedOnStatus);
            MyTimer.Start();
        }

        private void DoWorkFlowBasedOnStatus(object sender, EventArgs e)
        {
            pipesHelper.Listen(pipeName: pipeClientManager.GetPipeName());

            if (PipeProperties.Status == PipeStatus.Started)
            {
                InitiateElecting();
            }
            else if (PipeProperties.Status == PipeStatus.Stopped)
            {
                if (pipeClientManager.IsTimeHasBeenExceededAndShouldInitiateElection())
                {
                    PipeProperties.Status = PipeStatus.Sending;
                    InitiateElecting();
                }
            }
            else if (PipeProperties.Status == PipeStatus.Sending)
            {
                pipeClientManager.Send(messageType: MessageType.Coordinator);
                LogTransaction(MessageType.Coordinator);
            }

            PipeProperties.LastProcessingStamp = DateTime.Now;
        }

        void InitiateElecting()
        {
            PipeProperties.Status = PipeStatus.Sending;
            pipeClientManager.Send(messageType: MessageType.Election);
        }

        private void LogTransaction(MessageType messageType, bool isCurrentProcessTheSender = true, MessageLog messageLog = null)
        {
            MessageLog messageLogModel;
            if (isCurrentProcessTheSender)
                messageLogModel = new MessageLog(senderProcessId: processId, messageType: messageType);
            else
                messageLogModel = messageLog;

            string log = pipeClientManager.FormatMessageLog(messageLogModel, isCurrentProcessTheSender: isCurrentProcessTheSender);
            lstBox_messagesLogs.Items.Add(log);
        }
    }
}