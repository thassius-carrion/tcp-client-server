// See https://aka.ms/new-console-template for more information
using System.Collections;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using TCPClient.DataReader;


void Client()
{
    connection:
    try
    {
        TcpClient client = new TcpClient("127.0.0.1", 1300);
        //string messageToSend = "Ola meu nome é thass";

        string filePath = @"C:\dev\TCP-client-server\TCPClient\TCPClient\Resources\access_split.log";
        ArrayList fileRow = FileReader(filePath);
        List<AccessLog> listLogs = AccessLogReader(fileRow);

        Console.WriteLine("Starting to send data to server...");
        string jsonData = JsonSerializer.Serialize(listLogs);

        Console.WriteLine("Get bytes...");
        int byteCount = Encoding.ASCII.GetByteCount(jsonData + 1);

        Console.WriteLine("Get bytes 2...");
        byte[] data = new byte[byteCount];

        Console.WriteLine("Get bytes 3...");
        data = Encoding.ASCII.GetBytes(jsonData);

        Console.WriteLine("Get bytes 4...");
        NetworkStream stream = client.GetStream();
        stream.Write(data, 0, data.Length);

        Console.WriteLine("Get bytes 5...");
        StreamReader sr = new StreamReader(stream);

        Console.WriteLine("Get bytes 6...");
        string response = sr.ReadLine();
        Console.WriteLine(response);

        stream.Close();
        client.Close();
        Console.ReadKey();
    }
    catch(Exception ex)
    {
        Console.WriteLine("failed to connect...");
        goto connection;
    }
}

Client();


//LEITURA DO ARQUIVO .LOG PARA UM ARRAY DAS LINHAS DO ARQUIVO
ArrayList FileReader(string path)
{
    ArrayList fileRows = new ArrayList();
    string? line;

    using var file = new StreamReader(path);
    while ((line = file.ReadLine()) != null)
    {
        fileRows.Add(line);
    }
    file.Close();
    return fileRows;
}

// SEPARACAO LINHA A LINHA NOS ATRIBUTOS (IP, DATA)
List<AccessLog> AccessLogReader(ArrayList fileRows)
{
    List<AccessLog> listLogs = new();

    foreach (string row in fileRows)
    {
        string pattern = @"^([1-9]{0,3}\d+\.[1-9]{0,3}\d+\.[1-9]{0,3}\d+\.[1-9]{0,3}\d+) - - (\[[^\]]+\])";
        MatchCollection matches = Regex.Matches(row, pattern);

        foreach (Match match in matches)
        {
            string ip = match.Groups[1].Value;
            string data = match.Groups[2].Value;

            //Console.WriteLine("Ip : {0}", ip);
            //Console.WriteLine("Data : {0}", data);

            AccessLog log = new AccessLog(ip, data);
            listLogs.Add(log);
        }
    }

    return listLogs;
}

/*  TENTATIVAS REGEX
    var regex = "^(?<client>\\S+) \\S+ (?<userid>\\S+) \\[(?<datetime>[^\\]]+)\\] \"(?<method>[A - Z] +)(?<request>[^\"]+)? HTTP/[0-9.]+\"(?<status>[0 - 9]{ 3}) (?<size>[0 - 9] +| -)";
    var regex = new Regex(@"^([1-9]{0,3}\d+\.[1-9]{0,3}\d+\.[1-9]{0,3}\d+\.[1-9]{0,3}\d+) - - (\[[^\]]+\])", RegexOptions.IgnoreCase);
    var lineSplit = Regex.Split(row, regex);
 */

/* CONTADOR DE TEMPO DE EXECUCAO
    var sw = new Stopwatch();
    sw.Start();
    sw.Stop();
    Console.WriteLine("Tempo gasto : " + sw.ElapsedMilliseconds.ToString() + " milisegundos");
    Console.WriteLine("Tempo gasto : " + sw.Elapsed.ToString() + " segundos");
*/

