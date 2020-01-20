using Bl0g.Hook.Communication.Core.Shared;
using Bl0g.Hook.Core.GithubHook;
using Bl0g.Hook.Jobs.Interfaces;
using Bl0g.Hook.Workers.Core.Shared;
using Bl0g.Hook.Workers.Interfaces.Workers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bl0g.Hook.Jobs
{
    public class ProcessCommitInfoJob : IProcessJob<CommitInfo>
    {
        private readonly IEnqueueFilesWorker<CommitQueueMessage> _enqueueFilesWorker;

        public ProcessCommitInfoJob(
            IEnqueueFilesWorker<CommitQueueMessage> enqueueFilesWorker)
        {
            _enqueueFilesWorker = enqueueFilesWorker;
        }

        public async Task StartAsync(CommitInfo argument)
        {
            FilesGetRequest request = PrepareRequest(argument);
            await Enqueue(request, argument);
        }

        private FilesGetRequest PrepareRequest(CommitInfo argument)
        {
            string ownerName = argument.Repository.Owner.Name;
            string repositoryName = argument.Repository.Name;
            List<FileMetadata> metadata = new List<FileMetadata>();

            List<string> names = new List<string>();
            foreach (var commit in argument.Commits)
            {
                names.AddRange(commit.Added);
                names.AddRange(commit.Removed);
                names.AddRange(commit.Modified);
            }

            foreach (var name in names)
            {
                FileMetadata fileMetadata = new FileMetadata(name);
                metadata.Add(fileMetadata);
            }

            FilesGetRequest request = new FilesGetRequest(ownerName, repositoryName, metadata);


            return request;
        }

        private async Task Enqueue(FilesGetRequest request, CommitInfo commitInfo)
        {
            var queueMessage = new CommitQueueMessage { Body=request };
            await _enqueueFilesWorker.AddFileAsync(queueMessage);
        }
    }
}
