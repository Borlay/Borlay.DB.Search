using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Borlay.DB.Search.Tests
{
    [TestClass]
    public class FilterTests
    {
        [TestMethod]
        public void FilterByName()
        {
            using (var context = new TestContext())
            {
                var filter = new VartotojaiFilter()
                {
                    Name = "n2"
                };

                var vartotojai = context.Vartotojai.Filter(filter).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual("n2", vartotojai.First().Name);
                Assert.IsFalse(vartotojai.Any(v => v.Name == "n1"));
                Assert.IsFalse(vartotojai.Any(v => v.Name == "n3"));
            }
        }

        [TestMethod]
        public void IdIsGreaterOrEqual()
        {
            using (var context = new TestContext())
            {
                var filter = new VartotojaiFilter()
                {
                    Name = "n3",
                    Id = 6
                };

                var vartotojai = context.Vartotojai.Filter(filter).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual("n3", vartotojai.First().Name);
                Assert.IsTrue(vartotojai.All(v => v.Id >= 6));
            }
        }

        [TestMethod]
        public void ColumnNameAndStringContains()
        {
            using (var context = new TestContext())
            {
                var filter = new VartotojaiFilter()
                {
                    NameContains = "2",
                };

                var vartotojai = context.Vartotojai.Filter(filter).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual("n2", vartotojai.First().Name);
                Assert.IsFalse(vartotojai.Any(v => v.Name != "n2"));
            }
        }

        [TestMethod]
        public void ArrayContains()
        {
            using (var context = new TestContext())
            {
                var filter = new VartotojaiFilter()
                {
                    NameIn = new string[] { "n1", "n3" },
                    OrderColumn = "Name",
                    OrderDirection = Direction.Desc
                };

                var vartotojai = context.Vartotojai.Filter(filter).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual("n3", vartotojai.First().Name);
                Assert.AreEqual("n1", vartotojai.Last().Name);
                Assert.IsFalse(vartotojai.Any(v => v.Name == "n2"));
            }
        }
    }
}
