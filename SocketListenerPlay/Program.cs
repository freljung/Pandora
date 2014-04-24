using System;
using System.Collections.Generic;
using System.Linq;
using Pandora;

namespace SocketListenerPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Setup and use StructureMap
            var server = new WebServer();
            server.Setup();

            server.Start();
            Console.WriteLine("Web server started. Press any key to stop.");
            Console.ReadKey();

            Console.WriteLine("Web server stopped");
            server.Stop();

            Console.WriteLine("Good bye!");
        }
    }
}