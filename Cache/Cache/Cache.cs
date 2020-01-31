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
    class Cache <Tag>
    {
        IReplacementStrategy<Tag> replacementStrategy_;
        public Cache(CacheGeometry cacheGeometry)
        {
        }
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
