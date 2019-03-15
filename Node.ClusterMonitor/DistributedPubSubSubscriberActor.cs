using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Event;
using Shared.Events;

namespace Node.ClusterMonitor
{
    public class DistributedPubSubSubscriberActor : ReceiveActor
    {
        private readonly ILoggingAdapter Log = Context.GetLogger();

        public DistributedPubSubSubscriberActor()
        {
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            mediator.Tell(new Subscribe(Topics.Deployment, Self));

            Receive<SubscribeAck>(m =>
            {
                if (m.Subscribe.Topic.Equals(Topics.Deployment) && m.Subscribe.Ref.Equals(Self))
                {
                    Log.Info($"Subscribed to {Topics.Deployment}");
                }
            });

            Receive<SharedEvent>(m =>
            {
                switch (m.Topic)
                {
                    case Topics.Deployment:
                        var payload = m.Payload as DeploymentEvent;
                        Log.Info($"Deployment happened at {payload.Timestamp} {payload.Originator}");
                        break;
                }
            });
        }
    }
}