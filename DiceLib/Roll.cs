namespace DiceLib
{
    /// <summary>
    ///     A class representing a roll (a raw and a modified value)
    /// </summary>
    /// <typeparam name="T">
    ///     T can be anything
    /// </typeparam>
    public class Roll<T>
    {
        /// <summary>
        ///     The natural, unmodified value
        /// </summary>
        public readonly T NaturalValue;

        /// <summary>
        ///     The modified value, after applying bonuses
        /// </summary>
        public readonly T Value;

        /// <summary>
        ///     Instantiate a new Roll from a <paramref name="naturalValue" /> and a <paramref name="value" />
        /// </summary>
        /// <param name="naturalValue">The natural, unmodified value</param>
        /// <param name="value">The modified value, after applying bonuses</param>
        public Roll(T naturalValue, T value)
        {
            Value = value;
            NaturalValue = naturalValue;
        }
    }
}