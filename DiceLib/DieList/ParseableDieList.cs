using System;

namespace DiceLib
{
    /// <summary>
    ///     A parseable list of dice
    /// </summary>
    /// <typeparam name="T">A "Die" that implements IDie&lt;int&gt;, IParseable and has a parameterless constructor</typeparam>
    /// <remarks>
    ///     To compare two DieList&gt;T&lt;, StructuralComparisons.StructuralEqualityComparer.Equals()
    ///     To compare two DieList&gt;T&lt; ELEMENT BY ELEMENT, use Linq's SequenceEqual()
    /// </remarks>
    public class ParseableDieList<T> : DieList<T> where T : IDie<int>, IParseable, new()
    {
        /// <summary>
        ///     Parse a string representing a DieList&lt;T&gt;
        /// </summary>
        /// <param name="s">A string representation of the DieList&lt;T&gt;</param>
        /// <returns>The parsed DieList&lt;T&gt;</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="s" /> is null
        /// </exception>
        /// <exception cref="FormatException">
        ///     <paramref name="s" /> is not a valid DieList&lt;T&gt; representation.
        /// </exception>
        public static ParseableDieList<T> Parse(string s)
        {
            if (s == null) throw new ArgumentNullException("s");

            var result = new ParseableDieList<T>();

            foreach (string dieString in s.Split(";".ToCharArray()))
            {
                var iDie = new T();
                iDie.FromString(dieString);
                result.Add(iDie);
            }

            return result;
        }

        /// <summary>
        ///     Try to parse a string representing a DieList&lt;T&gt;
        /// </summary>
        /// <param name="s">A string representation of the DieList&lt;T&gt;</param>
        /// <param name="result">The parsed DieList&lt;T&gt; on success, null on failure</param>
        /// <returns>
        ///     true if <paramref name="s" /> was a valid representation of a DieList&lt;T&gt;
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="s" /> is null
        /// </exception>
        public static bool TryParse(string s, out ParseableDieList<T> result)
        {
            result = null;

            if (s == null) throw new ArgumentNullException("s");

            try
            {
                result = Parse(s);
                return true;
            }
            catch (Exception)   //catch everything since we don't know what Parseable.FromString might throw
            {
                    return false;
            }
        }
    }
}