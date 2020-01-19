using Bl0g.Hook.Bootstrappers.Interfaces.Queue;
using Bl0g.Hook.Communication.Clients.Interfaces.Queue;
using Bl0g.Hook.Communication.Core.Shared.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl0g.Hook.Bootstrappers
{
    public class QueueBootstrapper : IQueueBootstrapper
    {
        private readonly IDistributedQueueClient _queueClient;
        private readonly QueueOptionsCollection _queueOptionsCollection;

        public QueueBootstrapper(
            IDistributedQueueClient queueClient,
            QueueOptionsCollection queueOptionsCollection)
        {
            _queueClient = queueClient;
            _queueOptionsCollection = queueOptionsCollection;
        }

        public void Bootstrap()
        {
            _queueClient.Initialize();

            foreach (var queueOptions in _queueOptionsCollection.Options)
            {
                _queueClient.CreateQueueIfNotExists(queueOptions.QueueName);
            }
        }

        public async Task BootstrapAsync()
        {
            var queueCreationList = new LinkedList<Task>();

            _queueClient.Initialize();

            foreach (var queueOptions in _queueOptionsCollection.Options)
            {
                var creation = _queueClient.CreateQueueIfNotExistsAsync(queueOptions.QueueName);
                queueCreationList.AddLast(creation);
            }

            await Task.WhenAll(queueCreationList);
        }
    }
}
