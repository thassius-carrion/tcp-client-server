using MongoDB.Driver;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
using TCPServer.DataReceiver;
using TCPServer.Models;

namespace TCPServer
{
    public class Server
    {
        private TcpListener listener;
        public Server()
        {
            listener = new TcpListener(System.Net.IPAddress.Any, 1300);
        }

        public void StartServer()
        {
            try
            {
                listener.Start();

                while (true)
                {
                    //Connecting to TCPClient
                    Console.WriteLine("Waiting for a connection...");
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Client accepted.");

                    NetworkStream stream = client.GetStream();
                    StreamReader sr = new StreamReader(stream);
                    StreamWriter sw = new StreamWriter(stream);
                    var timer = new Stopwatch();
                    var timerTotal = new Stopwatch();
                    timerTotal.Start();

                    //Creating DBConnection
                    IMongoClient _client = new MongoClient(DatabaseSettings.ConnectionString);
                    IMongoDatabase _database = _client.GetDatabase(DatabaseSettings.DatabaseName);
                    var _collection = _database.GetCollection<AccessLog>(DatabaseSettings.AccessLogCollectionName);

                    //Receiving data 
                    string? data;
                    data = sr.ReadLine();
                    timer.Start();
                    var listLogDeserialized = JsonSerializer.Deserialize<List<AccessLog>>(data);
                    timer.Stop();
                    Console.WriteLine("Tempo gasto p/ receber e deserializar: " + timer.Elapsed.ToString() + " segundos");

                    //Sending response to Client
                    sw.WriteLine("Server received the data!");
                    sw.Flush();

                    //Sending data to Database
                    timer.Restart();
                    _collection.InsertManyAsync(listLogDeserialized);
                    timer.Stop();
                    Console.WriteLine("Data saved in Database!");
                    Console.WriteLine("Tempo gasto p/ salvar no MongoDB: " + timer.Elapsed.ToString() + " segundos");

                    timerTotal.Stop();
                    Console.WriteLine("Tempo total de execução: " + timerTotal.Elapsed.ToString() + " segundos");

                    Console.ReadKey();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong...");
            }

        }
    }
}
