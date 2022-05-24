using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TCPClient.DataReader
{
    public class DataLogReader
    {
        public DataLogReader()
        {
        }

        //LEITURA DO ARQUIVO .LOG PARA UM ARRAY DAS LINHAS DO ARQUIVO
        public ArrayList FileReader(string path)
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

        // SEPARACAO LINHA A LINHA NOS ATRIBUTOS (IP, DATA, ...)
        public List<AccessLog> AccessLogReader(ArrayList fileRows)
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

                    AccessLog log = new AccessLog(ip, data);
                    listLogs.Add(log);
                }
            }
            return listLogs;
        }

    }
}
