using System;
using DiceLib;
using NUnit.Framework;

namespace DiceLibTests
{
    [TestFixture]
    internal class DieTests
    {
        [Test]
        public void Average()
        {
            Assert.AreEqual(5.50m, new Die(10).Average);
            Assert.AreEqual(6.50m, new Die(1, 10, 1).Average);
            Assert.AreEqual(4.60m, new Die(1, 10, -1).Average);
        }

        [Test]
        public void Parse()
        {
            Assert.AreEqual(new Die(1), Die.Parse("d1"));
            Assert.Throws<FormatException>(() => Die.Parse("1  d     +    1000 "));
            Assert.Throws<ArgumentNullException>(() => Die.Parse(null));
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual("d10", new Die(10).ToString());
            Assert.AreEqual("d10+1", new Die(1, 10, 1).ToString());
            Assert.AreEqual("d10-1", new Die(1, 10, -1).ToString());
            Assert.AreEqual("3d10-100", new Die(3, 10, -100).ToString());
        }

        [Test]
        public void TryParse()
        {
            Die d;

            Assert.True(Die.TryParse("D1", out d));
            Assert.AreEqual(new Die(1), d);

            Assert.True(Die.TryParse("d10", out d));
            Assert.AreEqual(new Die(10), d);

            Assert.True(Die.TryParse("d10 + 1", out d));
            Assert.AreEqual(new Die(1, 10, 1), d);

            Assert.True(Die.TryParse("d10 + 10 ", out d));
            Assert.AreEqual(new Die(1, 10, 10), d);

            Assert.True(Die.TryParse("d10 +112 ", out d));
            Assert.AreEqual(new Die(1, 10, 112), d);

            Assert.True(Die.TryParse("d10+ 10", out d));
            Assert.AreEqual(new Die(1, 10, 10), d);

            Assert.True(Die.TryParse("    d10+1000 ", out d));
            Assert.AreEqual(new Die(1, 10, 1000), d);

            Assert.True(Die.TryParse("  d  10   +    1001 ", out d));
            Assert.AreEqual(new Die(1, 10, 1001), d);

            Assert.True(Die.TryParse("30  d  10   +    1000 ", out d));
            Assert.AreEqual(new Die(30, 10, 1000), d);

            Assert.False(Die.TryParse("0  d  1   +    1000 ", out d));
            Assert.IsNull(d);

            Assert.False(Die.TryParse("1  d  0   +    1000 ", out d));
            Assert.IsNull(d);

            Assert.False(Die.TryParse("1  d  1   +     ", out d));
            Assert.IsNull(d);

            Assert.False(Die.TryParse("1  d     +    1000 ", out d));
            Assert.IsNull(d);

            Assert.Throws<ArgumentNullException>(() => Die.TryParse(null, out d));
        }

        [Test]
        public void TryParseToString()
        {
            var die = new Die(1, 2, 3);
            Assert.AreEqual(die, Die.Parse(die.ToString()));
        }
    }
}