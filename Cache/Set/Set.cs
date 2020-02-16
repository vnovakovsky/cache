using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Cache.Set.Cs
{
    struct WordsMapEntry
    {
		public int tag;
        public int length; // length of serialized data
    }

    struct LinePrefix
    {
        public int isValid;
        public int firstTag;
        public int lastTag;
    }

    unsafe struct Line
    {
        private int             wordsInLine_;
        private int             wordSize_;
        private LinePrefix*     linePrefixPtr_;
        private WordsMapEntry*  wordsMap_;  // -1 indicates empty (NULL) entry
        private Byte*           words_;

        // Line: {LinePrefix, WordsMap[wordsInLine_] {{tag1, length1}, ...{tagWL, lengthWL}}, ...Words[wordsInLine_]{w1,..wWL}}
        public Line(int wordsInLine, int wordSize, Byte* lineBeg) : this()
        {
            this.wordsInLine_   = wordsInLine;
            this.wordSize_      = wordSize;
            this.linePrefixPtr_ = (LinePrefix*) lineBeg;
            this.wordsMap_      = (WordsMapEntry*)(getLinePrefixEnd());
            this.words_         = getWordsMapEnd();
        }

        public LinePrefix*      getLinePrefixPtr() { return linePrefixPtr_; }
        Byte*			        getLinePrefixEnd() { return (Byte*)linePrefixPtr_ + sizeof(LinePrefix); }
        public WordsMapEntry*   getWordsMapEntry(int i)
		{
			return (WordsMapEntry*)((Byte*) wordsMap_ + i* sizeof(WordsMapEntry));
        }
        Byte* getWordsMapEnd() { return (Byte*)getWordsMapEntry(wordsInLine_); }
        public Byte* getWord(int i) { return (Byte*)words_ + i * wordSize_; }
        public bool isValid() { return linePrefixPtr_->isValid == 1; }
        public void isValid(int value) { linePrefixPtr_->isValid = value; }
    }

    unsafe class Set
    {
        private readonly int kNumberOfLines_;
        private readonly int kWordsInLine_;
        private readonly int kBytesInLine;
        private const int    kBackTraceCoeff = 2;

        Byte*       buffer_; // owns memory used for set lines (payload)
        List<Line>  table_; // metainformation about set memory
        bool inRange(int low, int high, int x) { return (low <= x && x <= high); }

        public const int kTrue = 1;
        public const int kFalse = -1;
        public const int kNotFound = -1;
        public Set(int numberOfLines, int wordsInLine, int wordSize)
        {
            kNumberOfLines_ = numberOfLines;
            kWordsInLine_ = wordsInLine;
            kBytesInLine = sizeof(LinePrefix) + sizeof(WordsMapEntry) * wordsInLine + wordSize * wordsInLine;
            buffer_ = (Byte*)Marshal.AllocHGlobal(numberOfLines * kBytesInLine).ToPointer();
            table_ = new List<Line>(numberOfLines);
            for (int i = 0; i < numberOfLines; ++i)
            {
                Line line = new Line(wordsInLine, wordSize, buffer_ + i * kBytesInLine);
                table_.Add(line);
                table_[i].isValid(kFalse);
            }
        }
        public void PutWord(int firstTag, int currentTag, int index, void* data, int length, bool isFinal)
        {
            // check precondition
            Debug.Assert(length <= kBytesInLine);

            int targetLine = firstTag % kNumberOfLines_;
            table_[targetLine].getWordsMapEntry(index)->tag = currentTag;
            table_[targetLine].getWordsMapEntry(index)->length = length;

            Byte* sourceWordBegin   = (Byte*)data;
            Byte* sourceWordEnd     = (Byte*)data + length;
            Byte* destWordBegin     = table_[targetLine].getWord(index);

            while(sourceWordBegin != sourceWordEnd)
            {
                *destWordBegin++ = *sourceWordBegin++;
            }

            if (isFinal)
            {
                table_[targetLine].isValid(kTrue);
                table_[targetLine].getLinePrefixPtr()->firstTag = firstTag;
                table_[targetLine].getLinePrefixPtr()->lastTag = currentTag;
            }
        }
        public int FindLine(int tag, bool invalidate = false)
        {
            int backTraceDistance = kWordsInLine_ * kBackTraceCoeff;
            int firstTag = (tag - backTraceDistance > 0) ? tag - backTraceDistance : 0;
            for (int t = firstTag; t <= tag; ++t)
            {
                int candidateLine = t % kNumberOfLines_;

                if (table_[candidateLine].isValid())
                {
                    if (inRange(table_[candidateLine].getLinePrefixPtr()->firstTag
                        , table_[candidateLine].getLinePrefixPtr()->lastTag, tag))
                    {
                        if (invalidate)
                        {
                            table_[candidateLine].isValid(kFalse);
                            return kFalse;
                        }
                        return candidateLine;
                    }
                }
            }
            return kNotFound; // not found
        }
        public void* FindWord(int tag, int line, int* length)
        {
            for (int i = 0; i < kWordsInLine_; ++i)
            {
                if (tag == table_[line].getWordsMapEntry(i)->tag)
                {
                    *length = table_[line].getWordsMapEntry(i)->length;
                    return table_[line].getWord(i);
                }
            }
            return null;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    Marshal.FreeHGlobal(new IntPtr(buffer_));
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }
        }
    };


} // namespace Cache.Set.Cs

