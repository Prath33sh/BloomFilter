namespace BloomFilter
{
    // The IBloomFilter interface
    public interface IBloomFilter <T>
    {
        // The initialize method.
        void Initialize(int expectedItemCount, double acceptableErrorRate);

        // The insert method. Adds the input item's presence in the filter.
        void Insert(T item);

        // The 'might contain' method. Checks for membership in the filter.
        bool MightContain(T item);
    }
}
