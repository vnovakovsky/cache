#include "stdafx.h"

#include <algorithm>

#include "Set.hpp"

namespace cache
{

Set::Set(int numberOfLines)
	: bufSize_(numberOfLines * sizeof(Line))
	, nLines_(numberOfLines)
	, table(numberOfLines)
{
	for (int i = 0; i < numberOfLines; ++i)
	{
		table[i].isValid = -1;
	}
}

void Set::PutWord(Tag firstTag, Tag currentTag, int index, void* data, int length, bool isFinal)
{
	int targetLine = firstTag % nLines_;
	table[targetLine].wordsMap[index].tag = currentTag;
	table[targetLine].wordsMap[index].length = length;
	
	Word* sourceWordBegin	= (Word*)data;
	Word* sourceWordEnd		= (Word*)((Byte*)data + length);
	Word* destWordBegin = &table[targetLine].words[index];
	
	std::copy(sourceWordBegin, sourceWordEnd, destWordBegin);
	
	if (isFinal)
	{
		table[targetLine].isValid = 1;
		table[targetLine].firstTag = firstTag;
		table[targetLine].lastTag = currentTag;
	}
}


} // namespace cache