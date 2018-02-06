using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Fulfillment;
using Plugin.Xcentium.Shipping.Usps.Usps;

namespace Plugin.Xcentium.Shipping.Usps.Usps
{
    /// <summary>
    /// 
    /// </summary>
    public static class UspsShipping
    {

        /// <summary>
        /// 
        /// </summary>
        public static UspsClientPolicy UspsClientPolicy = new UspsClientPolicy();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cart"></param>
        /// <param name="getSellableItemPipeline"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static decimal GetCartShippingRate(string name, Cart cart, IGetSellableItemPipeline getSellableItemPipeline, CommercePipelineExecutionContext context)
        {

            var rates = GetCartShippingRates(cart, getSellableItemPipeline, context);

            if (rates == null || !rates.Any()) return 0m;
            try
            {
                return rates.FirstOrDefault(x => x.Key.ToLower() == name.ToLower()).Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0m;
        }

        internal static List<KeyValuePair<string, decimal>> GetCartShippingRates(Cart cart,
            IGetSellableItemPipeline getSellableItemPipeline, CommercePipelineExecutionContext context)
        {
            var input = new RequestInput();
            UspsClientPolicy = context.GetPolicy<UspsClientPolicy>();
            if (cart != null && cart.Lines.Any<CartLineComponent>() && cart.HasComponent<PhysicalFulfillmentComponent>())
            {
                var component = cart.GetComponent<PhysicalFulfillmentComponent>();

                var shippingParty = component?.ShippingParty;

                input.AddressLine1 = shippingParty?.Address1;
                input.AddressLine2 = shippingParty?.Address2;
                input.City = shippingParty?.City;
                input.CountryCode = shippingParty?.CountryCode;
                input.Country = shippingParty?.Country;
                input.StateCode = shippingParty?.StateCode;
                input.ZipPostalCode = shippingParty?.ZipPostalCode;

                input.PriceValue = cart.Totals.SubTotal.Amount;

                var height = 0.0M;
                var width = 0.0M;
                var length = 0.0m;
                var weight = 0.0m;

                foreach (var cartLineComponent in cart.Lines)
                {

                    // get specific weight value
                    var productArgument = ProductArgument.FromItemId(cartLineComponent.ItemId);
                    if (!productArgument.IsValid()) continue;
                    var sellableItem = getSellableItemPipeline.Run(productArgument, context).Result;

                    if (sellableItem != null && sellableItem.HasComponent<ItemSpecificationsComponent>())
                    {
                        var itemSpec = sellableItem.GetComponent<ItemSpecificationsComponent>();

                        if (itemSpec.Weight > 0) weight += itemSpec.Weight;

                        if (itemSpec.Height > 0) height += itemSpec.Height;

                        if (itemSpec.Width > 0 && itemSpec.Width > width) width = itemSpec.Width;

                        if (itemSpec.Length > 0 && itemSpec.Length > length) length = itemSpec.Length;

                    }

                }

                input.Height = Math.Ceiling(height).ToString(CultureInfo.CurrentCulture);
                input.Width = Math.Ceiling(width).ToString(CultureInfo.CurrentCulture);
                input.Length = Math.Ceiling(length).ToString(CultureInfo.CurrentCulture);
                input.Weight = Math.Ceiling(weight); 

            }

            var rates =  GetShippingRates(input, context);


            return rates;
        }

        private static List<KeyValuePair<string, decimal>> GetShippingRates(RequestInput input, CommercePipelineExecutionContext context)
        {
            var rates = new List<KeyValuePair<string, decimal>>();

            UspsClientPolicy = context.GetPolicy<UspsClientPolicy>();

            var serviceTypes = Constants.Usps.ShippingServices.Split('|').ToList();
            if (serviceTypes.Any())
            {
                var reqXml = string.Empty;

                if (input.CountryCode.ToLower() == "us")
                {

                    for (int i = 0; i < serviceTypes.Count; i++)
                    {
                        var elem = string.Format(Constants.Usps.DomesticPackageXml, i.ToString(),
                            serviceTypes[i].ToUpper(), UspsClientPolicy.ShipFromZip, input.ZipPostalCode, input.Weight,
                            UspsClientPolicy.ContainerType, UspsClientPolicy.PackageSize, input.Width, input.Length,
                            input.Height);

                        if (serviceTypes[i].ToUpper().Contains("FIRST"))
                        {
                            elem = elem.Replace("</Service>", "</Service><FirstClassMailType>Parcel</FirstClassMailType>");
                        }
                        if (serviceTypes[i].ToUpper().Contains("RETAIL GROUND") || serviceTypes[i].ToUpper().Contains("'ALL") || serviceTypes[i].ToUpper().Contains("ONLINE"))
                        {
                            elem = elem.Replace("</Package>", "<Machinable>true</Machinable></Package>");
                        }
                        reqXml += elem;

                    }

                    reqXml = string.Format(Constants.Usps.DomesticXml, UspsClientPolicy.Url, UspsClientPolicy.UserId, reqXml);
                }
                else
                {

                    reqXml = string.Format(Constants.Usps.InternationalPackageXml, "0", input.Weight,
                        input.PriceValue.ToString(CultureInfo.InvariantCulture), input.Country, input.Width,
                        input.Length, input.Height);

                        reqXml = string.Format(Constants.Usps.InternationalXml, UspsClientPolicy.Url, UspsClientPolicy.UserId, reqXml);
                }





                using (var client = new WebClient())
                {

                    //Send the request to USPS.
                    var responseData = client.DownloadData(reqXml);

                    //Convert byte stream to string data.
                    var strResponse = responseData.Aggregate("", (current, oItem) => current + (char) oItem);

                    var isInternational = strResponse.Contains("<Postage>");

                    if (!string.IsNullOrEmpty(strResponse) && (strResponse.Contains("<Rate>") || strResponse.Contains("<Postage>")))
                    {
                        if (isInternational)
                        {
                            strResponse = strResponse.Replace("<Package", "<IntPackage").Replace("</Package", "</IntPackage>").Replace("GXG", "Gxg").Replace("POBoxFlag", "PoBoxFlag");
                        }


                        var doc = new XmlDocument();
                        doc.LoadXml(strResponse);
                        var json = JsonConvert.SerializeXmlNode(doc);

                        json = json.Replace("@", "").Replace("?xml", "Xml").Replace("version", "Version").Replace("encoding", "Encoding").Replace("CLASSID", "Classid").Replace("ID", "Id");

                        try
                        {
                            if (isInternational)
                            {
                                var intPostageServices = Constants.Usps.Method;
                                var responsList = JsonConvert.DeserializeObject<IntUspsResponse>(json);
                                if (responsList?.IntlRateV2Response != null)
                                {
                                    var services = responsList.IntlRateV2Response.IntPackage.Service;
                                    if (services.Any())
                                    {
                                        foreach (var service in services)
                                        {
                                            if (service.Postage != null)
                                            {
                                                var postageType = intPostageServices[service.Id];
                                                decimal.TryParse(service.Postage,
                                                    out var totalChage);
                                                rates.Add(new KeyValuePair<string, decimal>(postageType, totalChage));
                                            }
                                        }
                                    }


                                }
                            }
                            else
                            {
                                var responsList = JsonConvert.DeserializeObject<UspsResponse>(json);
                                if (responsList?.RateV4Response != null)
                                {
                                    var packages = responsList.RateV4Response.Package;
                                    if (packages.Any())
                                    {
                                        foreach (var package in packages)
                                        {
                                            if (package.Postage != null)
                                            {
                                                var postageType = serviceTypes[int.Parse(package.Id)];
                                                decimal.TryParse(package.Postage.Rate,
                                                    out var totalChage);
                                                rates.Add(new KeyValuePair<string, decimal>(postageType, totalChage));
                                            }
                                        }
                                    }


                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);

                        }

                    }

                }

            }

            return rates;
        }
    }
}
