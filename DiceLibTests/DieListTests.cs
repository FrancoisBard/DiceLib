using System.Collections;
using System.Linq;
using DiceLib;
using NUnit.Framework;

namespace DiceLibTests
{
    [TestFixture]
    internal class DieListTests
    {
        [Test]
        public void Average()
        {
            Assert.AreEqual(11.10m,
                            new ParseableDieList<DungeonDie> {new DungeonDie(1, 10, 1), new DungeonDie(1, 10, -1)}
                                .Average);
        }

        [Test]
        public void Equals()
        {
            var dieSet1 = new ParseableDieList<DungeonDie> {new DungeonDie(1), new DungeonDie(2)};
            var dieSet2 = new ParseableDieList<DungeonDie> {new DungeonDie(1), new DungeonDie(2)};
            var dieSet3 = new ParseableDieList<DungeonDie> {new DungeonDie(2), new DungeonDie(1)};

            CollectionAssert.AreEquivalent(dieSet1, dieSet2);
            Assert.True(dieSet1.SequenceEqual(dieSet2));
            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(dieSet1, dieSet2));

            CollectionAssert.AreEquivalent(dieSet1, dieSet3);
            Assert.False(dieSet1.SequenceEqual(dieSet3));
            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(dieSet1, dieSet3));
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual("d10 ; 3d10-100",
                            (new ParseableDieList<DungeonDie> {new DungeonDie(10), new DungeonDie(3, 10, -100)})
                                .ToString());
        }

        [Test]
        public void TryParse()
        {
            ParseableDieList<DungeonDie> d;

            //both dice are valid
            Assert.True(ParseableDieList<DungeonDie>.TryParse("D1  ;30  d  10   +    1000 ", out d));
            Assert.AreEqual(new ParseableDieList<DungeonDie> {new DungeonDie(1), new DungeonDie(30, 10, 1000)}, d);

            //trailing ;
            Assert.False(ParseableDieList<DungeonDie>.TryParse("D1  ;30  d  10   +    1000 ;", out d));
            Assert.IsNull(d);

            //second die is invalid
            Assert.False(ParseableDieList<DungeonDie>.TryParse("D1  ;1  d  0   +    1000 ", out d));
            Assert.IsNull(d);
        }

        [Test]
        public void TryParseToString()
        {
            ParseableDieList<DungeonDie> result;
            var original = new ParseableDieList<DungeonDie> {new DungeonDie(10), new DungeonDie(3, 10, -100)};

            Assert.True(ParseableDieList<DungeonDie>.TryParse(original.ToString(), out result));
            Assert.AreEqual(original, result);
        }
    }
}