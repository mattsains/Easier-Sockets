using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EasierSockets
{
    public delegate string Dispatcher(string rx);
    public class Sock
    {
        private string separator = "\n";
        // the delegate to call when a message is received
        // TODO: this might be a performance killer because the threads need to lock this.
        private Dispatcher dispatcher;

        // threads serving clients
        //TODO: use thread pooling for efficiency
        private List<Thread> clientThreads = new List<Thread>();

        // listener thread - routes clients to threads in the list
        private Thread Listener;
        // The physical socket we're using
        private Socket ServerSocket;
        private IPEndPoint remoteEP;
        /// <summary>
        /// starts listening on the port specified
        /// </summary>
        /// <param name="port">The port to listen on</param>
        /// <param name="ip">The bind address. Default is all IPv4</param>
        /// TODO: Allow other types of comms, like UDP, IPv6, etc
        public Sock(int port, string ip = "any")
        {
            IPAddress IP;
            if (ip == "any")
                IP = IPAddress.Any;
            else
                try
                {
                    byte[] octets = new byte[4];
                    string[] exploded = ip.Split('.');
                    for (byte i = 0; i < 4; i++)
                        octets[i] = byte.Parse(exploded[i]);
                    IP = new IPAddress(octets);
                }
                catch (Exception)
                {
                    throw new Exception("IP Address malformed");
                }
            //IP Address should be valid by now, attempt a bind
            remoteEP = new IPEndPoint(IP, port);
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        /// <summary>
        /// Starts listening for connections
        /// When data is sent ending in separator, the Dispatcher delegate is called.
        /// WARNING: the delegate will be called on a new thread, so make this function
        /// thread-safe
        /// </summary>
        /// <param name="dispatch">A delegate accepting a string and sending a string</param>
        /// <param name="separator">what character/string signals the end of a message? eg newline, semicolon</param>
        public void Listen(Dispatcher dispatch, char separator='\n')
        {
            this.Listen(dispatch,separator.ToString());
        }
        public void Listen(Dispatcher dispatch, string separator="\n")
        {
            this.dispatcher = dispatch;
            this.separator=separator;
            Listener = new Thread(new ThreadStart(ListenAsync));
            Listener.Start();
        }
        public void Stop()
        {
            //TODO: need to change this so sockets end cleanly.
            Listener.Abort();
        }
        private void ListenAsync()
        {
            lock (ServerSocket)
            {
                lock (remoteEP)
                {
                    ServerSocket.Bind(remoteEP);
                }
                //start listening forever
                while (true)
                {
                    Socket handle = ServerSocket.Accept();

                }
            }
        }

        private void WaitForClient()
        {

        }
    }
}
