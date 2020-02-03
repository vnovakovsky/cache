using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class CacheGeometry
    {
        int numberOfWays_;
        /// <summary>
        ///   numberOflines_ = 2 ^ linesDegree.
        /// </summary>
        readonly int numberOflines_;
        readonly int linesPerSet_;
        readonly int wordsInLine_;
        /// <summary>
        ///   word size in bytes.
        /// </summary>
        readonly int wordSize_;

        /// <summary>
        ///   NumberOflines = 2 ^ linesDegree; word size in bytes.
        /// </summary>
        public CacheGeometry(int numberOfWays, int linesDegree, int wordsInLine, int wordSize)
        {
            NumberOfWays = numberOfWays;
            numberOflines_ = (int)Math.Pow(2, linesDegree);
            linesPerSet_ = numberOflines_ / numberOfWays_;
            wordsInLine_ = wordsInLine;
            wordSize_ = wordSize;
        }
        /// <summary>
        ///   NumberOflines = 2 ^ linesDegree.
        /// </summary>
        public int NumberOflines => numberOflines_;
        public int LinesPerSet => linesPerSet_;
        public int WordsInLine => wordsInLine_;
        /// <summary>
        ///   word size in bytes.
        /// </summary>
        public int WordSize => wordSize_;

        public int NumberOfWays
        {
            get => numberOfWays_; private set
            {
                if(value % 2 != 0)
                {
                    throw new ArgumentException("error: number of ways is not degree of 2");
                }
                numberOfWays_ = value;
            }
        }
    }
}
