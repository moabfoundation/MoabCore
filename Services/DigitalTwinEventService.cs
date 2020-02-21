using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class DigitalTwinEventService
    {
        //Connection String
        private readonly string connectionString;

        public DigitalTwinEventService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Models.Response CreateDigitalTwinEvent(Models.DigitalTwinEventRequest value, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO digital_twin_events (id, name, value, digital_twin, timestamp, organization) " +
                    "VALUES (@id, @name, @value, @digital_twin, @timestamp, @organization)";

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
                        command.Parameters.AddWithValue("@value", NpgsqlTypes.NpgsqlDbType.Varchar, value.Value);
                        command.Parameters.AddWithValue("@digital_twin", NpgsqlTypes.NpgsqlDbType.Bigint, value.DigitalTwin);
                        command.Parameters.AddWithValue("@timestamp", NpgsqlTypes.NpgsqlDbType.Timestamp, DateTime.UtcNow);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        response.Status = "success";
                        response.Message = "Digital Twin Event created";
                        response.Id = unixDateTime;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "Digital Twin Event creation failed");
                response.Status = "error";
                response.Message = "Digital Twin Event creation failed";
                response.Id = 0;
                return response;
            }
        }


        public List<Models.DigitalTwinEvent> GetAllDigitalTwinEvents(long digitalTwin, long organization)
        {
            List<Models.DigitalTwinEvent> digitalTwinEventsList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM digital_twin_events WHERE digital_twin = @digital_twin AND organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@digital_twin", NpgsqlTypes.NpgsqlDbType.Bigint, digitalTwin);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                //Initialize a Digital Twin Event 
                                Models.DigitalTwinEvent digitalTwinEvent = null;
                                //Create a List to hold multiple Digital Twin Events
                                digitalTwinEventsList = new List<Models.DigitalTwinEvent>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    digitalTwinEvent = new Models.DigitalTwinEvent();

                                    digitalTwinEvent.Id = Convert.ToInt64(reader["id"]);
                                    digitalTwinEvent.Name = Convert.ToString(reader["name"]).Trim();
                                    digitalTwinEvent.Value = Convert.ToString(reader["value"]).Trim();
                                    digitalTwinEvent.DigitalTwin = Convert.ToInt64(reader["digital_twin"]);
                                    digitalTwinEvent.Timestamp = Convert.ToDateTime(reader["timestamp"]);

                                    //Add to List
                                    digitalTwinEventsList.Add(digitalTwinEvent);
                                }
                            }
                        }
                    }
                }
                return digitalTwinEventsList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving digital twin events");
                return digitalTwinEventsList;
            }
        }

        public Models.DigitalTwinEvent GetDigitalTwinEvent(long id, long digitalTwin, long organization)
        {
            Models.DigitalTwinEvent digitalTwinEvent = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM digital_twin_events WHERE id = @id AND digital_twin = @digital_twin AND organization = @organization";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@digital_twin", NpgsqlTypes.NpgsqlDbType.Bigint, digitalTwin);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader != null && reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    digitalTwinEvent = new Models.DigitalTwinEvent();

                                    digitalTwinEvent.Id = Convert.ToInt64(reader["id"]);
                                    digitalTwinEvent.Name = Convert.ToString(reader["name"]).Trim();
                                    digitalTwinEvent.Value = Convert.ToString(reader["value"]).Trim();
                                    digitalTwinEvent.DigitalTwin = Convert.ToInt64(reader["digital_twin"]);
                                    digitalTwinEvent.Timestamp = Convert.ToDateTime(reader["timestamp"]);
                                }
                            }
                        }
                    }
                }
                return digitalTwinEvent;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving digital twin event");
                return digitalTwinEvent;
            }
        }


        public Models.Response DeleteDigitalTwinEvent(long id, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "DELETE FROM digital_twin_events WHERE id = @id AND organization = @organization";

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
                            response.Message = "Digital Twin Event deleted";
                            response.Id = id;
                        }
                        else
                        {
                            //Return Failure
                            response.Status = "error";
                            response.Message = "Digital Twin Event deletion failed";
                            response.Id = id;
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "Digital Twin Event deletion failed");
                response.Status = "error";
                response.Message = "Digital Twin Event deletion failed";
                response.Id = id;
                return response;
            }
        }


    }
}