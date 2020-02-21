using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class LoginService
    {
        //Connection String
        private readonly string connectionString;

        public LoginService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Models.LoginResponse Login(string emailAddress, string password)
        {
            Models.LoginResponse loginResponse = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT id, security_token FROM users WHERE email_address = @email_address AND password = @password";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@email_address", NpgsqlTypes.NpgsqlDbType.Varchar, emailAddress);
                        command.Parameters.AddWithValue("@password", NpgsqlTypes.NpgsqlDbType.Varchar, password);
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    loginResponse = new Models.LoginResponse();

                                    loginResponse.Id = Convert.ToInt64(reader["id"]);
                                    loginResponse.SecurityToken = Convert.ToInt64(reader["security_token"]);
                                }
                            }
                        }
                    }
                }
                return loginResponse;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving id and security token");
                return loginResponse;
            }
        }


    }
}
