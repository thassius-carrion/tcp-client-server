using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TCPServer.DataReceiver;

namespace TCPServer
{
    public class Server
    {
        TcpListener listener;
        public Server()
        {
            listener = new TcpListener(System.Net.IPAddress.Any, 1300);
        }
        
        public void StartServer()
        { 
            
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client accepted.");

                NetworkStream stream = client.GetStream();
                StreamReader sr = new StreamReader(client.GetStream());
                StreamWriter sw = new StreamWriter(client.GetStream());

                try
                {
                    byte[] buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);
                    int recv = 0;

                    foreach(byte b in buffer)
                    {
                        if (b != 0)
                        {
                            recv++;
                        }
                    }

                    string jsonData = Encoding.UTF8.GetString(buffer, 0, recv);

                    var listLogsDeserilized = JsonSerializer.Deserialize<List<AccessLog>>(jsonData);

                    foreach (AccessLog log in listLogsDeserilized)
                    {
                        Console.WriteLine(log.ip);
                        Console.WriteLine(log.data);
                    }

                    Console.WriteLine("Data received: " + jsonData);
                    sw.WriteLine("Server received the data!");
                    sw.Flush();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong...");
                    sw.WriteLine(ex.ToString());
                }

            }
        }
    }
}
