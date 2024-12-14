using System.Net;
using System.Text;
using System.Text.Json;


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
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Number { get; set; }
        public string? Password { get; set; }
        public string? Password2 { get; set; }
    }
}