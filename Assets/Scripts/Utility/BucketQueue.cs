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
        /// <summary>The initial number of buckets. Backed by C# List implementation so expands as needed</summary>
        private int _numBuckets;
        /// <summary>The initial size of each of the buckets. Expands as needed.</summary>
        private int _bucketSize;
        /// <summary>The actual array of queues</summary>
        private readonly List<Queue<T>> Buckets;

        /// <summary>
        /// CTor for the BucketQueue. Initializes the data structure and sets default sizes.
        /// </summary>
        /// <param name="initialBuckets">The initial number of obuckets. Expands as needed. Default 25</param>
        /// <param name="initialBucketSize">The intial size of each of the buckets. Expands as needed. Default 10</param>
        public BucketQueue(int initialBuckets = 25,int initialBucketSize = 10) {
            _numBuckets = initialBuckets;
            _bucketSize = initialBucketSize;
            Buckets = new List<Queue<T>>(_numBuckets);
            
        }

        /// <summary>
        /// Enqueues an item into the queue sorted based on the key.
        /// </summary>
        /// <param name="toAdd">The data item to add</param>
        /// <param name="key">The key to sort the new data item on</param>
        public void Enqueue(T toAdd,int key) {
            if (Buckets[key] == null) {
                Buckets[key] = new Queue<T>(_bucketSize);
            }
            Buckets[key].Enqueue(toAdd);
        }

        /// <summary>
        /// Removes and returns the first item in the Queue.
        /// </summary>
        /// <returns>The data item removed from the queue</returns>
        public T Dequeue() {
            int i = 0;
            while (Buckets[i].Count == 0) {
                ++i;
            }
            return Buckets[i].Dequeue();
        }

        /// <summary>
        /// Enumerator for the structure
        /// </summary>
        /// <returns>An enumerator of the underlying object type</returns>
        public IEnumerator<T> GetEnumerator() {
            for (int i = 0; i < Buckets.Count; ++i) {
                foreach (var itemInBucket in Buckets[i]) {
                    yield return itemInBucket;
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
    }
}
