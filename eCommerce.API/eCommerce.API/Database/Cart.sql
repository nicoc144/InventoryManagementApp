--This is the query for creating shopping carts and users
--If you haven't completed the steps in item.sql yet, please do so now before proceeding

--[STEP 1] CREATE THE TABLE FOR USERS
CREATE TABLE USERS(
	UserID INT IDENTITY(1,1) NOT NULL
	, Username varchar(255) NOT NULL
	, PasswordHash varchar(max) NOT NULL /*If you actually wanted a password hash you would use varbinary instead of varchar and use a security token given by some service*/
)

--[STEP 2] CREATE THE TABLE FOR CARTS
CREATE TABLE CART(
	CartID INT IDENTITY(1,1) NOT NULL
	, CartName nvarchar(255) NULL
	, UserID INT
)

--[STEP 3] CREATE THE TABLE FOR CART ITEMS
CREATE TABLE CARTITEMS(
	CartItemsTableID INT IDENTITY(1,1) NOT NULL /*ID for the CARTITEMS table */
	, ItemID INT NOT NULL /*ID of the item in the cart*/
	, Quantity int NULL /*Quantity of the item in the cart*/
	, Price numeric(10,2) NULL /*Price of the item in the cart (Can be different from the price in the inventory if there's a markdown or BOGO)"*/
	, CartID int NOT NULL /*ID of the cart the item is apart of*/
)
	
--[STEP 4] CREATE A DEFAULT USER
INSERT INTO USERS(Username, PasswordHash) VALUES ('default', ';1234567890')

--[STEP 5] CREATE THE DEFAULT CART (This is required)
INSERT INTO CART(CartName, UserID) VALUES('DefaultCart', 1)

--[STEP 6] CREATE SCHEMA FOR SHOPPING CART (SO YOU CAN HAVE MULTIPLE CARTS / WISHLISTS)
CREATE SCHEMA Cart /*Put CREATE UPDATE DELETE in this schema for Cart */

--[STEP 7] CREATE PROCEDURE ADD CART (THIS ALLOWS YOU TO MAKE MULTIPLE CARTS / WISHLISTS IN THE SHOP VIEW WITH DISTINCT NAME)
CREATE PROCEDURE Cart.AddCart
@CartName nvarchar(255)
, @UserID int
, @CartID int output
AS
BEGIN
	INSERT INTO CART(CartName, UserID) /*Don't include ID because the database is going to give it to you */
	VALUES(@CartName, 1) /*Can change this later if integrating multiple users*/

	SET @CartID = SCOPE_IDENTITY()
END

--[STEP 8] CREATE PROCEDURE UPDATE CART (ALLOWS YOU TO UPDATE CART / WISHLIST NAME)
CREATE PROCEDURE Cart.UpdateCart
@CartName nvarchar(255)
, @UserID int
, @CartID int 
AS
BEGIN
	UPDATE CART
	SET 
		CartName = @CartName
		, UserID = @UserID
	WHERE
		CartID = @CartID
END

--[STEP 9] CREATE PROCEDURE DELETE CART (ALLOWS YOU TO DELETE A CART / WISHLIST)
CREATE PROCEDURE Cart.DeleteCart @cartID int
AS
DELETE CART where CartID = @cartID

--[STEP 10] CREATE SCHEMA FOR THE ITEMS IN THE CART
CREATE SCHEMA CartItems

--[STEP 11] CREATE ADD FOR THE ITEMS IN THE CART
CREATE PROCEDURE CartItems.AddItemToCart
@CartID int
, @ItemID int
, @Quantity int
, @Price numeric(10,2)
, @CartItemsTableID int output
AS
BEGIN
	INSERT INTO CARTITEMS(CartID, ItemID, Quantity, Price) 
	VALUES(@CartID, @CartID, @Quantity, @Price)

	SET @CartItemsTableID = SCOPE_IDENTITY()
END

----------------------------
--ADDITIONAL FUNCTIONALITY--
----------------------------

--DISPLAY CARTS
SELECT CartID, REPLACE(CartName, '''','') as CartName, UserID FROM CART ORDER BY CartID asc

--LOOK AT THE CARTS, CART ITEMS, AND USERS
select * from CART c /*Select ALL from CART c*/
inner join CARTITEMS ci on c.CartID = ci.CartId /*Inner join CART with CARTITEMS where their CartID's are equal*/
left join ITEM i on ci.ItemID = i.ID /*Left join ITEM with CARTITEMS where their ItemID's are equal*/
where c.UserID = 1  
/*On clause describes how two tables are related, and controls which rows from the two tables are paired together*/
/*Where clause is used to filter rows after the tables are joined, controls which rows are displayed*/

--GET() ITEMS, this is the sql code used in the API get() statement for cart items
SELECT c.CartID as cartID,
i.ID as itemID, i.[Name], i.[Description], ci.Price, ci.Quantity
from CART c
inner join CARTITEMS ci ON c.CartID = ci.CartID
left join ITEM i ON ci.ItemID = i.ID
WHERE c.UserID = 1 AND c.CartID = @cartID

--CREATE MORE CARTS
INSERT INTO CART(CartName, UserID) VALUES('Wishlist1', 1)
INSERT INTO CART(CartName, UserID) VALUES('Wishlist2', 1)

--CREATE AN ITEM INSIDE THE DEFAULT CART (MAKE SURE YOU AT LEAST HAVE ONE ITEM IN THE INVENTORY ITEMS LIST)
INSERT INTO CARTITEMS(ItemID, Quantity, Price, CartID) VALUES(1, 20, 125.55, 1) /*Adds Item ID 1 into the cart*/

--Create a new cart
declare @newID int
exec Cart.AddCart @CartName = 'NewCart1'
, @UserID = 1
, @CartID = @newID out
select @newID

--CREATE CART ITEM LINKS (FOR MULTI SELECT OR DRAG AND DROP)
CREATE TABLE CARTITEMLINKS( /*In order to have a many to many relationship with cart items this is needed*/
	ID INT IDENTITY(1,1) NOT NULL
	, CartID int NOT NULL /*Cart you want to add to*/
	, CartItemID int NOT NULL /*Product you want to add*/
)

--INSERT VALUE INTO CARTITEMS GIVING THE CART ID, ITEM ID, ETC
INSERT INTO CARTITEMS(ItemInCartID, Quantity, Price, CartID) VALUES(1, 20, 2.21, 1)
INSERT INTO CARTITEMS(ItemInCartID, Quantity, Price, CartID) VALUES(2, 330, 22.51, 1)

--ADD ITEM ID 1 INTO DEFAULT CART
declare @newID int
exec CartItems.AddItemToCart @CartID = 2
, @ItemID = 2
, @Quantity = 3
, @Price = 5.00
, @CartItemsTableID = @newID out
select @newID

/*Display everything from the carts and cart items table*/
SELECT * FROM Cart c full outer join CartItems ci on ci.CartID = c.CartID

--CREATE A VIEW FOR THE CART VIEW
create view CartView
AS
select c.CartID as CartID 
, CartName
, UserID
, ci.CartItemsID as CartItemID
, ci.Quantity as CartPrice
, i.ID as ProductID
, i.Name as ProductName
, i.Description as ProductDescription
, i.Quantity as InventoryQuantity
, i.Price as InventoryPrice
from CART c
inner join CARTITEMS ci on c.CartID = ci.CartId
left join ITEM i on ci.ItemID = i.ID

--USE THE CART VIEW YOU CREATED ABOVE
select * from CartView

--DROP PROCEDURE
begin tran
DROP PROCEDURE DeleteCart

--CREATE A TABLE WHICH ONLY EXISTS FOR THIS SESSION/PROCESS USING # (Can't call this table in item.sql)
select * into #tempTable from CartView /*You can use temp tables to keep other processes on the sql server from interferring with your aggregation, read only processes so theyre also faster*/

--USE THE TEMP TABLE YOU CREATED ABOVE
select * from #tempTable

--SIMILAR TO TEMP TABLE (COMMON TABLE EXPRESSION)
;with MyCart (CartId, CartName) AS /*Basically works the same as how a select statement works, executes once but you can set it to a name*/
	select CartID, CartName from CART
) select * from MyCart

--EXECUTE PROCEDURE DELETE CART
exec DeleteCart @cartID = 2

--VALUES FOR USERS
Select * from Users u

--VALUES FOR CART
Select * from Cart c

--VALUES FOR CARTITEMS
Select * from CartItems ci

begin tran
drop table USERS
/*commit*/

begin tran
drop table CART
/*commit*/

begin tran
delete USERS
where UserID = 2

begin tran
drop table CARTITEMS
/*commit*/

--SHOW VALUES FOR THE CARTS AND ITEMS AT USERID 1 (doesn't work but still here to explain some concepts)
Select distinct CartName from CART c /*"distinct" specifies to return the list of carts for the user specified*/
	left join ITEM i on c.ItemID = i.ID /*Left join here ensures that all of the values left of the join statement (i.e. all of the values in the cart) are being displayed including null values*/
	inner join USERS u on u.ID = c.UserID /*Inner join here prevents null values from Cart and Users being displayed*/
	WHERE UserID = 1

--THIS IS BAD, DO NOT USE
/*This is bad because it can lead to redundancy where if you make an edit in the inventory it might not 
update for the cart, also not as scalable as having a separate table for cart items */
CREATE TABLE CART(
	CartID INT IDENTITY(1,1) NOT NULL
	, CartName nvarchar(255) NULL
	, ItemID INT
	, Quantity int NULL
	, Price numeric(10,2) NULL
	, UserID INT
)

/*This is the insert statement for the table above*/
INSERT INTO CART(ItemID, Quantity, Price, CartName, UserID) VALUES (22, 4, 288, 'Wishlist1', 1)