#ifndef SET_H
#define SET_H

#include <memory>
#include <vector>


namespace CacheSet
{
	typedef unsigned char	Byte;
	typedef int				Tag;

#pragma pack(1)
	typedef struct
	{
		int		tag;
		int		length; // length of serialized data
	} WordsMapEntry, *WordsMapEntryPtr;

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

		// Line: {LinePrefix, WordsMap[wordsInLine_] {{tag1, length1}, ...{tagWL, lengthWL}}, ...Words[wordsInLine_]{w1,..wWL}}

		Line(int wordsInLine, int wordSize, Byte* lineBeg)
			: wordsInLine_(wordsInLine)
			, wordSize_(wordSize)
			, linePrefixPtr_((LinePrefixPtr)lineBeg)
			, wordsMap_((WordsMapPtr)(getLinePrefixEnd()))
			, words_((WordsPtr)getWordsMapEnd())
		{
		}

		Line(const Line& rhs)
			: wordsInLine_(rhs.wordsInLine_)
			, wordSize_(rhs.wordSize_)
			, linePrefixPtr_(rhs.linePrefixPtr_)
			, wordsMap_(rhs.wordsMap_)
			, words_(rhs.words_)
		{
		}

		Line& operator=(Line& rhs)
		{
			if (this != &rhs)
			{
				wordsInLine_ = rhs.wordsInLine_;
				wordSize_ = rhs.wordSize_;
				linePrefixPtr_ = rhs.linePrefixPtr_;
				wordsMap_ = rhs.wordsMap_;
				words_ = rhs.words_;
			}
			return *this;
		}

		inline LinePrefixPtr	getLinePrefixPtr() { return linePrefixPtr_; }
		inline Byte*			getLinePrefixEnd() { return (Byte*)linePrefixPtr_ + sizeof(LinePrefix); }
		inline WordsMapEntryPtr getWordsMapEntry(const size_t i)
		{
			return (WordsMapEntryPtr)((Byte*)wordsMap_ + i * sizeof(WordsMapEntry));
		}
		inline Byte*			getWordsMapEnd() { return (Byte*)getWordsMapEntry(wordsInLine_); }
		inline Byte*			getWord(size_t i) { return (Byte*)words_ + i * wordSize_; }
		inline bool isValid() { return linePrefixPtr_->isValid == 1; }
		inline void isValid(int value) { linePrefixPtr_->isValid = value; }

	private:
		// the order of data members below is important
		int					wordsInLine_;
		int					wordSize_;
		LinePrefixPtr		linePrefixPtr_;
		WordsMapPtr			wordsMap_;	// -1 indicates empty (NULL) entry
		WordsPtr			words_;
	};

	class Set
	{
	private:
		const int				kNumberOfLines_;
		const int				kWordsInLine_;
		const size_t			kBytesInLine;
		static const size_t		kBackTraceCoeff = 2;
		std::unique_ptr<Byte>	buffer_; // owns memory used for set lines (payload)
		std::vector<Line>		table_; // metainformation about set memory
		inline bool	inRange(Tag low, Tag high, Tag x) { return (low <= x && x <= high); }
	public:
		const int kTrue = 1;
		const int kFalse = -1;
		const static int kNotFound = -1;
		Set(int numberOfLines, int wordsInLine, int wordSize);
		void PutWord(Tag firstTag, Tag currentTag, int index, void* data, int length, bool isFinal);
		int FindLine(Tag tag, bool invalidate = false);
		void* FindWord(Tag tag, int line, int* length);
	};


} // namespace CacheSet


#endif //SET_H
