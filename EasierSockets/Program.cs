using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasierSockets
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerSock sock = new ServerSock("any", 1234, "\n", new ClientStateChange(ClientConnection), new ClientRequest(ClientReceive));
            Console.WriteLine("Now Listening on port 1234");
        }
        static void ClientConnection(int id, bool state)
        {
            //this is called when a client connects or disconnects
            Console.WriteLine("Client {0} has {1}", id, state ? "connected" : "disconnected");
        }
        static string ClientReceive(int id, string msg)
        {
            Console.WriteLine("Client {0} says: {1}", id, msg);
            return string.Format("You said: {0}", msg);
        }
    }
}
