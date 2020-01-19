using System.Threading.Tasks;

namespace Bl0g.Hook.Communication.Clients.Interfaces.Queue
{
    public interface IDistributedQueueClient : IQueueClient
    {
        void Initialize();
    }
}
