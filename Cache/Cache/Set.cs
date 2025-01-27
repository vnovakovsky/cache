﻿//using CacheSet;
using static Cache.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cache
{
    public class Set <Tag> where Tag : unmanaged, IComparable
    {
        private readonly int id_;
        private readonly Cache.Set.ISet setProxy_;
        public Set(int numberOfLines, int wordsInLine, int wordSize, int id)
        {
            id_ = id;
            Cache.Set.ISet setProxy = SetFactory.Create(numberOfLines, wordsInLine, wordSize);
            setProxy_ = setProxy;
        }

        public Cache.Set.ISet SetProxy => setProxy_;

        public void PutWord(Tag tag, List<Word> data)
        {
            int iTag = ConvertToInt(tag);
            SetProxy.PutWord(iTag, data);
        }

        public Word FindWord(Tag tag, bool invalidate = false)
        {
            int iTag = ConvertToInt(tag);
            byte[] bytes = SetProxy.FindWord(iTag, invalidate);
            Word word = new Word(iTag, bytes, id_);
            return word;
        }
    }
}
