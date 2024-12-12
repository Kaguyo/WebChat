using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace backend
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();

            Console.WriteLine("Servidor iniciado... Aguardando requisições.");

            while (true) // Test
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;

                    if (request.HttpMethod == "POST")
                    {
                        using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                        {
                            string json = reader.ReadToEnd();

                            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(json);

                            Console.WriteLine($"Usuário recebido: {usuario.Username}, {usuario.PhoneNumber}");

                            HttpListenerResponse response = context.Response;
                            string responseString = "{\"message\": \"Usuário recebido com sucesso!\"}";
                            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                            response.ContentLength64 = buffer.Length;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                            response.OutputStream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar a requisição: {ex.Message}");
                }
            }
        }

        public class Usuario
        {
            public string Username { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public string Password2 { get; set; }
        }
    }
}
