using System;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Event;
using Shared.Events;

namespace Sidecar
{
    internal class SidecarActor : ReceiveActor
    {
        private ILoggingAdapter Log = Context.GetLogger();
        private readonly Sidecar _sidecar;

        public SidecarActor(Sidecar sidecar)
        {
            _sidecar = sidecar;

            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            Receive<SidecarMessages.PublishToCluster>(m =>
            {
                mediator.Tell(new Publish(m.Topic, new SharedEvent
                {
                    Topic = m.Topic,
                    Payload = m.Payload
                }));
            });
            
            Receive<SidecarMessages.Subscribe>(m =>
            {
                mediator.Tell(new Subscribe(m.Topic, Self));
            });

            Receive<SharedEvent>(o =>
            {
                Log.Debug("Received event: {0}", o.Topic);
                if (_sidecar._mapping.ContainsKey(o.Topic))
                {
                    _sidecar._mapping[o.Topic](o);
                }
            });
        }
    }
    
    internal class SidecarMessages {
        internal class PublishToCluster
        {
            public PublishToCluster(string topic, object payload)
            {
                Topic = topic;
                Payload = payload;
            }

            public string Topic { get; set; }
            public object Payload { get; set; }
        }
        
        internal class Subscribe
        {
            public Subscribe(string topic)
            {
                Topic = topic;
            }

            public string Topic { get; set; } 
        }
    }
}