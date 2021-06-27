using Ipfs.Engine;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Ipfs.Cli
{
    [Command("repo", Description = "Manage the IPFS repository")]
    [Subcommand(typeof(RepoGCCommand))]
    [Subcommand(typeof(RepoMigrateCommand))]
    [Subcommand(typeof(RepoStatCommand))]
    [Subcommand(typeof(RepoVerifyCommand))]
    [Subcommand(typeof(RepoVersionCommand))]
    class RepoCommand : CommandBase
    {
        public Program Parent { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult(0);
        }
    }

    [Command("gc", Description = "Perform a garbage collection sweep on the repo")]
    class RepoGCCommand : CommandBase
    {
        RepoCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            await Program.CoreApi.BlockRepository.RemoveGarbageAsync();
            return 0;
        }
    }

    [Command("verify", Description = "Verify all blocks in repo are not corrupted")]
    class RepoVerifyCommand : CommandBase
    {
        RepoCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            await Program.CoreApi.BlockRepository.VerifyAsync();
            return 0;
        }
    }

    [Command("stat", Description = "Repository information")]
    class RepoStatCommand : CommandBase
    {
        RepoCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            var stats = await Program.CoreApi.BlockRepository.StatisticsAsync();
            return Program.Output(app, stats, null);
        }
    }

    [Command("version", Description = "Repository version")]
    class RepoVersionCommand : CommandBase
    {
        RepoCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;

            var stats = await Program.CoreApi.BlockRepository.VersionAsync();
            return Program.Output(app, stats, null);
        }
    }

    [Command("migrate", Description = "Migrate to the version")]
    class RepoMigrateCommand : CommandBase
    {
        RepoCommand Parent { get; set; }

        [Argument(0, "version", "The version number of the repository")]
        [Required]
        public int Version { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            // TODO: Add option --pass
            string passphrase = "this is not a secure pass phrase";
            var ipfs = IpfsEngine.Create(passphrase.ToCharArray());

            await ipfs.MigrationManager.MirgrateToVersionAsync(Version);
            return 0;
        }
    }

}
