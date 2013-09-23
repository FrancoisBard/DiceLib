namespace DiceLib
{
    /// <summary>
    ///     An interface with some statistic features for numeric Dice
    /// </summary>
    /// <typeparam name="T">
    ///     T should be a numeric type such as int, long or decimal.
    ///     It should even be int or decimal.
    ///     Since there is no way to define such a generic constraint, i left T unconstrained.
    ///     Declared covariant because I could.
    /// </typeparam>
    public interface IRollableStatistics<out T>
    {
        /// <summary>
        ///     The smallest possible value
        /// </summary>
        T Min { get; }

        /// <summary>
        ///     The greatest possible value
        /// </summary>
        T Max { get; }

        /// <summary>
        ///     The average value
        /// </summary>
        /// <remarks>
        ///     It cannot return T or we would loose in precision if T was int.
        ///     There is still a loss of precision, and/or even of magnitude if T is double or BigInteger for instance
        /// </remarks>
        decimal Average { get; }
    }
}