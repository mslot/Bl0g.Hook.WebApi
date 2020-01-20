using Bl0g.Hook.Bootstrappers;
using Bl0g.Hook.Bootstrappers.Interfaces.Queue;
using Bl0g.Hook.Communication.Clients.Azure.StorageQueue;
using Bl0g.Hook.Communication.Clients.Github;
using Bl0g.Hook.Communication.Clients.Interfaces.Files;
using Bl0g.Hook.Communication.Clients.Interfaces.Queue;
using Bl0g.Hook.Communication.Core.Shared;
using Bl0g.Hook.Communication.Core.Shared.Options;
using Bl0g.Hook.Communication.Options;
using Bl0g.Hook.Core.GithubHook;
using Bl0g.Hook.Jobs;
using Bl0g.Hook.Jobs.Interfaces;
using Bl0g.Hook.WebApi.Middleware.Options;
using Bl0g.Hook.Workers;
using Bl0g.Hook.Workers.Core.Shared;
using Bl0g.Hook.Workers.Core.Shared.Options;
using Bl0g.Hook.Workers.Interfaces.Workers;
using Bl0g.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Bl0g.Hook.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var githubConnectionOptions = new GithubConnectionOptions();
            var queueOptionsCollection = new QueueOptionsCollection();

            var githubConnectionConfiguration = Configuration.GetSection("GithubConnectionOptions");
            var queueOptionsCollectionConfiguration = Configuration.GetSection("QueueOptionsCollection");

            githubConnectionConfiguration.Bind(githubConnectionOptions);
            queueOptionsCollectionConfiguration.Bind(queueOptionsCollection);

            services.AddSingleton(githubConnectionOptions);
            services.AddSingleton(queueOptionsCollection);
            services.AddSingleton((p) =>
                                        {
                                            CommitQueueWorkerOptions workerOptions = new CommitQueueWorkerOptions();
                                            var optionsCollection = p.GetRequiredService<QueueOptionsCollection>();
                                            var commitQueueOptions = optionsCollection.Options.FirstOrDefault(_ => _.QueueType == QueueType.CommitQueue);

                                            if (commitQueueOptions != null)
                                            {
                                                workerOptions.QueueName = commitQueueOptions.QueueName;
                                            }

                                            return workerOptions;
                                        });

            services.Configure<HMACRequestValidatorOptions>(Configuration.GetSection("HMACRequestValidatorOptions"));

            services.AddSingleton<IQueueBootstrapper, QueueBootstrapper>();
            services.AddSingleton<IQueueClientCreator, AzureStorageQueueClientCreator>();
            services.AddSingleton<IDistributedQueueClient, AzureStorageDistributedClient>();

            services.AddScoped<IProcessJob<CommitInfo>, ProcessCommitInfoJob>();
            services.AddScoped<IEnqueueFilesWorker<CommitQueueMessage>, CommitQueueWorker>();

            services.AddControllers();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IOptions<HMACRequestValidatorOptions> options,
            IQueueBootstrapper queueBootstrapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHMACRequestValidator(options.Value);
            queueBootstrapper.Bootstrap();

            if (!env.IsEnvironment("Local"))
                app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
