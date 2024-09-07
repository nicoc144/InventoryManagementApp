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
                    var sql = $"SELECT ID, REPLACE(name, '''','') as Name, Description, Price, Quantity FROM ITEM ORDER BY ID asc"; //The replace replaces the escaped single quote with nothing
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
                                Quantity = (int)reader["Quantity"]
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
                    var sql = $"SELECT CartID, REPLACE(CartName, '''','') as CartName, UserID FROM CART ORDER BY CartID asc"; //The replace replaces the escaped single quote with nothing
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
                                UserID = (int)reader["UserID"]
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

        public List<Item> GetItemsForCart(int activeCartID) //READ
        {
            var items = new List<Item>();
            using (SqlConnection SqlClient = new SqlConnection("Server=DESKTOP-52M94CU;Database=eCommerce;Trusted_Connection=yes;TrustServerCertificate=True"))
            {
                using (SqlCommand cmd = SqlClient.CreateCommand())
                {
                    //var sql = $"SELECT ID, REPLACE(name, '''','') as Name, Description, Price, Quantity FROM ITEM ORDER BY ID asc"; //The replace replaces the escaped single quote with nothing
                    var sql = $"SELECT * from CART c inner join CARTITEMS ci on c.CartID = ci.CartId left join ITEM i on ci.ItemID = i.ID where c.UserID = 1 and c.CartID = @cartID";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@cartID", activeCartID); //Safer than using string interpolation to pass in the activeCartID
                    cmd.CommandText = sql;
                    

                    try
                    {
                        SqlClient.Open();
                        var reader = cmd.ExecuteReader(); //gives back a sql reader

                        while (reader.Read())
                        {
                            items.Add(new Item
                            { 
                                ID = (int)reader["i.ID"],
                                Name = (string)reader["i.Name"],
                                Description = (string)reader["i.Description"],
                                Price = (decimal)reader["ci.Price"],
                                Quantity = (int)reader["ci.Quantity"]
                            });
                        }
                        SqlClient.Close();
                    }
                    catch (Exception ex) { }
                }
            }
            return items;

        }
    }
}
