using System;
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
			fpq.Add(13);
			val = fpq.Peek(out fNoMin);
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
		}
	}
}
