using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{

    // Delegate for passing received message back to caller
    public delegate void DelegateMessage(string Reply);

    public class PipesHelper
    {
        /// <summary>
        /// Send Message To Specific Pipe
        /// </summary>
        /// <param name="pipeName"></param>
        /// <param name="message"></param>
        /// <param name="TimeOut">maximum waiting time for pipe to become available (in ms)</param>
        public void Send(string pipeName, string message, int TimeOut = 10000)
        {
            try
            {
                NamedPipeClientStream pipeStream = new NamedPipeClientStream(".", pipeName, PipeDirection.Out, PipeOptions.Asynchronous);
                pipeStream.Connect(TimeOut);
                Debug.WriteLine("pipe connection established");

                byte[] buffer = Encoding.UTF8.GetBytes(message);
                pipeStream.BeginWrite(buffer, 0, buffer.Length, AsyncSend, pipeStream);
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);
            }
        }

        private void AsyncSend(IAsyncResult iar)
        {
            try
            {
                // Get the pipe
                NamedPipeClientStream pipeStream = (NamedPipeClientStream)iar.AsyncState;

                // End the write
                pipeStream.EndWrite(iar);
                pipeStream.Flush();
                pipeStream.Close();
                pipeStream.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public event DelegateMessage PipeMessage;
        string PipeName;

        public void Listen(string pipeName)
        {
            try
            {
                PipeName = pipeName;
                NamedPipeServerStream pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.In, 200, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                pipeServer.BeginWaitForConnection(new AsyncCallback(WaitForConnectionCallBack), pipeServer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void WaitForConnectionCallBack(IAsyncResult iar)
        {
            try
            {
                NamedPipeServerStream pipeServer = (NamedPipeServerStream)iar.AsyncState;
                pipeServer.EndWaitForConnection(iar);

                byte[] buffer = new byte[255];

                // Read the incoming message
                pipeServer.Read(buffer, 0, 255);

                // Convert byte buffer to string
                string stringData = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Debug.WriteLine(stringData + Environment.NewLine);

                // Pass message back to calling form
                PipeMessage.Invoke(stringData);

                // Kill original sever and create new wait server
                pipeServer.Close();
                pipeServer = null;
                pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.In, 200, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

                // Recursively wait for the connection again and again....
                pipeServer.BeginWaitForConnection(new AsyncCallback(WaitForConnectionCallBack), pipeServer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}