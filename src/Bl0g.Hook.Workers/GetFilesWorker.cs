using Bl0g.Hook.Communication.Clients.Interfaces.Files;
using Bl0g.Hook.Communication.Core.Shared;
using Bl0g.Hook.Workers.Interfaces.Workers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl0g.Hook.Workers
{
    public class GetFilesWorker : IGetFilesWorker<FilesGetRequest>
    {
        private readonly IFilesClient _filesClient;

        public GetFilesWorker(IFilesClient filesClient)
        {
            _filesClient = filesClient;
        }
        public async Task<IEnumerable<FileContent>> GetFilesAsync(FilesGetRequest argument)
        {
            var content = await _filesClient.GetContentsAync(argument);

            return content;
        }
    }
}

