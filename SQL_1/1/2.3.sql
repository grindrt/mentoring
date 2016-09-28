--2.3
use Northwind

--1
--select C.CustomerID, C.Region, R.RegionDescription
--from Customers as C 
--	inner join Region as R
--	on R.RegionDescription = 'Western' and C.Region is not null

--2
--select C.CompanyName, Count(O.CustomerID) as 'Orders'
--from Customers as C 
--	left join Orders as O
--	on C.CustomerID = O.CustomerID
--group by C.CompanyName
--order by Orders