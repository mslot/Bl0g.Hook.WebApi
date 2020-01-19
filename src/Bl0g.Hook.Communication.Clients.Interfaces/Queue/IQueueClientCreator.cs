using System;
using System.Collections.Generic;
using System.Text;

namespace Bl0g.Hook.Communication.Clients.Interfaces.Queue
{
    public interface IQueueClientCreator
    {
        IQueueClient Create(string connectionString, string queueName);
    }
}
