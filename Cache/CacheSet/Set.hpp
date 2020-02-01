#ifndef SET_H
#define SET_H

#include <vector>

namespace cache
{
	typedef unsigned char	Byte;
	typedef int				Tag;

	const unsigned int WORD_SIZE		= 16;
	const unsigned int WORDS_PER_LINE	= 2;

#pragma pack(1)
	struct WordsMapEntry
	{
		int		tag;
		int		length; // length of serialized data
	};

	struct Word
	{
		Byte data[WORD_SIZE];
	};

#pragma pack(1)
	struct Line
	{
		int					isValid;
		Tag					firstTag;
		Tag					lastTag;
		WordsMapEntry		wordsMap[WORDS_PER_LINE];	// -1 indicates empty (NULL) entry
		Word				words[WORDS_PER_LINE];
	};

	class Set
	{
	private:
		const size_t bufSize_;
		const int nLines_;
		std::vector<Line>	table;
	public:
		Set(int numberOfLines);
		void PutWord(Tag firstTag, Tag currentTag, int index, void* data, int length, bool isFinal);
	};


} // namespace cache
#endif //SET_H
