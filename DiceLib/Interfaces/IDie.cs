namespace DiceLib
{
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
}