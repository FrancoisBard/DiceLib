namespace DiceLib
{
    /// <summary>
    ///     A Dungeons &amp; Dragons 3 Skill d20, with Take10() and Take20() methods
    /// </summary>
    public class SkillDie : Die
    {
        /// <summary>
        ///     Instantiate a new skill die
        /// </summary>
        /// <param name="modifier">the bonus modifier</param>
        public SkillDie(int modifier = 0)
            : base(20, modifier)
        {
        }

        /// <summary>
        ///     Roll a natural 10 and apply the modifier
        /// </summary>
        /// <returns></returns>
        public int Take10()
        {
            return Roll(10);
        }

        /// <summary>
        ///     Roll a natural 20 and apply the modifier
        /// </summary>
        /// <returns></returns>
        public int Take20()
        {
            return Roll(20);
        }
    }
}