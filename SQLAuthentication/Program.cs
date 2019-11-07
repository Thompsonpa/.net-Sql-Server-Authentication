using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SQLAuthentication
{
    class Program
    {
        static void Main(string[] args)
        {
            //One Method you can do when connecting to a sql server
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost";   // update me, to sql server instance you are trying to connect too
                builder.UserID = "sa";              // update me, to userID or Username you are trying to connect with
                builder.Password = "your_password";      // update me, to to the password for the userID or username
                builder.InitialCatalog = "master"; // update me, Catalog is the DB name there can be multiple DBs in a server instance or DataSource
                Console.Write("Connecting to SQL Server method 1... ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            //Second Method you can use a connection string that is usually stored in the app.config file or web.config file od a .net application
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                Console.Write("Connecting to SQL Server method 2... ");
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");
                    connection.Close(); //closing connection so it does not stay open. Should always be closed after tasks or commands are run
                }
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            //to run a statement against the connection and get a result use the following after you open the connection with the DB
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                Console.Write("Connecting to SQL Server method 2... ");
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");
                    Console.WriteLine("Selecting all data in a table called users");
                    string SQLstatement = @"SELECT * FROM USERS";
                    using (SqlCommand command = new SqlCommand(SQLstatement, connection))
                    {
                        //Execute query into the datareader so we can see the data. You dont always need the SQL reader only when you want to retrieved data for your code
                        //Use command.ExecuteNonQuery(); and this is good for update statements or deletes no data return is needed.
                        SqlDataReader dr = command.ExecuteReader();

                        //Now lets check to see if we returned any data
                        if (dr.HasRows)
                        {
                            //data reader has rows so lets read the data
                            while (dr.Read())
                            {
                                //For this exmaple we will just pull the first 2 columns in each rows, there are better ways but this is just an example
                                Console.WriteLine("{0}\t{1}", dr.GetInt32(0), dr.GetString(1));
                            }
                        }
                        else
                        {
                            //no rows found
                            Console.WriteLine("No Rows Found");
                        }
                        dr.Close(); //close reader after use
                        Console.WriteLine("sql statement complete Done.");
                    }
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());


            }


        }
    }
}
