#pragma once

#include "Set.hpp"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace Cache;

namespace CacheSet {

	using cache::Set;

	public ref class SetProxy
	{
	private:
		Set*	set_;
	public:
		SetProxy(int numberOfLines, int wordsInLine, int wordSize)
		{
			set_ = new Set(numberOfLines);
		}

		void PutWord(int tag, List<Word^>^ data)
		{
			int i = 0;
			for each (Word^ word in data)
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

		//bool FindWord(int tag, array<unsigned char>^ bytes)
		array<unsigned char>^ FindWord(int tag)
		{
			int line = set_->FindLine(tag);
			if (-1 == line)
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
