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
            Assert.AreEqual(11.10m, new DieList<Die> {new Die(1, 10, 1), new Die(1, 10, -1)}.Average);
        }

        [Test]
        public void Equals()
        {
            var dieSet1 = new DieList<Die> {new Die(1), new Die(2)};
            var dieSet2 = new DieList<Die> {new Die(1), new Die(2)};
            var dieSet3 = new DieList<Die> {new Die(2), new Die(1)};

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
            Assert.AreEqual("d10 ; 3d10-100", (new DieList<Die> {new Die(10), new Die(3, 10, -100)}).ToString());
        }

        [Test]
        public void TryParse()
        {
            DieList<Die> d;

            //both dice are valid
            Assert.True(DieList<Die>.TryParse("D1  ;30  d  10   +    1000 ", out d));
            Assert.AreEqual(new DieList<Die> {new Die(1), new Die(30, 10, 1000)}, d);

            //trailing ;
            Assert.False(DieList<Die>.TryParse("D1  ;30  d  10   +    1000 ;", out d));
            Assert.IsNull(d);

            //second die is invalid
            Assert.False(DieList<Die>.TryParse("D1  ;1  d  0   +    1000 ", out d));
            Assert.IsNull(d);
        }

        [Test]
        public void TryParseToString()
        {
            DieList<Die> result;
            var original = new DieList<Die> {new Die(10), new Die(3, 10, -100)};

            Assert.True(DieList<Die>.TryParse(original.ToString(), out result));
            Assert.AreEqual(original, result);
        }
    }
}