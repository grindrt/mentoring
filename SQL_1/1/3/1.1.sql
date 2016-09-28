use Northwind

GO
if exists (select * from sysobjects where id = object_id('dbo.CreditCard') and sysstat & 0xf = 3)
	drop table "dbo"."CreditCard"
GO
create table "CreditCard" (
	"CreditCardID" int identity not null,
	"Date" date not null,
	"CardHolder" varchar(50),
	"EmployeeID" int null,
	CONSTRAINT "FK_CreditCard_Employees" FOREIGN KEY 
	(
		"EmployeeID"
	) REFERENCES "dbo"."Employees" (
		"EmployeeID"
	)
)