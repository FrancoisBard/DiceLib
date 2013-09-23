using System;
using DiceLib;
using NUnit.Framework;

namespace DiceLibTests
{
    [TestFixture]
    public class DungeonDieTests
    {
        [Test]
        public void AverageTest()
        {
            Assert.AreEqual(5.50m, new DungeonDie(10).Average);
            Assert.AreEqual(6.50m, new DungeonDie(1, 10, 1).Average);
            Assert.AreEqual(4.60m, new DungeonDie(1, 10, -1).Average);
            Assert.AreEqual(12m, new DungeonDie(2, 10, 1).Average);
            Assert.AreEqual(10m, new DungeonDie(2, 10, -1).Average);
            Assert.AreEqual(8.04m, new DungeonDie(2, 10, -3).Average);
        }

        //parameterLess
        [Test]
        public void DieTest()
        {
            Assert.AreEqual(new DungeonDie(), new DungeonDie(1, 1, 0));
        }

        //faces
        [Test]
        public void DieTest1()
        {
            Assert.AreEqual(new DungeonDie(2), new DungeonDie(1, 2, 0));
        }

        //faces, modifier
        [Test]
        public void DieTest2()
        {
            Assert.AreEqual(new DungeonDie(2, 3), new DungeonDie(1, 2, 3));
        }

        //number, faces, modifier
        [Test]
        public void DieTest3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DungeonDie(0, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new DungeonDie(1, 0, 0));
            Assert.DoesNotThrow(() => new DungeonDie(1, 1, 1));
            Assert.DoesNotThrow(() => new DungeonDie(1, 1, -1));
        }

        [Test]
        public void EqualsTest()
        {
            var die1 = new DungeonDie(1, 2, 3);
            var die2 = new DungeonDie(1, 2, 3);
            var die3 = new DungeonDie(1, 2, 4);
            Assert.True(die1.Equals(die2));
            Assert.False(die1.Equals(die3));
        }

        [Test]
        public void GetHashCodeTest()
        {
            int hash1 = new DungeonDie(2, 3, 4).GetHashCode();
            int hash2 = new DungeonDie(2, 3, 4).GetHashCode();
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetRollTest()
        {
            const int modifier = 4;
            var die = new DungeonDie(2, 3, modifier);
            Roll<int> roll = die.GetRoll();
            Assert.AreEqual(roll.Value, roll.NaturalValue + modifier);
        }

        [Test]
        public void MaxTest()
        {
            Assert.AreEqual(10, new DungeonDie(10).Max);
            Assert.AreEqual(11, new DungeonDie(1, 10, 1).Max);
            Assert.AreEqual(9, new DungeonDie(1, 10, -1).Max);
            Assert.AreEqual(21, new DungeonDie(2, 10, 1).Max);
            Assert.AreEqual(19, new DungeonDie(2, 10, -1).Max);
            Assert.AreEqual(17, new DungeonDie(2, 10, -3).Max);
        }

        [Test]
        public void MinTest()
        {
            Assert.AreEqual(1, new DungeonDie(10).Min);
            Assert.AreEqual(2, new DungeonDie(1, 10, 1).Min);
            Assert.AreEqual(1, new DungeonDie(1, 10, -1).Min);
            Assert.AreEqual(3, new DungeonDie(2, 10, 1).Min);
            Assert.AreEqual(1, new DungeonDie(2, 10, -1).Min);
            Assert.AreEqual(1, new DungeonDie(2, 10, -3).Min);
        }

        [Test]
        //see TryParse for more tests
        public void ParseTest()
        {
            Assert.AreEqual(new DungeonDie(1), DungeonDie.Parse("d1"));
            Assert.Throws<FormatException>(() => DungeonDie.Parse("1  d     +    1000 "));
            Assert.Throws<ArgumentOutOfRangeException>(() => DungeonDie.Parse("1  d   0  +    1 "));
            Assert.Throws<OverflowException>(() => DungeonDie.Parse("1  d   1  +    100000000000000000000 "));
            Assert.Throws<FormatException>(() => DungeonDie.Parse(""));
            Assert.Throws<ArgumentNullException>(() => DungeonDie.Parse(null));
        }

        //parameterless
        [Test]
        public void RollTest()
        {
            Assert.DoesNotThrow(() => new DungeonDie().Roll());
        }

        //Roll(naturalRoll)
        [Test]
        public void RollTest1()
        {
            const int modifier = 4;
            const int naturalValue = 178;
            var die = new DungeonDie(1, modifier);
            Assert.AreEqual(die.Roll(naturalValue), naturalValue + modifier);
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual("d10", new DungeonDie(10).ToString());
            Assert.AreEqual("d10+1", new DungeonDie(1, 10, 1).ToString());
            Assert.AreEqual("d10-1", new DungeonDie(1, 10, -1).ToString());
            Assert.AreEqual("3d10-100", new DungeonDie(3, 10, -100).ToString());

            var die = new DungeonDie(1, 2, 3);
            Assert.AreEqual(die, DungeonDie.Parse(die.ToString()));
        }

        [Test]
        public void TryFromStringTest()
        {
            var die1 = new DungeonDie(2, 3, 4);
            var die2 = new DungeonDie(2, 3, 5);

            Assert.AreNotEqual(die1, die2);

            Assert.DoesNotThrow(() => die2.FromString("2d3+4"));
            Assert.AreEqual(die1, die2);

            Assert.Throws<FormatException>(() => die1.FromString("1  d     +    1000 "));
            Assert.Throws<ArgumentOutOfRangeException>(() => die1.FromString("1  d   0  +    1 "));
            Assert.Throws<OverflowException>(() => die1.FromString("1  d   1  +    10000000000000000000000000 "));
            Assert.Throws<FormatException>(() => die1.FromString(""));
            Assert.Throws<ArgumentNullException>(() => die1.FromString(null));
        }

        [Test]
        public void TryParseTest()
        {
            DungeonDie d;

            Assert.True(DungeonDie.TryParse("D1", out d));
            Assert.AreEqual(new DungeonDie(1), d);

            Assert.True(DungeonDie.TryParse("d10", out d));
            Assert.AreEqual(new DungeonDie(10), d);

            Assert.True(DungeonDie.TryParse("d10 + 1", out d));
            Assert.AreEqual(new DungeonDie(1, 10, 1), d);

            Assert.True(DungeonDie.TryParse("d10 + 10 ", out d));
            Assert.AreEqual(new DungeonDie(1, 10, 10), d);

            Assert.True(DungeonDie.TryParse("d10 +112 ", out d));
            Assert.AreEqual(new DungeonDie(1, 10, 112), d);

            Assert.True(DungeonDie.TryParse("d10+ 10", out d));
            Assert.AreEqual(new DungeonDie(1, 10, 10), d);

            Assert.True(DungeonDie.TryParse("    d10+1000 ", out d));
            Assert.AreEqual(new DungeonDie(1, 10, 1000), d);

            Assert.True(DungeonDie.TryParse("  d  10   +    1001 ", out d));
            Assert.AreEqual(new DungeonDie(1, 10, 1001), d);

            Assert.True(DungeonDie.TryParse("30  d  10   +    1000 ", out d));
            Assert.AreEqual(new DungeonDie(30, 10, 1000), d);

            Assert.False(DungeonDie.TryParse("0  d  1   +    1000 ", out d));
            Assert.IsNull(d);

            Assert.False(DungeonDie.TryParse("1  d  0   +    1000 ", out d));
            Assert.IsNull(d);

            Assert.False(DungeonDie.TryParse("1  d  1   +     ", out d));
            Assert.IsNull(d);

            Assert.False(DungeonDie.TryParse("1  d     +    1000 ", out d));
            Assert.IsNull(d);

            Assert.False(DungeonDie.TryParse("1  d   1  +    10000000000000000000000000 ", out d));
            Assert.IsNull(d);

            Assert.Throws<ArgumentNullException>(() => DungeonDie.TryParse(null, out d));
        }
    }
}