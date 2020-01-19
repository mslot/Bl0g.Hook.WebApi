using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl0g.Hook.Workers.Interfaces.Workers
{
    public interface IEnqueueFilesWorker<T>
    {
        Task AddFileAsync(T content);
    }
}
