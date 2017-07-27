using Utility;

namespace Assets.Scripts.LevelGeneration {
    internal class LevelFeature : Feature {
        /// <summary> generates a feature and populates it with items in the validToPlace array </summary>
        public virtual void Generate(Range xRange, Range yRange, LevelDecoration[] validToPlace) { }
    }
}