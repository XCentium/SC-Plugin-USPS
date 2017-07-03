
Sitecore Commerce Engine USPS plugin
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

2. Add it as a dependency to your Sitecore Commerce Engine Project.Json file by adding the line below
    "Plugin.Xcentium.Shipping.Usps": "1.0.2301"

3. Add it as a dependency to your Adventure works or Habitat or Custom Site plugin' project.json file by adding the line below
    "Plugin.Xcentium.Shipping.Usps": "1.0.2301"

4. Add the settings below with your USPS API key to your Sitecore Commerce Engine's environment json files

```
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
          "WeightFieldName": "Weight",
          "HeightFieldName": "Height",
          "WidthFieldName": "Width",
          "LengthFieldName": "Length",
          "PolicyId": "UspsClientPolicy",
          "Models": {
            "$type": "System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Model, Sitecore.Commerce.Core]], mscorlib",
            "$values": [
            ]
          }
        },

```

5. Add the required shipper and ship from Zip code ("ShipFromZip") and default weight and dimension of a catalog product. The default values will be used when there is no dimension and weight specified in Sitecore for the product item(see notes below).

6. Install the USPS Shipping Option Pkg-1.0.zip in Sitecore when installed, select USPS Delivery as your shipping option.

7. You are ready to start using it. 

Note:
=====
- Each physical product that will be shipped to buyers should have dimensions and weight.
- You may need to extend your commerce product template either directly in Sitecore or by using Catalog & Inventory Schema Manager tool and add four additional fields (Length, Width, Height, Weight)
- If the fields or any of them are named differently from the above 4 names, you will need to set the actual name in the json policy config above changing the values of:

```
        "WeightFieldName": "Weight",
        "HeightFieldName": "Height",
        "WidthFieldName": "Width",
        "LengthFieldName": "Length",
```
- Set a desired value for "CompensationMultipier" default is 1 this is the value of the multiple of the cart total that will be paid by USPS if the package were to be damaged or lost in transit. Value of 1 means 100% compensation. Value of 2 means the compensation will be twice the original amount paid by the customer

- If you have any questions, comment or need us to help install, extend or adapt to your needs, do not hesitate to reachout to us at XCentium.




