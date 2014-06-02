using System;
using System.Collections.Generic;
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
		public void TestExtractMin()
		{
			bool fNoMin;
			var fpq = new FibonacciPriorityQueue<int> {13};
			var val = fpq.Peek(out fNoMin);
			Assert.IsFalse(fNoMin);
			Assert.AreEqual(13, val);
			fpq.Add(10);
			Assert.AreEqual(10, fpq.Peek());
			fpq.Add(11);
			Assert.AreEqual(10, fpq.Peek());
			Assert.AreEqual(10, fpq.ExtractMin());
			Assert.AreEqual(11, fpq.ExtractMin());
			Assert.AreEqual(13, fpq.ExtractMin());
			Assert.AreEqual(default(int), fpq.ExtractMin(out fNoMin));
			Assert.IsTrue(fNoMin);
			fpq.Add(10);
			fpq.Add(10);
			fpq.Add(10);
			Assert.AreEqual(10, fpq.ExtractMin());
			Assert.AreEqual(10, fpq.ExtractMin());
			Assert.AreEqual(10, fpq.ExtractMin());
			Assert.AreEqual(0, fpq.ExtractMin(out fNoMin));
			Assert.IsTrue(fNoMin);
		}

		[TestMethod]
		public void TestRandomOps()
		{
			var rnd = new Random(110456);
			var fpq = new FibonacciPriorityQueue<int>();
			var vals = new HashSet<int>();
			int val;

			for (var i = 0; i < 1000; i++)
			{
				Assert.AreEqual(fpq.Count, vals.Count);
				if (vals.Count == 0 || rnd.Next(100) > 30)
				{
					val = rnd.Next();
					if (vals.Contains(val))
					{
						// Sincs the set would only keep one copy of the value this would
						// screw us up if it were allowed to happen
						continue;
					}
					fpq.Add(val);
					vals.Add(val);
				}
				else
				{
					val = fpq.ExtractMin();
					Assert.IsTrue(vals.Contains(val));
					vals.Remove(val);
				}
			}
			Assert.AreEqual(vals.Count, fpq.Count);
			val = fpq.ExtractMin();
			Assert.IsTrue(vals.Contains(val));
			vals.Remove(val);
			var count = vals.Count;
			for (int i = 0; i < count; i++)
			{
				var next = fpq.ExtractMin();
				Assert.IsTrue(next > val);
				Assert.IsTrue(vals.Contains(next));
				vals.Remove(next);
				val = next;
			}
		}
	}
}
