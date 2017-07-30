/// <summary> The Utils File holds simple utility pices of Code </summary>

namespace Utility {
    /// <summary> The Range Object is a Utility to allow the modeling of a range of int values. </summary>
    public struct Range {
        /// <summary> The min value of the Range </summary>
        public int min;

        /// <summary> The max value of the Range </summary>
        public int max;

        /// <summary> min in an optional argument that will be set to 0 if not provided </summary>
        public Range(int max, int min = 0) {
            this.min = min;
            this.max = max;
        }
    }
}