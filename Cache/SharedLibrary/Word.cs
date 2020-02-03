using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class Word
    {
        byte[] buffer_;
        int tag_;
        int setIndex_;
        public bool isCached;
        public Word(int tag, byte[] bytes, int setIndex = -1)
        {
            Tag = tag;
            Buffer = bytes;
            SetIndex = setIndex;
        }
        public int Tag { get => tag_; private set => tag_ = value; }
        public byte[] Buffer { get => buffer_; private set => buffer_ = value; }
        public int SetIndex { get => setIndex_; set => setIndex_ = value; }
        public bool IsEmpty => Buffer == null;
        public static Word CreateEmpty(int iTag)
        {
            return new Word(iTag, null, -1);
        }
    }
}
