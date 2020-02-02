using Cache.ReplacementStrategy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    class Cache <Tag> where Tag : unmanaged
    {
        readonly CacheGeometry cacheGeometry_;
        List<Set<Tag>> sets_;
        IReplacementStrategy<Tag> replacementStrategy_;
        public Cache(CacheGeometry cacheGeometry)
        {
            cacheGeometry_ = cacheGeometry;
            Sets = new List<Set<Tag>>();
            for(int i = 0; i < CacheGeometry.NumberOfWays; ++i)
            {
                Sets.Add(new Set<Tag>(CacheGeometry.NumberOflines
                                        , CacheGeometry.WordsInLine
                                        , CacheGeometry.WordSize));
            }
        }

        //public CacheGeometry CacheGeometry { get => cacheGeometry_; private set => cacheGeometry_ = value; }
        public List<Set<Tag>> Sets { get => sets_; set => sets_ = value; }

        public CacheGeometry CacheGeometry => cacheGeometry_;

        public void ReadWord(Tag tag)
        {

        }
        public void SaveLine(Tag tag, Tag[] tags, List<Word> words)
        {

        }
        public void SetReplacementStrategy(IReplacementStrategy<Tag> replacementStrategy)
        {

        }
    }
}
