using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel;
using System.Data;
using WebStore.Library.DTO;
using WebStore.Library.Models;

namespace eCommerce.API.Database
{
    public class MSSQLContext
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
    }
}
