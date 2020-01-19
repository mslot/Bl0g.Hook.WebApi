using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bl0g.Hook.Bootstrappers.Interfaces.Queue
{
    public interface IQueueBootstrapper
    {
        Task BootstrapAsync();
        void Bootstrap();
    }
}
