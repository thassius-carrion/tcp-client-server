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

        public AccessLog(string Ip, string Data)
        {
            this.ip = Ip;
            this.data = Data;
        }

        public AccessLog() { }
    }
}
