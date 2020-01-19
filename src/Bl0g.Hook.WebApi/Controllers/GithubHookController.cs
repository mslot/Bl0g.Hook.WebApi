using System.Threading.Tasks;
using Bl0g.Hook.Core.GithubHook;
using Bl0g.Hook.Jobs.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bl0g.Hook.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubHookController : ControllerBase
    {
        private readonly IProcessJob<CommitInfo> _processJob;

        public GithubHookController(IProcessJob<CommitInfo> processJob)
        {
            _processJob = processJob;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CommitInfo commitInfo)
        {
            await _processJob.StartAsync(commitInfo);
            return Ok();
        }
    }
}