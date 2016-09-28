using System;
using System.Linq;
using System.Xml.Schema;
using DataModels;
using LinqToDB;
using LinqToDB.Data;

namespace ORMTask.Tasks
{
	public static class SecondTask
	{
		public static void Do()
		{
				ShowProductsWithCategory();

				ShowEmployeesWithRegion();

				ShowRegionStatistik();

				ShowEmployeesShippers();
		}

		//4
		private static void ShowEmployeesShippers()
		{using (var db = new DataConnection())
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
            }
		}

		//3
	    private static void ShowRegionStatistik()
	    {
	        using (var db = new DataConnection())
	        {
	            var result =
	                db.GetTable<Employee>()
	                    .GroupBy(employee => employee.Region)
	                    .Select(gr => new {Region = gr.Key, Count = gr.Count()});
	        }
	    }

	    //2
	    private static void ShowEmployeesWithRegion()
	    {
	        using (var db = new DataConnection())
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
	        }
	    }

	    //1
	    private static void ShowProductsWithCategory()
	    {
	        using (var db = new DataConnection())
	        {
	            var result = (from c in db.GetTable<Product>()
	                join category in db.GetTable<Category>() on c.CategoryID equals category.CategoryID
	                join supplier in db.GetTable<Supplier>() on c.SupplierID equals supplier.SupplierID
	                select new {c.ProductID, c.ProductName, category.CategoryName, supplier.CompanyName}).ToList();
	        }
	    }
	}

    public static class ThirdTask
    {
        public static void Do()
        {
            //AddNewEmployee();

           // ChangeProductCategory();

            AddProducts();

           // ChangeProductToAnalog();
        }

        //4
        private static void ChangeProductToAnalog()
        {
            using (var db = new DataConnection())
            {
                var orders = db.GetTable<OrderDetail>()
                    .Where(od => db.GetTable<Order>()
                        .Where(x => !x.ShippedDate.HasValue)
                        .Select(x => x.OrderID)
                        .Contains(od.OrderID))
                        .GroupBy(od => od.OrderID)
                        .Select(grp => grp)
                    .Take(5).ToList();

                foreach (var order in orders)
                {
                    var firstOrDefault = order.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        db.GetTable<OrderDetail>()
                            .Where(x => x.OrderID == order.Key && x.ProductID == firstOrDefault.ProductID)
                            .Set(x => x.ProductID, x => 2)
                            .Update();
                    }
                }
            }
        }

        //3
        private static void AddProducts()
        {
            using (var db = new DataConnection())
            {
                var supplier =
                    db.GetTable<Supplier>()
                        .FirstOrDefault(x => x.CompanyName.Equals("Test") && x.ContactName.Equals("Test"));
                var supplierId = supplier == null
                    ? (int)(decimal)db.GetTable<Supplier>().InsertWithIdentity(() => new Supplier {ContactName = "Test", CompanyName = "Test"})
                    : supplier.SupplierID;

                var category =
                    db.GetTable<Category>()
                        .FirstOrDefault(x => x.CategoryName.Equals("Test"));
                var categoryId = category == null
                    ? (int)(decimal)db.GetTable<Category>().InsertWithIdentity(() => new Category { CategoryName = "Test" })
                    : category.CategoryID;

                db.GetTable<Product>()
                    .Insert(() => new Product { SupplierID = supplierId, CategoryID = categoryId, ProductName = "test1" });
                db.GetTable<Product>()
                    .Insert(() => new Product { SupplierID = supplierId, CategoryID = categoryId, ProductName = "test2" });
                db.GetTable<Product>()
                    .Insert(() => new Product { SupplierID = supplierId, CategoryID = categoryId, ProductName = "test3" });
            }
        }

        //2
        private static void ChangeProductCategory()
        {
            using (var db = new DataConnection())
            {
                var products = db.GetTable<Product>().Where(x => x.CategoryID == 1).Take(2).Select(x => x.ProductID).ToList();
                foreach (var product in products)
                {
                    db.GetTable<Product>().Where(x => x.ProductID == product).Set(c => c.CategoryID, c => 2).Update();
                }
            }
        }

        //1 
        private static void AddNewEmployee()
        {
            using (var db = new DataConnection())
            {
                int identity = (int)(decimal)db.GetTable<Employee>()
                    .InsertWithIdentity(() => new Employee
                    {
                        FirstName = "Test",
                        LastName = "Test"
                    });
                var territoies = db.GetTable<Territory>().Select(x => x.TerritoryID).Take(3).ToList();
                foreach (var territory in territoies)
                {
                    db.GetTable<EmployeeTerritory>().Insert(() => new EmployeeTerritory
                    {
                        EmployeeID = identity,
                        TerritoryID = territory
                    });
                }
            }
        }
    }
}