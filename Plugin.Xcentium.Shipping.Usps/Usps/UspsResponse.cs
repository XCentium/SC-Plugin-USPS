using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Xcentium.Shipping.Usps.Usps
{
    public class Xml
    {
        public string Version { get; set; }
        public string Encoding { get; set; }
    }

    public class Error
    {
        public string Number { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public object HelpFile { get; set; }
        public object HelpContext { get; set; }
    }

    public class Postage
    {
        public string Classid { get; set; }
        public string MailService { get; set; }
        public string Rate { get; set; }
        public string CommercialRate { get; set; }
        public string CommercialPlusRate { get; set; }
    }

    public class Package
    {
        public string Id { get; set; }
        public Error Error { get; set; }
        public string ZipOrigination { get; set; }
        public string ZipDestination { get; set; }
        public string Pounds { get; set; }
        public string Ounces { get; set; }
        public string Container { get; set; }
        public string Size { get; set; }
        public string Zone { get; set; }
        public Postage Postage { get; set; }
        public string Machinable { get; set; }
    }

    public class RateV4Response
    {
        public List<Package> Package { get; set; }
    }

    public class UspsResponse
    {
        public Xml Xml { get; set; }
        public RateV4Response RateV4Response { get; set; }
    }

    //===================================================================


    public class Gxg
    {
        public string PoBoxFlag { get; set; }
        public string GiftFlag { get; set; }
    }

    public class ExtraService
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Available { get; set; }
        public string Price { get; set; }
    }

    public class ExtraServices
    {
        public ExtraService ExtraService { get; set; }
    }

    public class ServiceError
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }

    public class ServiceErrors
    {
        public ServiceError ServiceError { get; set; }
    }

    public class Service
    {
        public string Id { get; set; }
        public string Pounds { get; set; }
        public string Ounces { get; set; }
        public string Machinable { get; set; }
        public string MailType { get; set; }
        public Gxg Gxg { get; set; }
        public string Container { get; set; }
        public string Size { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }
        public string Height { get; set; }
        public string Girth { get; set; }
        public string Country { get; set; }
        public string Postage { get; set; }
        public ExtraServices ExtraServices { get; set; }
        public string ValueOfContents { get; set; }
        public string SvcCommitments { get; set; }
        public string SvcDescription { get; set; }
        public string MaxDimensions { get; set; }
        public string MaxWeight { get; set; }
        public string InsComment { get; set; }
        public ServiceErrors ServiceErrors { get; set; }
    }

    public class IntPackage
    {
        public string Id { get; set; }
        public string Prohibitions { get; set; }
        public string Restrictions { get; set; }
        public string Observations { get; set; }
        public string CustomsForms { get; set; }
        public string ExpressMail { get; set; }
        public string AreasServed { get; set; }
        public string AdditionalRestrictions { get; set; }
        public List<Service> Service { get; set; }
    }

    public class IntlRateV2Response
    {
        public IntPackage IntPackage { get; set; }
    }

    public class IntUspsResponse
    {
        public Xml Xml { get; set; }
        public IntlRateV2Response IntlRateV2Response { get; set; }
    }

}
