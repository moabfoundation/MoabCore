using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class DigitalTwinModelDynamicPropertyService
    {
        //Connection String
        private readonly string connectionString;

        public DigitalTwinModelDynamicPropertyService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Models.Response CreateDigitalTwinModelDynamicProperty(Models.DigitalTwinModelDynamicPropertyRequest value, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO digital_twin_model_dynamic_properties (id, name, measurement_type, data_type, unit_of_measure, digital_twin_model, organization) " +
                    "VALUES (@id, @name, @measurement_type, @data_type, @unit_of_measure, @digital_twin_model, @organization)";

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
                        command.Parameters.AddWithValue("@measurement_type", NpgsqlTypes.NpgsqlDbType.Bigint, value.MeasurementType);
                        command.Parameters.AddWithValue("@data_type", NpgsqlTypes.NpgsqlDbType.Bigint, value.DataType);
                        command.Parameters.AddWithValue("@unit_of_measure", NpgsqlTypes.NpgsqlDbType.Bigint, value.UnitOfMeasure);
                        command.Parameters.AddWithValue("@digital_twin_model", NpgsqlTypes.NpgsqlDbType.Bigint, value.DigitalTwinModel);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        response.Status = "success";
                        response.Message = "Digital Twin Model Dynamic Property created";
                        response.Id = unixDateTime;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "Digital Twin Model Dynamic Property creation failed");
                response.Status = "error";
                response.Message = "Digital Twin Model Dynamic Property creation failed";
                response.Id = 0;
                return response;
            }
        }


        public List<Models.DigitalTwinModelDynamicPropertyLimitedResponse> GetAllDigitalTwinModelDynamicProperties(long digitalTwinModel, long organization)
        {
            List<Models.DigitalTwinModelDynamicPropertyLimitedResponse> dynamicPropertyList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM digital_twin_model_dynamic_properties WHERE digital_twin_model = @digital_twin_model AND organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@digital_twin_model", NpgsqlTypes.NpgsqlDbType.Bigint, digitalTwinModel);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                //Initialize a Dynamic Property 
                                Models.DigitalTwinModelDynamicPropertyLimitedResponse dynamicProperty = null;
                                //Create a List to hold multiple Dynamic Properties
                                dynamicPropertyList = new List<Models.DigitalTwinModelDynamicPropertyLimitedResponse>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    dynamicProperty = new Models.DigitalTwinModelDynamicPropertyLimitedResponse();

                                    dynamicProperty.Id = Convert.ToInt64(reader["id"]);
                                    dynamicProperty.Name = Convert.ToString(reader["name"]).Trim();
                                    dynamicProperty.MeasurementType = Convert.ToInt64(reader["measurement_type"]);
                                    dynamicProperty.DataType = Convert.ToInt64(reader["data_type"]);
                                    dynamicProperty.UnitOfMeasure = Convert.ToInt64(reader["unit_of_measure"]);
                                    dynamicProperty.DigitalTwinModel = Convert.ToInt64(reader["digital_twin_model"]);

                                    //Add to List
                                    dynamicPropertyList.Add(dynamicProperty);
                                }
                            }
                        }
                    }
                }
                return dynamicPropertyList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving Digital Twin Model Dynamic Properties");
                return dynamicPropertyList;
            }
        }


        public Models.DigitalTwinModelDynamicPropertyLimitedResponse GetDigitalTwinModelDynamicProperty(long id, long digitalTwinModel, long organization)
        {
            Models.DigitalTwinModelDynamicPropertyLimitedResponse dynamicProperty = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM digital_twin_model_dynamic_properties WHERE id = @id AND digital_twin_model = @digital_twin_model AND organization = @organization";
            
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@digital_twin_model", NpgsqlTypes.NpgsqlDbType.Bigint, digitalTwinModel);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    dynamicProperty = new Models.DigitalTwinModelDynamicPropertyLimitedResponse();

                                    dynamicProperty.Id = Convert.ToInt64(reader["id"]);
                                    dynamicProperty.Name = Convert.ToString(reader["name"]).Trim();
                                    dynamicProperty.MeasurementType = Convert.ToInt64(reader["measurement_type"]);
                                    dynamicProperty.DataType = Convert.ToInt64(reader["data_type"]);
                                    dynamicProperty.UnitOfMeasure = Convert.ToInt64(reader["unit_of_measure"]);
                                    dynamicProperty.DigitalTwinModel = Convert.ToInt64(reader["digital_twin_model"]);
                                }
                            }
                        }
                    }
                }
                return dynamicProperty;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving Digital Twin Model Dynamic Property");
                return dynamicProperty;
            }
        }


        public Models.Response DeleteDigitalTwinModelDynamicProperty(long id, long digitalTwinModel, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "DELETE FROM digital_twin_model_dynamic_properties WHERE id = @id AND digital_twin_model = @digital_twin_model AND organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@digital_twin_model", NpgsqlTypes.NpgsqlDbType.Bigint, digitalTwinModel);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            //Return Success
                            response.Status = "success";
                            response.Message = "Digital Twin Model Dynamic Property deleted";
                            response.Id = id;
                        }
                        else
                        {
                            //Return Failure
                            response.Status = "error";
                            response.Message = "Digital Twin Model Dynamic Property deletion failed";
                            response.Id = id;
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "Digital Twin Model Dynamic Property deletion failed");
                response.Status = "error";
                response.Message = "Digital Twin Model Dynamic Property deletion failed";
                response.Id = id;
                return response;
            }
        }





    }
}