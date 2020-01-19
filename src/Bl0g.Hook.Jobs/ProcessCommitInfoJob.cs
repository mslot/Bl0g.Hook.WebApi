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
        private readonly IGetFilesWorker<FilesGetRequest> _getFilesWorker;
        private readonly IEnqueueFilesWorker<CommitQueueMessage> _enqueueFilesWorker;

        public ProcessCommitInfoJob(
            IGetFilesWorker<FilesGetRequest> getFilesWorker,
            IEnqueueFilesWorker<CommitQueueMessage> enqueueFilesWorker)
        {
            _getFilesWorker = getFilesWorker;
            _enqueueFilesWorker = enqueueFilesWorker;
        }

        public async Task StartAsync(CommitInfo argument)
        {
            FilesGetRequest request = PrepareRequest(argument);
            IEnumerable<FileContent> files = await _getFilesWorker.GetFilesAsync(request);
            await Enqueue(files);
        }

        private FilesGetRequest PrepareRequest(CommitInfo argument)
        {
            return null;
        }

        private async Task Enqueue(IEnumerable<FileContent> files)
        {
            var commitTasks = new LinkedList<Task>();
            foreach (var file in files)
            {
                var queueMessage = new CommitQueueMessage();
                var enqueueTask = _enqueueFilesWorker.AddFileAsync(queueMessage);
                commitTasks.AddLast(enqueueTask);
            }

            await Task.WhenAll(commitTasks);
        }
    }
}
