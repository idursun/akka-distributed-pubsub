using System;

namespace Shared.Events
{
    public class DeploymentEvent
    {
        public DateTime Timestamp { get; set; }
        public string Originator { get; set; }
        public string SqlInstanceName { get; set; }

        public override string ToString()
        {
            return $"{nameof(Timestamp)}: {Timestamp}, {nameof(Originator)}: {Originator}, {nameof(SqlInstanceName)}: {SqlInstanceName}";
        }
    }

    public class SharedEvent
    {
        public string Topic { get; set; }
        public object Payload { get; set; }

        public override string ToString()
        {
            return $"{nameof(Topic)}: {Topic}, {nameof(Payload)}: {Payload}";
        }
    }
}