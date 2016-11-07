using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.TestHelpers
{
    public class OrderSurragate : IDataContractSurrogate
    {
        public Type GetDataContractType(Type type)
        {
            return type;
        }

        // I don't really shure taht this is correct solution
        public object GetObjectToSerialize(object obj, Type targetType)
        {
            var order = obj as Order;
            if (order != null)
            {
                var customer = new Customer(order.Customer);
                var employee = new Employee(order.Employee);
                var shipper = new Shipper(order.Shipper);

                var resObj = new Order
                {
                    Customer = customer,
                    CustomerID = order.CustomerID,
                    Employee = employee,
                    EmployeeID = order.EmployeeID,
                    Freight = order.Freight,
                    OrderDate = order.OrderDate,
                    OrderID = order.OrderID,
                    RequiredDate = order.RequiredDate,
                    ShipAddress = order.ShipAddress,
                    ShipCity = order.ShipCity,
                    ShipCountry = order.ShipCountry,
                    ShipName = order.ShipName,
                    ShipPostalCode = order.ShipPostalCode,
                    ShipRegion = order.ShipRegion,
                    ShipVia = order.ShipVia,
                    ShippedDate = order.ShippedDate,
                    Shipper = shipper,
                    Order_Details = new List<Order_Detail>()
                };

                foreach (var orderDetail in order.Order_Details)
                {
                    resObj.Order_Details.Add(new Order_Detail
                    {
                        OrderID = orderDetail.OrderID,
                        Discount = orderDetail.Discount,
                        ProductID = orderDetail.ProductID,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = orderDetail.UnitPrice
                    });
                }

                return resObj;
            }


            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj; 
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            return null;
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            return null;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return null;
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }
    }
}