using System;
using System.Linq;
using DataModels;
using LinqToDB.Data;

namespace ORMTask.Tasks
{
	public static class SecondTask
	{
		public static void DO()
		{
			using (var db = new DataConnection())
			{
				ShowProductsWithCategory(db);

				ShowEmployeesWithRegion(db);

				ShowRegionStatistik(db);

				ShowEmployeesShippers(db);
			}
		}

		//4
		private static void ShowEmployeesShippers(DataConnection db)
		{
			LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
			var result = from employee in db.GetTable<Employee>()
				group employee by employee.EmployeeID
				into egrp
				select new
				{
					egrp.Key,
					Shippers =
						db.GetTable<Order>()
							.Where(order => (order.EmployeeID ?? 0) == egrp.Key)
							.Select(x => db.GetTable<Shipper>().FirstOrDefault(shipper => shipper.ShipperID == (x.ShipVia ?? 0)))
							.ToList()
				};

			Console.WriteLine("Task 2.3: ");
			foreach (var item in result)
			{
				Console.WriteLine("Employee: {0}, shippers {1}", item.Key, item.Shippers);
			}
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
		}

		//3
		private static void ShowRegionStatistik(DataConnection db)
		{
			var result =
				db.GetTable<Employee>()
					.GroupBy(employee => employee.Region)
					.Select(gr => new { Region = gr.Key, Count = gr.Count() });

			Console.WriteLine("Task 2.3: ");
			foreach (var item in result)
			{
				Console.WriteLine("Region: {0}, employees count {1}", item.Region, item.Count);
			}
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
		}

		//2
		private static void ShowEmployeesWithRegion(DataConnection db)
		{
			var result = db.GetTable<Employee>()
				.Join(db.GetTable<EmployeeTerritory>(),
					employee => employee.EmployeeID,
					employeeTerritory => employeeTerritory.EmployeeID,
					(employee, employeeTerritory) => new
					{
						employee.EmployeeID,
						employeeTerritory.Territory.Region.RegionDescription
					})
				.Distinct();

			Console.WriteLine("Task 2.2: ");
			foreach (var item in result)
			{
				Console.WriteLine("Employee: {0}, region {1}", item.EmployeeID, item.RegionDescription);
			}
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
		}

		//1
		private static void ShowProductsWithCategory(DataConnection db)
		{
			var result = (from c in db.GetTable<Product>()
				join category in db.GetTable<Category>() on c.CategoryID equals category.CategoryID
				join supplier in db.GetTable<Supplier>() on c.SupplierID equals supplier.SupplierID
				select new {c.ProductID, c.ProductName, category.CategoryName, supplier.CompanyName}).ToList();
			
			Console.WriteLine("Task 2.1: ");
			foreach (var item in result)
			{
				Console.WriteLine("Product: {0} - {1}, category {2}, supplier {3}", item.ProductID, item.ProductName, item.CategoryName, item.CompanyName);
			}
			Console.WriteLine("***********");
			Console.WriteLine("***********");
			Console.WriteLine("***********");
		}
	}
}