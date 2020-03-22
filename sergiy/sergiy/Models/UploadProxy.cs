using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sergiy.Models
{
    public class UploadProxy
    {
        public string IP { get; set; }
        public int Port { get; set; }

         public UploadProxy(string ip, int port)
        {
            IP = ip;
            Port = port;
        }
    }
}
