using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStore.Library.Models;

namespace eCommerce.API.Database
{
    public class MSSQLContext
    {
        Item AddItem(Item i)
        {
            using (SqlConnection SqlClient = new SqlConnection("Server = localdb; Database = eCommerce; Trusted_Connection = yes;"))
            {
                using (SqlCommand Command = SqlClient.CreateCommand())
                {
                    /*
                        exec Item.InsertItem @Name = 'ExampleProduct'
                        , @Description = 'ExampleProduct Desc'
                        , @Quantity = 10
                        , @Price = 1.23
                        , @ID = @newID out
                        select @newID
                    */
                    var sql = $"Item.InsertItem";
                    Command.CommandText = sql;
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.Add(new SqlParameter("Name", i.Name));
                    Command.Parameters.Add(new SqlParameter("Description", i.Description));
                    Command.Parameters.Add(new SqlParameter("Quantity", i.Quantity));
                    Command.Parameters.Add(new SqlParameter("Price", i.Price));

                    //All of the incrementing for the ID is handled by the 3 lines of code below
                    //Command.Parameters.Add(new SqlParameter("ID", ParameterDirection.Output, i.ID)); //Doesn't work
                    var id = new SqlParameter("ID", i.ID); //Make the parameter into an L value
                    id.Direction = ParameterDirection.Output; //Return the value to me (the database makes the value since its an id)
                    Command.Parameters.Add(id); //Add L value

                    SqlClient.Open();
                    Command.ExecuteNonQuery(); //User doesn't have to seee the value, it's in the variable which will be passed to the view
                                               //This is useful for insert, update, or delete where the results don't need to be displayed
                                               //to the user via the dataset.
                    SqlClient.Close();
                }
            }
            return i;
        }

        Item UpdateItem(Item i) //TODO, Probably won't work yet
        {
            using (SqlConnection SqlClient = new SqlConnection("Server = localdb; Database = eCommerce; Trusted_Connection = yes;"))
            {
                using (SqlCommand Command = SqlClient.CreateCommand())
                {
                    /*
                        exec Item.InsertItem @Name = 'ExampleProduct'
                        , @Description = 'ExampleProduct Desc'
                        , @Quantity = 10
                        , @Price = 1.23
                        , @ID = @newID out
                        select @newID
                    */
                    var sql = $"Item.InsertItem";
                    Command.CommandText = sql;
                    Command.CommandType = System.Data.CommandType.StoredProcedure;
                    Command.Parameters.Add(new SqlParameter("Name", i.Name));
                    Command.Parameters.Add(new SqlParameter("Description", i.Description));
                    Command.Parameters.Add(new SqlParameter("Quantity", i.Quantity));
                    Command.Parameters.Add(new SqlParameter("Price", i.Price));
                    Command.Parameters.Add(new SqlParameter("ID", i.ID));

                    //Don't need to increment the ID

                    SqlClient.Open();
                    Command.ExecuteNonQuery(); //User doesn't have to seee the value, it's in the variable which will be passed to the view
                                               //This is useful for insert, update, or delete where the results don't need to be displayed
                                               //to the user via the dataset.
                    SqlClient.Close();
                }
            }
            return i;
        }
    
    }
}
