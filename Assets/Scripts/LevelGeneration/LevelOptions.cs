using UnityEngine;
using System;

    [Serializable]
    public class LevelOptions : MonoBehaviour {
        /// <summary> Chunk size to generate </summary>
        public int ChunkSize;

        /// <summary> Collection of Doors to be used for generating the level </summary>
        public GameObject[] Doors;

        /// <summary> Collection of Enemies to be used for generating the level </summary>
        public GameObject[] Enemies;

        /// <summary> Collection of Floors to be used for generating the level </summary>
        public GameObject[] Floors;

        /// <summary> Collection of Loot to be used for generating the level </summary>
        public GameObject[] Loot;

        /// <summary> Maximum number of rooms to generate </summary>
        public int MaxChunks;

        /// <summary> Minimum number of rooms to generate </summary>
        public int MinChunks;

        /// <summary> Collection of Obstacles to be used for generating the level </summary>
        public GameObject[] Obstacles;

        /// <summary> Collection of Walls to be used for generating the level </summary>
        public GameObject[] Walls;

        /// <summary> sets up the level </summary>

    }
