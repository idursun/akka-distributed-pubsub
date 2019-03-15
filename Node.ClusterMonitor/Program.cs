using Akka.Actor;
using Akka.Configuration;

namespace Node.ClusterMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(@"
akka {
   actor.provider = cluster
   extensions = [""Akka.Cluster.Tools.PublishSubscribe.DistributedPubSubExtensionProvider,Akka.Cluster.Tools""]
   remote {
       dot-netty.tcp {
           port = 0
           hostname = 0.0.0.0
       }
   }
   cluster {
       seed-nodes = [""akka.tcp://ClusterSystem@0.0.0.0:8081""]
   }
}");

            var actorSystem = ActorSystem.Create("ClusterSystem", config);
            actorSystem.ActorOf<ClusterMonitorActor>();
            actorSystem.ActorOf<DistributedPubSubSubscriberActor>();
            actorSystem.WhenTerminated.Wait();
        }
    }
}