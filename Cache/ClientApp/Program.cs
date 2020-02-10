using Cache;
using Cache.CacheController;
using Cache.ReplacementStrategy;
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
        static DatabaseStorageMock<int, string> databaseStorage_ = new DatabaseStorageMock<int, string>();
        const int kNumberOfWays = 4;
        const int kLinesDegree = 4;
        const int kWordsInLine = 4;
        const int kWordSize = 8;
        static ICacheController<int> CreateController()
        {
            databaseStorage_.database_.Clear();
            FillDatabaseTest();
            CacheGeometry cacheGeometry = new CacheGeometry(numberOfWays: kNumberOfWays
                                                            , linesDegree: kLinesDegree
                                                            , wordsInLine: kWordsInLine
                                                            , wordSize: kWordSize);
            IReplacementStrategy<int> replacementStrategy = new LRUStrategy<int>(cacheGeometry);
            ICacheController<int> cacheController =
                CacheFactory<int>.Create(cacheGeometry, databaseStorage_, replacementStrategy);

            return cacheController;
        }
        static void Main(string[] args)
        {
            ICacheController<int> cacheController = CreateController();

            // write -> read
            byte[] bytes42 = Encoding.ASCII.GetBytes("first42");
            byte[] bytes43 = Encoding.ASCII.GetBytes("first43");
            Word word42 = new Word(42, bytes42);
            Word word43 = new Word(43, bytes43);

            cacheController.WriteWord(42, word42);
            Word wordBack42 = cacheController.ReadWord(42);

            cacheController.WriteWord(43, word43);
            Word wordBack43 = cacheController.ReadWord(43);

            // update -> read
            byte[] byteSecond42 = Encoding.ASCII.GetBytes("second42");
            byte[] byteSecond43 = Encoding.ASCII.GetBytes("second43");
            Word wordSecond42 = new Word(42, bytes42);
            Word wordSecond43 = new Word(43, bytes43);

            cacheController.WriteWord(42, word42);
            Word wordBackSecond42 = cacheController.ReadWord(42);

            cacheController.WriteWord(43, word43);
            Word wordBackSecond43 = cacheController.ReadWord(43);
        }
        static public void FillDatabaseTest()
        {
            for (int i = 0; i < Math.Pow(2, 8); ++i)
            {
                string s = string.Format("test{0:0000}", i);
                databaseStorage_.WriteWord(i, Convert(s));
            }

        }
        static byte[] Convert(string inValue)
        {
            return Encoding.ASCII.GetBytes(inValue);
        }
    }
}
