CREATE TABLE [dbo].[CreditCard]
(
	[CreditCardId] INT NOT NULL PRIMARY KEY, 
    [ExpirationDate] DATE NOT NULL, 
    [CardHolder] VARCHAR(50) NULL, 
	[EmployeeID] int NULL,
	CONSTRAINT [FK_CreditCard_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID])
)
