using TCPClient;

Client client = new Client();
client.StartClient();





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

