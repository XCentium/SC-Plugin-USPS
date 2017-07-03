using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Xcentium.Shipping.Usps.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Plugin.Xcentium.Shipping.Usps
{
    /// <summary>Defines the fulfillment ConfigureSitecore class.</summary>
    /// <seealso cref="T:Sitecore.Framework.Configuration.IConfigureSitecore" />
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>The configure services.</summary>
        /// <param name="services">The services.</param>

        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            Action<SitecorePipelinesConfigBuilder> actionDelegate = c => c
                    // This is how you can add the code block to run after a known code block has a modified the input. Same applies to Before
                    .ConfigurePipeline<ICalculateCartLinesPipeline>(
                        d =>
                        {
                            d.Add<UpdateCartLinesFulfillmentChargeBlock>().After<CalculateCartLinesFulfillmentBlock>();
                        })
                    .ConfigurePipeline<ICalculateCartPipeline>(
                        d => { d.Add<UpdateCartFulfillmentChargeBlock>().After<CalculateCartFulfillmentBlock>(); });
            services.Sitecore().Pipelines(actionDelegate);

        }
    }
}
