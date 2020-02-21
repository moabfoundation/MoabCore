using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class DigitalTwinModelService
    {
        //Connection String
        private readonly string connectionString;

        public DigitalTwinModelService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Models.Response CreateDigitalTwinModel(Models.DigitalTwinModelRequest value, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO digital_twin_models (id, name, description, organization, version) " +
                    "VALUES (@id, @name, @description, @organization, @version)";

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
                        command.Parameters.AddWithValue("@name", NpgsqlTypes.NpgsqlDbType.Varchar, value.Name);
                        command.Parameters.AddWithValue("@description", NpgsqlTypes.NpgsqlDbType.Varchar, value.Description);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Parameters.AddWithValue("@version", NpgsqlTypes.NpgsqlDbType.Bigint, value.Version);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        response.Status = "success";
                        response.Message = "digital twin model created";
                        response.Id = unixDateTime;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "digital twin model creation failed");
                response.Status = "error";
                response.Message = "digital twin model creation failed";
                response.Id = 0;
                return response;
            }
        }


        public List<Models.DigitalTwinModelLimitedResponse> GetAllDigitalTwinModels(long organization)
        {
            List<Models.DigitalTwinModelLimitedResponse> digitalTwinModelList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM digital_twin_models WHERE organization = @organization";

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
                                //Initialize a Digital Twin Model 
                                Models.DigitalTwinModelLimitedResponse digitalTwinModel = null;
                                //Create a List to hold multiple Digital Twin Models
                                digitalTwinModelList = new List<Models.DigitalTwinModelLimitedResponse>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    digitalTwinModel = new Models.DigitalTwinModelLimitedResponse();

                                    digitalTwinModel.Id = Convert.ToInt64(reader["id"]);
                                    digitalTwinModel.Name = Convert.ToString(reader["name"]).Trim();
                                    digitalTwinModel.Description = Convert.ToString(reader["description"]).Trim();
                                    digitalTwinModel.Version = Convert.ToInt64(reader["version"]);

                                    //Add to List
                                    digitalTwinModelList.Add(digitalTwinModel);
                                }
                            }
                        }
                    }
                }
                return digitalTwinModelList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving digital twin models");
                return digitalTwinModelList;
            }
        }


        public Models.DigitalTwinModelLimitedResponse GetDigitalTwinModel(long id, long organization)
        {
            Models.DigitalTwinModelLimitedResponse digitalTwinModel = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM digital_twin_models WHERE id = @id AND organization = @organization";

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
                                    digitalTwinModel = new Models.DigitalTwinModelLimitedResponse();

                                    digitalTwinModel.Id = Convert.ToInt64(reader["id"]);
                                    digitalTwinModel.Name = Convert.ToString(reader["name"]).Trim();
                                    digitalTwinModel.Description = Convert.ToString(reader["description"]).Trim();
                                    digitalTwinModel.Version = Convert.ToInt64(reader["version"]);

                                }
                            }
                        }
                    }
                }
                return digitalTwinModel;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving digital twin model");
                return digitalTwinModel;
            }
        }






        public Models.Response DeleteDigitalTwinModel(long id, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "DELETE FROM digital_twin_models WHERE id = @id AND organization = @organization";

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
                        response.Message = "digital twin model deleted";
                        response.Id = id;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "digital twin model deletion failed");
                response.Status = "error";
                response.Message = "digital twin model deletion failed";
                response.Id = id;
                return response;
            }
        }



    }
}
