using Mocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            DatabaseStorageMock<int, string> databaseStorage = new DatabaseStorageMock<int, string>();

            databaseStorage.WriteWord(1, Convert("a"));
            databaseStorage.WriteWord(2, Convert("b"));
            databaseStorage.WriteWord(3, Convert("c"));
            databaseStorage.WriteWord(4, Convert("d"));
            databaseStorage.WriteWord(5, Convert("e"));
        }
        static byte[] Convert(string inValue)
        {
            return Encoding.ASCII.GetBytes(inValue);
        }
    }
}
