using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class GroupService
    {
        //Connection String
        private readonly string connectionString;

        public GroupService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public Models.Response CreateGroup(Models.GroupRequest value, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "INSERT INTO groups (id, name, description, organization) " +
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
                        command.Parameters.AddWithValue("@name", NpgsqlTypes.NpgsqlDbType.Varchar, value.Name);
                        command.Parameters.AddWithValue("@description", NpgsqlTypes.NpgsqlDbType.Varchar, value.Description);
                        command.Parameters.AddWithValue("@organization", NpgsqlTypes.NpgsqlDbType.Bigint, organization);
                        command.Prepare();
                        command.ExecuteNonQuery();

                        //Log Success
                        response.Status = "success";
                        response.Message = "group created";
                        response.Id = unixDateTime;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "group creation failed");
                response.Status = "error";
                response.Message = "group creation failed";
                response.Id = 0;
                return response;
            }
        }


        public List<Models.Group> GetAllGroups(long organization)
        {
            List<Models.Group> GroupList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM groups WHERE organization = @organization";

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
                                //Initialize a Group 
                                Models.Group group = null;
                                //Create a List to hold multiple Groups
                                GroupList = new List<Models.Group>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    group = new Models.Group();

                                    group.Id = Convert.ToInt64(reader["id"]);
                                    group.Name = Convert.ToString(reader["name"]).Trim();
                                    group.Description = Convert.ToString(reader["description"]).Trim();

                                    //Add to List
                                    GroupList.Add(group);
                                }
                            }
                        }
                    }
                }
                return GroupList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving groups");
                return GroupList;
            }
        }

        public Models.Group GetGroup(long id, long organization)
        {
            Models.Group group = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM groups WHERE id = @id AND organization = @organization";

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
                                    group = new Models.Group();

                                    group.Id = Convert.ToInt64(reader["id"]);
                                    group.Name = Convert.ToString(reader["name"]).Trim();
                                    group.Description = Convert.ToString(reader["description"]).Trim();
                                }
                            }
                        }
                    }
                }
                return group;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving group");
                return group;
            }
        }


        public Models.Response DeleteGroup(long id, long organization)
        {
            Models.Response response = new Models.Response();

            try
            {
                //SQL Statement
                var sqlString = "DELETE FROM groups WHERE id = @id AND organization = @organization";

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
                            response.Message = "group deleted";
                            response.Id = id;
                        }
                        else
                        {
                            //Return Failure
                            response.Status = "error";
                            response.Message = "group deletion failed";
                            response.Id = id;
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "group deletion failed");
                response.Status = "error";
                response.Message = "group deletion failed";
                response.Id = id;
                return response;
            }
        }





    }
}
