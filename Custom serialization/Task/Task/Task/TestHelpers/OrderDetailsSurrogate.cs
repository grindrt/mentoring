using System.Runtime.Serialization;
using Task.DB;

namespace Task.TestHelpers
{
    public class OrderDetailsSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var orderDetail = (Order_Detail) obj;
            info.AddValue("Discount", orderDetail.Discount);
            info.AddValue("OrderID", orderDetail.OrderID);
            info.AddValue("Product", orderDetail.Product);
            info.AddValue("ProductID", orderDetail.ProductID);
            info.AddValue("Quantity", orderDetail.Quantity);
            info.AddValue("Order", orderDetail.Order);
            info.AddValue("UnitPrice", orderDetail.UnitPrice);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var orderDetail = (Order_Detail)obj;
            orderDetail.Discount = (float)info.GetValue("Discount", typeof(float));
            orderDetail.OrderID = info.GetInt32("OrderID");
            orderDetail.Product = (Product)info.GetValue("Product", typeof(Product));
            orderDetail.ProductID = info.GetInt32("ProductID");
            orderDetail.Quantity = (short)info.GetValue("Quantity", typeof(short));
            orderDetail.Order = (Order)info.GetValue("Order",typeof(Order));
            orderDetail.UnitPrice = info.GetDecimal("UnitPrice");
            return orderDetail;
        }
    }
}