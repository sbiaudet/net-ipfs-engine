using System;
using Ipfs.CoreApi;
using Ipfs.Engine;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static ServiceCollection AddIpfs(this ServiceCollection services, Action<IpfsEngineOptions> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();

            services.TryAddScoped<ICoreApi, IpfsEngine>();

            services.Configure(configure);

            return services;
        }
    }
}
