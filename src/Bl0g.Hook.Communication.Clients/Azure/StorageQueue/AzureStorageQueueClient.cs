using Bl0g.Hook.Communication.Clients.Interfaces.Queue;
using Bl0g.Hook.Communication.Core.Shared.Options;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bl0g.Hook.Communication.Clients.Azure.StorageQueue
{
    public class AzureStorageQueueClient : IQueueClient
    {
        private readonly QueueConnectionOptions _options;
        private CloudStorageAccount _storageAccount;
        private CloudQueueClient _queueClient;

        public AzureStorageQueueClient(QueueConnectionOptions options)
        {
            _options = options;
            _storageAccount = CloudStorageAccount.Parse(_options.ConnectionString);
            _queueClient = _storageAccount.CreateCloudQueueClient();
        }

        public void CreateQueueIfNotExists(string queueName)
        {
            /* TODO: 
             * maybe implement some logic that holds references? I dont know if this do anything 
             * under the hood (external call), if it does then we should keep the references in an internal table and
             * look them up and reuse them before doing the GetQueueReference call, but if it just creates a stub, then this is fine
             */
            var queue = _queueClient.GetQueueReference(queueName);

            queue.CreateIfNotExists();
        }

        public async Task CreateQueueIfNotExistsAsync(string queueName)
        {
            /* TODO: 
             * maybe implement some logic that holds references? I dont know if this do anything 
             * under the hood (external call), if it does then we should keep the references in an internal table and
             * look them up and reuse them before doing the GetQueueReference call, but if it just creates a stub, then this is fine
             */
            var queue = _queueClient.GetQueueReference(queueName);

            await queue.CreateIfNotExistsAsync();
        }

        public async Task SendAsync<T>(T obj, string queueName)
        {
            var queueReference = _queueClient.GetQueueReference(queueName);

            string message = JsonSerializer.Serialize<T>(obj);
            byte[] content = System.Text.Encoding.UTF8.GetBytes(message);

            await queueReference.AddMessageAsync(new CloudQueueMessage(content));
        }
    }
}
