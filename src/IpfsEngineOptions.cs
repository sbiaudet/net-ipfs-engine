using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Ipfs.Engine.Cryptography;
using Makaretu.Dns;

namespace Ipfs.Engine
{
    /// <summary>
    ///   Configuration options for the <see cref="IpfsEngine"/>.
    /// </summary>
    /// <seealso cref="IpfsEngine.Options"/>
    public class IpfsEngineOptions : IDisposable
    {
        /// <summary>
        ///   Repository options.
        /// </summary>
        public RepositoryOptions Repository { get; set; } = new RepositoryOptions();

        /// <summary>
        ///   KeyChain options.
        /// </summary>
        public KeyChainOptions KeyChain { get; set; } = new KeyChainOptions();

        /// <summary>
        ///   Provides access to the Domain Name System.
        /// </summary>
        /// <value>
        ///   Defaults to <see cref="DotClient"/>, DNS over TLS.
        /// </value>
        public IDnsClient Dns { get; set; } = new DotClient();

        /// <summary>
        ///   Block options.
        /// </summary>
        public BlockOptions Block { get; set; } = new BlockOptions();

        /// <summary>
        ///    Discovery options.
        /// </summary>
        public DiscoveryOptions Discovery { get; set; } = new DiscoveryOptions();

        /// <summary>
        ///   Swarm (network) options.
        /// </summary>
        public SwarmOptions Swarm { get; set; } = new SwarmOptions();

        /// <summary>
        ///   The password used to access the keychain.
        /// </summary>
        public SecureString Passphrase { get; set; }

        #region IDisposable Support
        bool disposedValue = false; // To detect redundant calls

        /// <summary>
        ///  Releases the unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">
        ///   <b>true</b> to release both managed and unmanaged resources; <b>false</b> 
        ///   to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    Passphrase?.Dispose();
                }
            }
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
