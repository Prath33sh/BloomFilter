// TODO: 
// 1. The BloomFilter implementation is for a regular single dimensional Bloom filter using 32-bit int hashes.
// 2. Various conversion related exceptions are not handled in this program.
// 3. String input currently is case sensitive.

namespace BloomFilter
{
    using System;
    using System.Collections;

    // BloomFilter implementation
    public class BloomFilter<T> : IBloomFilter<T>
    {
        // The actual Bloom filter bit structure
        private BitArray filter;
        // The size of the Bloom filter
        private int filterSize;
        // Number of hash functions
        private int hashCount;
        // The hash provider that will generate the hashes
        private IHashProvider<T> hashProvider;

        // Constructor
        public BloomFilter(IHashProvider<T> hashProvider)
        {
            this.hashProvider = hashProvider;
        }

        // Initializer. 
        // expectedItemCount: the expected number of elements that will be added to the filter
        // acceptableErrorRate: the allowed % of false positives
        public void Initialize(int expectedItemCount, double acceptableErrorRate)
        {
            this.filterSize = this.GetOptimumFilterSize(expectedItemCount, acceptableErrorRate);
            this.hashCount = this.GetOptimumHashes(this.filterSize, expectedItemCount);

            // Set the optimum filter size and initialize all bits to 0
            this.filter = new BitArray(this.filterSize, false);
        }

        // The Insert function
        // value: is the value of the type
        public void Insert(T value)
        {
            this.AddToFilter(this.GetHashes(value));
        }

        // The MightContain() function implementation
        // value: is the value of the type to be checked against the Bloom filter
        // Returns false if the element is definitely not present and true if it is probably present
        public bool MightContain(T value)
        {
            return this.CheckIfExists(this.GetHashes(value));
        }

        // Function returns the optimum filter size based on the input parameters
        // n : expected number of items in the filter
        // p : error/false positive rate
        // Returns the optimum size as an integer value
        public int GetOptimumFilterSize(int n, double p)
        {
            // Using the formula for size: m = ((n * log(p)) / (log(2)^2))
            return Math.Abs((int)(n * Math.Log(p) / Math.Pow(Math.Log(2), 2)));
        }

        // Function returns the optimum number of hash functions needed based on the input parameters
        // m : size of the filter
        // n : number of expected elements
        // Returns the optimum number of hash functions as an integer value
        public int GetOptimumHashes(int m, int n)
        {
            if (n == 0)
            {
                throw new ArgumentException();
            }
            // Using the formula for hash functions needed: k = ((m/n) * log(2))
            return Math.Abs((int)(m / n * Math.Log(2)));
        }

        // Function gets the required number of hashes
        // value: The input value tat needs to be added or inspected in the Bloom filter
        // Returns an array of int hashes
        private int[] GetHashes(T value)
        {
            // To minimize the need to call multiple hash functions the following formula is used
            // f(i,x) = h1(x) + i * h2(x) where i = number of hashes needed, h1 and h2 are 2 hash functions
            var hashes = new int[this.hashCount];
            var hashPair = this.hashProvider.GetHashPair(value);
            for (var i = 0; i < this.hashCount; i++)
            {
                hashes[i] = Math.Abs(hashPair[0] + (i * hashPair[1]));
            }

            return hashes;
        }

        // Private helper function adds the hashes in the Bloom filter
        // hashes: The array of hashes for the input value
        private void AddToFilter(int[] hashes)
        {
            foreach (int hash in hashes)
            {
                this.filter[hash % this.filterSize] = true;
            }
        }

        // Private helper function that checks for membership in the filter
        // hashes: The array of hashes for the input value
        // Returns a boolean with value set to True when the value may be present and False if not found.
        private bool CheckIfExists(int[] hashes)
        {
            var presence = true;
            foreach (int hash in hashes)
            {
                if (this.filter[hash % this.filterSize] == false)
                {
                    presence = false;
                    break;
                }
            }

            return presence;
        }
    }
}
