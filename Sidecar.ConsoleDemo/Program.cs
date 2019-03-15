using System;
using Shared.Events;

namespace Sidecar.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var sidecar = new Sidecar();
            sidecar.Connect();
            
            sidecar.Subscribe(Topics.Deployment, m =>
            {
                Console.WriteLine("Received a deployment message: {0}", m.ToString());
            });
            
            Console.ReadLine();
            sidecar.ShutDown();
        }
    }
}