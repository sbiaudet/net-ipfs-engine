using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ipfs.Cli
{
    [Command("swarm", Description = "Manage connections to the p2p network")]
    [Subcommand(typeof(SwarmConnectCommand))]
    [Subcommand(typeof(SwarmDisconnectCommand))]
    [Subcommand(typeof(SwarmPeersCommand))]
    [Subcommand(typeof(SwarmAddrsCommand))]
    class SwarmCommand : CommandBase
    {
        public Program Parent { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult(0);
        }
    }

    [Command("connect", Description = "Connect to a peer")]
    class SwarmConnectCommand : CommandBase
    {
        [Argument(0, "addr", "A multiaddress to the peer")]
        [Required]
        public string Address { get; set; }

        public SwarmCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;
            await Program.CoreApi.Swarm.ConnectAsync(Address);
            return 0;
        }
    }

    [Command("disconnect", Description = "Disconnect from a peer")]
    class SwarmDisconnectCommand : CommandBase
    {
        [Argument(0, "addr", "A multiaddress to the peer")]
        [Required]
        public string Address { get; set; }

        public SwarmCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;
            await Program.CoreApi.Swarm.DisconnectAsync(Address);
            return 0;
        }
    }

    [Command("peers", Description = "List of connected peers")]
    class SwarmPeersCommand : CommandBase
    {
        public SwarmCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;
            var peers = await Program.CoreApi.Swarm.PeersAsync();
            Program.Output(app, peers, (data, writer) =>
            {
                foreach (var peer in data)
                {
                    writer.WriteLine(peer.ConnectedAddress);
                }
            });
            return 0;
        }
    }

    [Command("addrs", Description = "List addresses of known peers")]
    class SwarmAddrsCommand : CommandBase
    {
        public SwarmCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;
            var peers = await Program.CoreApi.Swarm.AddressesAsync();
            Program.Output(app, peers, (data, writer) =>
            {
                foreach (var address in data.SelectMany(p => p.Addresses))
                {
                    writer.WriteLine(address);
                }
            });
            return 0;
        }
    }

}
