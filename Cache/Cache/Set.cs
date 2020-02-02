using CacheSet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class Set <Tag> where Tag : unmanaged
    {
        private readonly SetProxy setProxy_;
        public Set(int numberOfLines, int wordsInLine, int wordSize)
        {
            SetProxy setProxy = new SetProxy(numberOfLines, wordsInLine, wordSize);
            setProxy_ = setProxy;
        }

        public SetProxy SetProxy => setProxy_;

        public void PutWord(Tag tag, List<Word> data)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof(Tag));

            int iTag = (int)converter.ConvertFrom(null,
                CultureInfo.InvariantCulture, tag);
            SetProxy.PutWord(iTag, data);
        }
    }
}
