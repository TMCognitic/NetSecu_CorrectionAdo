using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Connection
    {
        private readonly string _connectionString;

        public Connection(string connectionString)
        {
            _connectionString = connectionString;
        }

        private static SqlConnection CreateConnection(string connectionString)
        {
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = connectionString;

            return dbConnection;
        }

        private static SqlCommand CreateCommand(Command command, SqlConnection dbConnection)
        {
            SqlCommand dbCommand = dbConnection.CreateCommand();                
            dbCommand.CommandText = command.Query;
            if (command.IsStoredProcedure)
            {
                dbCommand.CommandType = CommandType.StoredProcedure;
            }

            foreach (KeyValuePair<string, object> kvp in command.Parameters)
            {
                SqlParameter dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = kvp.Key;
                dbParameter.Value = kvp.Value;

                dbCommand.Parameters.Add(dbParameter);
            }

            return dbCommand;
        }

        public int ExecuteNonQuery(Command command)
        {
            //Créer la connexion
            using (SqlConnection dbConnection = CreateConnection(_connectionString))
            {
                //Créer la commande sur base de l'argument recu en paramètre
                using (SqlCommand dbCommand = CreateCommand(command, dbConnection))
                { 
                    //ouvrir la connexion
                    dbConnection.Open();
                    //appeler la méthode ExecuteNonQuery
                    return dbCommand.ExecuteNonQuery();                   
                }
            }
        }

        public object? ExecuteScalar(Command command)
        {
            //Créer la connexion
            using (SqlConnection dbConnection = CreateConnection(_connectionString))
            {
                //Créer la commande sur base de l'argument recu en paramètre
                using (SqlCommand dbCommand = CreateCommand(command, dbConnection))
                {
                    //ouvrir la connexion
                    dbConnection.Open();
                    //appeler la méthode ExecuteNonQuery
                    object? result = dbCommand.ExecuteScalar();
                    return result is DBNull ? null : result;
                }
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<SqlDataReader, TResult> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            using (SqlConnection dbConnection = CreateConnection(_connectionString))
            {
                //Créer la commande sur base de l'argument recu en paramètre
                using (SqlCommand dbCommand = CreateCommand(command, dbConnection))
                {                   
                    //ouvrir la connexion
                    dbConnection.Open();
                    //appeler la méthode ExecuteReader
                    using(SqlDataReader dbDataReader = dbCommand.ExecuteReader())
                    {
                        while(dbDataReader.Read())
                        {
                            yield return selector(dbDataReader);
                        }
                    }
                }
            }
        }

        public DataTable GetDataTable(Command command)
        {
            //Créer la connexion
            using (SqlConnection dbConnection = CreateConnection(_connectionString))
            {
                //Créer la commande sur base de l'argument recu en paramètre
                using (SqlCommand dbCommand = CreateCommand(command, dbConnection))
                {
                    using (SqlDataAdapter dbDataAdapter = new SqlDataAdapter())
                    {
                        DataTable dataTable = new DataTable();
                        dbDataAdapter.SelectCommand = dbCommand;
                        dbDataAdapter.Fill(dataTable);

                        return dataTable;
                    }
                }
            }
        }
    }
}
