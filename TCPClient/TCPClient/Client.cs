using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TCPClient.DataReader;

namespace TCPClient
{
    public class Client
    {
        private string ipClient = "127.0.0.1";
        private int port = 1300;
        private string filePath = @"C:\dev\TCP-client-server\TCPClient\TCPClient\Resources\access_split.log";

        DataLogReader dataReader = new DataLogReader();
        public Client()
        {
        }
        public void StartClient()
        {
        connection:
            try
            {
                TcpClient client = new TcpClient(ipClient, port);

                Console.WriteLine("Starting to read the file...");
                ArrayList fileRows = dataReader.FileReader(filePath);
                List<AccessLog> listLogs = dataReader.AccessLogReader(fileRows);

                Console.WriteLine("Starting to send data to server...");

                NetworkStream stream = client.GetStream();
                StreamWriter sw = new StreamWriter(stream);
                sw.AutoFlush = true;
                StreamReader sr = new StreamReader(stream);

                string logJsonData = JsonSerializer.Serialize(listLogs);
                sw.WriteLine(logJsonData);

                string response = sr.ReadLine();
                Console.WriteLine(response);

                stream.Close();
                client.Close();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed to connect...");
                goto connection;
            }

        }
    }
}
