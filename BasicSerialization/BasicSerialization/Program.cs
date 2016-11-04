using System;
using System.IO;
using System.Xml.Serialization;
using BasicSerialization.Models;

namespace BasicSerialization
{
    class Program
    {
        static void Main(string[] args)
		{
			Serialize();
	        Deserialize();

	        Console.ReadKey();
        }

	    private static void Serialize()
	    {
		    var serializer = new XmlSerializer(typeof (Catalog));
		    try
		    {
			    using (var fs = new FileStream("books.xml", FileMode.OpenOrCreate))
			    {
				    var book = new Book
				    {
					    Id = "id29",
					    Author = "Test_Author",
					    Description = "New test book",
					    Genre = Genre.Fantasy.ToString(),
					    PublishDate = DateTime.Now,
					    Publisher = "Test App",
					    RegistrationDate = DateTime.Now,
					    Title = "New test book that serialized from test app"
				    };

				    serializer.Serialize(fs, new Catalog(new[] {book}));

				    Console.WriteLine("Success");
			    }
		    }
		    catch (Exception exception)
		    {
			    Console.WriteLine(exception.Message);
		    }
	    }

	    private static void Deserialize()
	    {
		    var serializer = new XmlSerializer(typeof (Catalog));
		    try
		    {
			    using (var fs = new FileStream("books.xml", FileMode.Open))
			    {
				    var catalog = (Catalog) serializer.Deserialize(fs);
				    Console.WriteLine(catalog.Books.Length);
			    }
		    }
		    catch (FileNotFoundException e)
		    {
			    Console.WriteLine("Could not found books.xml file.");
		    }
		    catch (Exception exception)
		    {
			    Console.WriteLine(exception.Message);
		    }
	    }
    }
}
