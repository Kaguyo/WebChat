using System.Data;
using System.Data.SQLite;
using System.Net;
using System.Text;
using System.Text.Json;
using static Program;


public class Program
{
    public static void Main(string[] args)
    {
        const int PORT = 5000;
        HttpListener listener = new HttpListener();
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
                        using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8))
                        {
                            string json = reader.ReadToEnd();
                            Console.WriteLine(json);
                            User? user = JsonSerializer.Deserialize<User>(json);
                            Console.WriteLine($"Json: {JsonSerializer.Serialize(user)}");
                            Console.WriteLine($"Id: {user.Id}");
                            Console.WriteLine($"Name: {user.Nome}");
                            Console.WriteLine($"Number: {user.Number}");
                            Console.WriteLine($"Password: {user.Password}");
                            Console.WriteLine($"Password2: {user.Password2}");
                            string caminho = "C:\\Users\\Fabricio\\Codes\\Project_Conta\\WebChat\\xDataBase\\Users.sqlite";
                            bool fileExists = System.IO.File.Exists(caminho);
                            if(fileExists){
                                Console.WriteLine("O arquivo já existe");
                            }else{
                                Console.WriteLine("O arquivo não existe");
                            }
                            // DalHelper.CriarBancoSQLite();
                            DalHelper.CriarTabelaSQLite();
                            DalHelper.Add(user);
                            DalHelper.Dispose();
                            byte[] buffer = Encoding.UTF8.GetBytes("Dados recebidos com sucesso");
                            response.OutputStream.Write(buffer, 0, buffer.Length);
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
                Console.WriteLine("\n=========================\n");
            }
        }
    }
    public class User
    {
        public int? Id { get; set; }
        public string? Nome { get; set; }
        public string? Number { get; set; }
        public string? Password { get; set; }
        public string? Password2 { get; set; }
    }
}

public class DalHelper()
    {
        private static SQLiteConnection sqliteConnection;

        // public DalHelper(){}


        private static SQLiteConnection DbConnection(){
            sqliteConnection = new SQLiteConnection("Data Source=C:\\Users\\Fabricio\\Codes\\Project_Conta\\WebChat\\xDataBase\\Users.sqlite");
            sqliteConnection.Open();
            return sqliteConnection;
        }

        public static void Dispose(){
            sqliteConnection.Dispose();
        }

        public static void CriarBancoSQLite(){
            try{
                SQLiteConnection.CreateFile(@"C:\Users\Fabricio\Codes\Project_Conta\WebChat\xDataBase\Users.sqlite");
            }
            catch{
                throw;
            }
        }
        
        public static void CriarTabelaSQLite(){
            try{
                using(var cmd = DbConnection().CreateCommand()){
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Users(Id int, Name Varchar(50), Number Varchar(50), Password Varchar(50), Password2 Varchar(50))";
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public static DataTable GetUsers(){
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try{
                using (var cmd = DbConnection().CreateCommand()){
                    cmd.CommandText = "SELECT * FROM Users";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex){
                throw ex;
            }
        }

        public static DataTable GetUser(int Id){
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand()){
                    cmd.CommandText = "SELECT * FROM Users Where Id=" + Id;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Add(User user){
            try{
                using (var cmd = DbConnection().CreateCommand()){
                    cmd.CommandText = "INSERT INTO Users(Id, Name, Number, Password, Password2) values (@Id, @Nome, @Number, @Password, @Password2)";
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Nome", user.Nome);
                    cmd.Parameters.AddWithValue("@Number", user.Number);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Password2", user.Password2);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex){
                throw ex;
            }
        }

        public static void Update(User user){
            try {
                using (var cmd = new SQLiteCommand(DbConnection())){
                    if (user.Id != null){
                        cmd.CommandText = "UPDATE Users SET Name=@Name, Number=@Number, Password=@Password WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", user.Id);
                        cmd.Parameters.AddWithValue("@Name", user.Nome);
                        cmd.Parameters.AddWithValue("@Number", user.Number);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex){
                throw ex;
            }
        }

        public static void Delete(int Id){
            try {
                using(var cmd = new SQLiteCommand(DbConnection())){
                    cmd.CommandText="DELETE FROM Users WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex){
                throw ex;
            }
        }


}