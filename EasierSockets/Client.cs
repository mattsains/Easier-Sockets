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
    /// Makes client side of socket comms easier.
    /// </summary>
    public class ClientSock
    {
        Socket sock;
        string separator;
        /// <summary>
        /// Connects to a TCP host
        /// </summary>
        /// <param name="host">the server to connect to</param>
        /// <param name="port">the port to use</param>
        /// <param name="separator">what signals the end of a message?</param>
        public ClientSock(string host, int port, string separator = "\n")
        {
            IPHostEntry hostip = Dns.GetHostEntry(host);
            IPAddress ip = hostip.AddressList[0];
            IPEndPoint endpoint = new IPEndPoint(ip, port);
            //TODO: Allow different modes like UDP
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(endpoint);
            this.separator = separator;
        }
        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="message">The message to send. The separator is added automatically</param>
        /// <returns>the response by the server</returns>
        public string send(string message)
        {
            byte[] tx = Encoding.ASCII.GetBytes(message + separator);
            sock.Send(tx);
            byte[] rx = { };
            sock.Receive(rx);
            return Encoding.ASCII.GetString(rx);
        }
        /// <summary>
        /// Destructor: closes the connection
        /// </summary>
        public ~ClientSock()
        {
            sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
    }
}