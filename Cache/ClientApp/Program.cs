using Cache;
using Mocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
        static byte[] Convert(string inValue)
        {
            return Encoding.ASCII.GetBytes(inValue);
        }
    }
}
