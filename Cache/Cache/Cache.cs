using Cache.ReplacementStrategy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Cache.Util;

namespace Cache
{
    public class Cache <Tag> where Tag : unmanaged, IComparable
    {
        readonly CacheGeometry cacheGeometry_;
        List<Set<Tag>> sets_;
        //IReplacementStrategy<Tag> replacementStrategy_;
        public Cache(CacheGeometry cacheGeometry)
        {
            cacheGeometry_ = cacheGeometry;
            Sets = new List<Set<Tag>>();
            for(int i = 0; i < CacheGeometry.NumberOfWays; ++i)
            {
                Sets.Add(new Set<Tag>(CacheGeometry.LinesPerSet
                                        , CacheGeometry.WordsInLine
                                        , CacheGeometry.WordSize
                                        , i));
            }
        }

        public List<Set<Tag>> Sets { get => sets_; private set => sets_ = value; }

        public CacheGeometry CacheGeometry => cacheGeometry_;

        public Word ReadWord(Tag tag, bool invalidate = false)
        {
            int iTag = ConvertToInt(tag);
            for (int i = 0; i < CacheGeometry.NumberOfWays; ++i)
            {
                Word word = sets_[i].FindWord(tag, invalidate);
                if (!word.IsEmpty)
                    return word;
            }
            return Word.CreateEmpty(iTag);
        }
        // write-through approach
        public void SaveLine(int setIndex, Tag tag, List<Word> words)
        {
            // check preconditions
            foreach(var w in words)
            {
                if (w.Buffer.Length > CacheGeometry.WordSize)
                {
                    throw new ArgumentOutOfRangeException(
                        string.Format("Supplied word with length {0}B exeeds cache word buffer with size {1}B"
                                                                  , w.Buffer.Length, CacheGeometry.WordSize));
                }
            }

            sets_[setIndex].PutWord(tag, words);
        }
    }
}
