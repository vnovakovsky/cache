﻿using System;
using Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Cache
{
    [TestClass]
    public class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            CacheLib.Initialize();
            Logger.Initialize();
        }
    }
}
