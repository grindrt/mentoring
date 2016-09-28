--2.2
use Northwind

--1
--select YEAR(OrderDate) as 'Year', COUNT(OrderID) as 'Total'
--from Orders
--group by YEAR(OrderDate)

----check
--select COUNT(OrderID) as 'Total'
--from Orders
--where OrderDate between '1998-01-01' and '1998-12-31'

--2
--select 
--	(
--		select LastName + ' ' + FirstName
--		from Employees
--		where EmployeeID = Orders.EmployeeID
--	) as 'Seller', 
--	COUNT(EmployeeID) as 'Amount'
--from Orders
--group by orders.EmployeeID
--order by Amount desc

--3
--select orders.CustomerID, orders.EmployeeID, COUNT(OrderID)
--from Orders
--where Orders.OrderDate between '1998-01-01' and '1998-12-31'
--group by orders.CustomerID, orders.EmployeeID

----check
--select orders.CustomerID, orders.EmployeeID
--from Orders
--where Orders.OrderDate between '1998-01-01' and '1998-12-31' 
--	and orders.CustomerID = 'ALFKI'
--	and orders.EmployeeID = 1

--4
--select E.City, Count(E.EmployeeID) as 'Employees', Count(C.CustomerID) as 'Customers'
--from Employees as E, Customers as C
--where E.City = C.City 
--group by E.City
--having Count(E.EmployeeID) > 2 and Count(C.CustomerID)>2

--5
--select City, COUNT(Customers.CompanyName)
--from Customers
--group by City

--6
--select E.EmployeeID,
--(
--	select R.FirstName + ' ' + R.LastName
--	from Employees as R
--	where R.EmployeeID = E.ReportsTo
--) as 'Employer'
--from Employees as E
--where ReportsTo is not null
--group by E.EmployeeID, E.ReportsTo