namespace BloomFilter
{
    // The IHashProvider interface
    public interface IHashProvider<T>
    {
        // The GetHashpair method. Expected to return a pair of hashes for any input value.
        int[] GetHashPair(T item);
    }
}
