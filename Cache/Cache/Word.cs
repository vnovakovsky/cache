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
        public Word(byte[] bytes)
        {
        }

        public byte[] Buffer { get => buffer_; set => buffer_ = value; } //Todo read only?
    }
}
