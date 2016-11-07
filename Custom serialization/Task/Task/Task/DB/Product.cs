using System.Linq;
using System.Runtime.Serialization;

namespace Task.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class Product : ISerializable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public int? SupplierID { get; set; }

        public int? CategoryID { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }

        public Product(SerializationInfo info, StreamingContext context)
        {
            ProductID = info.GetInt32("ProductID");
            ProductName = info.GetString("ProductName");
            SupplierID = (int?)info.GetValue("SupplierID", typeof(object));
            CategoryID = (int?)info.GetValue("CategoryID", typeof(object));
            QuantityPerUnit = info.GetString("QuantityPerUnit");
            UnitPrice = (decimal?)info.GetValue("UnitPrice", typeof(object));
            UnitsInStock = (short?)info.GetValue("UnitsInStock", typeof(object));
            UnitsOnOrder = (short?)info.GetValue("UnitsOnOrder", typeof(object));
            ReorderLevel = (short?)info.GetValue("ReorderLevel", typeof(object));
            Discontinued = info.GetBoolean("Discontinued");
            Category = (Category)info.GetValue("Category", typeof(Category));
            Order_Details = (Order_Detail[])info.GetValue("Order_Details", typeof(Order_Detail[]));
            Supplier = (Supplier)info.GetValue("Supplier", typeof(Supplier));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ProductID", ProductID);
            info.AddValue("ProductName", ProductName);
            info.AddValue("SupplierID", SupplierID);
            info.AddValue("CategoryID", CategoryID);
            info.AddValue("QuantityPerUnit", QuantityPerUnit);
            info.AddValue("UnitPrice", UnitPrice);
            info.AddValue("UnitsInStock", UnitsInStock);
            info.AddValue("UnitsOnOrder", UnitsOnOrder);
            info.AddValue("ReorderLevel", ReorderLevel);
            info.AddValue("Discontinued", Discontinued);
            info.AddValue("Category", Category);
            info.AddValue("Order_Details", Order_Details.ToArray());
            info.AddValue("Supplier", Supplier);
        }
    }
}
