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

int Set::FindLine(Tag tag)
{
	int outlookDistance = WORDS_PER_LINE * 2;
	Tag firstTag = (tag - outlookDistance > 0) ? tag - outlookDistance : 0;
	for (int t = firstTag; t <= tag; ++t)
	{
		int candidateLine = t % nLines_;

		if (table[candidateLine].isValid == 1)
		{
			if (inRange(table[candidateLine].firstTag, table[candidateLine].lastTag, tag))
			{
				return candidateLine;
			}
		}
	}
	return -1; // not found
}

void* Set::FindWord(Tag tag, int line, int* length)
{
	for (int i = 0; i < WORDS_PER_LINE; ++i)
	{
		if (tag == (table[line].wordsMap[i]).tag)
		{
			*length = (table[line].wordsMap[i]).length;
			return &(table[line].words[i]);
		}
	}
	return 0;
}


} // namespace cache