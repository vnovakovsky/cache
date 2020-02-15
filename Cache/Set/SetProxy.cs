using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Cache.Set.Cs
{
    public class SetProxy
    {
        
        Set set_;

        public SetProxy(int numberOfLines, int wordsInLine, int wordSize)
        {
            set_ = new Set(numberOfLines, wordsInLine, wordSize);
        }

        public void PutWord(int tag, List<Cache.Word> data)
        {
            int i = 0;
            foreach(Cache.Word word in data)
            {
                bool isFinal = data.Count - i == 1 ? true : false;
                unsafe
                {
                    fixed (void* buf = word.Buffer)
                    {
                        set_.PutWord(tag, word.Tag, i, buf, word.Buffer.Length, isFinal);
                    }
                }
                ++i;
            }
        }
        public byte[] FindWord(int tag, bool invalidate)
        {
            int line = set_.FindLine(tag, invalidate);
            if (Set.kNotFound == line)
            {
                return null;
            }
            unsafe
            {
                int length;
                void* pWord = set_.FindWord(tag, line, &length);
                if (pWord != null)
                {
                    byte[] bytes = new byte[length];
                    Marshal.Copy(new IntPtr(pWord),     // source
                        bytes, 0, length);              // destination
                    return bytes;
                }
            }
            return null;
        }
    };
}
