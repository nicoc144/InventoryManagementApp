CREATE DATABASE eCommerce

/* 
public string? Name { get; set; }
public string? Description { get; set; }
public decimal Price { get; set; }
public int ID { get; set; }
public int Quantity { get; set; }
public bool IsBOGO { get; set; }
public double Markdown {  get; set; }
public decimal TotalForThisItem { get; set; }
public string? ExpirationDate { get; set; }
public string? SellByDate { get; set; }
public string? AllergyWarning { get; set; }
*/

CREATE TABLE ITEM(
	ID int IDENTITY(1,1) NOT NULL, /*"IDENTITY(1,1) means ID will start at 1 and increase in an increment of 1 */
	[Name] nvarchar(255) NULL, /*"Name" is a reserved word, that's why it's in square brackets
								"varchar" stands for variable character array, the "var" part means that the array can be up to (255) chars or less, and if less reclaim the space
								"varchar" only supports ASCII, "nvarchar" supports the extended character set like unicode
								"NULL" means that name can be null */
	[Description] nvarchar(max) NULL, /*MAX means allocate as much space for the char array as the addressing scheme allows (~2 bil characters) */
	Quantity int NULL,
	Price numeric(10,2) NULL, /*"numeric(10,2)" is like a double/float/decimal. Numeric(10,2) means the max number of digits is 10 (including after decimal) and the precision is 2 */
	IsBOGO bit NULL, /*"bit" represents bool values */
	Markdown numeric(5,2) NULL,
	TotalForThisItem numeric(10,2) NULL,
	ExpirationDate nvarchar(255) NULL,
	SellByDate nvarchar(255) NULL,
	AllergyWarning nvarchar(255) NULL,
)

DROP TABLE ITEM /*Deletes table (Commented out to prevent accidental execution) */

select * from ITEM ORDER BY ID asc

select ID, Name, Description, Price from ITEM ORDER BY ID asc

sp_help ITEM

INSERT INTO ITEM ([Name], [Description], [Quantity], [Price]) VALUES ('I Pod Touch', 'I Pod Touch 16GB', 90, 125.55)
INSERT INTO ITEM ([Name], [Description], [Quantity], [Price]) VALUES ('I Pod Nano', 'I Pod Nano 16GB', 90, 75.55)
INSERT INTO ITEM ([Name], [Description], [Quantity], [Price]) VALUES ('I Pad Mini', 'I Pad Mini 64GB', 90, 335.55)

begin tran /*Begin transaction, if something bad happens simply run the command "rollback." Call this before doing update or delete. If its all good, run "commit."*/

DELETE ITEM 
WHERE ID = 2 /*Deletes the item at the specified id*/

begin tran
update ITEM
set Name = 'I Pod Shuffle',
Description = 'I Pod Shuffle 8GB',
Price = 55.55
Where ID = 2
commit

CREATE SCHEMA Item /*Put CRUD inside of this schema*/

CREATE PROCEDURE Item.InsertItem /*Create a procedure with (name of schema).(name of stored procedure) format */
@Name nvarchar(255)
, @Description nvarchar(MAX)
, @Quantity int
, @Price numeric(10,2)
, @ID int output /* The output keyword makes ID a reference parameter, which says that when you call this proecture you get a value put 
into the parameter rather than putting a value in the parameter to send to the store procedure */
AS
BEGIN
	INSERT INTO ITEM([Name], [Description], Quantity, Price) /*Don't include ID because the database is going to give it to you */
	VALUES(@Name, @Description,@Quantity, @Price)

	SET @ID = SCOPE_IDENTITY()
END

declare @newID int
exec Item.InsertItem @Name = 'ExampleProduct'
, @Description = 'ExampleProduct Desc'
, @Quantity = 10
, @Price = 1.23
, @ID = @newID out
select @newID

CREATE PROCEDURE Item.UpdateItem
@Name nvarchar(255)
, @Description nvarchar(MAX)
, @Quantity int
, @Price numeric(10,2)
, @ID int /* This is no longer output because this will be specified by the user */
AS
BEGIN
	UPDATE ITEM
	SET 
		Name = @Name
		, Description = @Description
		, Quantity = @Quantity
		, Price = @Price
	WHERE
		ID = @ID
END