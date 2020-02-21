using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class DigitalTwinService
    {
        //Connection String
        private readonly string connectionString;

        public DigitalTwinService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Models.DigitalTwinResponse CreateDigitalTwin(Models.DigitalTwinRequest value, long organization)
        {
            Models.DigitalTwinResponse response = new Models.DigitalTwinResponse();

            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO digital_twins (id, name, description, security_token, digital_twin_model, organization, enabled, group, created) " +
                    "VALUES (@id, @name, @description, @security_token, @digital_twin_model, @organization, @enabled, @group, @created)";

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
                        command.Parameters.AddWithValue("@name", NpgsqlTypes.NpgsqlDbType.Varchar, value.Name);
                        command.Parameters.AddWithValue("@description", NpgsqlTypes.NpgsqlDbType.Varchar, value.Description);
                        command.Parameters.AddWithValue("@security_token", NpgsqlTypes.NpgsqlDbType.Bigint, securityToken);
                        command.Parameters.AddWithValue("@digital_twin_model", NpgsqlTypes.NpgsqlDbType.Bigint, value.DigitalTwinModel);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Parameters.AddWithValue("@enabled", NpgsqlTypes.NpgsqlDbType.Bigint, value.Enabled);
                        command.Parameters.AddWithValue("@group", NpgsqlTypes.NpgsqlDbType.Bigint, value.Group);
                        command.Parameters.AddWithValue("@created", NpgsqlTypes.NpgsqlDbType.Timestamp, utcDateTime);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        response.Status = "success";
                        response.Message = "digital twin created";
                        response.Id = unixDateTime;
                        response.SecurityToken = securityToken;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "digital twin creation failed");
                response.Status = "error";
                response.Message = "digital twin creation failed";
                response.Id = 0;
                response.SecurityToken = 0;
                return response;
            }
        }


        public List<Models.DigitalTwinLimitedResponse> GetAllDigitalTwins(long organization)
        {
            List<Models.DigitalTwinLimitedResponse> digitalTwinList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM digital_twins WHERE organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                //Initialize a Digital Twin 
                                Models.DigitalTwinLimitedResponse digitalTwin = null;
                                //Create a List to hold multiple Digital Twins
                                digitalTwinList = new List<Models.DigitalTwinLimitedResponse>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    digitalTwin = new Models.DigitalTwinLimitedResponse();

                                    digitalTwin.Id = Convert.ToInt64(reader["id"]);
                                    digitalTwin.Name = Convert.ToString(reader["name"]).Trim();
                                    digitalTwin.Description = Convert.ToString(reader["description"]).Trim();
                                    digitalTwin.SecurityToken = Convert.ToInt64(reader["security_token"]);
                                    digitalTwin.DigitalTwinModel = Convert.ToInt64(reader["digital_twin_model"]);
                                    digitalTwin.Enabled = Convert.ToInt64(reader["enabled"]);
                                    digitalTwin.Group = Convert.ToInt64(reader["group"]);
                                    digitalTwin.Created = Convert.ToDateTime(reader["created"]);

                                    //Add to List
                                    digitalTwinList.Add(digitalTwin);
                                }
                            }
                        }
                    }
                }
                return digitalTwinList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving digital twins");
                return digitalTwinList;
            }
        }


        public Models.DigitalTwinLimitedResponse GetDigitalTwin(long id, long organization)
        {
            Models.DigitalTwinLimitedResponse digitalTwin = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM digital_twins WHERE id = @id AND organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    digitalTwin = new Models.DigitalTwinLimitedResponse();

                                    digitalTwin.Id = Convert.ToInt64(reader["id"]);
                                    digitalTwin.Name = Convert.ToString(reader["name"]).Trim();
                                    digitalTwin.Description = Convert.ToString(reader["description"]).Trim();
                                    digitalTwin.SecurityToken = Convert.ToInt64(reader["security_token"]);
                                    digitalTwin.DigitalTwinModel = Convert.ToInt64(reader["digital_twin_model"]);
                                    digitalTwin.Enabled = Convert.ToInt64(reader["enabled"]);
                                    digitalTwin.Group = Convert.ToInt64(reader["group"]);
                                    digitalTwin.Created = Convert.ToDateTime(reader["created"]);
                                }
                            }
                        }
                    }
                }
                return digitalTwin;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving digital twin");
                return digitalTwin;
            }
        }






        public Models.Response DeleteDigitalTwin(long id, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "DELETE FROM digital_twins WHERE id = @id AND organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Return Success
                        response.Status = "success";
                        response.Message = "digital twin deleted";
                        response.Id = id;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "digital twin deletion failed");
                response.Status = "error";
                response.Message = "digital twin deletion failed";
                response.Id = id;
                return response;
            }
        }



    }
}
