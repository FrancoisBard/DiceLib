using System;

namespace DiceLib
{
    /// <summary>
    ///     A class to represent a critical hit information
    /// </summary>
    /// <remarks>see http://www.dandwiki.com/wiki/Critical_hits#Critical_Hits </remarks>
    public class Critical
    {
        /// <summary>
        ///     The multiplier to apply to the damage roll
        /// </summary>
        public readonly int Multiplier;

        /// <summary>
        ///     A normal hit is a critical strike above this value (included)
        /// </summary>
        public readonly int Threat;

        /// <summary>
        ///     Instantiate a new critical hit information
        /// </summary>
        /// <param name="threat">A normal hit is a critical strike above this value (included)</param>
        /// <param name="multiplier">The multiplier to apply to the damage roll</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Threat must be between 2 and 20 included
        ///     Multiplier must be strictly positive
        /// </exception>
        public Critical(int threat = 20, int multiplier = 2)
        {
            //1 doesn't make any sense, it's an automatic miss.
            if ((threat < 2) || (threat > 20))
            {
                throw new ArgumentOutOfRangeException("threat",
                                                      String.Format(
                                                          "threat must be between 2 and 20 included, was {0}", threat));
            }

            if (multiplier < 1)
            {
                throw new ArgumentOutOfRangeException("multiplier",
                                                      String.Format("multiplier must be strictly positive, was {0}",
                                                                    multiplier));
            }

            Threat = threat;
            Multiplier = multiplier;
        }
    }
}