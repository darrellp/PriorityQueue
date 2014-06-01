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
			//Assert.IsNotNull(new FibonacciPriorityQueue<FibonacciElementWrapper<int>>());
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void TestPeekException()
		{
			//(new FibonacciPriorityQueue<FibonacciElementWrapper<int>>()).Peek();
		}
	}
}
