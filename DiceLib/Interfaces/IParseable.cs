using System;

namespace DiceLib
{
    /// <summary>
    ///     An interface to expose the static Parse and TryParse methods as instance methods.
    /// </summary>
    /// <remarks>Prefer the static Parse/TryParse when you know which class you need.</remarks>
    public interface IParseable
    {
        /// <summary>
        ///     The instance method equivalent to Parse.
        ///     Mutate the current instance on success
        /// </summary>
        /// <param name="s">the string representation of an instance of this class</param>
        /// <returns>true on success</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="s" /> is null
        /// </exception>
        /// <exception cref="FormatException">
        ///     <paramref name="s" /> is not a valid representation of a Die.
        /// </exception>
        /// <remarks>Prefer the static Parse or TryParse methods when you know which class you need.</remarks>
        void FromString(string s);
    }
}