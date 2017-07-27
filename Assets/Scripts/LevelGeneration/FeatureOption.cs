using Assets.Scripts.LevelGeneration;
using Utility;

/// <summary>
///     The "rule book" for level generation. This class serves as a reference
///     for the level; a range of chunks to generate (at least and at most),
///     as well as weights for how richly populated certain feature representations
///     are (walls, enemies, loot, etc.)
/// </summary>
public class FeatureOption {
    /// <summary>The default minimum number of chunks a level should enforcably generate.</summary>
    private const int MIN_CHUNKS = 30;

    /// <summary>The default maximum number of chunks a level should enforcably generate.</summary>
    private const int DEFAULT_MAX_CHUNKS = 50;

    /// <summary>Enforces a generateable level.</summary>
    /// <param name="chunksToMake">
    ///     Default argument parameter enforces a level with sane defaults, but a
    ///     value should be provided to this nonetheless.
    /// </param>
    public FeatureOption(Range chunksToMake) {
        FeatureWeights = new float[(int) LevelDecoration.TOTAL];
        Chunks = chunksToMake;
    }

    /// <summary>A value [0.0, 1.0] that weighs the calculation of how many of these decorations to place.</summary>
    /// <remarks>
    ///     For example, FeatureWeights[levelRepresentations.Loot] can be adjusted to
    ///     (in/de)crease loot for the whole level.
    /// </remarks>
    public float[] FeatureWeights { get; private set; }

    /// <summary>The minimum and maximum number of chunks to generate.</summary>
    public Range Chunks { get; private set; }

    /// <summary>Sets the specified weight to the given value.</summary>
    /// <param name="rep">The enum to target.</param>
    /// <param name="weight">A decimal value in the interval [0.0, 1.0] to set as a weight.</param>
    public FeatureOption SetWeight(LevelDecoration rep, float weight) {
        if (weight >= 0.0 && weight <= 1.0) {
            FeatureWeights[(int) rep] = weight;
        }

        return this;
    }
}