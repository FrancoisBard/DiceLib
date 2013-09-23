namespace DiceLib
{
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
}