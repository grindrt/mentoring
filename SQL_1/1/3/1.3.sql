use Northwind

IF NOT EXISTS (SELECT * FROM sys.columns 
	WHERE name = N'SinceDate' and object_id = OBJECT_ID(N'[dbo].[Customers]'))
BEGIN
alter table [dbo].[Customers]
	add SinceDate date null;
END
	
execute sp_rename
	@objname = N'[dbo].[Region]',
	@newname = N'Regions'