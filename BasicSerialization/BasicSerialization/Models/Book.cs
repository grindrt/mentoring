using System;
using System.Xml.Serialization;

namespace BasicSerialization.Models
{
    public class Book
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "isbn")]
        [XmlIgnore]
        public string Isbn { get; set; }

        [XmlElement(ElementName = "author")]
        public string Author { get; set; }

        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

		[XmlElement(ElementName = "genre")]
        public string Genre { get; set; }

        [XmlElement(ElementName = "publisher")]
        public string Publisher { get; set; }

        [XmlIgnore]
        public DateTime PublishDate { get; set; }

        [XmlElement(ElementName = "publish_date")]
        public string PublishDateString
        {
			get { return PublishDate.ToString("yyyy-MM-dd"); }
            set { PublishDate = DateTime.Parse(value); } 
        }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlIgnore]
        public DateTime RegistrationDate { get; set; }

        [XmlElement(ElementName = "registration_date")]
        public string RegistrationDateeString
        {
			get { return RegistrationDate.ToString("yyyy-MM-dd"); }
            set { RegistrationDate = DateTime.Parse(value); }
        }

        public Book()
        {
        }

	    public Book(string id, string isbn, string author, string title, string genre, string publisher, DateTime publishDate, string description, DateTime registrationDate)
	    {
		    Id = id;
		    Isbn = isbn;
		    Author = author;
		    Title = title;
		    Genre = genre;
		    Publisher = publisher;
		    PublishDate = publishDate;
		    Description = description;
		    RegistrationDate = registrationDate;
	    }
    }
}