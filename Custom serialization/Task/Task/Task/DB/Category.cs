using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Runtime.Serialization;

namespace Task.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [DataContract]
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [DataMember]
        public int CategoryID { get; set; }

        [Required]
        [DataMember]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [DataMember]
        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [DataMember]
        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        [DataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            var objContext = context.Context as IObjectContextAdapter;
            if (objContext != null)
            {
                objContext.ObjectContext.LoadProperty(this, x=>x.Products);
            }
        }

        //[OnDeserializing]
        //internal void OnDeserializingMethod(StreamingContext context)
        //{
        //    Products = new List<Product>((IEnumerable<Product>)context.Context);
        //}
    }
}
