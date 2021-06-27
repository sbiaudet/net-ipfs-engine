using Ipfs.Engine;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Ipfs.Cli
{
    [Command("stats", Description = "Query IPFS statistics")]
    [Subcommand(typeof(StatsBandwidthCommand))]
    [Subcommand(typeof(StatsRepoCommand))]
    [Subcommand(typeof(StatsBitswapCommand))]
    class StatsCommand : CommandBase
    {
        public Program Parent { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult(0);
        }
    }

    [Command("bw", Description = "IPFS bandwidth information")]
    class StatsBandwidthCommand : CommandBase
    {
        StatsCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            var stats = await Program.CoreApi.Stats.BandwidthAsync();
            return Program.Output(app, stats, null);
        }
    }

    [Command("repo", Description = "Repository information")]
    class StatsRepoCommand : CommandBase
    {
        StatsCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            var stats = await Program.CoreApi.Stats.RepositoryAsync();
            return Program.Output(app, stats, null);
        }
    }

    [Command("bitswap", Description = "Bitswap information")]
    class StatsBitswapCommand : CommandBase
    {
        StatsCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            var stats = await Program.CoreApi.Stats.BitswapAsync();
            return Program.Output(app, stats, null);
        }
    }

}
