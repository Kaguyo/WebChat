using System.Data;
using System.Data.SQLite;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;


public class Program
{
    public static void Main(string[] args)
    {
        const int PORT = 5000;
        
        HttpListener listener = new();
        listener.Prefixes.Add($"http://localhost:{PORT}/");
        listener.Start();

        Console.WriteLine($"Servidor iniciado... Aguardando requisições. (PORT:{PORT})\n");

        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");

            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.OutputStream.Close();
                continue;
            }

            if (request.ContentType == null)
            {
                Console.WriteLine("\nContentType de REQUEST null encontrada:");
                Console.WriteLine(request.ContentType);
                foreach (string header in request.Headers)
                {
                    Console.WriteLine($"{header}: {request.Headers[header]}");
                }
                continue;
            }
            else
            {
                if (request.HttpMethod == "POST" && request.ContentType.Contains("application/json"))
                {
                    Console.Clear();
                    try
                    {
                        Console.Clear();
                        using (StreamReader reader = new(request.InputStream, Encoding.UTF8))
                        {
                            string json = reader.ReadToEnd();
                            User? user = JsonSerializer.Deserialize<User>(json);
                            if (!File.Exists(DalHelper.caminhoUsers))
                            {
                                DalHelper.CriarBancoSQLite(DalHelper.caminhoUsers);
                            }
                            if (!File.Exists(DalHelper.caminhoUsers)) DalHelper.CriarTabelaSQLite("Users", "Users.sqlite"); // CREATE TABLE IF NOT EXISTS (tabelaName)
                            if (user != null && user.Nome != "") DalHelper.AddUser(user, "Users.sqlite"); // INSERTS INTO Users (object user)
                            byte[] buffer = Encoding.UTF8.GetBytes("Dados recebidos com sucesso");
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                            // DalHelper.DropTable("Users","Users.sqlite"); // Drops table by Table name
                            // DalHelper.DeleteAllFromTable("Users", "Users.sqlite"); // Deletes ALL from table specified in specified sqlite file
                            DalHelper.DbDispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar os dados ERR: {ex.Message}");
                        Console.WriteLine($"Detalhes do Erro: {ex}");

                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        byte[] buffer = Encoding.UTF8.GetBytes("Erro ao processar os dados");
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                }
                Console.WriteLine("Task finished\n=========================\nAguardando requisições...\n");
            }
        }
    }
    public class User
    {
        public string? Nome { get; set; }
        public string? Number { get; set; }
        public string? Password { get; set; }
        public string? Password2 { get; set; }
    }
    public class DalHelper()
    {
        internal static string caminho = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "xDataBase");
        internal static string caminhoUsers = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "xDataBase", "Users.sqlite");
        internal static string connectionString = $"Data Source={caminho}"; // aponta conexao para string caminho
        private static SQLiteConnection sqliteConnection = null!;

        private static SQLiteConnection DbConnection(string sqliteFile) // Abre conexao SQlite
        {
            sqliteConnection = new SQLiteConnection($"{connectionString}\\{sqliteFile}");
            sqliteConnection.Open();
            return sqliteConnection;
        }
        public static void DbDispose() // Descarta conexao SQLite
        { 
            try
            {
                if (sqliteConnection != null && sqliteConnection.State != ConnectionState.Closed)
                {
                    sqliteConnection.Dispose();
                    PrintCurrentLine("SQLite connection disposed successfully.");
                }
                else
                {
                    Console.WriteLine("Fail during connection disposal in void DbDispose:");
                    PrintCurrentLine("SQLite connection was either already closed or not initialized.");
                }
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Error during connection disposal in void DbDispose: {ex.Message}");
                throw;
            }
        }

        public static void CriarBancoSQLite(string fileName)  // CREATES IF NOT EXISTS {string file.sqlite}
        {
            try
            {
                if (!Path.Exists($"{caminho}\\{fileName}"))
                {
                    SQLiteConnection.CreateFile($"{caminho}\\{fileName}");
                    PrintCurrentLine("CREATING SQLite file...");
                }
                else throw new Exception($"File {fileName} already exists at path:\n{caminho}");
            }
            catch(Exception ex)
            {
                PrintCurrentLine($"Fail when trying to create file... {ex.Message}");
                throw;
            }
        }

        public static void CriarTabelaSQLite(string tabelaName, string sqliteFile)  // CREATES TABLE IF NOT EXISTS {string tabelaName}
        {
            try
            {
                if (tabelaName != null && sqliteFile != null)
                using(var cmd = DbConnection(sqliteFile).CreateCommand())
                {
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS {tabelaName}(Id INTEGER NOT NULL, Name Varchar(50), Number Varchar(50), Password Varchar(50), PRIMARY KEY (Id))";
                    cmd.ExecuteNonQuery();
                    PrintCurrentLine($"CREATING TABLE {tabelaName}...");
                } 
                else 
                {
                    Console.WriteLine($"Error during CREATE TABLE in void CriarTabelaSQLite:");
                    PrintCurrentLine("Parameters(string tabelaName or string sqliteFile) are null.");
                }
                
            }
            catch(Exception ex)
            {
                PrintCurrentLine($"Error during CREATE TABLE in void CriarTabelaSQLITE: {ex.Message}");
                throw;
            }
        }

        public static void AddUser(User user, string sqliteFile) // INSERTS INTO Users values {User user.properties}
        {
            try
            {
                using (var cmd = DbConnection(sqliteFile).CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO Users(Name, Number, Password) values (@Nome, @Number, @Password)";
                    cmd.Parameters.AddWithValue("@Nome", user.Nome);
                    cmd.Parameters.AddWithValue("@Number", user.Number);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine($"INSERTING INTO Users...");
            }
            catch(Exception ex)
            {
                PrintCurrentLine($"Error during INSERT INTO Users in void AddUser: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetUsers(string sqliteFile) // SELECTS ALL From Users
        {
            SQLiteDataAdapter da;
            DataTable dt = new();
            try
            {
                using (var cmd = DbConnection(sqliteFile).CreateCommand())
                {
                    
                    cmd.CommandText = "SELECT * FROM Users";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection(sqliteFile));
                    da.Fill(dt);
                    if (Path.Exists(caminhoUsers)) PrintCurrentLine("Selecting all from TABLE Users...");
                    else throw new Exception($"Path not found ({caminhoUsers})");
                    
                    return dt;
                }
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Error during SELECT ALL FROM Users in DataTable GetUser: {ex.Message}");
                throw;
            }
        }

        public static DataTable GetUser(int id, string sqliteFile) // SELECTS ALL FROM Users WHERE Id={int id}
        {
            SQLiteDataAdapter da;
            DataTable dt = new();
            try
            {
                using (var cmd = DbConnection(sqliteFile).CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Users Where Id=" + id;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection(sqliteFile));
                    da.Fill(dt);
                    PrintCurrentLine($"SELECTING User by ID ({id})...");
                    return dt;
                }
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Error SELECTING User by ID in DataTable GetUser: {ex.Message}");
                throw;
            }
            
        }
        public static void UpdateUsers(User user, string sqliteFile) // UPDATES Users no arquivo {sqliteFile} 
        {
            try 
            {
                using (SQLiteCommand cmd = new(DbConnection(sqliteFile)))
                {
                    cmd.CommandText = "UPDATE Users SET Name=@Name, Number=@Number, Password=@Password WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Name", user.Nome);
                    cmd.Parameters.AddWithValue("@Number", user.Number);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine("UPDATING Users...");
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Error during UPDATE Users in void UpdateUsers: {ex.Message}");
                throw;
            }
        }

        public static void DropTable(string tableName, string sqliteFile) // DROPS TABLE IF EXISTS {string tableName}
        {
            try
            {
                using (var cmd = DbConnection(sqliteFile).CreateCommand())
                {
                    cmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine($"TABLE {tableName} was removed successfully.");
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Error during DROP TABLE {tableName} in void DropTable: {ex.Message}");
                throw;
            }
        }
    
        public static void DeleteUserById(int id, string sqliteFile) // DELETES FROM Users by ID {int id}
        {
            try 
            {
                using(SQLiteCommand cmd = new(DbConnection(sqliteFile)))
                {
                    cmd.CommandText="DELETE FROM Users WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine($"DELETING FROM Users WHERE Id = ({id})...");
            }
            catch(Exception ex)
            {
                PrintCurrentLine($"Error during DELETE execution in void DeleteUserById: {ex.Message}");
                throw;
            }
        }
        public static void DeleteAllFromTable(string tableName, string sqliteFile) // DELETES ALL TABLES FROM {string tableName}
        {
            try 
            {
                using (SQLiteCommand cmd = new(DbConnection(sqliteFile)))
                {
                    cmd.CommandText = $"DELETE FROM {tableName}";
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine($"DELETING ALL TABLES FROM {tableName}...");
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Error during DELETE FROM {tableName} in void DeleteAllFromTable: {ex.Message}");
                throw;
            }
        }
    }
    public static void PrintCurrentLine(string comentario, [CallerLineNumber] int lineNumber = 0)
    {
        Console.WriteLine($"{comentario} ln:{lineNumber}\n");
    }
}