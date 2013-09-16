using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceLib
{
    /// <summary>
    ///     A list of dice
    /// </summary>
    /// <typeparam name="T">A "Die" that implements IDie&lt;int&gt;, IParseable and as a parameterless constructor</typeparam>
    /// <remarks>
    ///     To compare two DieList&gt;T&lt;, StructuralComparisons.StructuralEqualityComparer.Equals()
    ///     To compare two DieList&gt;T&lt; ELEMENY BY ELEMENT, use Linq's SequenceEqual()
    /// </remarks>
    /// //todo stop inheriting from list and use composition instead
    public class DieList<T> : List<T>, IDie<int> where T : IDie<int>, IParseable, new()
    {
        #region IDie<int>

        /// <summary>
        ///     The smallest possible value
        /// </summary>
        public int Min
        {
            get { return this.Sum(die => die.Min); }
        }

        /// <summary>
        ///     The greatest possible value
        /// </summary>
        public int Max
        {
            get { return this.Sum(die => die.Max); }
        }

        /// <summary>
        ///     The average value
        /// </summary>
        public decimal Average
        {
            get { return this.Sum(die => die.Average); }
        }

        /// <summary>
        ///     Roll the die
        /// </summary>
        /// <returns>the resulting value of the roll</returns>
        public int Roll()
        {
            return this.Sum(die => die.Roll());
        }

        #endregion

        #region parse / ToString

        /// <summary>
        ///     Return a string that represents the current DieList&lt;T&gt;
        /// </summary>
        /// <returns>the canonical string representation of the DieList&lt;T&gt;</returns>
        public override string ToString()
        {
            return String.Join(" ; ", this.Select(die => die.ToString()).ToArray());
        }

        /// <summary>
        ///     Parse a string representing a DieList&lt;T&gt;
        /// </summary>
        /// <param name="stringRepresentation">A string representation of the DieList&lt;T&gt;</param>
        /// <returns>The parsed DieList&lt;T&gt;</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="stringRepresentation" /> is null
        /// </exception>
        /// <exception cref="FormatException">
        ///     <paramref name="stringRepresentation" /> is not a valid DieList&lt;T&gt; representation
        /// </exception>
        public static DieList<T> Parse(string stringRepresentation)
        {
            DieList<T> result;
            if (!TryParse(stringRepresentation, out result))
            {
                throw new FormatException();
            }
            return result;
        }

        /// <summary>
        ///     Try to parse a string representing a DieList&lt;T&gt;
        /// </summary>
        /// <param name="stringRepresentation">A string representation of the DieList&lt;T&gt;</param>
        /// <param name="result">The parsed DieList&lt;T&gt; on success, null on failure</param>
        /// <returns>
        ///     true if <paramref name="stringRepresentation" /> was a valid representation of a DieList&lt;T&gt;
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="stringRepresentation" /> is null
        /// </exception>
        public static bool TryParse(string stringRepresentation, out DieList<T> result)
        {
            if (stringRepresentation == null)
            {
                throw new ArgumentNullException();
            }

            result = new DieList<T>();
            string[] array = stringRepresentation.Split(";".ToCharArray());

            foreach (string s in array)
            {
                var iDie = new T();
                if (!iDie.TryFromString(s))
                {
                    result = null;
                    return false;
                }
                result.Add(iDie);
            }

            return true;
        }

        #endregion

        #region equality implementation

        /// <summary>
        ///     override equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if the DieList&lt;T&gt; is equal to the current object</returns>
        /// <remarks>be careful when extending DieList&lt;T&gt; to provide a coherent equality implementation</remarks>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            var other = (DieList<T>) obj;

            if (Count != other.Count)
                return false;

            var hash = new Dictionary<T, int>();

            foreach (T iDie in this)
            {
                if (hash.ContainsKey(iDie))
                {
                    hash[iDie]++;
                }
                else
                {
                    hash.Add(iDie, 1);
                }
            }

            foreach (T iDie in other)
            {
                if (!hash.ContainsKey(iDie) || hash[iDie] == 0)
                {
                    return false;
                }
                hash[iDie]--;
            }

            return true;
        }

        /// <summary>
        ///     override GetHashCode
        /// </summary>
        /// <returns>A hash code</returns>
        public override int GetHashCode()
        {
            return this.Aggregate(0, (i, die) => i ^ die.GetHashCode());
        }

        #endregion
    }
}