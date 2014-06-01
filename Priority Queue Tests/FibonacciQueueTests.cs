using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Priority_Queue;
using fWrap = Priority_Queue.FibonacciElementWrapper<int>;

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
		}
	}
}
