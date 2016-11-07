using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;

namespace Task
{
	[TestClass]
	public class SerializationSolutions
	{
		Northwind dbContext;

		[TestInitialize]
		public void Initialize()
		{
			dbContext = new Northwind();
		}

		[TestMethod]
		public void SerializationCallbacks()
		{
            dbContext.Configuration.ProxyCreationEnabled = false;
            
            var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(new NetDataContractSerializer(new StreamingContext(StreamingContextStates.All, dbContext)), true);

            var categories = dbContext.Categories.ToList();

            tester.SerializeAndDeserialize(categories);
		}

        //done
		[TestMethod]
		public void ISerializable()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(new NetDataContractSerializer(), true);
			var products = dbContext.Products.ToList();

		    var product = products.FirstOrDefault();

            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
            objectContext.LoadProperty(product, f => f.Order_Details);
            objectContext.LoadProperty(product, f => f.Supplier);
            objectContext.LoadProperty(product, f => f.Category);

            tester.SerializeAndDeserialize(new List<Product> { product });
		}

        //done
		[TestMethod]
		public void ISerializationSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

            var sc = new StreamingContext(StreamingContextStates.All);

		    var surrogateSelector = new SurrogateSelector();
            surrogateSelector.AddSurrogate(typeof(Order_Detail), sc, new OrderDetailsSurrogate());

		    var serializer = new NetDataContractSerializer(sc, int.MaxValue, false, FormatterAssemblyStyle.Full, surrogateSelector);

		    var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(serializer, true);

            var od = dbContext.Order_Details.FirstOrDefault();
            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
            objectContext.LoadProperty(od, f => f.Order);
            objectContext.LoadProperty(od, f => f.Product);

			tester.SerializeAndDeserialize(new List<Order_Detail>{od});
		}

        //done
		[TestMethod]
		public void IDataContractSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = true;
			dbContext.Configuration.LazyLoadingEnabled = true;

            var orderSurragate = new OrderSurragate();
		    IEnumerable<Type> knownTypes = new List<Type>
		    {
		        typeof(Customer),
		        typeof(Employee),
		        typeof(ICollection<Order_Detail>),
                typeof(Order_Detail),
		        typeof(Shipper),
                typeof(Order)
		    };

		    var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(new DataContractSerializer(typeof(IEnumerable<Order>), knownTypes, int.MaxValue, false, true, orderSurragate), true);
			var orders = dbContext.Orders.ToList();

			tester.SerializeAndDeserialize(new List<Order>{orders.FirstOrDefault()});
		}
	}
}
