namespace Shared.Events
{
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