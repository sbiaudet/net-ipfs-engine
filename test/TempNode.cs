﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Opt = Microsoft.Extensions.Options;

namespace Ipfs.Engine
{
    /// <summary>
    ///   Creates a temporary node.
    /// </summary>
    /// <remarks>
    ///   A temporary node has its own repository and listening address.
    ///   When it is disposed, the repository is deleted.
    /// </remarks>
    class TempNode : IpfsEngine
    {
        static int nodeNumber;

        public TempNode()
            : base(Opt.Options.Create(new IpfsEngineOptions()))
        {
            var passPhrase = new SecureString();
            foreach (var c in "xyzzy")
            {
                passPhrase.AppendChar(c);
            }
            Options.Passphrase = passPhrase;

            Options.Repository.Folder = Path.Combine(Path.GetTempPath(), $"ipfs-{nodeNumber++}");
            Options.KeyChain.DefaultKeyType = "ed25519";
            Config.SetAsync(
                "Addresses.Swarm",
                JToken.FromObject(new string[] { "/ip4/0.0.0.0/tcp/0" })
            ).Wait();
        }

        //xyzzy
        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Directory.Exists(Options.Repository.Folder))
            {
                Directory.Delete(Options.Repository.Folder, true);
            }
        }
    }
}
