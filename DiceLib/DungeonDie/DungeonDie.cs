using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiceLib
{
    /// <summary>
    ///     Implements a classic D&amp;D die.
    /// </summary>
    /// <remarks>
    ///     A classic die is a n-faced die, with faces ranging from 1 to n.
    ///     Each face has the same probability.
    ///     A classic die is written 'd{n}', where {n} is the number of faces.
    ///     For instance, a 20-faced die is written 'd20'.
    ///     A die can have modifiers.
    ///     A modifier is a number that you add or subtract to the result of a roll.
    ///     In Dungeons &amp; Dragons 3, the result of a roll after applying all modifiers cannot be inferior to 1
    ///     For instance, if you roll 2 on a d3 - 2, you obtain 1 (not 0).
    /// </remarks>
    public class DungeonDie : IDungeonDie<int>, IParseable
    {
        #region members

        private static readonly Regex DieRepresentationRegex;
        private readonly Die _die;

        /// <summary>
        ///     The number of faces, or equivalently the highest face
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Faces must be strictly positive</exception>
        public int Faces
        {
            get { return _die.Faces; }
            set { _die.Faces = value; }
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
            get { return _die.Number; }
            set { _die.Number = value; }
        }

        #endregion

        #region constructors

        /// <summary>
        ///     static constructor to initialize the RegEx
        /// </summary>
        static DungeonDie()
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
        public DungeonDie()
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
        public DungeonDie(int faces)
            : this(faces, 0)
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
        public DungeonDie(int faces, int modifier)
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
        public DungeonDie(int number, int faces, int modifier)
        {
            _die = new Die(number, faces);
            Modifier = modifier;
        }

        #endregion

        #region IRollableModified

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

        private int NaturalRoll()
        {
            return _die.Roll();
        }

        #endregion

        #region IRollableStatistics

        /// <summary>
        ///     The smallest possible value of a <c>Roll</c>
        /// </summary>
        public int Min
        {
            get { return Roll(_die.Min); }
        }

        /// <summary>
        ///     The biggest possible value of a <c>Roll</c>
        /// </summary>
        public int Max
        {
            get { return Roll(_die.Max); }
        }

        /// <summary>
        ///     The average value of a <c>Roll</c>
        /// </summary>
        public decimal Average
        {
            get
            {
                if (Modifier >= 0) return _die.Average + Modifier;
                return AllPossibleRollResults().Sum()/(decimal) (Math.Pow(Faces, Number));
            }
        }

        #endregion

        #region IRollable

        /// <summary>
        ///     Roll the die
        /// </summary>
        /// <returns>the result after applying the modifier (if any)</returns>
        public int Roll()
        {
            return Roll(NaturalRoll());
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
            var that = (DungeonDie) obj;
            return Faces == that.Faces && Number == that.Number && Modifier == that.Modifier;
        }

        /// <summary>
        ///     == operator override
        /// </summary>
        /// <param name="x">one Die</param>
        /// <param name="y">another Die</param>
        /// <returns>True if both Dies are equals or both are null</returns>
        public static bool operator ==(DungeonDie x, DungeonDie y)
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
        public static bool operator !=(DungeonDie x, DungeonDie y)
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

        #region string representation manipulation

        /// <summary>
        ///     The instance method equivalent to Parse.
        ///     Mutate the current instance on success, throw an exception on error
        /// </summary>
        /// <param name="s">the string representation of an instance of this class</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="s" /> is null
        /// </exception>
        /// <exception cref="FormatException">
        ///     <paramref name="s" /> is not a valid representation of a Die
        /// </exception>
        /// <exception cref="OverflowException">
        ///     the Die number, faces and modifier must be in the range of an integer
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     the Die number and faces must be strictly positive
        /// </exception>
        /// <remarks>Prefer the static Parse or TryParse methods when you know which class you need.</remarks>
        public void FromString(string s)
        {
            var die = Parse(s);
            Number = die.Number;
            Faces = die.Faces;
            Modifier = die.Modifier;
        }

        /// <summary>
        ///     ToString allows us to print the object
        /// </summary>
        /// <returns>The canonical string representation of the die</returns>
        public override string ToString()
        {
            return String.Format(
                "{0}d{1}{2}",
                (Number == 1) ? "" : Number.ToString(),
                Faces,
                (Modifier == 0) ? "" : ((Modifier > 0) ? "+" : "-") + Math.Abs(Modifier)
                );
        }

        /// <summary>
        ///     Try to create a die from a string representation
        /// </summary>
        /// <param name="s">The string representation of the die (ex: "d10 + 1")</param>
        /// <param name="result">The Die returned if the string representation was valid</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="s" /> is null
        /// </exception>
        public static bool TryParse(string s, out DungeonDie result)
        {
            result = null;

            try
            {
                result = Parse(s);
                return true;
            }
            catch (Exception ex)
            {
                //don't include ArgumentNullException
                if (ex is FormatException || ex is OverflowException || ex is ArgumentOutOfRangeException)
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        ///     Create a die from a string representation or throw exception
        /// </summary>
        /// <param name="s">a string representation of a die (ex: "d10 + 1")</param>
        /// <returns>a die</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="s" /> is null
        /// </exception>
        /// <exception cref="FormatException">
        ///     <paramref name="s" /> is not a valid representation of a Die
        /// </exception>
        /// <exception cref="OverflowException">
        ///     the Die number, faces and modifier must be in the range of an integer
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     the Die number and faces must be strictly positive
        /// </exception>
        public static DungeonDie Parse(string s)
        {
            int number = 1;
            int faces;
            int modifier = 0;

            Match m = DieRepresentationRegex.Match(s);
            if (!m.Success) throw new FormatException();

            //number
            if (m.Groups[1].Value != "")
            {
                number = Int32.Parse(m.Groups[1].Value);
            }

            //faces
            faces = Int32.Parse(m.Groups[2].Value);

            //modifier
            if (m.Groups[3].Value != "")
            {
                modifier = Int32.Parse(m.Groups[5].Value);

                if (m.Groups[4].Value == "-")
                {
                    modifier = -modifier;
                }
            }

            return new DungeonDie(number, faces, modifier);
        }

        #endregion

        #region all possible rolls

        private IEnumerable<int[]> AllPossibleRolls()
        {
            //initialize possibleRoll at (1, 1, ... , 1) and bestRoll at (faces, faces, ... , faces)
            int[] possibleRoll = new int[Number].Select(x => 1).ToArray();
            int[] bestRoll = new int[Number].Select(x => Faces).ToArray();

            //yield the first result
            yield return possibleRoll;

            //loop until we reach the max
            while (!possibleRoll.SequenceEqual(bestRoll))
            {
                GetNextPossibleRoll(ref possibleRoll, Faces);
                yield return possibleRoll;
            }
        }

        private IEnumerable<int> AllPossibleRollResults()
        {
            return AllPossibleRolls().Select(roll => Roll(roll.Sum()));
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