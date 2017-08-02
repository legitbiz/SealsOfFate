using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    /// <summary>
    /// A priority queue implemented as an array of queue 'buckets'. It is sorted based on integer
    /// data. As such, it works best for items that are sorted on a small set of quantized values.
    /// </summary>
    /// <typeparam name="T">The data type of the BucketQueue</typeparam>
    class BucketQueue<T> : IEnumerable<T> {
        public int Count { get; set; }
        /// <summary>The initial number of buckets. Backed by C# List implementation so expands as needed</summary>
        /// <summary>The initial size of each of the buckets. Expands as needed.</summary>
        private int _bucketSize;
        /// <summary>The actual array of queues</summary>
        private readonly SortedDictionary<int, List<T>> Buckets;

        /// <summary>
        /// CTor for the BucketQueue. Initializes the data structure and sets default sizes.
        /// </summary>
        /// <param name="initialBucketSize">The intial size of each of the buckets. Expands as needed. Default 10</param>
        public BucketQueue(int initialBucketSize = 10) {
            Count = 0;
            _bucketSize = initialBucketSize;
            Buckets = new SortedDictionary<int, List<T>>();
            
        }

        /// <summary>
        /// Enqueues an item into the queue sorted based on the key.
        /// </summary>
        /// <param name="toAdd">The data item to add</param>
        /// <param name="key">The key to sort the new data item on</param>
        public void Enqueue(T toAdd,int key) {
            Debug.Assert(key < 1000, "Key is not an insanely big value");
            if (!Buckets.ContainsKey(key)) {
                Buckets[key] = new List<T>(_bucketSize);
            }
            Buckets[key].Add(toAdd);
            ++Count;
        }

        public void LogContents() {
            Debug.Log("DUMP: Reported Bucket Queue Size: " + Count);
            Debug.Log("DUMP: Number of Keys/Buckets: " + Buckets.Keys.Count);
            Debug.Log("DUMP: Total number of items: " + Buckets.Values.Sum(q => q.Count));
            Debug.Log("DUMP: Total number of distinct items: " + Buckets.Values.SelectMany(q=>q).Count());
            Debug.Log("DUMP: Contents of Bucket Queue:");
            foreach (var item in this) {
                UnityEngine.Debug.Log("DUMP: Item in Bucket Queue: " + item);
            }
        }

        /// <summary>
        /// Removes and returns the first item in the Queue.
        /// </summary>
        /// <returns>The data item removed from the queue</returns>
        public T Dequeue() {
            if (Count == 0) {
                throw new InvalidOperationException("Empty Queue");
            }
            var ret = Peek();
            Buckets.Values.DefaultIfEmpty(null).First(b => b != null).RemoveAt(0);
            --Count;
            return ret;
        }

        /// <summary>
        /// Enumerator for the structure
        /// </summary>
        /// <returns>An enumerator of the underlying object type</returns>
        public IEnumerator<T> GetEnumerator() {
            return Buckets.Keys.SelectMany(key => Buckets[key]).GetEnumerator();
        }

        /// <summary>
        /// Required for the IEnumerable interface
        /// </summary>
        /// <returns>A basic Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Index operator for the Bucket Queue. Used to read the elements in a particular bucket.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>An IEnumerable of elements in the particular bucket. Returns an empty enumerable
        /// if the bucket is empty.</returns>
        public IEnumerable<T> this[int key] {
            get {
                return !Buckets.ContainsKey(key) ? Enumerable.Empty<T>() : Buckets[key];
            } 
        }

        /// <summary>
        /// Removes a single item from the queue with a better performance than the general remove.
        /// </summary>
        /// <param name="toRemove">The element to remove</param>
        /// <param name="key">The key that toRemove is associated with</param>
        /// <returns>False if unable to find the given item to remove in the given bucket. True otherwise.</returns>
        /// <throws>Throws an ArgumentException if the BucketQueue doesn't have a bucket at the given key</throws>
        /// <remarks>Only removes the first matching item encountered in the bucket.</remarks>
        public bool Remove(T toRemove, int key) {
            if (!Buckets.ContainsKey(key) || Buckets[key] == null) {
                throw new ArgumentException("Bucket specified by key does not exist");
            }
            --Count;
            return Buckets[key].Remove(toRemove);
        }

        /// <summary>
        /// Removes a single item from the queue without knowing the bucket it's in. Has O(n) worst case performance.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <returns>False if unable to find the given item. True otherwise.</returns>
        /// <remarks>Only removes the first matching item encountered.</remarks>
        public bool Remove(T toRemove) {
            if (Buckets.Values.Any(bucket => bucket != null && bucket.Remove(toRemove))) {
                --Count;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the first item in the queue
        /// </summary>
        /// <returns>The item at the front of the queue</returns>
        /// <throws>Throws an InvalidOperationException if the queue is empty</throws>
        public T Peek() {
            if (Count == 0) {
                throw new InvalidOperationException("Empty Queue");}
            return Buckets.Values.DefaultIfEmpty(null).First(b => b != null && b.Count != 0).First();
        }
    }
}
