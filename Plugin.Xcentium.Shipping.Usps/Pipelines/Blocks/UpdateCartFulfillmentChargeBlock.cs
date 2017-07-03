using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Xcentium.Shipping.Usps.Usps;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Pipelines;

namespace Plugin.Xcentium.Shipping.Usps.Pipelines.Blocks
{

    /// <summary>
    /// 
    /// </summary>
    public class UpdateCartFulfillmentChargeBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IGetSellableItemPipeline _getSellableItemPipeline;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getSellableItemPipeline"></param>
        public UpdateCartFulfillmentChargeBlock(IGetSellableItemPipeline getSellableItemPipeline)
        {
            _getSellableItemPipeline = getSellableItemPipeline;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {
            var adjustments = arg.Adjustments;

            if (adjustments == null || !adjustments.Any()) return await Task.FromResult(arg);


            var fulfillmentComponent = arg.GetComponent<PhysicalFulfillmentComponent>();

            var postageSelection = fulfillmentComponent.FulfillmentMethod.Name;

            var postalPrice = UspsShipping.GetCartShippingRate(postageSelection, arg, _getSellableItemPipeline, context);


            var currency = context.CommerceContext.CurrentCurrency();

            var money = new Money(currency, postalPrice);
            arg.Adjustments[0].Adjustment = money;



            return await Task.FromResult(arg);
        }
    }

}
