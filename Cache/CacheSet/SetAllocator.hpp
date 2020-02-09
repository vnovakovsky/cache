#pragma once

template<typename T>
class PreAllocator
{
private:
	T * memory_ptr;
	std::size_t memory_size;

public:
	typedef std::size_t size_type;
	typedef ptrdiff_t difference_type;
	typedef T* pointer;
	typedef const T* const_pointer;
	typedef T& reference;
	typedef const T& const_reference;
	typedef T value_type;


	PreAllocator(T* memory_ptr, std::size_t memory_size) throw() : memory_ptr(memory_ptr), memory_size(memory_size) {};
	PreAllocator(const PreAllocator& other) throw() : memory_ptr(other.memory_ptr), memory_size(other.memory_size) {};

	template<typename U>
	PreAllocator(const PreAllocator<U>& other) throw() : memory_ptr(other.memory_ptr), memory_size(other.memory_size) {};

	template<typename U>
	PreAllocator& operator = (const PreAllocator<U>& other) { return *this; }
	PreAllocator<T>& operator = (const PreAllocator& other) { return *this; }
	~PreAllocator() {}

	pointer address(reference value) const { return &value; }
	const_pointer address(const_reference value) const { return &value; }

	pointer allocate(size_type n, const void* hint = 0) { return memory_ptr; }
	void deallocate(T* ptr, size_type n) {}

	void construct(pointer ptr, const T& val) { new (ptr) T(val); }

	template<typename U>
	void destroy(U* ptr) { ptr->~U(); }
	void destroy(pointer ptr) { ptr->~T(); }

	size_type max_size() const { return memory_size; }

	template<typename U>
	struct rebind
	{
		typedef PreAllocator<U> other;
	};
};

