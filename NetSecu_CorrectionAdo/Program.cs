using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Tools;

namespace NetSecu_CorrectionAdo
{
    class Info
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }

    internal class Program
    {
        const string CONNECTION_STRING = @"Data Source=DESKTOP-BRIAREO\SQL2019DEV;Initial Catalog=ADO;Integrated Security=True;";

        static void Main(string[] args)
        {
            Connection connection = new Connection(CONNECTION_STRING);

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
            Command command = new Command("SELECT Id, LastName, FirstName FROM V_Student;", false);

            IEnumerable<Info> infos = connection.ExecuteReader(command, (dr) => new Info() { Id = (int)dr["Id"], LastName = (string)dr["LastName"], FirstName = (string)dr["FirstName"] });
            
            using(IEnumerator<Info> enumerator = infos.GetEnumerator())
            {
                while(enumerator.MoveNext())
                {
                    Info info = enumerator.Current;

                    Console.WriteLine(info.LastName);
                }
            }

            Console.WriteLine();

            foreach(Info info in infos)
            {
                Console.WriteLine(info.LastName);
            }

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

            Student student = new Student()
            {
                LastName = "Doe",
                FirstName = "Jane",
                BirthDate = new DateTime(1970, 1, 1),
                YearResult = 17,
                SectionId = 1010
            };

            //Exercice Insérer un étudiant
            //using (SqlConnection dbConnection = new SqlConnection())
            //{
            //    dbConnection.ConnectionString = CONNECTION_STRING;

            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        ///
            //        /// Concaténation => Risque d'injection SQL
            //        ///
            //        dbCommand.CommandText = $"INSERT INTO Student (LastName, FirstName, BirthDate, YearResult, SectionID) OUTPUT inserted.Id VALUES ('{student.LastName}', '{student.FirstName}', '{student.BirthDate:yyyy/MM/dd}', {student.YearResult}, {student.SectionId});";
            //        dbConnection.Open();
            //        student.Id = (int)dbCommand.ExecuteScalar();

            //        Console.WriteLine($"Id généré de l'étudiant : {student.Id}");
            //    }
            //}

            ////Requête paramètrée
            //using (SqlConnection dbConnection = new SqlConnection())
            //{
            //    dbConnection.ConnectionString = CONNECTION_STRING;

            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = $"INSERT INTO Student (LastName, FirstName, BirthDate, YearResult, SectionId) OUTPUT inserted.Id VALUES (@LastName, @FirstName, @BirthDate, @YearResult, @SectionId);";
            //        dbCommand.Parameters.AddWithValue("LastName", student.LastName);
            //        dbCommand.Parameters.AddWithValue("FirstName", student.FirstName);
            //        dbCommand.Parameters.AddWithValue("BirthDate", student.BirthDate);
            //        dbCommand.Parameters.AddWithValue("YearResult", student.YearResult);
            //        dbCommand.Parameters.AddWithValue("SectionId", student.SectionId);
            //        dbConnection.Open();
            //        student.Id = (int)dbCommand.ExecuteScalar();

            //        Console.WriteLine($"Id généré de l'étudiant : {student.Id}");
            //    }
            //}

            //Procédure stockée
            //using (SqlConnection dbConnection = new SqlConnection())
            //{
            //    dbConnection.ConnectionString = CONNECTION_STRING;

            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = $"UpdateStudent";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.AddWithValue("Id", 26);
            //        dbCommand.Parameters.AddWithValue("SectionId", 1310);
            //        dbCommand.Parameters.AddWithValue("YearResult", 16);
            //        dbConnection.Open();
            //        int rows = dbCommand.ExecuteNonQuery();

            //        Console.WriteLine($"Nombre de ligne modifiée : {rows}");
            //    }
            //}

            //using (SqlConnection dbConnection = new SqlConnection())
            //{
            //    dbConnection.ConnectionString = CONNECTION_STRING;

            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = $"DeleteStudent";
            //        dbCommand.CommandType = CommandType.StoredProcedure;
            //        dbCommand.Parameters.AddWithValue("Id", 27);
            //        dbConnection.Open();
            //        int rows = dbCommand.ExecuteNonQuery();

            //        Console.WriteLine($"Nombre de ligne modifiée : {rows}");
            //    }
            //}


            ///AVANT
            //using (SqlConnection dbConnection = new SqlConnection())
            //{
            //    dbConnection.ConnectionString = CONNECTION_STRING;

            //    using (SqlCommand dbCommand = dbConnection.CreateCommand())
            //    {
            //        dbCommand.CommandText = $"INSERT INTO Student (LastName, FirstName, BirthDate, YearResult, SectionId) OUTPUT inserted.Id VALUES (@LastName, @FirstName, @BirthDate, @YearResult, @SectionId);";
            //        dbCommand.Parameters.AddWithValue("LastName", student.LastName);
            //        dbCommand.Parameters.AddWithValue("FirstName", student.FirstName);
            //        dbCommand.Parameters.AddWithValue("BirthDate", student.BirthDate);
            //        dbCommand.Parameters.AddWithValue("YearResult", student.YearResult);
            //        dbCommand.Parameters.AddWithValue("SectionId", student.SectionId);
            //        dbConnection.Open();
            //        student.Id = (int)dbCommand.ExecuteScalar();

            //        Console.WriteLine($"Id généré de l'étudiant : {student.Id}");
            //    }
            //}

            ///Après
            //Connection connection = new Connection(CONNECTION_STRING);
            //Command command = new Command($"INSERT INTO Student (LastName, FirstName, BirthDate, YearResult, SectionId) OUTPUT inserted.Id VALUES (@LastName, @FirstName, @BirthDate, @YearResult, @SectionId);", false);
            //command.AddParameter("LastName", "Doe");
            //command.AddParameter("FirstName", "John");
            //command.AddParameter("BirthDate", DateTime.Now);
            //command.AddParameter("YearResult", 12);
            //command.AddParameter("SectionId", null);
            //int? id = (int?)connection.ExecuteScalar(command);

        }
    }
}