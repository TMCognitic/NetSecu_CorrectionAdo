using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Connection
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _factory;

        public Connection(DbProviderFactory factory, string connectionString)
        {
            _connectionString = connectionString;
            _factory = factory;
        }

        private static DbConnection CreateConnection(DbProviderFactory factory, string connectionString)
        {
            DbConnection? dbConnection = factory.CreateConnection();

            if (dbConnection is null)
                throw new InvalidOperationException("the prodiver can't create the connection");

            dbConnection.ConnectionString = connectionString;

            return dbConnection;
        }

        private static DbCommand CreateCommand(Command command, DbConnection dbConnection)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();                
            dbCommand.CommandText = command.Query;
            if (command.IsStoredProcedure)
            {
                dbCommand.CommandType = CommandType.StoredProcedure;
            }

            foreach (KeyValuePair<string, object> kvp in command.Parameters)
            {
                DbParameter dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = kvp.Key;
                dbParameter.Value = kvp.Value;

                dbCommand.Parameters.Add(dbParameter);
            }

            return dbCommand;
        }

        public int ExecuteNonQuery(Command command)
        {
            //Créer la connexion
            using (DbConnection dbConnection = CreateConnection(_factory, _connectionString))
            {
                //Créer la commande sur base de l'argument recu en paramètre
                using (DbCommand dbCommand = CreateCommand(command, dbConnection))
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
            using (DbConnection dbConnection = CreateConnection(_factory, _connectionString))
            {
                //Créer la commande sur base de l'argument recu en paramètre
                using (DbCommand dbCommand = CreateCommand(command, dbConnection))
                {
                    //ouvrir la connexion
                    dbConnection.Open();
                    //appeler la méthode ExecuteNonQuery
                    object? result = dbCommand.ExecuteScalar();
                    return result is DBNull ? null : result;
                }
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<DbDataReader, TResult> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            using (DbConnection dbConnection = CreateConnection(_factory, _connectionString))
            {
                //Créer la commande sur base de l'argument recu en paramètre
                using (DbCommand dbCommand = CreateCommand(command, dbConnection))
                {                   
                    //ouvrir la connexion
                    dbConnection.Open();
                    //appeler la méthode ExecuteReader
                    using(DbDataReader dbDataReader = dbCommand.ExecuteReader())
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
            using (DbConnection dbConnection = CreateConnection(_factory, _connectionString))
            {
                //Créer la commande sur base de l'argument recu en paramètre
                using (DbCommand dbCommand = CreateCommand(command, dbConnection))
                {
                    DbDataAdapter? dbDataAdapter = _factory.CreateDataAdapter();

                    if(dbDataAdapter is null)
                        throw new InvalidOperationException("the prodiver can't create the data adapter");

                    using (dbDataAdapter)
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
