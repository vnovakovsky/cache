#ifndef SET_H
#define SET_H

#include <vector>
#include "SetAllocator.hpp"

namespace cache
{
	typedef unsigned char	Byte;
	typedef int				Tag;

	const unsigned int WORD_SIZE = 8;
	const unsigned int WORDS_PER_LINE = 4;

#pragma pack(1)
	class WordsMapEntry
	{
	public:
		int		tag;
		int		length; // length of serialized data
	};

	class Word
	{
	public:
		typedef std::vector<Byte, PreAllocator<Byte> >		Buffer;
		Word(Byte* beg, std::size_t memory_size)
			:buffer_(0, PreAllocator<Byte>(beg, memory_size))
		{

		}
		Buffer::iterator begin() { return buffer_.begin(); }
		Buffer::iterator end() { return buffer_.end(); }
		//private:
		Buffer			buffer_; //[WORD_SIZE];
	};

#pragma pack(1)
	class Line
	{
	public:
		typedef struct
		{
			int					isValid;
			Tag					firstTag;
			Tag					lastTag;
		} LinePrefix, *LinePrefixPtr;

		typedef std::vector<WordsMapEntry, PreAllocator<WordsMapEntry> >	WordsMap;
		typedef std::vector<Word, PreAllocator<Word> >						Words;

		Line(int wordsInLine, int wordSize, Byte* lineBeg)
			: linePrefixPtr_((LinePrefixPtr)lineBeg)
			/*, wordsMap_(0, SetAllocator<WordsMap>((WordsMapEntry*)((Byte*)linePrefixPtr_ + sizeof(LinePrefix))
			, sizeof(WordsMapEntry) * wordsInLine))*/
			//, words_(0, SetAllocator<Byte>((Byte*)(&wordsMap_[0]) + sizeof(WordsMapEntry) * wordsInLine, wordsInLine * wordSize))
		{
			WordsMapEntry* wordsMapEntry = (WordsMapEntry*)((Byte*)linePrefixPtr_ + sizeof(LinePrefix));
			size_t size = sizeof(WordsMapEntry) * wordsInLine;
			wordsMap_(PreAllocator<WordsMapEntry>(wordsMapEntry, size));
		}
		LinePrefixPtr getLinePrefixPtr() { return linePrefixPtr_; }
		WordsMap& getWordsMap() { return wordsMap_; }
		Words& getWords() { return words_; }
		bool isValid() { linePrefixPtr_->isValid; }
		bool isValid(bool value) { linePrefixPtr_->isValid = value; }

		//private:		
		LinePrefixPtr		linePrefixPtr_;
		WordsMap			wordsMap_;	// -1 indicates empty (NULL) entry
		Words				words_;
	};

	class Set
	{
		//private:
	public:
		const int nLines_;
		std::vector<Line>	table; // metainformation about set memory
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
