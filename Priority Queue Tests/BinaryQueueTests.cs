using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Priority_Queue;

namespace Priority_Queue_Tests
{
	[TestClass]
	public class BinaryQueueTests
	{
		[TestMethod]
		public void TestCreation()
		{
			Assert.IsNotNull(new BinaryPriorityQueue<int>());
		}

		[TestMethod]
		public void TestEmptyPeek()
		{
			bool fNoMin;
			(new BinaryPriorityQueue<int>()).Peek(out fNoMin);
			Assert.IsTrue(fNoMin);
		}

		[TestMethod]
		public void TestEmptyPop()
		{
			bool fNoMin;
			(new BinaryPriorityQueue<int>()).Pop(out fNoMin);
			Assert.IsTrue(fNoMin);
		}

		[TestMethod]
		public void TestPq()
		{
			// ReSharper disable once UseObjectOrCollectionInitializer
			var pq = new BinaryPriorityQueue<int>();

			pq.Add(80);
			Assert.AreEqual(80, pq.Peek());
			pq.Add(90);
			Assert.AreEqual(2, pq.Count);
			Assert.AreEqual(80, pq.Peek());
			Assert.AreEqual(80, pq.Pop());
			Assert.AreEqual(90, pq.Peek());
			pq.Add(30);
			pq.Add(95);
			pq.Add(85);
			pq.Add(20);
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(20, pq.Pop());
			Assert.AreEqual(30, pq.Pop());
			Assert.AreEqual(3, pq.Count);
			pq.Add(50);
			pq.Add(35);
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(35, pq.Pop());
			Assert.AreEqual(50, pq.Pop());
			Assert.AreEqual(85, pq.Pop());
			Assert.AreEqual(90, pq.Pop());
			Assert.AreEqual(95, pq.Pop());
			Assert.AreEqual(0, pq.Count);

			pq = new BinaryPriorityQueue<int>((i1, i2) => i1.CompareTo(i2));
			pq.Add(80);
			Assert.AreEqual(80, pq.Peek());
			pq.Add(90);
			Assert.AreEqual(2, pq.Count);
			Assert.AreEqual(80, pq.Peek());
			Assert.AreEqual(80, pq.Pop());
			Assert.AreEqual(90, pq.Peek());
			pq.Add(30);
			pq.Add(95);
			pq.Add(85);
			pq.Add(20);
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(20, pq.Pop());
			Assert.AreEqual(30, pq.Pop());
			Assert.AreEqual(3, pq.Count);
			pq.Add(50);
			pq.Add(35);
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(35, pq.Pop());
			Assert.AreEqual(50, pq.Pop());
			Assert.AreEqual(85, pq.Pop());
			Assert.AreEqual(90, pq.Pop());
			Assert.AreEqual(95, pq.Pop());
			Assert.AreEqual(0, pq.Count);
		}
	}
}
