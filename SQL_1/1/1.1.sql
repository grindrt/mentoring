--1.1
use Northwind

--1
select OrderId, ShippedDate, ShipVia
from Orders
where ShipVia>=2 and ShippedDate >= '1998-05-06'

--2
select OrderID,
case when ShippedDate is NULL then 'Not Shipped'
end as 'ShippedDate'
from Orders
where ShippedDate is NULL

--3
select 
	OrderID as 'Order Number',
	case when ShippedDate is NULL then 'Not Shipped' 
	else CONVERT(varchar(10),ShippedDate,20)
	end as 'Shipped Date'
from Orders
where ShippedDate >= '1998-05-06' or ShippedDate is NULL
