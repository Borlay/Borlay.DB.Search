using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Borlay.DB.Search.Tests
{
    [TestClass]
    public class QueryableTests
    {
        [TestMethod]
        public void ContainsAndAsc()
        {
            using (var context = new TestContext())
            {
                var names = new string[] { "n1", "n3" };

                var vartotojai = context.Vartotojai.Contains("Name", names).OrderBy("Name", Direction.Asc).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual("n1", vartotojai.First().Name);
                Assert.IsFalse(vartotojai.Any(v => v.Name == "n2"));
            }
        }

        [TestMethod]
        public void ContainsAndDesc()
        {
            using (var context = new TestContext())
            {
                var names = new string[] { "n1", "n3" };

                var vartotojai = context.Vartotojai.Contains("Name", names).OrderBy("Name", Direction.Desc).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual("n3", vartotojai.First().Name);
                Assert.IsFalse(vartotojai.Any(v => v.Name == "n2"));
            }
        }

        [TestMethod]
        public void WhereAndDesc()
        {
            using (var context = new TestContext())
            {
                var vartotojai = context.Vartotojai.Where("Name", "n2").OrderBy("Name", Direction.Desc).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual("n2", vartotojai.First().Name);
                Assert.IsFalse(vartotojai.Any(v => v.Name == "n1"));
                Assert.IsFalse(vartotojai.Any(v => v.Name == "n3"));
            }
        }

        [TestMethod]
        public void ContainsWhereAndDesc()
        {
            using (var context = new TestContext())
            {
                var names = new string[] { "n2", "n1" };
                var vartotojai = context.Vartotojai.Contains("Name", names).Where("Name", "n2")
                    .OrderBy("Name", Direction.Desc).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual("n2", vartotojai.First().Name);
                Assert.IsFalse(vartotojai.Any(v => v.Name == "n1"));
                Assert.IsFalse(vartotojai.Any(v => v.Name == "n3"));
            }
        }

        [TestMethod]
        public void ContainsWhereAndDescNone()
        {
            using (var context = new TestContext())
            {
                var names = new string[] { "n3", "n1" };
                var vartotojai = context.Vartotojai.Contains("Name", names).Where("Name", "n2")
                    .OrderBy("Name", Direction.Desc).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual(0, vartotojai.Count());
            }
        }

        [TestMethod]
        public void WhereAndDescNone()
        {
            using (var context = new TestContext())
            {
                var vartotojai = context.Vartotojai.Where("Name", "n2").Where("Name", "n3")
                    .OrderBy("Name", Direction.Desc).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual(0, vartotojai.Count());
            }
        }

        [TestMethod]
        public void WhereAndWhereAndDesc()
        {
            using (var context = new TestContext())
            {
                var vartotojai = context.Vartotojai.Where("Name", "n3").Where("Id", 5)
                    .OrderBy("Name", Direction.Desc).ToArray();

                Assert.IsNotNull(vartotojai, "Vartotojai nerasti");
                Assert.AreEqual(1, vartotojai.Count());
                Assert.AreEqual("n3", vartotojai.First().Name);
                Assert.AreEqual(5, vartotojai.First().Id);
            }
        }
    }
}
