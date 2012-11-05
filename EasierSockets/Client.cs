using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EasierSockets
{
    /// <summary>
    /// Called when the server sends a message
    /// </summary>
    /// <param name="msg">the message the server sent</param>
    public delegate void ServerMessage(string msg);
    /// <summary>
    /// Called when the connection is closed by the server
    /// </summary>
    public delegate void ServerDisconnect();
    /// <summary>
    /// Makes client side of socket comms easier.
    /// </summary>
    public class ClientSock
    {
        private Socket sock;
        private string separator;
        private Thread t;
        private ServerDisconnect serverDisconnect;
        private ServerMessage serverMessage;
        /// <summary>
        /// Connects to a TCP host
        /// </summary>
        /// <param name="host">the server to connect to</param>
        /// <param name="port">the port to use</param>
        /// <param name="separator">what signals the end of a message?</param>
        public ClientSock(string host, int port, string separator, ServerDisconnect discon, ServerMessage mess)
        {
            IPHostEntry hostip = Dns.GetHostEntry(host);
            IPAddress ip = hostip.AddressList[0];
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            //TODO: Allow different modes like UDP
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.separator = separator;
            this.serverDisconnect = discon;
            this.serverMessage = mess;
            sock.Connect(endpoint);
            t = new Thread(new ThreadStart(WaitForServer));
            t.Start();
        }
        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="message">The message to send. The separator is added automatically</param>
        /// <returns>Whether the send was successful (also calls delegate if unsuccessful</returns>
        public bool Send(string message)
        {
            byte[] tx = Encoding.ASCII.GetBytes(message + separator);
            try
            {
                sock.Send(tx);
            }
            catch (SocketException) { serverDisconnect(); return false; }
            return true;
        }
        /// <summary>
        /// endless function to receive messages from server
        /// </summary>
        private void WaitForServer()
        {
            string data = "";
            while (sock.Connected)
            {
                byte[] rx = new byte[1024];
                int bytesRec = 0;
                try
                {
                    bytesRec = sock.Receive(rx);
                }
                catch (SocketException)
                {
                    // let the user know the server has gone.
                    serverDisconnect();
                    return;
                }
                if (bytesRec == 0)
                {
                    serverDisconnect();
                    return;
                }
                data += Encoding.ASCII.GetString(rx, 0, bytesRec);
                if (data.Contains(separator))
                {
                    string[] messages = data.Split(new string[] { separator }, StringSplitOptions.None);
                    for (int i = 0; i < messages.Length - 1; i++)
                        serverMessage(messages[i]);
                    data = messages[messages.Length - 1];
                }
            }
            //inform the user that the server is gone
            serverDisconnect();
        }
        /// <summary>
        /// Destructor: closes the connection
        /// </summary>
        ~ClientSock()
        {
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
    }
}