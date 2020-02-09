#ifndef SET_ALLOCACTOR_H
#define SET_ALLOCACTOR_H


#include <cstdint>
#include <iterator>
#include <vector>
#include <iostream>

template <typename T>
class SetAllocator
{
//private:
//public:
	T * memory_ptr;
	std::size_t memory_size;

public:
	typedef std::size_t     size_type;
	typedef T*              pointer;
	typedef T               value_type;

	SetAllocator(T* memory_ptr, std::size_t memory_size) : memory_ptr(memory_ptr), memory_size(memory_size) {}

	SetAllocator(const SetAllocator& other) throw() : memory_ptr(other.memory_ptr), memory_size(other.memory_size) {};

	template<typename U>
	SetAllocator(const SetAllocator<U>& other) throw() : memory_ptr(other.memory_ptr), memory_size(other.memory_size) {};

	template<typename U>
	SetAllocator& operator = (const SetAllocator<U>& other) { return *this; }
	SetAllocator<T>& operator = (const SetAllocator& other) { return *this; }
	~SetAllocator() {}


	pointer allocate(size_type n, const void* hint = 0) { return memory_ptr; }
	void deallocate(T* ptr, size_type n) {}

	size_type max_size() const { return memory_size; }
};


#endif // !SET_ALLOCACTOR_H