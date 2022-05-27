using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly string ipClient = "127.0.0.1";
        private readonly int port = 1300;
        private readonly string filePath = @"C:\dev\TCP-client-server\TCPClient\TCPClient\Resources\access_split.log";

        public Client()
        {
        }
        public void StartClient()
        {
        connection:
            try
            {
                TcpClient client = new TcpClient(ipClient, port);
                Console.WriteLine("Connected to server!");

                NetworkStream stream = client.GetStream();
                StreamWriter sw = new StreamWriter(stream);
                StreamReader sr = new StreamReader(stream);
                sw.AutoFlush = true;
                var timer = new Stopwatch();

                Console.WriteLine("Starting to read the file...");
                timer.Start();
                ArrayList fileRows = DataLogReader.FileReader(filePath);
                List<AccessLog> listLogs = DataLogReader.AccessLogReader(fileRows);
                timer.Stop();
                Console.WriteLine("Tempo gasto p/ ler arquivo : " + timer.Elapsed.ToString() + " segundos");
                Console.WriteLine("LINHAS LIDAS:" + listLogs.Count);

                Console.WriteLine("Starting to send data to server...");
                timer.Restart();
                string logJsonData = JsonSerializer.Serialize(listLogs);
                sw.WriteLine(logJsonData);
                timer.Stop();
                Console.WriteLine("Tempo gasto de mandar p/ server : " + timer.Elapsed.ToString() + " segundos");

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
