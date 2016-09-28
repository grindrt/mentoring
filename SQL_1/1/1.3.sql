--1.3
use Northwind

--1
--select distinct OrderID
--from [Order Details]
--where Quantity between 3 and 10

--2
--select CustomerID, Country
--from Customers
--where SUBSTRING(Country,1,1) between 'b' and 'g'
--order by Country

--3
select CustomerID, Country
from Customers
where SUBSTRING(Country,1,1) between 'b' and 'g'
order by Country
