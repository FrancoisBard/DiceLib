namespace DiceLib
{
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