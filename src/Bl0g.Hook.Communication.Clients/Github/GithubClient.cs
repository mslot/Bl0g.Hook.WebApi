using Bl0g.Hook.Communication.Clients.Interfaces.Files;
using Bl0g.Hook.Communication.Core.Shared;
using Bl0g.Hook.Communication.Options;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl0g.Hook.Communication.Clients.Github
{
    public class GithubClient : IFilesClient
    {
        private readonly GithubConnectionOptions _connectionOptions;

        public GithubClient(GithubConnectionOptions connectionOptions)
        {
            _connectionOptions = connectionOptions;
        }

        public async Task<IEnumerable<FileContent>> GetContentsAync(FilesGetRequest request)
        {
            var files = new LinkedList<FileContent>();

            GitHubClient client = new GitHubClient(new ProductHeaderValue(_connectionOptions.ProductName));
            client.Credentials = new Credentials(_connectionOptions.Username, _connectionOptions.Password);

            /*
             * TODO: This could be broken up, so all retrievals was done at once
             *       and awaited afterwards
             */
            foreach (var fileMetadata in request.FilesMetadata)
            {
                var content = await client.Repository.Content.GetAllContents(
                    request.OwnerName,
                    request.RepositoryName,
                    fileMetadata.Name);

                foreach (var contentRepo in content)
                {
                    //TODO: this new, should it be moved out? Or is it ok that the client is responsible for it?
                    files.AddLast(new FileContent(contentRepo.Name, contentRepo.Content));
                }
            }

            return files;
        }
    }
}
