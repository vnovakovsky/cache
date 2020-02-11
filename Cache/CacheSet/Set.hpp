#ifndef SET_H
#define SET_H

#include <memory>
#include <vector>


namespace cache
{
	typedef unsigned char	Byte;
	typedef int				Tag;

	//const unsigned int WORD_SIZE = 8;
	//const unsigned int WORDS_PER_LINE = 4;

#pragma pack(1)
	typedef struct
	{
	public:
		int		tag;
		int		length; // length of serialized data
	} WordsMapEntry, *WordsMapEntryPtr;

	class Word
	{
	public:
		typedef Byte*		Buffer;
		Word()
			:buffer_(nullptr)
		{
		}
		private:
		Buffer			buffer_; //[WORD_SIZE];
	};

#pragma pack(1)
	typedef struct LinePrefixTag
	{
		LinePrefixTag()
			: isValid(-1)
			, firstTag(0)
			, lastTag(0)
		{
		}

		int					isValid;
		Tag					firstTag;
		Tag					lastTag;
	} LinePrefix, *LinePrefixPtr;

#pragma pack(1)
	class Line
	{
	public:
		
		typedef WordsMapEntry*		WordsMapPtr;
		typedef Byte*				WordsPtr;
		
		Line()
			: wordsInLine_(0)
			, wordSize_(0)
			, linePrefixPtr_(nullptr)
			, wordsMap_(nullptr)
			, words_(nullptr)
		{
		}

		Line(int wordsInLine, int wordSize, Byte* lineBeg)
			: wordsInLine_(wordsInLine)
			, wordSize_(wordSize)
			, linePrefixPtr_((LinePrefixPtr)lineBeg)
			, wordsMap_((WordsMapPtr)((Byte*)linePrefixPtr_ + sizeof(LinePrefix)))
			, words_((WordsPtr)((Byte*)(&wordsMap_[0]) + sizeof(WordsMapEntry) * wordsInLine))
		{
		}
		Line& operator=(Line& rhs)
		{
			if (this != &rhs)
			{
				wordsInLine_	= rhs.wordsInLine_;
				wordSize_		= rhs.wordSize_;
				linePrefixPtr_	= rhs.linePrefixPtr_;
				wordsMap_		= rhs.wordsMap_;
				words_			= rhs.words_;
			}
			return *this;
		}

		LinePrefixPtr getLinePrefixPtr() { return linePrefixPtr_; }
		WordsMapEntryPtr getWordsMapEntry(size_t i) { return WordsMapEntryPtr((Byte*)wordsMap_ + i * sizeof(WordsMapEntry)); }
		Byte* getWord(size_t i) { return (Byte*)words_ + i * wordSize_; }
		bool isValid() { return linePrefixPtr_->isValid == 1; }
		void isValid(int value) { linePrefixPtr_->isValid = value; }

		private:
			int wordsInLine_;
			int wordSize_;
			LinePrefixPtr		linePrefixPtr_;
			WordsMapPtr			wordsMap_;	// -1 indicates empty (NULL) entry
			WordsPtr			words_;
	};

	class Set
	{
	private:
		const int				nLines_;
		const int				wordsInLine_;
		const size_t			kBytesInLine;
		std::unique_ptr<Byte>	buffer_; // owns memory used for set lines (payload)
		std::vector<Line>		table; // metainformation about set memory
	public:
		const int kTrue = 1;
		const int kFalse = -1;
		const static int kNotFound = -1;
		Set(int numberOfLines, int wordsInLine, int wordSize);
		void PutWord(Tag firstTag, Tag currentTag, int index, void* data, int length, bool isFinal);
		int FindLine(Tag tag, bool invalidate = false);
		void* FindWord(Tag tag, int line, int* length);
		bool inRange(Tag low, Tag high, Tag x)
		{
			return (low <= x && x <= high);
		}
	};


} // namespace cache
#endif //SET_H
