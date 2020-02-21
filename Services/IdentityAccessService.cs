using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MoabCore.Services
{
    public class IdentityAccessService
    {
        //Connection String
        private readonly string connectionString;

        public IdentityAccessService()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public bool IsThingAuthorized(System.Net.Http.HttpRequestMessage context, out long organizationOut, out long identityOut, out long digitalTwinModelOut)
        {
            organizationOut = 0;
            identityOut = 0;
            digitalTwinModelOut = 0;

            try
            {
                //Get contents of Authorization Header and stip out Bearer
                var credentials =  context.Headers.GetValues("Authorization").First().Substring(7);

                //Test for presence of Identity and Security Token
                if (credentials.Any())
                {
                    var credentialArray = credentials.Split('.');
                    long identity = Convert.ToInt64(credentialArray[0]);
                    long securityToken = Convert.ToInt64(credentialArray[1]);

                    //Validate Identity and Security Token
                    if (ValidateThingCredentials(identity, securityToken))
                    {
                        //Make Idenity Available to Controller Action Method
                        identityOut = identity;

                        //Make Organization Available to Controller Action Method
                        organizationOut = GetThingOrganization(identity, securityToken);

                        //Make Digital Twin Model Available to Controller Action Method
                        digitalTwinModelOut = GetModel(identity);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        private bool ValidateThingIdentity(long id)
        {
            try
            {
                //SQL Statement
                var sqlString = "SELECT COUNT(*) FROM things WHERE id = @id";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
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
                //_logger.LogError(ex, "identity validation failed");
                return false;
            }
        }


        private bool ValidateThingCredentials(long id, long securityToken)
        {
            try
            {
                //SQL Statement
                var sqlString = "SELECT COUNT(*) FROM things WHERE id = @id AND security_token = @securitytoken";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@securitytoken", NpgsqlTypes.NpgsqlDbType.Bigint, securityToken);
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
                //_logger.LogError(ex, "credential validation failed");
                return false;
            }
        }


        private long GetThingOrganization(long id, long securityToken)
        {
            try
            {
                //SQL Statement
                var sqlString = "SELECT organization FROM things WHERE id = @id AND security_token = @securitytoken";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@securitytoken", NpgsqlTypes.NpgsqlDbType.Bigint, securityToken);
                        command.Prepare();
                        return Convert.ToInt64(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "organization retrieval failed");
                return 0;
            }
        }


        private long GetModel(long id)
        {
            try
            {
                //SQL Statement
                var sqlString = "SELECT model FROM things WHERE id = @id";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Prepare();
                        return Convert.ToInt64(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "model retrieval failed");
                return 0;
            }
        }




        public bool IsUserAuthorized(System.Net.Http.HttpRequestMessage context, out long organizationOut, out long identityOut, out long roleOut)
        {
            organizationOut = 0;
            identityOut = 0;
            roleOut = 0;

            try
            {
                //Get contents of Authorization Header and stip out Bearer
                var credentials = context.Headers.GetValues("Authorization").First().Substring(7);

                //Test for presence of Identity and Security Token
                if (credentials.Any())
                {
                    //var credentialArray = credentials.Split(".");
                    var credentialArray = credentials.Split('.');

                    long identity = Convert.ToInt64(credentialArray[0]);
                    long securityToken = Convert.ToInt64(credentialArray[1]);

                    //Validate Identity and Security Token
                    if (ValidateUserCredentials(identity, securityToken))
                    {
                        //Make Idenity Available to Controller Action Method
                        identityOut = identity;

                        //Make Organization Available to Controller Action Method
                        organizationOut = GetUserOrganization(identity, securityToken);

                        //Make Role Available to Controller Action Method
                        roleOut = GetUserRole(identity, securityToken);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        private bool ValidateUserCredentials(long id, long securityToken)
        {
            try
            {
                //SQL Statement
                var sqlString = "SELECT COUNT(*) FROM users WHERE id = @id AND security_token = @securitytoken";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@securitytoken", NpgsqlTypes.NpgsqlDbType.Bigint, securityToken);
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
                //_logger.LogError(ex, "credential validation failed");
                return false;
            }
        }


        private long GetUserOrganization(long id, long securityToken)
        {
            try
            {
                //SQL Statement
                var sqlString = "SELECT organization FROM users WHERE id = @id AND security_token = @securitytoken";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@securitytoken", NpgsqlTypes.NpgsqlDbType.Bigint, securityToken);
                        command.Prepare();
                        return Convert.ToInt64(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "organization retrieval failed");
                return 0;
            }
        }


        private long GetUserRole(long id, long securityToken)
        {
            try
            {
                //SQL Statement
                var sqlString = "SELECT role FROM users WHERE id = @id AND security_token = @securitytoken";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(sqlString, connection))
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Bigint, id);
                        command.Parameters.AddWithValue("@securitytoken", NpgsqlTypes.NpgsqlDbType.Bigint, securityToken);
                        command.Prepare();
                        return Convert.ToInt64(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Exception
                //_logger.LogError(ex, "role retrieval failed");
                return 0;
            }
        }




    }
}
