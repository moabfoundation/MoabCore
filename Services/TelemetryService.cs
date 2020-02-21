using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class TelemetryService
    {
        //Connection String
        private readonly string connectionString;

        public TelemetryService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Models.Response CreateTelemetry(JObject digitalTwinData, long digitalTwin, long organization, long digitalTwinModel)
        {
            Models.Response response = new Models.Response();

            //string jsonString;
            //jsonString = JsonSerializer.Serialize(data);

            try
            {
                //SQL Statement
                string sqlString = "INSERT INTO telemetry (id, digital_twin, digital_twin_data, timestamp, organization, digital_twin_model) " +
                    "VALUES (@id, @digital_twin, @digital_twin_data, @timestamp, @organization, @digital_twin_model)";

                //Create UNIX Timestamp
                var utcDateTimeWrite = DateTime.UtcNow;
                var dto = new DateTimeOffset(utcDateTimeWrite);
                var unixDateTime = dto.ToUnixTimeMilliseconds();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    connection.TypeMapper.UseJsonNet();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, unixDateTime);
                        command.Parameters.AddWithValue("@digital_twin", NpgsqlTypes.NpgsqlDbType.Bigint, digitalTwin);
                        command.Parameters.AddWithValue("@digital_twin_data", NpgsqlTypes.NpgsqlDbType.Json, digitalTwinData);
                        command.Parameters.AddWithValue("@timestamp", NpgsqlTypes.NpgsqlDbType.Timestamp, DateTime.UtcNow);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Parameters.AddWithValue("@digital_twin_model", NpgsqlTypes.NpgsqlDbType.Bigint, digitalTwinModel);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        response.Status = "success";
                        response.Message = "telemetry saved";
                        response.Id = unixDateTime;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "telemetry save failed");
                response.Status = "error";
                response.Message = "telemetry save failed";
                response.Id = 0;
                return response;
            }
        }

        public List<Models.Telemetry> GetAllTelemetry(long organization)
        {
            List<Models.Telemetry> telemetryList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM telemetry WHERE organization = @organization";

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
                                //Initialize Telemetry Object
                                Models.Telemetry telemetry = null;
                                //Create a List to hold multiple Telemetry Objects
                                telemetryList = new List<Models.Telemetry>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    telemetry = new Models.Telemetry();

                                    telemetry.Id = Convert.ToInt64(reader["id"]);
                                    telemetry.DigitalTwin = Convert.ToInt64(reader["digital_twin"]);
                                    telemetry.DigitalTwinData = Convert.ToString(reader["digital_twin_data"]).Trim();
                                    telemetry.Timestamp = Convert.ToDateTime(reader["timestamp"]);
                                    telemetry.DigitalTwinModel = Convert.ToInt64(reader["digital_twin_model"]);

                                    //Add to List
                                    telemetryList.Add(telemetry);
                                }
                            }
                        }
                    }
                }
                return telemetryList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving telemetry");
                return telemetryList;
            }
        }




        public List<Models.Telemetry> GetTelemetry(long id, long organization)
        {
            List<Models.Telemetry> telemetryList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM telemetry WHERE id = @id AND organization = @organization";

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
                                //Initialize Telemetry Object
                                Models.Telemetry telemetry = null;
                                //Create a List to hold multiple Telemetry Objects
                                telemetryList = new List<Models.Telemetry>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    telemetry = new Models.Telemetry();

                                    telemetry.Id = Convert.ToInt64(reader["id"]);
                                    telemetry.DigitalTwin = Convert.ToInt64(reader["digital_twin"]);
                                    telemetry.DigitalTwinData = Convert.ToString(reader["digital_twin_data"]).Trim();
                                    telemetry.Timestamp = Convert.ToDateTime(reader["timestamp"]);
                                    telemetry.DigitalTwinModel = Convert.ToInt64(reader["digital_twin_model"]);

                                    //Add to List
                                    telemetryList.Add(telemetry);
                                }
                            }
                        }
                    }
                }
                return telemetryList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving telemetry");
                return telemetryList;
            }
        }





        public Models.Response DeleteTelemetry(long id, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "DELETE FROM telemetry WHERE id = @id AND organization = @organization";

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
                        response.Message = "telemetry deleted";
                        response.Id = id;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "telemetry deletion failed");
                response.Status = "error";
                response.Message = "telemetry deletion failed";
                response.Id = id;
                return response;
            }
        }




    }
}
