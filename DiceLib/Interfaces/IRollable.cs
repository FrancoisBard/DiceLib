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
}