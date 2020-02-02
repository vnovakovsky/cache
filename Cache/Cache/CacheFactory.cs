using Cache.ReplacementStrategy;
using static Cache.Util;
using System;

namespace Cache
{
    public static class CacheFactory<Tag> where Tag : unmanaged
    {
        public static CacheController<Tag> Create(CacheGeometry cacheGeometry, IStorage<Tag> storage)
        {
            /*  the purpose of this factory is to check 
                whether it's possible to convert Tag to int in run-time
            */
            Tag tag = default;
            int obj2 = ConvertToInt(tag);

            CacheController<Tag> cacheController = new CacheController<Tag>(cacheGeometry, storage);
            return cacheController;
        }
        public static CacheController<Tag> Create(CacheGeometry cacheGeometry, IStorage<Tag> storage
                                                    , IReplacementStrategy<Tag> replacementStrategy)
        {
            CacheController<Tag> cacheController = Create(cacheGeometry, storage);
            cacheController.ReplacementStrategy = replacementStrategy;
            return cacheController;
        }
    }
}
