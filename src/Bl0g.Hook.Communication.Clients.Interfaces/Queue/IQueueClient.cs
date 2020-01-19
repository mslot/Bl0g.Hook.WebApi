using System.Threading.Tasks;

namespace Bl0g.Hook.Communication.Clients.Interfaces.Queue
{
    public interface IQueueClient
    {
        Task SendAsync<T>(T obj, string queueName);
        Task CreateQueueIfNotExistsAsync(string queueName);
        void CreateQueueIfNotExists(string queueName);

    }
}
