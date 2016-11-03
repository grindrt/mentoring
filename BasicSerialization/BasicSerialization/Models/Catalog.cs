using System;
using System.Xml.Serialization;

namespace BasicSerialization.Models
{
    [XmlRoot(ElementName = "catalog", Namespace = "http://library.by/catalog", DataType = "Array")]
    public class Catalog
    {
        [XmlElement(ElementName = "book")]
        public Book[] Books { get; set; }

        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlAttribute("date")]
        public string DateString
        {
            get { return Date.ToString("YYYY-MM-dd"); }
            set { Date = DateTime.Parse(value); }
        }

        public Catalog()
        {
        }
    }
}