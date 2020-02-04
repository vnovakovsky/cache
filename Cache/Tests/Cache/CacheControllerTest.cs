﻿using System;
using System.Text;
using Cache;
using Cache.ReplacementStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;

namespace Tests.Cache
{
    [TestClass]
    public class CacheControllerTest
    {
        DatabaseStorageMock<int, string> databaseStorage_ = new DatabaseStorageMock<int, string>();
        const int kNumberOfWays = 4;
        const int kLinesDegree = 4;
        const int kWordsInLine = 4;
        const int kWordSize = 4;

        CacheController<int> CreateController()
        {
            databaseStorage_.database_.Clear();
            FillDatabaseTest();
            CacheGeometry cacheGeometry = new CacheGeometry(numberOfWays: kNumberOfWays
                                                            , linesDegree: kLinesDegree
                                                            , wordsInLine: kWordsInLine
                                                            , wordSize: kWordSize);
            IReplacementStrategy<int> replacementStrategy = new LRUStrategy<int>(cacheGeometry);
            CacheController<int> cacheController =
                CacheFactory<int>.Create(cacheGeometry, databaseStorage_, replacementStrategy);

            return cacheController;
        }
        [TestMethod]
        public void ReadWordTest1()
        {
            CacheController<int> cacheController = CreateController();
            Word word42     = cacheController.ReadWord(42);
            Word word42Hit  = cacheController.ReadWord(42); // read hit
            Assert.AreEqual(word42.Tag, word42Hit.Tag);

            Word word = cacheController.ReadWord(43);
            Word wordHit = cacheController.ReadWord(43); // read hit
            Assert.AreEqual(word.Tag, wordHit.Tag);
        }
        [TestMethod]
        public void ReadWordTest2()
        {
            CacheController<int> cacheController = CreateController();
            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                Word wordHit = cacheController.ReadWord(i); // read hit
                Assert.AreEqual(word.Tag, wordHit.Tag);
            } while (!databaseStorage_.EOF(i++));

        }
        [TestMethod]
        public void ReadWordTest3()
        {
            CacheController<int> cacheController = CreateController();
            const int wordsInLine = 4;

            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                if (i % wordsInLine == 0)
                {
                    Assert.AreEqual(word.isCached, false);
                }
                else
                {
                    Assert.AreEqual(word.isCached, true);
                }
            } while (!databaseStorage_.EOF(i++));
        }
        [TestMethod]
        public void ReadWordTest4()
        {
            CacheController<int> cacheController = CreateController();
            const int wordsInLine = 4;

            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                if (i % wordsInLine == 0)
                {
                    Assert.AreEqual(word.isCached, false);
                    word = cacheController.ReadWord(i);
                    Assert.AreEqual(word.isCached, true);
                }
                else
                {
                    Assert.AreEqual(word.isCached, true);
                }
            } while (!databaseStorage_.EOF(i++));
        }
        [TestMethod]
        public void ReadWordTest5_FirstRepresentativeZero()
        {
            CacheController<int> cacheController = CreateController();
            int linesPerSet = (int)Math.Pow(2, kLinesDegree) / kNumberOfWays; ;

            // iterate throug all members of class [0] = Z linesPerSet_Mod = Z mod linesPerSet_Mod
            int representative = 0;
            int tag = 0;
            for (int c = 0; tag < databaseStorage_.MaxKey(); ++c)
            {
                tag = representative + linesPerSet * c;
                if (tag >= databaseStorage_.MaxKey())
                    break;
                Word word = cacheController.ReadWord(tag);
                Assert.AreEqual(word.SetIndex, c % kNumberOfWays);
                
            }
        }
        [TestMethod]
        public void ReadWordTest6_AllRepresentatives()
        {
            CacheController<int> cacheController = CreateController();
            int linesPerSet_Mod = (int)Math.Pow(2, kLinesDegree) / kNumberOfWays;

            // iterate throug all representatives of ring (Z mod linesPerSet_Mod): [0], [1], ..., [linesPerSet_Mod - 1]
            // rep = representative
            for (int rep = 0; rep < linesPerSet_Mod; ++rep)
            {
                // iterate throug all members of class [a] = Z linesPerSet_Mod = Z mod linesPerSet_Mod
                // [a] = {x: z mod linesPerSet_Mod == a, z E Z}
                //int tag = 0;
                //int tag = representative + linesPerSet_Mod * 0;
                //for (int c = 0; tag < databaseStorage_.MaxKey(); ++c)
                for (int c = 0, tag = rep + linesPerSet_Mod * c;
                    tag < databaseStorage_.MaxKey();
                    ++c, tag = rep + linesPerSet_Mod * c)
                {
                    //tag = representative + linesPerSet_Mod * c;
                    //if (tag >= databaseStorage_.MaxKey())
                    //    break;
                    Word word = cacheController.ReadWord(tag);
                 
                    Assert.AreEqual(word.SetIndex, c % kNumberOfWays);
                }
            }
        }
        private void ReadWord(CacheController<int> cacheController, int tag)
        {
            Word word = cacheController.ReadWord(tag);
            Word wordHit = cacheController.ReadWord(tag); // read hit
            Assert.AreEqual(word.Tag, wordHit.Tag);
        }
        [TestMethod]
        public void FillDatabaseTest()
        {
            for(int i = 0; i < Math.Pow(2, 8); ++i)
            {
                string s = string.Format("test{0:0000}", i);
                databaseStorage_.WriteWord(i, Convert(s));
            }
 
            Assert.AreEqual(databaseStorage_.database_.Count, Math.Pow(2, 8));
        }
        static byte[] Convert(string inValue)
        {
            return Encoding.ASCII.GetBytes(inValue);
        }
    }
}
