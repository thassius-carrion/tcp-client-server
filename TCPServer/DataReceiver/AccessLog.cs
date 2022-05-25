using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.DataReceiver
{
    public class AccessLog
    {
        public string ip { get; set; }
        public string data { get; set; }
        public string httpMethod { get; set; }
        public string url { get; set; }
        public string httpProtocol { get; set; }
        public string statusCode { get; set; }
        public string size { get; set; }

        public AccessLog(string Ip, string Data, string HttpMethod, string Url, string HttpProtocol, string StatusCode, string Size)
        {
            this.ip = Ip;
            this.data = Data;
            this.httpMethod = HttpMethod;
            this.url = Url;
            this.httpProtocol = HttpProtocol;
            this.statusCode = StatusCode;
            this.size = Size;
        }

        public AccessLog() { }
    }
}
