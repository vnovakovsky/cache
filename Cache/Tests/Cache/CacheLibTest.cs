using System;
using Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Cache
{
    [TestClass]
    public class CacheLibTest
    {
        [TestMethod]
        public void LogDebug()
        {
            CacheLib.LogDebug("debug test string");
        }

        [TestMethod]
        public void LogInfo()
        {
            CacheLib.LogInfo("info test string");
        }
    }
}
