using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasierSockets
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("host: ");
            string server=Console.ReadLine();
            Console.Write("port: ");
            ClientSock cs = new ClientSock(server, int.Parse(Console.ReadLine()), "\n", new ServerDisconnect(disconn), new ServerMessage(msg));
            while (true)
            {
                Console.Write(">> ");
                cs.send(Console.ReadLine());
            }
        }
        public static void disconn()
        {
            Environment.Exit(0);
        }
        public static void msg(string m)
        {
            Console.WriteLine("<< {0}", m);
        }
    }
}
