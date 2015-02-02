using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.Owin.Hosting;
using MinimalOwinWebApiSelfHost.Models;

namespace MinimalOwinWebApiSelfHost
{
    class Program
    {
        static void Main()
        {
            string baseUri = "http://localhost:8080";

            Console.WriteLine("Starting web server..");
            WebApp.Start<Startup>(baseUri);
            Console.WriteLine("Web server listening at {0}", baseUri);
            Console.WriteLine("Press Enter to quit.");
            Console.ReadLine();
        }
    }
}
