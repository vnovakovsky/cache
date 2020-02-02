using Cache.ReplacementStrategy;
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
            int obj2 = (int)System.Convert.ChangeType(tag, typeof(int));

            throw new NotImplementedException();
        }
        public static CacheController<Tag> Create(CacheGeometry cacheGeometry, IStorage<Tag> storage
                                                    , IReplacementStrategy<Tag> replacementStrategy)
        {
            Tag tag = default;
            int obj2 = (int)System.Convert.ChangeType(tag, typeof(int));

            throw new NotImplementedException();
        }
    }
}
