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
            ServerSock sock = new ServerSock(1234);
            Dispatcher disp = new Dispatcher(handler);
            sock.Listen(disp,"\n");
            Console.WriteLine("Now Listening on port 1234");
        }
        static string handler(string message)
        {
            Console.WriteLine(message);
            return string.Format("You sent: {0}", message);
        }
    }
}
