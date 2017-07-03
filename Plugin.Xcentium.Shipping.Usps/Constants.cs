using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace Plugin.Xcentium.Shipping.Usps
{

    /// <summary>
    /// 
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// 
        /// </summary>
        public struct Usps
        {
            /// <summary>
            /// 
            /// </summary>
            public const string DomesticXml = "{0}?API=RateV4&XML=<RateV4Request USERID=\"{1}\"><Revision/>{2}</RateV4Request>";

            /// <summary>
            /// 
            /// </summary>
            public const string DomesticPackageXml = "<Package ID=\"{0}\"><Service>{1}</Service><ZipOrigination>{2}</ZipOrigination><ZipDestination>{3}</ZipDestination><Pounds>{4}</Pounds><Ounces>0</Ounces><Container>{5}</Container><Size>{6}</Size><Width>{7}</Width><Length>{8}</Length><Height>{9}</Height></Package>";

            /// <summary>
            /// 
            /// </summary>
            public const string InternationalXml = "{0}?API=IntlRateV2&XML=<IntlRateV2Request USERID=\"{1}\"><Revision/>{2}</IntlRateV2Request>";

            /// <summary>
            /// 
            /// </summary>
            public const string InternationalPackageXml = "<Package ID='{0}'><Pounds>{1}</Pounds><Ounces>0</Ounces><Machinable>True</Machinable><MailType>Package</MailType><GXG><POBoxFlag>N</POBoxFlag><GiftFlag>N</GiftFlag></GXG><ValueOfContents>{2}</ValueOfContents><Country>{3}</Country><Container>RECTANGULAR</Container><Size>REGULAR</Size><Width>{4}</Width><Length>{5}</Length><Height>{6}</Height><Girth>0</Girth><CommercialFlag>N</CommercialFlag></Package>";

            /// <summary>
            /// 
            /// </summary>
            public const string ShippingServices = "First Class|First Class Commercial|First Class HFP Commercial|Priority|Priority Commercial|Priority Cpp|Priority HFP Commercial|Priority HFP CPP|Priority Mail Express|Priority Mail Express Commercial|Priority Mail Express CPP|Priority Mail Express Sh|Priority Mail Express Sh Commercial|Priority Mail Express HFP|Priority Mail Express HFP Commercial|Priority Mail Express HFP CPP|Retail Ground|Media|Library";

            /// <summary>
            /// 
            /// </summary>
            public static Dictionary<string, string> Method = new Dictionary<string, string>()
            {
                {"4", "Global Express Guaranteed"},
                {"12", "USPS Gxg Envelopes"},
                {"1", "Priority Mail Express International"},
                {"11", "Priority Mail International Large Flat Rate Box"},
                {"9", "Priority Mail International Medium Flat Rate Box"},
                {"25", "Priority Mail International Large Video Flat Rate priced box"},
                {"15", "First-Class Package International Service"}
            };

        }

    }
}
