using Bl0g.Hook.Communication.Core.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl0g.Hook.Workers.Interfaces.Workers
{
    //TODO: Figure out a better name than "GetFiles"
    public interface IGetFilesWorker<TArgument>
    {
        Task<IEnumerable<FileContent>> GetFilesAsync(TArgument argument);
    }
}
