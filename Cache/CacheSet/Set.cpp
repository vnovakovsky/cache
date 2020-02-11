#include "stdafx.h"

#include <algorithm>
#include <cassert>

#include "Set.hpp"

namespace cache
{

	Set::Set(int numberOfLines, int wordsInLine, int wordSize)
		: kNumberOfLines_(numberOfLines)
		, kWordsInLine_(wordsInLine)
		, kBytesInLine(sizeof(LinePrefix) + sizeof(WordsMapEntry) * wordsInLine + wordSize * wordsInLine)
		, buffer_(new Byte[numberOfLines * kBytesInLine])
		, table_(numberOfLines)
	{
		for (int i = 0; i < numberOfLines; ++i)
		{
			Line line(wordsInLine, wordSize, buffer_.get() + i * kBytesInLine);
			table_[i] = line;
			table_[i].isValid(kFalse);
		}
	}

	void Set::PutWord(Tag firstTag, Tag currentTag, int index, void* data, int length, bool isFinal)
	{
		// check precondition
		assert(length <= kBytesInLine);
		int targetLine = firstTag % kNumberOfLines_;
		table_[targetLine].getWordsMapEntry(index)->tag = currentTag;
		table_[targetLine].getWordsMapEntry(index)->length = length;

		Byte* sourceWordBegin = (Byte*)data;
		Byte* sourceWordEnd = (Byte*)data + length;
		Byte* destWordBegin = (Byte*)table_[targetLine].getWord(index);

		std::copy(sourceWordBegin, sourceWordEnd, destWordBegin);

		if (isFinal)
		{
			table_[targetLine].isValid(kTrue);
			table_[targetLine].getLinePrefixPtr()->firstTag = firstTag;
			table_[targetLine].getLinePrefixPtr()->lastTag = currentTag;
		}
	}

	int Set::FindLine(Tag tag, bool invalidate)
	{
		int backTraceDistance = kWordsInLine_ * kBackTraceCoeff;
		Tag firstTag = (tag - backTraceDistance > 0) ? tag - backTraceDistance : 0;
		for (int t = firstTag; t <= tag; ++t)
		{
			int candidateLine = t % kNumberOfLines_;

			if (table_[candidateLine].isValid())
			{
				if (inRange(table_[candidateLine].getLinePrefixPtr()->firstTag
					, table_[candidateLine].getLinePrefixPtr()->lastTag, tag))
				{
					if (invalidate)
					{
						table_[candidateLine].isValid(kFalse);
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
		for (int i = 0; i < kWordsInLine_; ++i)
		{
			if (tag == table_[line].getWordsMapEntry(i)->tag)
			{
				*length = table_[line].getWordsMapEntry(i)->length;
				return table_[line].getWord(i);
			}
		}
		return 0;
	}


} // namespace cache