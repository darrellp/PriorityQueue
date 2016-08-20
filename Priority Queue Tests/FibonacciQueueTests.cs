using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Priority_Queue;

namespace Priority_Queue_Tests
{
	[TestClass]
	public class FibonacciQueueTests
	{
		[TestMethod]
		public void TestCreation()
		{
			Assert.IsNotNull(new FibonacciPriorityQueue<int>());
		}

		[TestMethod]
		public void TestPeek()
		{
			var fpq = new FibonacciPriorityQueue<int>();
			var val = fpq.Peek();
			Assert.AreEqual(default(int), val);
			bool fNoMin;
			val = fpq.Peek(out fNoMin);
			Assert.IsTrue(fNoMin);
			Assert.AreEqual(default(int), val);
		}

        [TestMethod]
        public void TestExtractMinTyped()
        {
            bool fNoMin;
            var fpq = new FibonacciTypedQueue<int> { 13 };
            int val = fpq.Peek(out fNoMin);
            Assert.IsFalse(fNoMin);
            Assert.AreEqual(13, val);
            fpq.Add(10);
            Assert.AreEqual(10, (int)fpq.Peek());
            fpq.Add(11);
            Assert.AreEqual(10, (int)fpq.Peek());
            Assert.AreEqual(10, (int)fpq.Pop());
            Assert.AreEqual(11, (int)fpq.Pop());
            Assert.AreEqual(13, (int)fpq.Pop());
            Assert.AreEqual(null, fpq.Pop(out fNoMin));
            Assert.IsTrue(fNoMin);
            fpq.Add(10);
            fpq.Add(10);
            fpq.Add(10);
            Assert.AreEqual(10, (int)fpq.Pop());
            Assert.AreEqual(10, (int)fpq.Pop());
            Assert.AreEqual(10, (int)fpq.Pop());
            Assert.AreEqual(null, fpq.Pop(out fNoMin));
            Assert.IsTrue(fNoMin);
        }

        [TestMethod]
        public void TestRandomOps()
        {
            var rnd = new Random(110456);
            var fpq = new FibonacciTypedQueue<int>();

            // We have to make these Pqt<int> because they're the keys we'll be using
            // to modify/delete elements from the queue.
            var vals = new HashSet<Pqt<int>>();

            // We have to make this Pqt<int> because it will be used as a key
            Pqt<int> val;

            for (var i = 0; i < 1000; i++)
            {
                Assert.AreEqual(fpq.Count, vals.Count);
                if (vals.Count == 0 || rnd.Next(100) > 30)
                {
                    val = rnd.Next();
                    if (vals.Contains(val))
                    {
                        // Since the vals set would only keep one copy of the value this would
                        // screw us up if it were allowed to happen
                        continue;
                    }
                    vals.Add(fpq.Add(val));
                }
                else if (rnd.Next(100) < 5)
                {
                    val = vals.First();
                    fpq.Delete(val);
                    vals.Remove(val);
                }
                else if (rnd.Next(100) < 20)
                {
                    val = vals.First();
                    // We have to make newval a Pqt<int> to preserve it's key when we add it
                    // to vals.  If it's just an int then the add to vals below will coerce
                    // it's int value to a Pqt<int> just fine but will have a null in the
                    // cookie.
                    Pqt<int> newval = rnd.Next(val);
                    // DecreaseKeyTyped will transfer cookie value from the old to the
                    // new FpqInt.
                    fpq.DecreaseKey(val, newval);
                    vals.Remove(val);
                    vals.Add(newval);
                    Assert.IsTrue(fpq.Validate());
                }
                else
                {
                    val = fpq.Pop();
                    Assert.IsTrue(vals.Contains(val));
                    vals.Remove(val);
                }
            }
            Assert.AreEqual(vals.Count, fpq.Count);
            Assert.AreEqual(vals.Count, fpq.ToList().Count);
            val = fpq.Pop();
            Assert.IsTrue(vals.Contains(val));
            vals.Remove(val);
            var count = vals.Count;
            for (var i = 0; i < count; i++)
            {
                var next = fpq.Pop();
                Assert.IsTrue(next > val);
                Assert.IsTrue(vals.Contains(next));
                vals.Remove(next);
                val = next;
            }
        }

        [TestMethod]
		public void TestDecreaseKey()
		{
			var fpq = new FibonacciTypedQueue<int>();
			var i1 = fpq.Add(100);
			var i2 = fpq.Add(300);
			fpq.Add(400);
			fpq.Add(200);
			fpq.Add(500);
			fpq.Add(600);
			fpq.Add(10);
			Assert.AreEqual(10, (int)fpq.Pop());
			Assert.AreEqual(100, (int)fpq.Peek());
			fpq.DecreaseKey(i1, 99);
			Assert.AreEqual(99, (int)fpq.Peek());
			fpq.DecreaseKey(i2, 98);
			Assert.AreEqual(98, (int)fpq.Peek());
		}
	}
}
