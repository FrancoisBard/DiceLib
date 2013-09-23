namespace DiceLib
{
    /// <summary>
    ///     A Class for handling a complete Attack+Damage roll
    /// </summary>
    public class AttackDamage
    {
        /// <summary>
        ///     The Attack Die
        /// </summary>
        public readonly AttackDie Attack;

        /// <summary>
        ///     The Damage Die
        /// </summary>
        public readonly DungeonDie Damage;

        /// <summary>
        ///     A new couple of Attack + Damage Dice
        /// </summary>
        /// <param name="attack"></param>
        /// <param name="damage"></param>
        public AttackDamage(AttackDie attack, DungeonDie damage)
        {
            Attack = attack;
            Damage = damage;
        }

        /// <summary>
        ///     Roll against a given armor class
        /// </summary>
        /// <param name="armorClass"></param>
        /// <returns>The amount of damage inflicted. Returns 0 if it didn't hit or hit and inflicted 0 damages.</returns>
        public int RollAgainst(int armorClass)
        {
            Roll<int> attackRoll = Attack.GetRoll();

            if (IsHit(attackRoll, armorClass))
            {
                int throws = IsCritical(attackRoll.NaturalValue, armorClass) ? Attack.Critical.Multiplier : 1;
                int result = 0;
                for (int i = 0; i < throws; i++)
                {
                    result += Damage.Roll();
                }
                return result;
            }

            return 0;
        }

        private static bool IsHit(Roll<int> attackRoll, int armorClass)
        {
            return (attackRoll.NaturalValue == 20) || (attackRoll.NaturalValue != 1 && (attackRoll.Value >= armorClass));
        }

        private bool IsCritical(int naturalRoll, int armorClass)
        {
            return naturalRoll >= Attack.Critical.Threat && IsHit(Attack.GetRoll(), armorClass);
        }
    }
}