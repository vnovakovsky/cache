using CacheSet;
using static Cache.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class Set <Tag> where Tag : unmanaged
    {
        private readonly int id_;
        private readonly SetProxy setProxy_;
        public Set(int numberOfLines, int wordsInLine, int wordSize, int id)
        {
            id_ = id;
            SetProxy setProxy = new SetProxy(numberOfLines, wordsInLine, wordSize);
            setProxy_ = setProxy;
        }

        public SetProxy SetProxy => setProxy_;

        public void PutWord(Tag tag, List<Word> data)
        {
            int iTag = ConvertToInt(tag);
            SetProxy.PutWord(iTag, data);
        }

        public Word FindWord(Tag tag)
        {
            int iTag = ConvertToInt(tag);
            byte[] bytes = SetProxy.FindWord(iTag);
            Word word = new Word(iTag, bytes, id_);
            return word;
        }
    }
}
