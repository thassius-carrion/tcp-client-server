using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Models
{
    public static class DatabaseSettings
    {
        public static readonly string ConnectionString = "mongodb://127.0.0.1:27017";
        public static readonly string DatabaseName = "testdb";
        public static readonly string AccessLogCollectionName = "LogCollection";
    }
}
