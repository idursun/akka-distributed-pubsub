using System;
using System.Threading;
using Shared.Events;

namespace Sidecar.ProducerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var sidecar = new Sidecar();
            
            sidecar.Connect();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("sending one");
                sidecar.Publish(Topics.Deployment, new DeploymentEvent
                {
                    Originator = "Sidecar.Producer",
                    SqlInstanceName = "some instance",
                    Timestamp = DateTime.UtcNow
                });
                
                Thread.Sleep(10000);
            }

            Console.WriteLine("shutting down");
            sidecar.ShutDown();
        }
    }
}