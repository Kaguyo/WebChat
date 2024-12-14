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

        Console.WriteLine($"Servidor iniciado... Aguardando requisições. (PORT:{PORT})");

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
                    try
                    {
                        using (StreamReader reader = new(request.InputStream, Encoding.UTF8))
                        {
                            string json = reader.ReadToEnd();
                            User? user = JsonSerializer.Deserialize<User>(json);
                            if (!File.Exists(DalHelper.caminho))
                            {
                                DalHelper.CriarBancoSQLite();
                            }
                                DalHelper.CriarTabelaSQLite();
                                if (user != null)DalHelper.Add(user);
                                DalHelper.DbDispose();
                            byte[] buffer = Encoding.UTF8.GetBytes("Dados recebidos com sucesso");
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                            // DalHelper.DeleteAll();
                            // DalHelper.DroparTabelaSQLite("Users");
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
                Console.WriteLine("\nTask finished\n=========================\n");
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
        internal static string caminho = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "xDataBase", "Users.sqlite");
        internal static string connectionString = $"Data Source={caminho}";
        private static SQLiteConnection sqliteConnection = null!;

        private static SQLiteConnection DbConnection()
        {
            sqliteConnection = new SQLiteConnection(connectionString);
            sqliteConnection.Open();
            return sqliteConnection;
        }
        public static void DbDispose()
        {
            sqliteConnection.Dispose();
        }


        public static void CriarBancoSQLite()
        {
            try
            {
                SQLiteConnection.CreateFile(caminho);
                PrintCurrentLine("Criando arquivo sqlite...");
            }
            catch
            {
                PrintCurrentLine("Falha ao tentar criar arquivo sqlite...");
                throw;
            }
        }
        
        public static void CriarTabelaSQLite()
        {
            try
            {
                using(var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Users(Id INTEGER NOT NULL, Name Varchar(50), Number Varchar(50), Password Varchar(50), PRIMARY KEY (Id))";
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine("Criando tabela...");
            }
            catch(Exception ex)
            {
                PrintCurrentLine($"Erro em CREATE TABLE em public static void CriarTabelaSQLITE: {ex.Message}");
                throw;
            }
        }
        public static void DroparTabelaSQLite(string tableName)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine($"Tabela {tableName} removida com sucesso.");
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Erro em DROP TABLE em public static void DroparTabelaSQLite: {ex.Message}");
                throw;
            }
        }
        public static DataTable? GetUsers()
        {
            SQLiteDataAdapter? da;
            DataTable dt = new();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    
                    cmd.CommandText = "SELECT * FROM Users";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    PrintCurrentLine("Selecionando Users...");
                    return dt;
                }
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Erro em SELECT em public static DataTable AddGetUsers: {ex.Message}");
                throw;
            }
        }

        public static DataTable? GetUser(int Id){
            SQLiteDataAdapter? da;
            DataTable dt = new();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Users Where Id=" + Id;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    PrintCurrentLine($"Seleciando User por ID({Id})...");
                    return dt;
                }
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Erro em SELECT em public static AddGetUser: {ex.Message}");
                throw;
            }
            
        }
        public static void Add(User user)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Users(Name, Number, Password) values (@Nome, @Number, @Password)";
                    cmd.Parameters.AddWithValue("@Nome", user.Nome);
                    cmd.Parameters.AddWithValue("@Number", user.Number);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine("Inserindo into Users...");
            }
            catch(Exception ex)
            {
                PrintCurrentLine($"Erro em INSERT INTO em public static void Add: {ex.Message}");
                throw;
            }
        }
        public static void Update(User user)
        {
            try 
            {
                using (SQLiteCommand cmd = new(DbConnection()))
                {
                    cmd.CommandText = "UPDATE Users SET Name=@Name, Number=@Number, Password=@Password WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Name", user.Nome);
                    cmd.Parameters.AddWithValue("@Number", user.Number);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine("Realizando UPDATE em users...");
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Erro em UPDATE em public static void Add: {ex.Message}");
                throw;
            }
        }
        public static void Delete(int Id)
        {
            try 
            {
                using(SQLiteCommand cmd = new(DbConnection()))
                {
                    cmd.CommandText="DELETE FROM Users WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine($"DELETANDO FROM Users WHERE Id = {Id}...");
            }
            catch(Exception ex)
            {
                PrintCurrentLine($"Erro em DELETE em public static void Delete: {ex.Message}");
                throw;
            }
        }
        public static void DeleteAll()
        {
            try 
            {
                using (SQLiteCommand cmd = new(DbConnection()))
                {
                    cmd.CommandText = "DELETE FROM Users";
                    cmd.ExecuteNonQuery();
                }
                PrintCurrentLine($"DELETANDO FROM Users...(ALL)");
            }
            catch (Exception ex)
            {
                PrintCurrentLine($"Erro em DELETE ALL em public static void DeleteAll: {ex.Message}");
                throw;
            }
        }
        
    }
    public static void PrintCurrentLine(string comentario, [CallerLineNumber] int lineNumber = 0)
    {
        Console.WriteLine($"{comentario} ln:{lineNumber}");
    }
}

