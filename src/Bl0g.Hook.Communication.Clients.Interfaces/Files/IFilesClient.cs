using Bl0g.Hook.Communication.Core.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bl0g.Hook.Communication.Clients.Interfaces.Files
{
    public interface IFilesClient
    {
        Task<IEnumerable<FileContent>> GetContentsAync(FilesGetRequest request);
    }
}
