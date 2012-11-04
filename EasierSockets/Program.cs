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
            sock.Listen(disp,"\n\r");
            Console.WriteLine("Now Listening");
        }
        static string handler(string message)
        {
            Console.WriteLine(message);
            return string.Format("You sent: {0}", message);
        }
    }
}
