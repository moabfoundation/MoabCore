using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class UserService
    {
        //Connection String
        private readonly string connectionString;

        public UserService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Models.UserResponse CreateUser(Models.UserRequest value, long organization)
        {
            Models.UserResponse response = new Models.UserResponse();

            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO users (id, first_name, description, organization, email_address, password, security_token, role, user_group, agreed_to_terms_and_policies, enabled, last_name) " +
                    "VALUES (@id, @first_name, @description, @organization, @email_address, @password, @security_token, @role, @user_group, @agreed_to_terms_and_policies, @enabled, @last_name)";

                //Create UNIX Timestamp
                var utcDateTime = DateTime.UtcNow;
                var dto = new DateTimeOffset(utcDateTime);
                var unixDateTime = dto.ToUnixTimeMilliseconds();

                var random = new Random();
                int rnd = random.Next(1000000000, 2000000000);
                long securityToken = unixDateTime - rnd;

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, unixDateTime);
                        command.Parameters.AddWithValue("@first_name", NpgsqlTypes.NpgsqlDbType.Varchar, value.FirstName);
                        command.Parameters.AddWithValue("@last_name", NpgsqlTypes.NpgsqlDbType.Varchar, value.LastName);
                        command.Parameters.AddWithValue("@description", NpgsqlTypes.NpgsqlDbType.Varchar, value.UserDescription);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Parameters.AddWithValue("@email_address", NpgsqlTypes.NpgsqlDbType.Varchar, value.UserEmailAddress);
                        command.Parameters.AddWithValue("@password", NpgsqlTypes.NpgsqlDbType.Varchar, value.UserPassword);
                        command.Parameters.AddWithValue("@security_token", NpgsqlTypes.NpgsqlDbType.Bigint, securityToken);
                        command.Parameters.AddWithValue("@role", NpgsqlTypes.NpgsqlDbType.Bigint, value.Role);
                        command.Parameters.AddWithValue("@user_group", NpgsqlTypes.NpgsqlDbType.Bigint, value.UserGroup);
                        command.Parameters.AddWithValue("@agreed_to_terms_and_policies", NpgsqlTypes.NpgsqlDbType.Bigint, value.AgreeToTermsAndPolicies);
                        command.Parameters.AddWithValue("@enabled", NpgsqlTypes.NpgsqlDbType.Bigint, value.Enabled);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        response.Status = "success";
                        response.Message = "user created";
                        response.Id = unixDateTime;
                        response.SecurityToken = securityToken;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "user creation failed");
                response.Status = "error";
                response.Message = "user creation failed";
                response.Id = 0;
                response.SecurityToken = 0;
                return response;
            }
        }


        public List<Models.UserLimitedResponse> GetAllUsers(long organization)
        {
            List<Models.UserLimitedResponse> UserList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM users WHERE organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                //Initialize a User 
                                Models.UserLimitedResponse user = null;
                                //Create a List to hold multiple Users
                                UserList = new List<Models.UserLimitedResponse>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    user = new Models.UserLimitedResponse();

                                    user.Id = Convert.ToInt64(reader["id"]);
                                    user.FirstName = Convert.ToString(reader["first_name"]).Trim();
                                    user.LastName = Convert.ToString(reader["last_name"]).Trim();
                                    user.UserDescription = Convert.ToString(reader["description"]).Trim();
                                    user.UserEmailAddress = Convert.ToString(reader["email_address"]).Trim();
                                    user.Role = Convert.ToInt64(reader["role"]);
                                    user.UserGroup = Convert.ToInt64(reader["user_group"]);
                                    user.AgreeToTermsAndPolicies = Convert.ToInt64(reader["agreed_to_terms_and_policies"]);
                                    user.Enabled = Convert.ToInt64(reader["enabled"]);
                                    //Add to List
                                    UserList.Add(user);
                                }
                            }
                        }
                    }
                }
                return UserList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving users");
                return UserList;
            }
        }


        public Models.UserLimitedResponse GetUser(long id, long organization)
        {
            Models.UserLimitedResponse user = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM users WHERE id = @id AND organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    user = new Models.UserLimitedResponse();

                                    user.Id = Convert.ToInt64(reader["id"]);
                                    user.FirstName = Convert.ToString(reader["first_name"]).Trim();
                                    user.LastName = Convert.ToString(reader["last_name"]).Trim();
                                    user.UserDescription = Convert.ToString(reader["description"]).Trim();
                                    user.UserEmailAddress = Convert.ToString(reader["email_address"]).Trim();
                                    user.Role = Convert.ToInt64(reader["role"]);
                                    user.UserGroup = Convert.ToInt64(reader["user_group"]);
                                    user.AgreeToTermsAndPolicies = Convert.ToInt64(reader["agreed_to_terms_and_policies"]);
                                    user.Enabled = Convert.ToInt64(reader["enabled"]);
                                }
                            }
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving user");
                return user;
            }
        }






        public Models.Response DeleteUser(long id, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "DELETE FROM users WHERE id = @id AND organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            //Return Success
                            response.Status = "success";
                            response.Message = "user deleted";
                            response.Id = id;
                        }
                        else
                        {
                            //Return Failure
                            response.Status = "error";
                            response.Message = "user deletion failed";
                            response.Id = id;
                        }

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "user deletion failed");
                response.Status = "error";
                response.Message = "user deletion failed";
                response.Id = id;
                return response;
            }
        }



    }
}
