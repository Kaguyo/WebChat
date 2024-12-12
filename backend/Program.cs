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
            const int PORT = 5000;
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{PORT}/");
            listener.Start();

            Console.WriteLine($"Servidor iniciado... Aguardando requisições. (PORT:{PORT})");

            while (true)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;

                    context.Response.AddHeader("Access-Control-Allow-Origin", "http://127.0.0.1:5500");
                    context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                    context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

                    if (request.HttpMethod == "OPTIONS")
                    {
                        context.Response.StatusCode = 200; // OK 
                        context.Response.Close();
                        continue;
                    }

                    if (request.HttpMethod == "POST")
                    {
                        using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                        {
                            string json = reader.ReadToEnd();

                            // Tentativa de desserialização do JSON
                            try
                            {
                                Usuario? usuario = JsonConvert.DeserializeObject<Usuario>(json);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                Console.WriteLine($"Usuário recebido: {usuario.Username}, {usuario.PhoneNumber}");
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                                // Respondendo com uma mensagem de sucesso
                                HttpListenerResponse response = context.Response;
                                string responseString = "{\"message\": \"Usuário recebido com sucesso!\"}";
                                byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                                response.ContentLength64 = buffer.Length;
                                response.OutputStream.Write(buffer, 0, buffer.Length);
                                response.OutputStream.Close();
                            }
                            catch (JsonException ex)
                            {
                                Console.WriteLine($"Erro de desserialização: {ex.Message}");
                                context.Response.StatusCode = 400; // Bad Request
                                context.Response.Close();
                            }
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 405; // Method Not Allowed
                        context.Response.Close();
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
            public string Username { get; set; } = "";
            public string PhoneNumber { get; set; } = "";
            public string Password { get; set; } = "";
            public string Password2 { get; set; } = "";
        }
    }
}
