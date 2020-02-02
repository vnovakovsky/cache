using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class CacheGeometry
    {
        readonly int numberOfWays_;
        /// <summary>
        ///   numberOflines_ = 2 ^ linesDegree.
        /// </summary>
        int numberOflines_;
        int wordsInLine_;
        /// <summary>
        ///   word size in bytes.
        /// </summary>
        int wordSize_;

        /// <summary>
        ///   NumberOflines = 2 ^ linesDegree; word size in bytes.
        /// </summary>
        public CacheGeometry(int numberOfWays, int linesDegree, int wordsInLine, int wordSize)
        {
            numberOfWays_ = numberOfWays;
            NumberOflines = (int)Math.Pow(2, linesDegree);
            WordsInLine = wordsInLine;
            WordSize = wordSize;
        }
        public int NumberOfWays => numberOfWays_;
        /// <summary>
        ///   NumberOflines = 2 ^ linesDegree.
        /// </summary>
        public int NumberOflines { get => numberOflines_; private set => numberOflines_ = value; }
        public int WordsInLine { get => wordsInLine_; private set => wordsInLine_ = value; }
        /// <summary>
        ///   word size in bytes.
        /// </summary>
        public int WordSize { get => wordSize_; private set => wordSize_ = value; }
    }
}
