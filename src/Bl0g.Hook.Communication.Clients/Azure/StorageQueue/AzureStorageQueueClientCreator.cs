using Bl0g.Hook.Communication.Clients.Interfaces.Queue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bl0g.Hook.Communication.Clients.Azure.StorageQueue
{
    public class AzureStorageQueueClientCreator : IQueueClientCreator
    {

        public IQueueClient Create(string connectionString, string queueName)
        {
            return new AzureStorageQueueClient(
                new Core.Shared.Options.QueueConnectionOptions 
                { 
                    ConnectionString = connectionString 
                });
        }
    }
}
