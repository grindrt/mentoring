--2.4
use Northwind

--1
--select S.CompanyName
--from Suppliers as S
--where S.SupplierID in
--( 
--	select P.SupplierID
--	from Products as P
--	where P.UnitsInStock = 0
--)

--2
--select E.EmployeeID, E.FirstName + ' ' + E.LastName
--from Employees as E
--where (
--	select Count(O.EmployeeID) 
--	from Orders as O 
--	where o.EmployeeID = E.EmployeeID 
--	group by o.EmployeeID) > 150

--3
--select *
--from Customers as C
--where not exists
--	(
--		select o.CustomerID
--		from Orders as O
--		where O.CustomerID = C.CustomerID
--	)