﻿namespace RWEST.Nexus.MDM.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [CollectionDataContract(Namespace = "http://schemas.rwe.com/nexus", ItemName = "ProductTenorType")]
    [XmlRoot(Namespace = "http://schemas.rwe.com/nexus")]
    [XmlType(Namespace = "http://schemas.rwe.com/nexus")]
    public class ProductTenorTypeList : List<ProductTenorType>
    {
    }
}