using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework.Constraints;
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

        /// <summary>
        /// Removes and returns the first item in the Queue.
        /// </summary>
        /// <returns>The data item removed from the queue</returns>
        public T Dequeue() {
            if (Count == 0) {
                throw new InvalidOperationException("Empty Queue");
            }
            var ret = Peek();
            Buckets.Values.First(b => b != null).RemoveAt(0);
            --Count;
            return ret;
        }

        /// <summary>
        /// Enumerator for the structure
        /// </summary>
        /// <returns>An enumerator of the underlying object type</returns>
        public IEnumerator<T> GetEnumerator() {
            for (int i = 0; i < Buckets.Count; ++i) {
                if (Buckets.ContainsKey(i)) {
                    foreach (var itemInBucket in Buckets[i]) {
                        yield return itemInBucket;
                    }
                }
            }
        }

        /// <summary>
        /// Required for the IEnumerable interface
        /// </summary>
        /// <returns>A basic Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Removes a single item from the queue.
        /// </summary>
        /// <param name="toRemove"></param>
        /// <param name="key"></param>
        public void Remove(T toRemove, int key) {
            if (!Buckets.ContainsKey(key) || Buckets[key] == null) {
                throw new ArgumentException("key");
            }
            --Count;
            Buckets[key].Remove(toRemove);
        }

        /// <summary>
        /// Returns the first item in the queue
        /// </summary>
        /// <returns>The item at the front of the queue</returns>
        public T Peek() {
            if (Count == 0) {
                throw new InvalidOperationException("Empty Queue");}
            return Buckets.Values.First(b => b != null && b.Count != 0).First();
        }
    }
}
