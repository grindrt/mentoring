using System;
using System.Xml.Serialization;

namespace BasicSerialization.Models
{
    [XmlRoot(ElementName = "catalog", Namespace = "http://library.by/catalog")]
    public class Catalog
    {
        [XmlElement(ElementName = "book")]
        public Book[] Books { get; set; }

        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlAttribute("date")]
        public string DateString
        {
            get { return Date.ToString("yyyy-MM-dd"); }
            set { Date = DateTime.Parse(value); }
        }

        public Catalog()
        {
        }

		public Catalog(Book[] books)
		{
			Books = books;
		}

        public Catalog(Book[] books, DateTime date)
        {
            Books = books;
            Date = date;
        }
    }
}