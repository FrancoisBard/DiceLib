namespace DiceLib
{
    /// <summary>
    ///     A Dungeons &amp; Dragons 3 d20
    /// </summary>
    /// <remarks>
    ///     even if the rulebooks tend to associate the damage die and the critical information,
    ///     it is more natural to see the critical information as part of the attack die.
    /// </remarks>
    public class AttackDie : Die
    {
        /// <summary>
        ///     The critical strike information
        /// </summary>
        public readonly Critical Critical;

        /// <summary>
        ///     instantiate a new attack die
        /// </summary>
        /// <param name="modifier">the modifier to apply to the d20</param>
        /// <param name="critical">the critical strike information</param>
        public AttackDie(int modifier = 0, Critical critical = null)
            : base(20, modifier)
        {
            Critical = critical ?? new Critical();
        }

        /// <summary>
        ///     instantiate a new attack die
        /// </summary>
        /// <param name="modifier">the modifier to apply to the d20</param>
        /// <param name="threat">A normal hit is a critical strike above this value (included)</param>
        /// <param name="multiplier">The multiplier to apply to the damage roll</param>
        public AttackDie(int modifier, int threat, int multiplier)
            : this(modifier, new Critical(threat, multiplier))
        {
        }
    }
}