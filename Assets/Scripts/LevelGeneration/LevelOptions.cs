using System;
using UnityEngine;

/// <summary> enum abstraction of each tile that could be places on the board </summary>
public enum LevelDecoration {
    NOTHING,
    Floor,
    Wall,
    Enemy,
    Loot,
    Obstacles,
    Start,
    Exit,
    TOTAL
}

[Serializable]
public class LevelOptions : MonoBehaviour {
    /// <summary> Chunk size to generate </summary>
    public int ChunkSize;

    /// <summary> Collection of Doors to be used for generating the level </summary>
    public GameObject[] Doors;

    /// <summary> Collection of Enemies to be used for generating the level </summary>
    public GameObject[] Enemies;


    /// <summary> Weight for Enemies </summary>
    [Range(0f, 1f)] public float EnemyWeight;

    /// <summary> Collection of Floors to be used for generating the level </summary>
    public GameObject[] Floors;

    /// <summary> Collection of Loot to be used for generating the level </summary>
    public GameObject[] Loot;

    /// <summary> Weight for Loot </summary>
    [Range(0f, 1f)] public float LootWeight;

    /// <summary> Maximum number of rooms to generate </summary>
    public int MaxChunks;

    /// <summary> Minimum number of rooms to generate </summary>
    public int MinChunks;

    /// <summary> Collection of Obstacles to be used for generating the level </summary>
    public GameObject[] Obstacles;

    /// <summary> Weight for Obstacles </summary>
    [Range(0f, 1f)] public float ObstaclesWeight;

    /// <summary> Collection of Walls to be used for generating the level </summary>
    public GameObject[] Walls;
}