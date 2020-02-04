using System;
using static Cache.Util;
using Cache.CacheController;
using Cache.ReplacementStrategy;


namespace Cache
{
    public static class CacheFactory<Tag> where Tag : unmanaged
    {
        public static ICacheController<Tag> Create(CacheGeometry cacheGeometry, IStorage<Tag> storage)
        {
            /*  the purpose of this factory is to check 
                whether it's possible to convert Tag to int in run-time
            */
            Tag tag = default;
            int obj2 = ConvertToInt(tag);

            ICacheController<Tag> cacheController = new AroundCacheController<Tag>(cacheGeometry, storage);
            return cacheController;
        }
        public static ICacheController<Tag> Create(CacheGeometry cacheGeometry, IStorage<Tag> storage
                                                    , IReplacementStrategy<Tag> replacementStrategy)
        {
            ICacheController<Tag> cacheController = Create(cacheGeometry, storage);
            cacheController.ReplacementStrategy = replacementStrategy;
            return cacheController;
        }
    }
}
