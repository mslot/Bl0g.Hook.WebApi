using Bl0g.Hook.Communication.Clients.Interfaces.Queue;
using Bl0g.Hook.Workers.Core.Shared;
using Bl0g.Hook.Workers.Core.Shared.Options;
using Bl0g.Hook.Workers.Interfaces.Workers;
using System.Threading.Tasks;

namespace Bl0g.Hook.Workers
{
    public class CommitQueueWorker : IEnqueueFilesWorker<CommitQueueMessage>
    {
        private readonly IDistributedQueueClient _queueClient;
        private readonly CommitQueueWorkerOptions _options;

        public CommitQueueWorker(
            IDistributedQueueClient queueClient,
            CommitQueueWorkerOptions options)
        {
            _queueClient = queueClient;
            _options = options;
        }
        public async Task AddFileAsync(CommitQueueMessage content)
        {
            await _queueClient.SendAsync(content, _options.QueueName);
        }
    }
}
