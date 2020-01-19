using System;
using Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class CacheLibTest
    {
        [TestMethod]
        public void LogDebug()
        {
            CacheLib.Initialize();
            CacheLib.LogDebug("debug test string");
        }
    }
}
