using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class RoleService
    {
        //Connection String
        private readonly string connectionString;

        public RoleService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<Models.Role> GetAllRoles()
        {
            List<Models.Role> RoleList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM roles";

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
                                //Initialize a Role 
                                Models.Role role = null;
                                //Create a List to hold multiple Roles
                                RoleList = new List<Models.Role>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    role = new Models.Role();

                                    role.Id = Convert.ToInt64(reader["id"]);
                                    role.Name = Convert.ToString(reader["name"]).Trim();
                                    role.Description = Convert.ToString(reader["description"]).Trim();
                                    //Add to List
                                    RoleList.Add(role);
                                }
                            }
                        }
                    }
                }
                return RoleList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving roles");
                return RoleList;
            }
        }


        public Models.Role GetRole(long id)
        {
            Models.Role role = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM roles WHERE id = @id";

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
                                    role = new Models.Role();

                                    role.Id = Convert.ToInt64(reader["id"]);
                                    role.Name = Convert.ToString(reader["name"]).Trim();
                                    role.Description = Convert.ToString(reader["description"]).Trim();
                                }
                            }
                        }
                    }
                }
                return role;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving role");
                return role;
            }
        }




    }
}