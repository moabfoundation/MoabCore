using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class DataTypeService
    {
        //Connection String
        private readonly string connectionString;

        public DataTypeService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<Models.DataType> GetAllDataTypes()
        {
            List<Models.DataType> DataTypeList = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM data_types";

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
                                //Initialize a Data Type 
                                Models.DataType dataType = null;
                                //Create a List to hold multiple Data Types
                                DataTypeList = new List<Models.DataType>();

                                while (reader.Read())
                                {
                                    //Create and hydrate a new Object
                                    dataType = new Models.DataType();

                                    dataType.Id = Convert.ToInt64(reader["id"]);
                                    dataType.Name = Convert.ToString(reader["name"]).Trim();

                                    //Add to List
                                    DataTypeList.Add(dataType);
                                }
                            }
                        }
                    }
                }
                return DataTypeList;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving data types");
                return DataTypeList;
            }
        }

        public Models.DataType GetDataType(long id)
        {
            Models.DataType dataType = null;

            try
            {
                //SQL Statement
                var sqlString = "SELECT * FROM data_types WHERE id = @id";

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
                                    dataType = new Models.DataType();

                                    dataType.Id = Convert.ToInt64(reader["id"]);
                                    dataType.Name = Convert.ToString(reader["name"]).Trim();
                                }
                            }
                        }
                    }
                }
                return dataType;
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "error retrieving data type");
                return dataType;
            }
        }


    }
}