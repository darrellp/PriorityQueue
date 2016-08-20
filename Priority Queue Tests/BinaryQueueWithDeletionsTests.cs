using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Priority_Queue;

namespace Priority_Queue_Tests
{
	/// <summary>
	/// Summary description for BinaryQueueWithDeletionsTests
	/// </summary>
	[TestClass]
	public class BinaryQueueWithDeletionsTests
	{
		[TestMethod]
		public void TestPQWithDeletions()
		{
            // BinaryQueueDithDeletionsTyped puts a wrapper around each newly
            // added item and returns the wrapper in Add().  This slows things
            // down a teeny bit and requires you to keep track of the wrapper
            // separately from the added object but also means you don't have
            // to implement IBinaryDeletionElement on the added objects.
            var pq = new BinaryQueueDithDeletionsTyped<int>();

			pq.Add(40);
			pq.Add(90);
			pq.Add(20);
			var pq30 = pq.Add(30);
			pq.Add(35);
			pq.Add(50);
			pq.Add(85);
			pq.Delete(pq30);
			Assert.IsTrue(pq.FValidate());
			Assert.AreEqual(6, pq.Count);
			int cEnums = (pq.ToList()).Count();
			Assert.AreEqual(6, cEnums);
			// Pop everything off
			pq.Pop();
			pq.Pop();
			pq.Pop();
			pq.Pop();
			pq.Pop();
			pq.Pop();

			var pq80 = pq.Add(80);
			pq.Delete(pq80);
			pq.Add(80);
			Assert.AreEqual(80, pq.Peek());
			pq.Add(90);
			Assert.AreEqual(2, pq.Count);
			Assert.AreEqual(80, pq.Peek());
			Assert.AreEqual(80, pq.Pop());
			Assert.AreEqual(90, pq.Peek());
			pq.Add(30);
			pq.Add(90);
			pq.Add(85);
			pq.Add(20);
			// 90, 90, 30, 85, 20
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(20, pq.Pop());
			Assert.AreEqual(30, pq.Pop());
			// 90, 90, 85
			Assert.AreEqual(3, pq.Count);
			pq.Add(50);
			pq.Add(35);
			// 90, 90, 85, 50, 35
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(35, pq.Pop());
			Assert.AreEqual(50, pq.Pop());
			Assert.AreEqual(85, pq.Pop());
			Assert.AreEqual(90, pq.Pop());
			Assert.AreEqual(90, pq.Pop());
			Assert.AreEqual(0, pq.Count);

			pq.Add(35);
			var pq50 = pq.Add(50);
			pq.Add(20);
			pq.Add(85);
			pq30 = pq.Add(30);
			pq.Add(90);
			pq.Add(80);

			pq.Delete(pq50);
			pq.Delete(pq30);
			// 35, 20, 85, 90, 80
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(20, pq.Pop());
			Assert.AreEqual(35, pq.Pop());
			Assert.AreEqual(80, pq.Pop());
			Assert.AreEqual(85, pq.Pop());
			Assert.AreEqual(90, pq.Pop());
			Assert.AreEqual(0, pq.Count);
		}
		// ReSharper restore CSharpWarnings::CS1591

		class PQWDElement : IBinaryQueueDeletionElement
		{
			// ReSharper disable once MemberCanBePrivate.Local
			public int Val { get; set; }

			#region Constructor
			public PQWDElement(int val)
			{
				Val = val;
			}
            #endregion

            #region PriorityQueueDeletionElement Members
            public int Index { get; set; }
            #endregion

            #region IComparable Members

             int IComparable.CompareTo(object obj)
			{
				if (Val > ((PQWDElement)obj).Val)
				{
					return 1;
				}
				if (Val < ((PQWDElement)obj).Val)
				{
					return -1;
				}
				return 0;
			}

			#endregion

		}

		[TestMethod]
		public void TestManualPQWithDeletions()
		{
            // In contrast to BinaryQueueWithDeletionsTyped,
            // BinaryQueueWithDeletions requires all added elements to 
            // implement IBinaryQueueDeletionElement.  This is a little
            // faster and means you don't have to keep track of a 
            // separate wrapper in order to remove things from the queue.
            var pq = new BinaryQueueWithDeletions<PQWDElement>();

		    var add1 = new PQWDElement(10);
		    pq.Add(add1);
            Assert.AreEqual(10, pq.Peek().Val);
		    var add2 = new PQWDElement(20);
		    pq.Add(add2);
            Assert.AreEqual(10, pq.Peek().Val);
		    pq.Delete(add1);
            Assert.AreEqual(20, pq.Peek().Val);
		    pq.Delete(add2);
            Assert.AreEqual(0, pq.Count);
		}
    }
}
