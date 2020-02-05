#ifndef SET_H
#define SET_H

#include <vector>

namespace cache
{
	typedef unsigned char	Byte;
	typedef int				Tag;

	const unsigned int WORD_SIZE		= 8;
	const unsigned int WORDS_PER_LINE	= 4;

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
		const int nLines_;
		std::vector<Line>	table;
	public:
		const int kTrue = 1;
		const int kFalse = -1;
		const static int kNotFound = -1;
		Set(int numberOfLines);
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
