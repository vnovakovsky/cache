using System;
using Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Cache
{
    [TestClass]
    public class SetTest
    {
        [TestMethod]
        public void CreateSet()
        {
            Set<int> set = new Set<int>(4, 2, 16);
        }
    }
}
