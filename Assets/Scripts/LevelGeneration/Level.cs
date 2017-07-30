using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration {
    public class Level : Feature {
        private const int CULL_RATIO = 2;
        private const int CULL_THRESHOLD = 6;
        private int chunkSize;
        public Feature MiniMap = new Feature();

        /// <summary>
        ///     This Function generates a level and populates it,
        /// </summary>
        /// <param name="minChunks">The minimum number of chunks to try and create</param>
        /// <param name="maxChunks">The maximum number of chunks to try and create</param>
        /// <param name="chunkDimension">A scalar used for expanding the level, Should be odd and >2 </param>
        /// <param name="enemyWeight">The maximum ratio of enemies for the level</param>
        /// <param name="lootWeight">The maximum ratio of loot for the level</param>
        /// <param name="obstaclesWeight">The maximum ratio of obstacles for the level</param>
        public void Generate(int minChunks,
                             int maxChunks,
                             int chunkDimension,
                             float enemyWeight,
                             float lootWeight,
                             float obstaclesWeight) {
            // setup the graph
            chunkSize = chunkDimension;
            var numChunks = Random.Range(minChunks, maxChunks);
            // size the map to allow for each room take up the maximum and leave space for tunnels
            XRange.max = YRange.max = chunkDimension * numChunks + 2;
            XRange.min = YRange.min = 0;
            FeatureMap = new int[XRange.max, YRange.max];
            // generate the map after setting it up.
            BuildMiniMap(numChunks);
            BuildChunks(chunkDimension);
            FillInWalls();
            Decorate(numChunks, enemyWeight, lootWeight, obstaclesWeight);
        }

        /// <summary>
        ///     Builds out a random miniMap of the level with the given number of chunks.
        /// </summary>
        /// <param name="numChunks">The number of chunks to place during build</param>
        private void BuildMiniMap(int numChunks) {
            // Potential spots to place new chunks at.
            var potentialSlots = new List<Vector2>();
            // make the miniMap large enough
            MiniMap.FeatureMap = new int[numChunks, numChunks];
            // make the centerpoint the first chunk to be placed
            potentialSlots.Add(new Vector2(numChunks / 2, numChunks / 2));
            // place chunks until there are none left to place
            for (var i = 0; i < numChunks; ++i) {
                // grab a random chunk from the list of potential slots if there are any.
                if (potentialSlots.Count > 0) {
                    var toPlace = potentialSlots[Random.Range(0, potentialSlots.Count)];
                    // remove the chosen chunk from the future options
                    potentialSlots.Remove(toPlace);
                    // if the number of available spots is large cull some to produce more interesting options
                    // Both the number that count is compared to and the ratio to cull shoould be played with as meta parameters
                    if (potentialSlots.Count > CULL_THRESHOLD) {
                        var numToRemove = potentialSlots.Count / CULL_RATIO;
                        for (var j = 0; j < numToRemove - 1; ++j) {
                            var toRemove = potentialSlots[Random.Range(0, potentialSlots.Count)];
                            potentialSlots.Remove(toRemove);
                        }
                    }
                    // Mark the chosen spot 
                    MiniMap.FeatureMap[(int) toPlace.x, (int) toPlace.y] = 1;
                    // the following blocks check the cardinal directions and if they are in bounds and have not already on the list they are added as available spots.
                    if (toPlace.x < MiniMap.FeatureMap.GetLength(0) - 1 &&
                        MiniMap.FeatureMap[(int) toPlace.x + 1, (int) toPlace.y] != 1 &&
                        potentialSlots.Contains(new Vector2(toPlace.x + 1, toPlace.y)) != true) {
                        potentialSlots.Add(new Vector2(toPlace.x + 1, toPlace.y));
                    }
                    if (toPlace.x > 0 && MiniMap.FeatureMap[(int) toPlace.x - 1, (int) toPlace.y] != 1 &&
                        potentialSlots.Contains(new Vector2(toPlace.x - 1, toPlace.y)) != true) {
                        potentialSlots.Add(new Vector2(toPlace.x - 1, toPlace.y));
                    }
                    if (toPlace.y < MiniMap.FeatureMap.GetLength(0) - 1 &&
                        MiniMap.FeatureMap[(int) toPlace.x, (int) toPlace.y + 1] != 1 &&
                        potentialSlots.Contains(new Vector2(toPlace.x, toPlace.y + 1)) != true) {
                        potentialSlots.Add(new Vector2(toPlace.x, toPlace.y + 1));
                    }
                    if (toPlace.y > 0 && MiniMap.FeatureMap[(int) toPlace.x, (int) toPlace.y - 1] != 1 &&
                        potentialSlots.Contains(new Vector2(toPlace.x, toPlace.y - 1)) != true) {
                        potentialSlots.Add(new Vector2(toPlace.x, toPlace.y - 1));
                    }
                }
            }
        }

        /// <summary>
        ///     This function translates the MiniMap into a full map by expanding the chunks into undecorated floors.
        /// </summary>
        /// <param name="chunkDimension">The Scalar that will be used for expansion</param>
        private void BuildChunks(int chunkDimension) {
            for (var x = 1; x < MiniMap.FeatureMap.GetLength(0) - 1; ++x) {
                for (var y = 1; y < MiniMap.FeatureMap.GetLength(1) - 1; ++y) {
                    if (MiniMap.FeatureMap[x, y] == 1) {
                        for (var j = 0; j < chunkDimension; ++j) {
                            for (var k = 0; k < chunkDimension; ++k) {
                                FeatureMap[x * chunkDimension + j, y * chunkDimension + k] =
                                    (int) LevelDecoration.Floor;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     This function goes through the map and addds walls to empty spaces adjacent to floors.
        /// </summary>
        private void FillInWalls() {
            for (var x = 0; x < FeatureMap.GetLength(0); x++) {
                for (var y = 0; y < FeatureMap.GetLength(1); y++) {
                    if (FeatureMap[x, y] == 0 && CheckAdjacent(x, y) > 0) {
                        FeatureMap[x, y] = (int) LevelDecoration.Wall;
                    }
                }
            }
        }

        /// <summary>
        ///     Decorates a level with the maximimum ratios that are given
        /// </summary>
        /// <param name="enemyWeight">The maximum ratio of enemies for the level</param>
        /// <param name="lootWeight">The maximum ratio of loot for the level</param>
        /// <param name="obstaclesWeight">The maximum ratio of obstacles for the level</param>
        private void Decorate(int numChunks, float enemyWeight, float lootWeight, float obstaclesWeight) {
            var toDecorate = new List<Vector2>();
            for (var x = 0; x < MiniMap.FeatureMap.GetLength(0); ++x) {
                for (var y = 0; y < MiniMap.FeatureMap.GetLength(1); ++y) {
                    if (MiniMap.FeatureMap[x, y] == 1) {
                        toDecorate.Add(new Vector2(x, y));
                    }
                }
            }

            var numEnemies = enemyWeight * numChunks;
            var numLoot = lootWeight * numChunks;
            var numObstacles = obstaclesWeight * numChunks;

            while (toDecorate.Count > 0) {
                var selectedChunk = toDecorate[Random.Range(0, toDecorate.Count)];
                toDecorate.Remove(selectedChunk);

                for (var x = 0; x < chunkSize; ++x) {
                    for (var y = 0; y < chunkSize; ++y) {
                        var roll = Random.Range(0f, 1f);
                        if (roll >= .75 && numEnemies > 0) {
                            FeatureMap[(int) selectedChunk.x * chunkSize + x, (int) selectedChunk.y * chunkSize + y] =
                                (int) LevelDecoration.Enemy;
                            --numEnemies;
                        } else if (roll >= .50 && numObstacles > 0) {
                            FeatureMap[(int) selectedChunk.x * chunkSize + x, (int) selectedChunk.y * chunkSize + y] =
                                (int) LevelDecoration.Wall;
                            --numObstacles;
                        } else if (roll >= .25 && numLoot > 0) {
                            FeatureMap[(int) selectedChunk.x * chunkSize + x, (int) selectedChunk.y * chunkSize + y] =
                                (int) LevelDecoration.Loot;
                            --numLoot;
                        }
                    }
                }
            }
        }
    }
}