using System.Data;
using System.Data.Common;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Dapper;
using Npgsql;

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
                    Console.WriteLine(json.Length);
                    response.StatusCode = (int)HttpStatusCode.OK;
                    byte[] buffer = Encoding.UTF8.GetBytes("Dados recebidos com sucesso");
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }
            } 
            catch(Exception ex)
            {
                Console.WriteLine($"Erro ao processar os dados ERR: {ex.Message}" );
                Console.WriteLine($"Detalhes do Erro: {ex}");

                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                byte[] buffer = Encoding.UTF8.GetBytes("Erro ao processar os dados");
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
        }
        Console.WriteLine("\n=========================\n");
    }
}
// public class User
// {
//     public string Nome { get; set; }

//     public int Number { get; set; }

//     public string Password { get; set; }

// }

// public User() { }   

// public User (string nome, int number, string password)
// {
//     Nome = nome;
//     Number = number;
//     Password = password;
// }

// private void ObterUsers()
// {
//     var repository = new UserRepository();
//     Users = repository.Get();

    
// }

// public class DbConnection : IDisposable
// {
//     public NpgsqlConnection Connection { get; set; }

//     public DbConnection{
//         DbConnection = new NpgsqlConnection("Server=localhost;Port=3306;User Id=root; Password=1234;");
//         Connection.Open();
//     }

//     public void Dispose()
//     {
//         Connection.Dispose();
//     }

// }

// public class UserRepository
// {
//     public bool Add(User User)
//     {
//         using var conn = new DbConnection();

//         string query = @"INSERT INTO BD_USERS.Users
//                         (@name, @number, @password)
//                         VALUES(?, ?, ?);";

//         var result = conn.Connection.Execute(sql: query, param: User);

//         return result == 1;
//     }

//     public List<User> Get()
//     {
//         using var conn = new DbConnection();

//         string query = @"SELECT id, name, `number`, password FROM BD_USERS.Users;";

//         var alunos = conn.Connection.Query<User>(sql: query);

//         return alunos.ToList();
//     }
// }


    
    
