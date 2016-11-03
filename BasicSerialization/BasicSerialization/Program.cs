using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using BasicSerialization.Models;

namespace BasicSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new XmlSerializer(typeof(Catalog));
            using (var fs = new FileStream("books.xml", FileMode.OpenOrCreate))
            {
                var catalog = (Catalog) serializer.Deserialize(fs);

                Console.WriteLine(catalog.Books.Length);
            }

            Console.ReadKey();
        }
    }
}
