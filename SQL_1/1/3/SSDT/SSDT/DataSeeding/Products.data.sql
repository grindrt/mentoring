go
set identity_insert [dbo].[Products] on
merge into [dbo].[Products] as target
using(VALUES
	(1,'Chai',1,1,'10 boxes x 20 bags',18,39,0,10,0),
	(2,'Chang',1,1,'24 - 12 oz bottles',19,17,40,25,0)
) as source([ProductID],[ProductName],[SupplierID],[CategoryID],[QuantityPerUnit],[UnitPrice],[UnitsInStock],[UnitsOnOrder],[ReorderLevel],[Discontinued]) 
on target.[ProductID] = source.[ProductID]
when not matched by target then
	insert([ProductID],[ProductName],[SupplierID],[CategoryID],[QuantityPerUnit],[UnitPrice],[UnitsInStock],[UnitsOnOrder],[ReorderLevel],[Discontinued])
	values([ProductID],[ProductName],[SupplierID],[CategoryID],[QuantityPerUnit],[UnitPrice],[UnitsInStock],[UnitsOnOrder],[ReorderLevel],[Discontinued]);
go
set identity_insert [dbo].[Products] off
