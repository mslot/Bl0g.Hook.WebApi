using Bl0g.Hook.Communication.Clients.Interfaces.Queue;
using Bl0g.Hook.Communication.Core.Shared.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl0g.Hook.Communication.Clients.Azure.StorageQueue
{
    public class AzureStorageDistributedClient : IDistributedQueueClient
    {
        private readonly QueueOptionsCollection _optionsCollection;
        private readonly IQueueClientCreator _clientCreator;
        private Dictionary<string, IQueueClient> _clients = new Dictionary<string, IQueueClient>();

        public AzureStorageDistributedClient(
            QueueOptionsCollection optionsCollection,
            IQueueClientCreator clientCreator
            )
        {
            _optionsCollection = optionsCollection;
            _clientCreator = clientCreator;
        }

        public void CreateQueueIfNotExists(string queueName)
        {
            var client = GetClient(queueName);
            client.CreateQueueIfNotExists(queueName);
        }

        public async Task CreateQueueIfNotExistsAsync(string queueName)
        {
            var client = GetClient(queueName);
            await client.CreateQueueIfNotExistsAsync(queueName);
        }

        public async Task SendAsync<T>(T obj, string queueName)
        {
            var client = GetClient(queueName);
            await client.SendAsync(obj, queueName);
        }

        public void Initialize()
        {
            foreach(var options in _optionsCollection.Options)
            {
                var client = _clientCreator.Create(
                    options.QueueConnectionOptions.ConnectionString,
                    options.QueueName);

                _clients.Add(
                    options.QueueName, 
                    client);
            }
        }

        private IQueueClient GetClient(string queueName)
        {
            if(!_clients.TryGetValue(queueName, out IQueueClient client))
            {
                client = null; //TODO: Maybe have a good default client here? NULL isnt good, or wait for C# 8.0 which have nullable reference types
            }
            return client;
        }
    }
}
