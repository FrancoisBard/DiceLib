using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiceLib
{
    /// <summary>
    ///     Implements a classic die.
    ///     A classic die is a n-faced die, with faces ranging from 1 to n.
    ///     Each face has the same probability.
    ///     A classic die is written 'd{n}', where {n} is the number of faces.
    ///     For instance, a 20-faced die is written 'd20'.
    ///     A die can have modifiers.
    ///     A modifier is a number that you add or subtract to the result of a roll.
    ///     In Dungeons &amp; Dragons 3, the result of a roll after applying all modifiers cannot be inferior to 1
    ///     For instance, if you roll 2 on a d3 - 2, you obtain 1 (not 0).
    /// </summary>
    public class Die : IDungeonDie<int>, IParseable
    {
        #region members

        private static readonly Random RandomGenerator = new Random();
        private static readonly Regex DieRepresentationRegex;

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
                CheckFaces(value);
                _faces = value;
            }
        }

        /// <summary>
        ///     The final modifier applied on the result of the die roll
        /// </summary>
        public int Modifier { get; set; }

        /// <summary>
        ///     The number of dice
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Number must be strictly positive</exception>
        public int Number
        {
            get { return _number; }
            set
            {
                CheckNumber(value);
                _number = value;
            }
        }

        private static bool CheckNumber(int number, bool throwException = true)
        {
            return CheckStrictlyPositive(number, "Number", throwException);
        }

        private static bool CheckFaces(int faces, bool throwException = true)
        {
            return CheckStrictlyPositive(faces, "Faces", throwException);
        }

        private static bool CheckStrictlyPositive(int value, string property, bool throwException)
        {
            if (value < 1)
            {
                if (throwException)
                {
                    throw new ArgumentOutOfRangeException(
                        property,
                        String.Format("{0} must be strictly positive, was {1}", property, value));
                }
                return false;
            }
            return true;
        }

        #endregion

        #region constructors

        /// <summary>
        ///     static constructor to initialize the RegEx
        /// </summary>
        static Die()
        {
            const string regexPatternNumber = @"(\d*)";
            const string regexPatternFaces = @"(\d+)";
            const string regexPatternModifier = @"( ([+-]) \s* (\d+) )";

            string regexPattern = String.Format(@"^\s* {0} \s* [dD] \s* {1} \s* {2}? \s*$", regexPatternNumber,
                                                regexPatternFaces, regexPatternModifier);

            const RegexOptions regexOptions = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;

            DieRepresentationRegex = new Regex(regexPattern, regexOptions);
        }

        /// <summary>
        ///     Create a die
        /// </summary>
        public Die()
            : this(1, 1, 0)
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
            : this(1, faces, 0)
        {
        }

        /// <summary>
        ///     Create a die
        /// </summary>
        /// <param name="faces">The number of faces, or equivalently the highest face</param>
        /// <param name="modifier">The final modifier applied on the result of the die roll</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="faces" /> must be strictly positive
        /// </exception>
        public Die(int faces, int modifier)
            : this(1, faces, modifier)
        {
        }

        /// <summary>
        ///     Create a die
        /// </summary>
        /// <param name="faces">The number of faces, or equivalently the highest face</param>
        /// <param name="number">The number of dice</param>
        /// <param name="modifier">The final modifier applied on the result of the die roll</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="number" /> must be strictly positive
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="faces" /> must be strictly positive
        /// </exception>
        public Die(int number, int faces, int modifier)
        {
            Number = number;
            Faces = faces;
            Modifier = modifier;
        }

        #endregion

        #region IRollable

        /// <summary>
        ///     Roll the die
        /// </summary>
        /// <returns>the roll information</returns>
        public Roll<int> GetRoll()
        {
            int naturalRoll = NaturalRoll();
            return new Roll<int>(naturalRoll, Roll(naturalRoll));
        }

        /// <summary>
        ///     return the value (after modification) from the <paramref name="naturalRoll" />
        /// </summary>
        /// <param name="naturalRoll">the natural roll, ie the raw value before any bonuses</param>
        /// <returns>the roll information, containing both he natural and modified roll</returns>
        public int Roll(int naturalRoll)
        {
            return Math.Max(1, naturalRoll + Modifier);
        }

        /// <summary>
        ///     The smallest possible value of a <c>Roll</c>
        /// </summary>
        public virtual int Min
        {
            get { return Math.Max(1, Number + Modifier); }
        }

        /// <summary>
        ///     The biggest possible value of a <c>Roll</c>
        /// </summary>
        public virtual int Max
        {
            get { return Math.Max(1, Number*Faces + Modifier); }
        }

        /// <summary>
        ///     The average value of a <c>Roll</c>
        /// </summary>
        public decimal Average
        {
            get
            {
                if (Modifier >= 0) return Number*(((decimal) (1 + Faces))/2) + Modifier;
                return AllPossibleRollResults().Sum()/(decimal) (Math.Pow(Faces, Number));
            }
        }

        int IRollable<int>.Roll()
        {
            return Roll();
        }

        /// <summary>
        ///     Roll the die
        /// </summary>
        /// <returns>the result after applying the modifier (if any)</returns>
        public int Roll()
        {
            return Roll(NaturalRoll());
        }

        private int NaturalRoll()
        {
            int naturalRoll = 0;

            for (int i = 0; i <= Number; i++)
            {
                naturalRoll += RandomGenerator.Next(1, Faces + 1);
            }

            return naturalRoll;
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
            var that = (Die) obj;
            return Faces == that.Faces && Number == that.Number && Modifier == that.Modifier;
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
                hashCode = (hashCode*397) ^ Number;
                hashCode = (hashCode*397) ^ Modifier;
                return hashCode;
            }
        }

        #endregion

        #region parse, FromString/ToString, copy

        /// <summary>
        ///     The instance method equivalent to Parse.
        ///     Mutate the current instance on success, throw an exception on error
        /// </summary>
        /// <param name="stringRepresentation">the string representation of an instance of this class</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="stringRepresentation" /> is null
        /// </exception>
        /// <exception cref="FormatException">
        ///     <paramref name="stringRepresentation" /> is not a valid representation of a Die
        /// </exception>
        /// <remarks>Prefer the static Parse method when you know which class you need.</remarks>
        public void FromString(string stringRepresentation)
        {
            Copy(Parse(stringRepresentation));
        }

        /// <summary>
        ///     The instance method equivalent to TryParse.
        ///     Mutate the current instance on success, return false or throw an exception on error
        /// </summary>
        /// <param name="stringRepresentation">the string representation of an instance of this class</param>
        /// <returns>true on success</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="stringRepresentation" /> is null
        /// </exception>
        /// <remarks>Prefer the static TryParse method when you know which class you need.</remarks>
        public bool TryFromString(string stringRepresentation)
        {
            Die die;
            if (TryParse(stringRepresentation, out die))
            {
                Copy(die);
                return true;
            }
            return false;
        }

        /// <summary>
        ///     ToString allows us to print the object
        /// </summary>
        /// <returns>The canonical string representation of the die</returns>
        public override string ToString()
        {
            string number = (Number == 1) ? "" : Number.ToString();
            int faces = Faces;
            string modifier = (Modifier == 0) ? "" : ((Modifier > 0) ? "+" : "-") + Math.Abs(Modifier);

            return String.Format("{0}d{1}{2}", number, faces, modifier);
        }

        private void Copy(Die die)
        {
            Number = die.Number;
            Faces = die.Faces;
            Modifier = die.Modifier;
        }


        /// <summary>
        ///     Try to create a die from a string representation
        /// </summary>
        /// <param name="stringRepresentation">The string representation of the die (ex: "d10 + 1")</param>
        /// <param name="result">The Die returned if the string representation was valid</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="stringRepresentation" /> is null
        /// </exception>
        public static bool TryParse(string stringRepresentation, out Die result)
        {
            if (stringRepresentation == null)
            {
                throw new ArgumentNullException();
            }

            int number = 1;
            int faces;
            int modifier = 0;
            result = null;

            Match m = DieRepresentationRegex.Match(stringRepresentation);

            if (!m.Success)
            {
                return false;
            }

            //number
            if (m.Groups[1].Value != "")
            {
                if (!Int32.TryParse(m.Groups[1].Value, out number) || !CheckNumber(number, false)) return false;
            }

            //faces
            if (!Int32.TryParse(m.Groups[2].Value, out faces) || !CheckFaces(faces, false)) return false;

            //modifier
            if (m.Groups[3].Value != "")
            {
                if (!Int32.TryParse(m.Groups[5].Value, out modifier)) return false;

                if (m.Groups[4].Value == "-")
                {
                    modifier = -modifier;
                }
            }

            //we're good
            result = new Die(number, faces, modifier);
            return true;
        }

        /// <summary>
        ///     Create a die from a string representation or throw exception
        /// </summary>
        /// <param name="stringRepresentation">a string representation of a die (ex: "d10 + 1")</param>
        /// <returns>a die</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="stringRepresentation" /> is null
        /// </exception>
        /// <exception cref="FormatException">
        ///     <paramref name="stringRepresentation" /> is not a valid representation of a Die
        /// </exception>
        public static Die Parse(string stringRepresentation)
        {
            Die result;
            if (!TryParse(stringRepresentation, out result))
            {
                throw new FormatException();
            }

            return result;
        }

        #endregion

        #region all possible rolls

        private IEnumerable<int[]> AllPossibleRolls()
        {
            var possibleRoll = new int[Number];
            var bestRoll = new int[Number];

            //initialize possibleRoll at (1, 1, ... , 1) and bestRoll at (faces, faces, ... , faces)
            for (int i = 0; i < possibleRoll.Length; i++)
            {
                possibleRoll[i] = 1;
                bestRoll[i] = Faces;
            }

            //yield the first result
            yield return possibleRoll;

            //loop until we reach the max
            //while (!StructuralComparisons.StructuralEqualityComparer.Equals(possibleRoll, bestRoll))
            while (!possibleRoll.SequenceEqual(bestRoll))
            {
                GetNextPossibleRoll(ref possibleRoll, Faces);
                yield return possibleRoll;
            }
        }

        private IEnumerable<int> AllPossibleRollResults()
        {
            return AllPossibleRolls().Select(roll => Math.Max(1, roll.Sum() + Modifier));
        }

        private void GetNextPossibleRoll(ref int[] possibleRoll, int maxRoll)
        {
            for (int i = 0; i < possibleRoll.Length; i++)
            {
                if (possibleRoll[i] < maxRoll)
                {
                    possibleRoll[i]++;
                    break;
                }
                possibleRoll[i] = 1;
            }
        }

        #endregion
    }
}