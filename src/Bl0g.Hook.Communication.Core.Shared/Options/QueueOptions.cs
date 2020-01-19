namespace Bl0g.Hook.Communication.Core.Shared.Options
{
    public class QueueOptions
    {
        public QueueConnectionOptions QueueConnectionOptions {get;set;}
        public string QueueName { get; set; }
        public QueueType QueueType { get; set; }
    }
}
