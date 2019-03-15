using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Configuration;
using Shared.Events;

namespace Sidecar
{
    public class Sidecar
    {
        private ActorSystem _actorSystem;
        private IActorRef _sidecarActor;
        internal IDictionary<string, Action<SharedEvent>> _mapping;

        public Sidecar()
        {
            _mapping = new Dictionary<string, Action<SharedEvent>>();
        }

        public void Connect()
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
            _actorSystem = ActorSystem.Create("ClusterSystem", config);
            _sidecarActor = _actorSystem.ActorOf(Props.Create(() => new SidecarActor(this)));
        }

        public void ShutDown()
        {
            _actorSystem.Terminate().Wait();
        }

        public void Subscribe(string topic, Action<SharedEvent> callback)
        {
           _sidecarActor.Tell(new SidecarMessages.Subscribe(topic));
           _mapping[topic] = callback;
        }

        public void Publish(string topic, object payload)
        {
            _sidecarActor.Tell(new SidecarMessages.PublishToCluster(topic, payload));  
        }
    }
}