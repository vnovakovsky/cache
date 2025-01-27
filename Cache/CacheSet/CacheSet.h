#ifndef CACHESET_H
#define CACHESET_H

#include "Set.hpp"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace Cache;

namespace CacheSet 
{

	using CacheSet::Set;

	public ref class SetProxy : public Cache::Set::ISet
	{
	private:
		Set*	set_;
	public:
		SetProxy(int numberOfLines, int wordsInLine, int wordSize)
		{
			set_ = new Set(numberOfLines, wordsInLine, wordSize);
		}

		virtual void PutWord(int tag, List<Cache::Word ^> ^data)
		{
			int i = 0;
			for each (Cache::Word^ word in data)
			{
				using System::Runtime::InteropServices::Marshal;
				IntPtr ptr = Marshal::AllocHGlobal(word->Buffer->Length);
				int bufLength = word->Buffer->Length;
				try
				{
					bool isFinal = data->Count - i == 1 ? true : false;
					Marshal::Copy(word->Buffer , 0, ptr, bufLength);
					set_->PutWord(tag, word->Tag, i, ptr.ToPointer(), bufLength, isFinal);
				}
				finally
				{
					Marshal::FreeHGlobal(ptr);
				}
				++i;
			}
		}

		virtual array<unsigned char, 1> ^ FindWord(int tag, bool invalidate)
		{
			int line = set_->FindLine(tag, invalidate);
			if (Set::kNotFound == line)
			{
				return nullptr;
			}
			int length;
			void* pWord = set_->FindWord(tag, line, &length);
			if (pWord != 0)
			{
				array<unsigned char>^ bytes = gcnew array<unsigned char>(length);
				Marshal::Copy(IntPtr(pWord),	// source
					bytes, 0, length);			// destination
				return bytes;
			}
			return nullptr;
		}

		!SetProxy()
		{
			delete set_;
		}
	};
}


#endif // !CACHESET_H