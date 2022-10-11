using System.Data;
using System.Data.SqlClient;

namespace NetSecu_CorrectionAdo
{
    internal class Program
    {
        const string CONNECTION_STRING = @"Data Source=DESKTOP-BRIAREO\SQL2019DEV;Initial Catalog=ADO;Integrated Security=True;";

        static void Main(string[] args)
        {
            ////Exercice Récuperer les champs Id, LastName, FirstName depuis la vue V_Student en mode connecté
            //using (SqlConnection dbConnection = new SqlConnection())
            //{
            //    dbConnection.ConnectionString = CONNECTION_STRING;

            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SELECT Id, LastName, FirstName FROM V_Student;";
            //        dbConnection.Open();

            //        using (SqlDataReader dbDataReader = dbCommand.ExecuteReader())
            //        {
            //            while(dbDataReader.Read())
            //            {
            //                Console.WriteLine($"{(int)dbDataReader["Id"]:D2} : {(string)dbDataReader["FirstName"]} {(string)dbDataReader["LastName"]}");
            //            }
            //        }
            //    }
            //}

            ////Exercice Récuperer les sections en mode déconnecté
            //using (SqlConnection dbConnection = new SqlConnection())
            //{
            //    dbConnection.ConnectionString = CONNECTION_STRING;

            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SELECT Id, SectionName FROM Section;";

            //        using (SqlDataAdapter dbDataAdapter = new SqlDataAdapter())
            //        {
            //            dbDataAdapter.SelectCommand = dbCommand;
            //            DataTable dataTable = new DataTable();
            //            dbDataAdapter.Fill(dataTable);

            //            foreach(DataRow dataRow in dataTable.Rows)
            //            {
            //                Console.WriteLine($"{(int)dataRow["Id"]} : {(string)dataRow["SectionName"]}");
            //            }
            //        }
            //    }
            //}

            ////Exercice Récuperer la moyenne (précise) des étudiants
            //using (SqlConnection dbConnection = new SqlConnection())
            //{
            //    dbConnection.ConnectionString = CONNECTION_STRING;

            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = "SELECT AVG(CONVERT(FLOAT, YearResult)) FROM Student;";
            //        dbConnection.Open();
            //        double moyenne = (double)dbCommand.ExecuteScalar();

            //        Console.WriteLine(moyenne);
            //    }
            //}
        }
    }
}