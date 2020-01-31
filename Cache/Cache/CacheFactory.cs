using Cache.ReplacementStrategy;
using System;

namespace Cache
{
    public static class CacheFactory<Tag>
    {
        public static CacheController<Tag> Create(CacheGeometry cacheGeometry, IStorage<Tag> storage)
        {
            throw new NotImplementedException();
        }
        public static CacheController<Tag> Create(CacheGeometry cacheGeometry, IStorage<Tag> storage
                                                    , IReplacementStrategy<Tag> replacementStrategy)
        {
            throw new NotImplementedException();
        }
    }
}
