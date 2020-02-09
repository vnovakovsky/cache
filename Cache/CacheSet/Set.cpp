#include "stdafx.h"

#include <algorithm>

#include "Set.hpp"

namespace cache
{

	Set::Set(int numberOfLines, int wordsInLine, int wordSize)
		: nLines_(numberOfLines)
		, wordsInLine_(wordsInLine)
		, kBytesInLine(sizeof(LinePrefix) + sizeof(WordsMapEntry) * wordsInLine + wordSize * wordsInLine)
		, buffer_(new Byte[numberOfLines * kBytesInLine])
		, table(numberOfLines)
	{
		for (int i = 0; i < numberOfLines; ++i)
		{
			Line line(wordsInLine, wordSize, buffer_ + i * kBytesInLine);
			table[i] = line;
			table[i].isValid(kFalse);
		}
	}

	void Set::PutWord(Tag firstTag, Tag currentTag, int index, void* data, int length, bool isFinal)
	{
		int targetLine = firstTag % nLines_;
		table[targetLine].getWordsMapEntry(index)->tag = currentTag;
		table[targetLine].getWordsMapEntry(index)->length = length;

		Byte* sourceWordBegin = (Byte*)data;
		Byte* sourceWordEnd = (Byte*)data + length;
		Byte* destWordBegin = (Byte*)table[targetLine].getWord(index);

		std::copy(sourceWordBegin, sourceWordEnd, destWordBegin);

		if (isFinal)
		{
			table[targetLine].isValid(kTrue);
			table[targetLine].getLinePrefixPtr()->firstTag = firstTag;
			table[targetLine].getLinePrefixPtr()->lastTag = currentTag;
		}
	}

	int Set::FindLine(Tag tag, bool invalidate)
	{
		int backTraceDistance = wordsInLine_ * 2;
		Tag firstTag = (tag - backTraceDistance > 0) ? tag - backTraceDistance : 0;
		for (int t = firstTag; t <= tag; ++t)
		{
			int candidateLine = t % nLines_;

			if (table[candidateLine].isValid())
			{
				if (inRange(table[candidateLine].getLinePrefixPtr()->firstTag
					, table[candidateLine].getLinePrefixPtr()->lastTag, tag))
				{
					if (invalidate)
					{
						table[candidateLine].isValid(kFalse);
						return kFalse;
					}
					return candidateLine;
				}
			}
		}
		return kNotFound; // not found
	}

	void* Set::FindWord(Tag tag, int line, int* length)
	{
		for (int i = 0; i < wordsInLine_; ++i)
		{
			if (tag == table[line].getWordsMapEntry(i)->tag)
			{
				*length = table[line].getWordsMapEntry(i)->length;
				return table[line].getWord(i);
			}
		}
		return 0;
	}


} // namespace cache