--This is the query for creating items for the inventory 
--Please execute every step below to create the Database, Tables, and Procedures

--[STEP 1] CREATE DATABASE
CREATE DATABASE eCommerce

--[STEP 2] CREATE THE TABLE FOR ITEM
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

--[OPTIONAL STEP] CREATE SOME VALUES FOR THE ITEM TABLE
INSERT INTO ITEM ([Name], [Description], [Quantity], [Price]) VALUES ('[InsertBrandName] Phone', 'Phone manufactured by [InsertBrandName] in [InsertManufactureLocation]', 90, 889.99)
INSERT INTO ITEM ([Name], [Description], [Quantity], [Price]) VALUES ('[InsertBrandName] Headphones', 'Headphones manufactured by [InsertBrandName] in [InsertManufactureLocation]', 100, 155.88)
INSERT INTO ITEM ([Name], [Description], [Quantity], [Price]) VALUES ('[InsertBrandName] Tablet', 'Tablet manufactured by [InsertBrandName] in [InsertManufactureLocation]', 22, 335.55)

--[STEP 3] CREATE A SCHEMA FOR ITEM
CREATE SCHEMA Item /*Put CRUD inside of this schema*/

--[STEP 4] CREATE A PROCEDURE TO INSERT AN ITEM
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

--[STEP 5] CREATE A PROCEDURE TO UPDATE AN ITEM
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

--[STEP 6] CREATE A PROCEDURE TO DELETE AN ITEM
CREATE PROCEDURE Item.DeleteItem
@itemID int
AS
BEGIN
	DELETE ITEM where ID = @itemID
END

----------------------------
--ADDITIONAL FUNCTIONALITY--
----------------------------

--LOOK AT THE ITEMS IN THE TABLE ORDERED BY ID
select * from ITEM ORDER BY ID asc

--LOOK AT THE MOST IMPORTANT VALUES IN THE ITEM TABLE
select ID, Name, Description, Price from ITEM ORDER BY ID asc

--INSERT ITEM USING THE INSERTITEM PROCEDURE
declare @newID int
exec Item.InsertItem @Name = 'ExampleProduct'
, @Description = 'ExampleProduct Desc'
, @Quantity = 10
, @Price = 1.23
, @ID = @newID out
select @newID

--UPDATE ITEM AT A SPECIFIED ID
begin tran
update ITEM
set Name = '[InsertBrandName] UpdatedIten',
Description = 'This item was updated',
Price = 55.55
Where ID = 2
commit /*Commit if the transaction goes the way you want it to */

--DELETE AN INDIVIDUAL ITEM USING PROCEDURE
exec Item.DeleteItem @itemID=2

--DELETE AN INDIVIDUAL ITEM
begin tran 
DELETE ITEM 
WHERE ID = 2 /*Deletes the item at the specified id*/

--DROP THE WHOLE TABLE
begin tran /*Begin transaction, if something bad happens simply run the command "rollback." Call this before doing update or delete. If its all good, run "commit."*/
DROP TABLE ITEM /*Deletes table*/

sp_help ITEM