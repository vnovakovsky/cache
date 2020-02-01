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
        public Word(int tag, byte[] bytes)
        {
            Tag = tag;
            Buffer = bytes;
        }
        public int Tag { get => tag_; set => tag_ = value; }
        public byte[] Buffer { get => buffer_; set => buffer_ = value; } //Todo read only?
    }
}
