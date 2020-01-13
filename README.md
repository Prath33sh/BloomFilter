# BloomFilter
A C# implementation of BloomFilter.

To know more about BloomFilters read the Wikipedia article: https://en.wikipedia.org/wiki/Bloom_filter

# About this implementation

This is a proof of concept implementation of a BloomFilter in C#.
MumurHash is the primary hashing algorithm used in this project.
A naive hashing method is also provided just for demo purposes.

The dependencies are as below:
1. Murmur Hash NuGet by Darren Kopp (to be used as a standard hashing algorithm in addition to a naive non-standard algorithm that I came up with)
2. NUnit 3 Test Adapter - Visual studio extension (for unit tests)


