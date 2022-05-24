using MongoDB.Driver;
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

                    //Reading data sent by Client
                    string? data;
                    data = sr.ReadLine();

                    var listLogDeserialized = JsonSerializer.Deserialize<List<AccessLog>>(data);

                    //Sending response to CLient
                    sw.WriteLine("Server received the data!");
                    sw.Flush();

                    //Creating DBConnection
                    IMongoClient _client = new MongoClient(DatabaseSettings.ConnectionString);
                    IMongoDatabase _database = _client.GetDatabase(DatabaseSettings.DatabaseName);
                    var _collection = _database.GetCollection<AccessLog>(DatabaseSettings.AccessLogCollectionName);

                    //Sending data to Database
                    _collection.InsertManyAsync(listLogDeserialized);
                    Console.WriteLine("Data saved in Database!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong...");
            }

        }
    }
}
