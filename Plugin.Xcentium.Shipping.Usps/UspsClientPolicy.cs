using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Plugin.Xcentium.Shipping.Usps
{

    /// <summary>
    /// 
    /// </summary>
    public class UspsClientPolicy: Policy
    {
        /// <summary>
        /// 
        /// </summary>
        public UspsClientPolicy()
        {
            UserId = "178XCENT0790";
            Url = "http://production.shippingapis.com/ShippingAPI.dll";
            ShipFromZip = "73301";
            ContainerType = "VARIABLE";
            PackageSize = "REGULAR";



            this.Length = "2";
            this.Width = "2";
            this.Height = "2";
            this.Weight = "0.2";

        }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShipFromZip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContainerType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PackageSize { get; set; }


        // Cart item dimension and weight if not set in catalog
        /// <summary>
        /// 
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Weight { get; set; }

    }
}
