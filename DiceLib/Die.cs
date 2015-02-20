using System;
using System.Linq;

namespace DiceLib
{
    /// <summary>
    ///     Implements a classic die.
    /// </summary>
    public class Die : IDie<int>
    {
        #region members

        private static readonly Random RandomGenerator = new Random();

        private int _faces;
        private int _number;

        /// <summary>
        ///     The number of faces, or equivalently the highest face
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Faces must be strictly positive</exception>
        public int Faces
        {
            get { return _faces; }
            set
            {
                CheckStrictlyPositive(value, "Faces");
                _faces = value;
            }
        }

        /// <summary>
        ///     The number of dice
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Number must be strictly positive</exception>
        public int Number
        {
            get { return _number; }
            set
            {
                CheckStrictlyPositive(value, "Number");
                _number = value;
            }
        }

        private static void CheckStrictlyPositive(int value, string property)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(
                    property,
                    String.Format("{0} must be strictly positive, was {1}", property, value));
        }

        #endregion

        #region constructors

        /// <summary>
        ///     Create a die
        /// </summary>
        public Die()
            : this(1)
        {
        }

        /// <summary>
        ///     Create a die
        /// </summary>
        /// <param name="faces">The number of faces, or equivalently the highest face</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="faces" /> must be strictly positive
        /// </exception>
        public Die(int faces)
            : this(1, faces)
        {
        }

        /// <summary>
        ///     Create a die
        /// </summary>
        /// <param name="faces">The number of faces, or equivalently the highest face</param>
        /// <param name="number">The number of dice</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="number" /> must be strictly positive
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="faces" /> must be strictly positive
        /// </exception>
        public Die(int number, int faces)
        {
            Number = number;
            Faces = faces;
        }

        #endregion

        #region IRollable

        /// <summary>
        ///     Roll the die
        /// </summary>
        /// <returns>the result of the roll</returns>
        public int Roll()
        {
            return Enumerable.Repeat(Faces, Number).Sum(x => RandomGenerator.Next(1, x + 1));
        }

        #endregion

        #region IRollableStatistics

        /// <summary>
        ///     The smallest possible value of a <c>Roll</c>
        /// </summary>
        public int Min
        {
            get { return Number; }
        }

        /// <summary>
        ///     The biggest possible value of a <c>Roll</c>
        /// </summary>
        public int Max
        {
            get { return Number * Faces; }
        }

        /// <summary>
        ///     The average value of a <c>Roll</c>
        /// </summary>
        public decimal Average
        {
            get { return Number * (((decimal)(1 + Faces)) / 2); }
        }

        #endregion

        #region equality implementation

        /// <summary>
        ///     Two (not null) dice are equal if they have the exact same type (namely Dice, or a derived type), and the values Faces, Number and Modifier are equal.
        /// </summary>
        /// <returns>True if the dice are equal</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            var that = (Die)obj;
            return Faces == that.Faces && Number == that.Number;
        }

        /// <summary>
        ///     == operator override
        /// </summary>
        /// <param name="x">one Die</param>
        /// <param name="y">another Die</param>
        /// <returns>True if both Dies are equals or both are null</returns>
        public static bool operator ==(Die x, Die y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(null, x)) return false;
            return x.Equals(y);
        }

        /// <summary>
        ///     != operator override
        /// </summary>
        /// <param name="x">one Die</param>
        /// <param name="y">another Die</param>
        /// <returns>True if !(x == y)</returns>
        public static bool operator !=(Die x, Die y)
        {
            return !(x == y);
        }

        /// <summary>
        ///     calculate a hash based on the values of Faces, Number and Modifier
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Faces;
                hashCode = (hashCode * 397) ^ Number; //todo why 397 ?
                return hashCode;
            }
        }

        #endregion
    }
}