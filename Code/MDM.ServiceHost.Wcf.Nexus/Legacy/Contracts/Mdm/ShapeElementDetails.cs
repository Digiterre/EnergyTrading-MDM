﻿namespace RWEST.Nexus.MDM.Contracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract(Namespace = "http://schemas.rwe.com/nexus")]
    [XmlType(Namespace = "http://schemas.rwe.com/nexus")]
    public class ShapeElementDetails
    {
        [DataMember(Order = 1)]
        [XmlElement]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        [XmlElement]
        public DateRange Period { get; set; }
    }
}