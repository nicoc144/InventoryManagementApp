using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel;
using System.Data;
using WebStore.Library.DTO;
using WebStore.Library.Models;

namespace eCommerce.API.Database
{
    public class  MSSQLContext() //manages database connections for inventory and shop, fetches the actual data from database
    {
        public Item AddItem(Item i) //ADD AND UPDATE
        {
            using (SqlConnection SqlClient = new SqlConnection(@"Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                if (i.ID == 0) //ADD
                {
                    using (SqlCommand cmd = SqlClient.CreateCommand())
                    {
                        var sql = $"Item.InsertItem"; //This is the sql code for inserting item
                        cmd.CommandText = sql; //Sets the text of the command to the InsertItem procedure sql code
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; //This is a stored procedure
                        cmd.Parameters.Add(new SqlParameter("Name", i.Name));
                        cmd.Parameters.Add(new SqlParameter("Description", i.Description));
                        cmd.Parameters.Add(new SqlParameter("Quantity", i.Quantity));
                        cmd.Parameters.Add(new SqlParameter("Price", i.Price));
                        cmd.Parameters.Add(new SqlParameter("IsBogo", i.IsBOGO));
                        cmd.Parameters.Add(new SqlParameter("Markdown", i.Markdown));

                        //All of the incrementing for the ID is handled by the 3 lines of code below
                        //Command.Parameters.Add(new SqlParameter("ID", ParameterDirection.Output, i.ID)); //Doesn't work
                        var id = new SqlParameter("ID", i.ID); //Make the parameter into an L value
                        id.Direction = ParameterDirection.Output; //Return the value to me (the database makes the value since its an id)
                        cmd.Parameters.Add(id); //Add L value

                        try
                        {
                            SqlClient.Open();
                            cmd.ExecuteNonQuery(); //User doesn't have to seee the value, it's in the variable which will be passed to the view
                                                   //This is useful for insert, update, or delete where the results don't need to be displayed
                                                   //to the user via the dataset.
                            SqlClient.Close();
                            i.ID = (int)id.Value;
                        }
                        catch (Exception ex) { }
                    }
                }
                else //UPDATE
                {
                    using (SqlCommand cmd = SqlClient.CreateCommand())
                    {
                        var sql = $"Item.UpdateItem"; //This is the sql code for inserting item
                        cmd.CommandText = sql; //Sets the text of the command to the InsertItem procedure sql code
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; //This is a stored procedure
                        cmd.Parameters.Add(new SqlParameter("Name", i.Name));
                        cmd.Parameters.Add(new SqlParameter("Description", i.Description));
                        cmd.Parameters.Add(new SqlParameter("Quantity", i.Quantity));
                        cmd.Parameters.Add(new SqlParameter("Price", i.Price));
                        cmd.Parameters.Add(new SqlParameter("ID", i.ID));
                        cmd.Parameters.Add(new SqlParameter("IsBogo", i.IsBOGO));
                        cmd.Parameters.Add(new SqlParameter("Markdown", i.Markdown));

                        try
                        {
                            SqlClient.Open();
                            cmd.ExecuteNonQuery();
                            SqlClient.Close();
                        }
                        catch (Exception ex) { }
                    }

                }
            }
            return i;
        }

        public List<Item> GetItems() //READ
        {
            var items = new List<Item>();
            using (SqlConnection SqlClient = new SqlConnection("Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                using (SqlCommand cmd = SqlClient.CreateCommand())
                {
                    //var sql = $"SELECT * FROM ITEM";
                    var sql = $"SELECT ID, REPLACE(name, '''','') as Name, Description, Price, Quantity, IsBOGO, Markdown FROM ITEM ORDER BY ID asc"; //The replace replaces the escaped single quote with nothing
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText= sql;

                    try
                    {
                        SqlClient.Open();
                        var reader = cmd.ExecuteReader(); //gives back a sql reader

                        while (reader.Read())
                        {
                            items.Add(new Item
                            {
                                ID = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                Description = (string)reader["Description"],
                                Price = (decimal)reader["Price"],
                                Quantity = (int)reader["Quantity"],
                                IsBOGO = (bool)reader["IsBOGO"],
                                Markdown = (double)reader["Markdown"]
                            });
                        }
                        SqlClient.Close();
                    }catch (Exception ex) { }
                }
            }
            return items;

        }

        public Item DeleteItem(int id) //DELETE
        {
            Item returnItem = GetItems().FirstOrDefault(i => i.ID == id); //Find the item in the list
           
            using (SqlConnection SqlClient = new SqlConnection(@"Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                using (SqlCommand cmd = SqlClient.CreateCommand())
                {
                    var sql = $"Item.DeleteItem";
                    cmd.CommandText = sql;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("itemID", id)); //Set the id you passed in to be the @itemID on the database

                    try
                    {
                        SqlClient.Open();
                        cmd.ExecuteNonQuery();
                        SqlClient.Close();

                    }
                    catch (Exception ex) { }
                }
                return returnItem;
            }
        }

        public List<ShoppingCart> GetCarts() //READ
        {
            var carts = new List<ShoppingCart>();
            using (SqlConnection SqlClient = new SqlConnection("Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                using (SqlCommand cmd = SqlClient.CreateCommand())
                {
                    var sql = $"SELECT CartID, REPLACE(CartName, '''','') as CartName, UserID " +
                              $"FROM CART ORDER BY CartID asc"; //The replace replaces the escaped single quote with nothing
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    try
                    {
                        SqlClient.Open();
                        var reader = cmd.ExecuteReader(); //gives back a sql reader

                        while (reader.Read())
                        {
                            carts.Add(new ShoppingCart
                            {
                                ShoppingCartID = (int)reader["CartID"],
                                ShoppingCartName = (string)reader["CartName"],
                                UserID = (int)reader["UserID"],
                                Contents = GetItemsForCart((int)reader["CartID"]) //get the items inside each cart
                            });
                        }
                        SqlClient.Close();
                    }
                    catch (Exception ex) { }

                }
            }
            return carts;
        }

        public ShoppingCart AddCart(ShoppingCart c) //ADD AND UPDATE
        {
            using (SqlConnection SqlClient = new SqlConnection(@"Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                if (c.ShoppingCartID == 0) //ADD
                {
                    using (SqlCommand cmd = SqlClient.CreateCommand())
                    {
                        var sql = $"Cart.AddCart"; //This is the sql code for inserting item
                        cmd.CommandText = sql; //Sets the text of the command to the InsertItem procedure sql code
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; //This is a stored procedure
                        cmd.Parameters.Add(new SqlParameter("CartName", c.ShoppingCartName));
                        cmd.Parameters.Add(new SqlParameter("UserID", c.UserID));

                        //All of the incrementing for the ID is handled by the 3 lines of code below
                        //Command.Parameters.Add(new SqlParameter("ID", ParameterDirection.Output, i.ID)); //Doesn't work
                        var id = new SqlParameter("CartID", c.ShoppingCartID); //Make the parameter into an L value
                        id.Direction = ParameterDirection.Output; //Return the value to me (the database makes the value since its an id)
                        cmd.Parameters.Add(id); //Add L value

                        try
                        {
                            SqlClient.Open();
                            cmd.ExecuteNonQuery(); //User doesn't have to seee the value, it's in the variable which will be passed to the view
                                                   //This is useful for insert, update, or delete where the results don't need to be displayed
                                                   //to the user via the dataset.
                            SqlClient.Close();
                            c.ShoppingCartID = (int)id.Value;
                        }
                        catch (Exception ex) { }
                    }
                }
                else //UPDATE
                {
                    using (SqlCommand cmd = SqlClient.CreateCommand())
                    {
                        var sql = $"Cart.UpdateCart"; //This is the sql code for inserting item
                        cmd.CommandText = sql; //Sets the text of the command to the InsertItem procedure sql code
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; //This is a stored procedure

                        cmd.Parameters.Add(new SqlParameter("CartName", c.ShoppingCartName));
                        cmd.Parameters.Add(new SqlParameter("UserID", c.UserID));
                        cmd.Parameters.Add(new SqlParameter("CartID", c.ShoppingCartID));
                        try
                        {
                            SqlClient.Open();
                            cmd.ExecuteNonQuery();
                            SqlClient.Close();
                        }
                        catch (Exception ex) { }
                    }

                }
            }
            return c;

        }
        public ShoppingCart DeleteCart(int id) //DELETE
        {
            ShoppingCart returnCart = GetCarts().FirstOrDefault(i => i.ShoppingCartID == id); //Find the item in the list

            using (SqlConnection SqlClient = new SqlConnection(@"Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                using (SqlCommand cmd = SqlClient.CreateCommand())
                {
                    var sql = $"Cart.DeleteCart";
                    cmd.CommandText = sql;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("cartID", id)); //Set the id you passed in to be the @itemID on the database

                    try
                    {
                        SqlClient.Open();
                        cmd.ExecuteNonQuery();
                        SqlClient.Close();

                    }
                    catch (Exception ex) { }
                }
            }
            return returnCart;
        }

        public List<ItemDTO> GetItemsForCart(int activeCartID) //READ FOR ITEMS IN CART
        {
            var items = new List<ItemDTO>();
            using (SqlConnection SqlClient = new SqlConnection("Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                using (SqlCommand cmd = SqlClient.CreateCommand())
                {
                    //var sql = $"SELECT * from CART c inner join CARTITEMS ci on c.CartID = ci.CartId left join ITEM i on ci.ItemID = i.ID where c.UserID = 1 and c.CartID = @cartID";
                    var sql = $"SELECT c.CartID as cartID, ci.CartItemsTableID as cartItemsTableID, i.Markdown as ItemMarkdown, i.IsBOGO as ItemIsBOGO, i.ID as itemID, i.[Name], i.[Description], ci.Price, ci.Quantity from CART c inner join CARTITEMS ci ON c.CartID = ci.CartID left join ITEM i ON ci.ItemID = i.ID WHERE c.UserID = 1 AND c.CartID = @cartID" ;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@cartID", activeCartID); //Safer than using string interpolation to pass in the activeCartID
                    cmd.CommandText = sql;

                    try
                    {
                        SqlClient.Open();
                        var reader = cmd.ExecuteReader(); //gives back a sql reader

                        while (reader.Read())
                        {
                            decimal calcPrice = 0;
                            double Markdown = ((double)reader["ItemMarkdown"])/100;
                            if(Markdown > 0)
                            {
                                calcPrice = Math.Round((decimal)reader["Price"] - ((decimal)reader["Price"] * (decimal)Markdown),2);
                            }
                            else
                            {
                                calcPrice = (decimal)reader["Price"];
                            }
                            items.Add(new ItemDTO
                            {
                                ID = (int)reader["ItemID"],
                                Name = (string)reader["Name"],
                                Description = (string)reader["Description"],
                                Price = calcPrice,
                                Quantity = (int)reader["Quantity"],
                                IsBOGO = (bool)reader["ItemIsBOGO"],
                                Markdown = (double)reader["ItemMarkdown"]
                            });
                        }
                        SqlClient.Close();
                    }
                    catch (Exception ex) { }
                }
            }
            return items;
        }

        public Item AddItemToCart(Item i, int activeCartID) //ADD / UPDATE FOR INDIVIDUAL ITEMS IN THE CART
        {
            ItemDTO? updateItem = GetItemsForCart(activeCartID).FirstOrDefault(item => item.ID == i.ID); //Check if the item already exists in this cart
            using (SqlConnection SqlClient = new SqlConnection(@"Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                if(updateItem == null) //If the item does not yet exist in the cart
                {
                    
                    using (SqlCommand cmd = SqlClient.CreateCommand())
                    {
                        var sql = $"CartItems.AddItemToCart"; //This is the sql code for inserting item
                        cmd.CommandText = sql; //Sets the text of the command to the InsertItem procedure sql code
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; //This is a stored procedure
                        cmd.Parameters.Add(new SqlParameter("ItemID", i.ID));
                        cmd.Parameters.Add(new SqlParameter("Quantity", i.Quantity));
                        cmd.Parameters.Add(new SqlParameter("Price", i.Price));
                        cmd.Parameters.Add(new SqlParameter("CartID", activeCartID)); //Need to set the cart ID for this item to the ID we passed in
                                                                                      //this is how the database knows what Shopping Cart this item belongs to.
                        var id = new SqlParameter("CartItemsTableID", SqlDbType.Int); //Make the parameter into an L value
                        id.Direction = ParameterDirection.Output; //Return the value to me (the database makes the value since its an id)
                        cmd.Parameters.Add(id); //Add L value
                        try
                        {
                            SqlClient.Open();
                            cmd.ExecuteNonQuery();
                            SqlClient.Close();
                        }
                        catch (Exception ex) { }
                    }
                }
                else
                {
                    using (SqlCommand cmd = SqlClient.CreateCommand())
                    {
                        var sql = $"CartItems.UpdateItemInCart"; //This is the sql code for inserting item
                        cmd.CommandText = sql; //Sets the text of the command to the InsertItem procedure sql code
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; //This is a stored procedure
                        cmd.Parameters.Add(new SqlParameter("ItemID", i.ID));
                        cmd.Parameters.Add(new SqlParameter("Quantity", i.Quantity));
                        cmd.Parameters.Add(new SqlParameter("Price", i.Price));
                        cmd.Parameters.Add(new SqlParameter("CartID", activeCartID));

                        try
                        {
                            SqlClient.Open();
                            cmd.ExecuteNonQuery();
                            SqlClient.Close();
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return i;
        }

        public Item DeleteCartItem(int itemID, int activeCartID) //DELETE FOR ITEMS IN CART
        {
            ItemDTO returnItem = GetItemsForCart(activeCartID).FirstOrDefault(i => i.ID == itemID); //Find the item in the list

            using (SqlConnection SqlClient = new SqlConnection(@"Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                using (SqlCommand cmd = SqlClient.CreateCommand())
                {
                    var sql = $"CartItems.DeleteItem";
                    cmd.CommandText = sql;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("CartItemsTableID", itemID)); //Set the id you passed in to be the @itemID on the database

                    try
                    {
                        SqlClient.Open();
                        cmd.ExecuteNonQuery();
                        SqlClient.Close();

                    }
                    catch (Exception ex) { }
                }
                return new Item(returnItem);
            }
        }
    }
}
