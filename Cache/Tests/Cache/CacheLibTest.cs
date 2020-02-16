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
            Logger.LogDebug("debug test string");
        }

        [TestMethod]
        public void LogInfo()
        {
            Logger.LogInfo("info test string");
        }
    }
}
