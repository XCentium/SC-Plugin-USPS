
Sitecore Commerce Engine USPS plugin (Sitecore Experience Commerce 9.1)
======================================

This plugin allows the user to integrate USPS as a Fulfillment Method for shipping. 
- It is very easy to integrate or extend to fit your needs.
- If will fetch shipping rates from USPS based on the weight and dimensions of the items in your cart.
- It will fetch rates for either International or USA shipping

It also comes with a Sitecore Module which will add USPS as a Fulfilment option under your Sitecore Commerce node in Sitecore. 

Grouping
========
This is a shipping/Fulfillment plugin using USPS Api

Sponsor
=======
This plugin was sponsored and created by XCentium.

How to Install
==============

1. Copy it to your Sitecore Commerce Engine Solution and add it as a project 

2. Add it as a dependency to your Sitecore Commerce Engine Project

3. Add it as a dependency to your Adventure works or Habitat or Custom Site plugin' project

4. Create a json file name "Plugin.Xcentium.Shipping.Usps.PolicySet-1.0.0.json" with the json below under the Environments folder of your Sitecore Commerce Engine Project. Add your USPS userid to the json

```
{
  "$type": "Sitecore.Commerce.Core.PolicySet, Sitecore.Commerce.Core",
  "Id": "Entity-PolicySet-UspsPolicySet",
  "Version": 1,
  "IsPersisted": false,
  "Name": "UspsPolicySet",
  "Policies": {
    "$type": "System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Policy, Sitecore.Commerce.Core]], mscorlib",
    "$values": [
     {
        "$type": "Plugin.Xcentium.Shipping.Usps.UspsClientPolicy, Plugin.Xcentium.Shipping.Usps",
        "UserId": "XXXXXXXXX",
        "Url": "http://production.shippingapis.com/ShippingAPI.dll",
        "ShipFromZip": "73301",
        "ContainerType": "VARIABLE",
        "PackageSize": "REGULAR",
        "Weight": "0.2",
        "Width": "2",
        "Length": "2",
        "Height": "2",
        "PolicyId": "UspsClientPolicy",
        "Models": {
			"$type": "System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Model, Sitecore.Commerce.Core]], mscorlib",
			"$values": [
			]
        }
      }
    ]
  }
}


```

Inside your corresponding json files for PlugIn.Habitat.CommerceAuthoring-1.0.0.json, PlugIn.Habitat.CommerceMinions-1.0.0.json and PlugIn.Habitat.CommerceShops-1.0.0.json
 you will need to add the line below. I added mine after braintree entry

 ```
       {
        "$type": "Sitecore.Commerce.Core.PolicySetPolicy, Sitecore.Commerce.Core",
        "PolicySetId": "Entity-PolicySet-UspsPolicySet"
      },


```

5. Add the required shipper and ship from Zip code ("ShipFromZip") and default weight and dimension of a catalog product. The default values will be used when there is no dimension and weight specified in Sitecore for the product item(see notes below).

6. Install the USPS Shipping Option Pkg-1.0.zip in Sitecore when installed, select USPS Delivery as your shipping option.

7. You are ready to start using it. 

Note:
=====
- Each physical product that will be shipped to buyers should have dimensions and weight.

- Set a desired value for "CompensationMultipier" default is 1 this is the value of the multiple of the cart total that will be paid by USPS if the package were to be damaged or lost in transit. Value of 1 means 100% compensation. Value of 2 means the compensation will be twice the original amount paid by the customer

- If you have any questions, comment or need us to help install, extend or adapt to your needs, do not hesitate to reachout to us at XCentium.




