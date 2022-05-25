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

                    //Creating DBConnection
                    IMongoClient _client = new MongoClient(DatabaseSettings.ConnectionString);
                    IMongoDatabase _database = _client.GetDatabase(DatabaseSettings.DatabaseName);
                    var _collection = _database.GetCollection<AccessLog>(DatabaseSettings.AccessLogCollectionName);

                    //Receiving data 
                    var sw1 = new Stopwatch();
                    sw1.Start();
                    string? data;
                    data = sr.ReadLine();
                    var listLogDeserialized = JsonSerializer.Deserialize<List<AccessLog>>(data);
                    sw1.Stop();
                    Console.WriteLine("Tempo gasto p/ receber dados do Client e deserializar: " + sw1.Elapsed.ToString() + " segundos");

                    //Sending response to Client
                    sw.WriteLine("Server received the data!");
                    sw.Flush();

                    //Sending data to Database
                    var sw2 = new Stopwatch();
                    sw2.Start();
                    _collection.InsertManyAsync(listLogDeserialized);
                    sw2.Stop();
                    Console.WriteLine("Data saved in Database!");
                    Console.WriteLine("Tempo gasto p/ salvar no MongoDB: " + sw2.Elapsed.ToString() + " segundos");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong...");
            }

        }
    }
}
