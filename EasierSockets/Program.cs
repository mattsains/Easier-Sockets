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
            Sock sock = new Sock(1234);
            Dispatcher disp = new Dispatcher(handler);
            sock.Listen(disp);
            Console.ReadLine();
        }
        static string handler(string message)
        {
            return string.Format("You sent: {0}", message);
        }
    }
}
