using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TCPClient.DataReader
{
    public static class DataLogReader
    {

        public static ArrayList FileReader(string path)
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

        public static List<AccessLog> AccessLogReader(ArrayList fileRows)
        {
            List<AccessLog> listLogs = new();

            foreach (string row in fileRows)
            {
                string pattern = "^([0-9]{0,3}\\d+\\.[0-9]{0,3}\\d+\\.[0-9]{0,3}\\d+\\.[0-9]{0,3}\\d+) - - (\\[[^\\]]+\\]) \"([A-Z]+) ([^ \"]+)? (HTTP/[0-9.]+)\" ([0-9]{0,3}\\d+) ([0-9]*)";
                MatchCollection matches = Regex.Matches(row, pattern);

                foreach (Match match in matches)
                {
                    string ip = match.Groups[1].Value;
                    string data = match.Groups[2].Value;
                    string httpMethod = match.Groups[3].Value;
                    string url = match.Groups[4].Value;
                    string httpProtocol = match.Groups[5].Value;
                    string statusCode = match.Groups[6].Value;
                    string size = match.Groups[7].Value;

                    AccessLog log = new AccessLog(ip, data, httpMethod, url, httpProtocol, statusCode, size);
                    listLogs.Add(log);
                }
            }
            return listLogs;
        }

    }
}
