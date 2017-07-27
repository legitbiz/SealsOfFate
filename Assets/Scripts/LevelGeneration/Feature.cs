using System;
using UnityEngine;
using Utility;

namespace Assets.Scripts.LevelGeneration {
    /// <summary> The Feature Class is the base class for features on the map </summary>
    public class Feature {
        public int[,] FeatureMap;

        /// <summary> Represents the relative range of x values in the feature </summary>
        public Range XRange;

        /// <summary> Represents the relative range of y values in the feature </summary>
        public Range YRange;

        /// <summary> stiches a feature into this feature at the given starting point </summary>
        /// <param name="toStitch"> The feature being added to this feature </param>
        /// <param name="startPoint"> Feature will be filled in by being added up and right of the listed point </param>
        /// <returns> True of the stitching happened and False if there wasn't enough room</returns>
        public bool StitchFeature(Feature toStitch, Vector2 startPoint) {
            if (FeatureMap == null) {
                throw new Exception();
            }

            if (toStitch.XRange.max + (int) startPoint.x > XRange.max ||
                toStitch.YRange.max + (int) startPoint.y > YRange.max) {
                return false;
            }

            for (var x = 0; x < toStitch.XRange.max; ++x) {
                for (var y = 0; y < toStitch.YRange.max; ++y) {
                    FeatureMap[x + (int) startPoint.x, y + (int) startPoint.y] = toStitch.FeatureMap[x, y];
                }
            }
            return true;
        }

        /// <summary>
        ///     Counts the adjacent floor tiles of a given index in the feature map
        /// </summary>
        /// <param name="x"> X index to examine</param>
        /// <param name="y"> Y index to examine</param>
        /// <returns>Number of Adjacent floor tiles</returns>
        public int CheckAdjacent(int x, int y) {
            var ret = 0;
            if (x > 0 && FeatureMap[x - 1, y] == (int) LevelDecoration.Floor) {
                ++ret;
            }
            if (x < FeatureMap.GetLength(0) - 1 && FeatureMap[x + 1, y] == (int) LevelDecoration.Floor) {
                ++ret;
            }
            if (y > 0 && FeatureMap[x, y - 1] == (int) LevelDecoration.Floor) {
                ++ret;
            }
            if (y < FeatureMap.GetLength(1) - 1 && FeatureMap[x, y + 1] == (int) LevelDecoration.Floor) {
                ++ret;
            }
            return ret;
        }
    }
}