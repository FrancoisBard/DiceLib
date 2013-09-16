namespace DiceLib
{
    /// <summary>
    ///     An interface for the simplest Die possible.
    /// </summary>
    /// <typeparam name="T">
    ///     T can be anything.
    ///     Declared covariant because I could.
    /// </typeparam>
    public interface IRollable<out T>
    {
        /// <summary>
        ///     Roll the die
        /// </summary>
        /// <returns>the resulting value of the roll</returns>
        T Roll();
    }

    /// <summary>
    ///     An interface for supporting roll modifiers.
    /// </summary>
    /// <typeparam name="T">
    ///     T can be anything
    /// </typeparam>
    public interface IRollableModified<T>
    {
        /// <summary>
        ///     Roll the die
        /// </summary>
        /// <returns>the roll information</returns>
        Roll<T> GetRoll();

        /// <summary>
        ///     return the value (after modification) from the <paramref name="naturalRoll" />
        /// </summary>
        /// <param name="naturalRoll">the natural roll, ie the raw value before any bonuses</param>
        /// <returns>the roll information, containing both he natural and modified roll</returns>
        T Roll(T naturalRoll);
    }

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

    /// <summary>
    ///     An interface to expose the static Parse and TryParse methods as instance methods.
    /// </summary>
    /// <remarks>Prefer the static Parse/TryParse when you know which class you need.</remarks>
    public interface IParseable
    {
        /// <summary>
        ///     The instance method equivalent to Parse.
        ///     Mutate the current instance on success, throw an exception on error
        /// </summary>
        /// <param name="s">the string representation of an instance of this class</param>
        /// <remarks>Prefer the static Parse method when you know which class you need.</remarks>
        void FromString(string s);

        /// <summary>
        ///     The instance method equivalent to TryParse.
        ///     Mutate the current instance on success, return false or throw an exception on error
        /// </summary>
        /// <param name="s">the string representation of an instance of this class</param>
        /// <returns>true on success</returns>
        /// <remarks>Prefer the static TryParse method when you know which class you need.</remarks>
        bool TryFromString(string s);
    }

    /// <summary>
    ///     an interface for a Die
    /// </summary>
    /// <typeparam name="T">
    ///     T can be anything but should probably be numeric <see cref="IRollableStatistics&lt;T&gt;" />"/>
    ///     Declared covariant because I could.
    /// </typeparam>
    public interface IDie<out T> : IRollable<T>, IRollableStatistics<T>
    {
    }

    /// <summary>
    ///     an interface for a Die with modifiers
    /// </summary>
    /// <typeparam name="T">
    ///     T can be anything but should probably be numeric <see cref="IRollableStatistics&lt;T&gt;" />
    /// </typeparam>
    public interface IDungeonDie<T> : IDie<T>, IRollableModified<T>
    {
    }
}