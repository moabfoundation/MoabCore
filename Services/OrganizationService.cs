using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class OrganizationService
    {
        //Connection String
        private readonly string connectionString;

        public OrganizationService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Models.OrganizationResponse Initialize(Models.OrganizationRequest value)
        {
            Models.OrganizationResponse response = new Models.OrganizationResponse();

            bool existingOrganization;
            long organization;
            long thingGroup;
            long user;
            long securityToken = 0;

            try
            {
                //Ensure user agreed to the terms and policies before creating a new Organization
                if (value.AgreeToTermsAndPolicies != 0)
                {
                    //Test to see if an Organization of the same name already exists
                    if (existingOrganization = TestForExistingOrganization(value))
                    {
                        //Log Failure
                        response.Status = "error";
                        response.Message = "organization already exists";
                        response.Id = 0;
                        response.SecurityToken = 0;
                        return response;
                    }
                    else
                    {
                        //Create a new Organization
                        if((organization = CreateOrganization(value)) != 0)
                        {
                            if ((thingGroup = CreateThingGroup(organization)) != 0)
                            {
                                //Create a User with Creator Role priveleges within the new Organization
                                if ((user = CreateUser(value, organization, out securityToken)) != 0)
                                {

                                    //Log Success
                                    response.Status = "success";
                                    response.Message = "initialization succeeded";
                                    response.Id = user;
                                    response.SecurityToken = securityToken;
                                    return response;
                                }
                                else
                                {
                                    //Log Failure
                                    response.Status = "error";
                                    response.Message = "user was not created";
                                    response.Id = 0;
                                    response.SecurityToken = 0;
                                    return response;
                                }
                            }
                            else
                            {
                                //Log Failure
                                response.Status = "error";
                                response.Message = "thing group was not created";
                                response.Id = 0;
                                response.SecurityToken = 0;
                                return response;
                            }
                        }
                        else
                        {
                            //Log Failure
                            response.Status = "error";
                            response.Message = "organization was not created";
                            response.Id = 0;
                            response.SecurityToken = 0;
                            return response;
                        }
                    }
                }
                else
                {
                    //Log Failure
                    response.Status = "error";
                    response.Message = "user did not agree to terms and policies";
                    response.Id = 0;
                    response.SecurityToken = 0;
                    return response;
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "initialization failed");

                response.Status = "error";
                response.Message = "initialization failed";
                response.Id = 0;
                return response;
            }
        }

        private bool TestForExistingOrganization(Models.OrganizationRequest value)
        {
            try
            {
                //SQL Statement
                var sqlString = "SELECT COUNT(*) FROM organizations WHERE name = @name";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@name", NpgsqlTypes.NpgsqlDbType.Varchar, value.OrganizationName);
                        command.Prepare();
                        var count = Convert.ToInt64(command.ExecuteScalar());
                        if (count >= 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "organization name verification failed");
                return false;
            }
        }

        private long CreateOrganization(Models.OrganizationRequest value)
        {
            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO organizations (id, name, description) " +
                    "VALUES (@id, @name, @description)";

                //Create UNIX Timestamp
                var utcDateTime = DateTime.UtcNow;
                var dto = new DateTimeOffset(utcDateTime);
                var unixDateTime = dto.ToUnixTimeMilliseconds();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, unixDateTime);
                        command.Parameters.AddWithValue("@name", NpgsqlTypes.NpgsqlDbType.Varchar, value.OrganizationName);
                        command.Parameters.AddWithValue("@description", NpgsqlTypes.NpgsqlDbType.Varchar, value.OrganizationDescription);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        return unixDateTime;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "organization creation failed");
                return 0;
            }
        }


        private long CreateUserGroup(long organization)
        {
            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO user_groups (id, name, description, organization) " +
                    "VALUES (@id, @name, @description, @organization)";

                //Create UNIX Timestamp
                var utcDateTime = DateTime.UtcNow;
                var dto = new DateTimeOffset(utcDateTime);
                var unixDateTime = dto.ToUnixTimeMilliseconds();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, unixDateTime);
                        command.Parameters.AddWithValue("@name", NpgsqlTypes.NpgsqlDbType.Varchar, "Global");
                        command.Parameters.AddWithValue("@description", NpgsqlTypes.NpgsqlDbType.Varchar, "Global Group");
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        return unixDateTime;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "user group creation failed");
                return 0;
            }
        }

        private long CreateThingGroup(long organization)
        {
            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO thing_groups (id, name, description, organization) " +
                    "VALUES (@id, @name, @description, @organization)";

                //Create UNIX Timestamp
                var utcDateTime = DateTime.UtcNow;
                var dto = new DateTimeOffset(utcDateTime);
                var unixDateTime = dto.ToUnixTimeMilliseconds();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, unixDateTime);
                        command.Parameters.AddWithValue("@name", NpgsqlTypes.NpgsqlDbType.Varchar, "Global");
                        command.Parameters.AddWithValue("@description", NpgsqlTypes.NpgsqlDbType.Varchar, "Global Group");
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        return unixDateTime;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "thing group creation failed");
                return 0;
            }
        }


        private long CreateUser(Models.OrganizationRequest value, long organization, out long securityTokenOut)
        {
            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO users (id, first_name, description, organization, email_address, password, security_token, role, agreed_to_terms_and_policies, enabled, last_name) " +
                    "VALUES (@id, @first_name, @description, @organization, @email_address, @password, @security_token, @role, @agreed_to_terms_and_policies, @enabled, @last_name)";

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
                        command.Parameters.AddWithValue("@role", NpgsqlTypes.NpgsqlDbType.Bigint, 1);
                        command.Parameters.AddWithValue("@agreed_to_terms_and_policies", NpgsqlTypes.NpgsqlDbType.Bigint, value.AgreeToTermsAndPolicies);
                        command.Parameters.AddWithValue("@enabled", NpgsqlTypes.NpgsqlDbType.Bigint, 1);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        securityTokenOut = securityToken;
                        return unixDateTime;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "user creation failed");
                securityTokenOut = 0;
                return 0;
            }
        }




        /**
        public List<Models.Organization> GetAllOrganizations(long organization)
        {
            List<Models.Organization> OrganizationList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM organizations WHERE organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@organization", organization);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                //Initialize an Organization 
                                Models.Organization org = null;
                                //Create a List to hold multiple Organizations
                                OrganizationList = new List<Models.Organization>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    org = new Models.Organization();

                                    org.Id = Convert.ToInt64(reader["id"]);
                                    org.Name = Convert.ToString(reader["name"]).Trim();
                                    org.Description = Convert.ToString(reader["description"]).Trim();

                                    //Add to List
                                    OrganizationList.Add(org);
                                }
                            }
                        }
                    }
                }
                return OrganizationList;
            }
            catch (Exception ex)
            {
                //Log Exception
                _logger.LogError(ex, "group creation failed");

                return OrganizationList;
            }
        }
        **/


        /**
        public Models.Organization GetOrganization(long id)
        {
            Models.Organization organization = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM organizations WHERE id = @id";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    organization = new Models.Organization();

                                    organization.Id = Convert.ToInt64(reader["id"]);
                                    organization.Name = Convert.ToString(reader["name"]).Trim();
                                    organization.Description = Convert.ToString(reader["description"]).Trim();
                                }
                            }
                        }
                    }
                }
                return organization;
            }
            catch (Exception ex)
            {
                //Log Exception
                return organization;
            }
        }
        **/





        public Models.Response DeleteOrganization(long id)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "DELETE FROM organizations WHERE id = @id";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Return Success
                        response.Status = "success";
                        response.Message = "organization deleted";
                        response.Id = id;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "organization deletion failed");

                response.Status = "error";
                response.Message = "organization deletion failed";
                response.Id = id;
                return response;
            }
        }



    }
}
