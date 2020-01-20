using System.Collections.Generic;

namespace Bl0g.Hook.Communication.Core.Shared
{
    public class FilesGetRequest
    {
        public string OwnerName { get; }
        public string RepositoryName { get; }
        public IEnumerable<FileMetadata> FilesMetadata { get; }

        public FilesGetRequest(
            string ownerName, 
            string repositoryName, 
            IEnumerable<FileMetadata> filesMetadata)
        {
            OwnerName = ownerName;
            RepositoryName = repositoryName;
            FilesMetadata = filesMetadata;
        }
    }
}
