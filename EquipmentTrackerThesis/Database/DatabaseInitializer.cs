using Microsoft.Data.SqlClient;
using System.Text;

namespace EquipmentTrackerThesis.Database
{
    /// <summary>
    /// This class connects to an existng database or creates one if there is not existed one.
    /// </summary>
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        /// <summary>
        /// This method stores the connection string to the database. 
        /// </summary>
        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// This method tries to connect to exixting database. If database not exists, the program creates a default one.
        /// </summary>
        public void InitializeDatabase()
        {
            using (SqlConnection connection = new(_connectionString))
            {
                try
                {
                    //Tries to open the given path
                    connection.Open();

                    //If dtabese is not exists the program tries to crete it
                    if (!DatabaseExists(connection, "ManagementSystemDB"))
                    {
                        ExecuteSqlScript(connection, "ManagementSystemDB.sql");
                    }
                    else
                    {
                        Console.WriteLine("Database already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// This method checks if the specified database is existed or not.
        /// </summary>
        /// <param name="connection">The connection string to the database.</param>
        /// <param name="databaseName">The name of the database you are searching.</param>
        /// <returns></returns>
        private static bool DatabaseExists(SqlConnection connection, string databaseName)
        {
            string query = $"SELECT COUNT(*) FROM master.dbo.sysdatabases WHERE name = '{databaseName}'";

            using SqlCommand cmd = new(query, connection);
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            return result > 0;
        }

        //Develope: It wuld be a better solution to use JSON files instead of query.
        /// <summary>
        /// This method opens and runs the query to create a sample database.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="scriptFileName"></param>
        private static void ExecuteSqlScript(SqlConnection connection, string scriptFileName)
        {
        string currentDirectory = Directory.GetCurrentDirectory();
        string scriptFilePath = Path.Combine(currentDirectory, scriptFileName);

        if (!File.Exists(scriptFilePath))
        {
        Console.WriteLine($"Error: Script file not found at {scriptFilePath}");
        return;
        }

        string script;
        using (StreamReader reader = new(scriptFilePath, Encoding.GetEncoding(1250)))
        {
        script = reader.ReadToEnd();
        }

        // GO mentén feldaraboljuk és külön futtatjuk
        var batches = System.Text.RegularExpressions.Regex
        .Split(script, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        foreach (var batch in batches)
        {
        string trimmed = batch.Trim();
        if (string.IsNullOrWhiteSpace(trimmed)) continue;

        using SqlCommand cmd = new(trimmed, connection);
        cmd.ExecuteNonQuery();
        }
        }
    }
}
