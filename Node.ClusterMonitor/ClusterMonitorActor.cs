using System;
using Akka.Actor;
using Akka.Cluster;
using Akka.Event;

namespace Node.ClusterMonitor
{
    public class ClusterMonitorActor : UntypedActor
    {
        private readonly ILoggingAdapter Log = Context.GetLogger();
        private readonly Cluster Cluster = Cluster.Get(Context.System);

        protected override void PreStart()
        {
            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(3),
                TimeSpan.FromSeconds(10),
                Self, "print", Self
            );
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case string _:
                    Cluster.SendCurrentClusterState(Self);
                    break;

                case ClusterEvent.CurrentClusterState clusterState when clusterState.Leader != null:
                    Log.Info("Leader is {0}", clusterState.Leader.ToString());
                    foreach (var member in clusterState.Members)
                    {
                        Log.Info("Member: {0}", member.ToString());
                    }
                    foreach (var member in clusterState.Unreachable)
                    {
                        Log.Info("Unreachable: {0}", member.ToString());
                    }
                    break;
            }
        }
    }
}