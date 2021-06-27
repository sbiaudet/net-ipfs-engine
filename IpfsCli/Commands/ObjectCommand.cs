using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Ipfs.Engine.UnixFileSystem;
using System.IO;

namespace Ipfs.Cli
{
    [Command("object", Description = "Manage IPFS objects")]
    [Subcommand(typeof(ObjectLinksCommand))]
    [Subcommand(typeof(ObjectGetCommand))]
    [Subcommand(typeof(ObjectDumpCommand))]
    [Subcommand(typeof(ObjectStatCommand))]
    class ObjectCommand : CommandBase
    {
        public Program Parent { get; set; }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult(0);
        }
    }

    [Command("links", Description = "Information on the links pointed to by the IPFS block")]
    class ObjectLinksCommand : CommandBase
    {
        [Argument(0, "cid", "The content ID of the object")]
        [Required]
        public string Cid { get; set; }

        ObjectCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;
            var links = await Program.CoreApi.Object.LinksAsync(Cid);

            return Program.Output(app, links, (data, writer) =>
            {
                foreach (var link in data)
                {
                    writer.WriteLine($"{link.Id.Encode()} {link.Size} {link.Name}");
                }
            });
        }
    }

    [Command("get", Description = "Serialise the DAG node")]
    class ObjectGetCommand : CommandBase
    {
        [Argument(0, "cid", "The content ID of the object")]
        [Required]
        public string Cid { get; set; }

        ObjectCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;
            var node = await Program.CoreApi.Object.GetAsync(Cid);

            return Program.Output(app, node, null);
        }
    }

    [Command("dump", Description = "Dump the DAG node")]
    class ObjectDumpCommand : CommandBase
    {
        [Argument(0, "cid", "The content ID of the object")]
        [Required]
        public string Cid { get; set; }

        ObjectCommand Parent { get; set; }

        class Node
        {
            public DagNode Dag;
            public DataMessage DataMessage;
        }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;
            var node = new Node();
            var block = await Program.CoreApi.Block.GetAsync(Cid);
            node.Dag = new DagNode(block.DataStream);
            node.DataMessage = ProtoBuf.Serializer.Deserialize<DataMessage>(node.Dag.DataStream);

            return Program.Output(app, node, null);
        }
    }

    [Command("stat", Description = "Stats for the DAG node")]
    class ObjectStatCommand : CommandBase
    {
        [Argument(0, "cid", "The content ID of the object")]
        [Required]
        public string Cid { get; set; }

        ObjectCommand Parent { get; set; }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var Program = Parent.Parent;
            var stat = await Program.CoreApi.Object.StatAsync(Cid);

            return Program.Output(app, stat, null);
        }
    }
}
