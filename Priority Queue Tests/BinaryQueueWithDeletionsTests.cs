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
			var priorityQueue = new BinaryQueueWithDeletions<PQWDElement>();

			var pq80 = new PQWDElement(80);
			var pq90 = new PQWDElement(90);
			var pq30 = new PQWDElement(30);
			var pq85 = new PQWDElement(85);
			var pq20 = new PQWDElement(20);
			var pq40 = new PQWDElement(40);
			var pq50 = new PQWDElement(50);
			var pq35 = new PQWDElement(35);

			priorityQueue.Add(pq40);
			priorityQueue.Add(pq90);
			priorityQueue.Add(pq20);
			priorityQueue.Add(pq30);
			priorityQueue.Add(pq35);
			priorityQueue.Add(pq50);
			priorityQueue.Add(pq85);
			priorityQueue.Delete(pq30);
			Assert.IsTrue(priorityQueue.FValidate());
			Assert.AreEqual(6, priorityQueue.Count);
			int cEnums = priorityQueue.Cast<IBinaryQueueElement>().Count();
			Assert.AreEqual(6, cEnums);
			// Pop everything off
			priorityQueue.Pop();
			priorityQueue.Pop();
			priorityQueue.Pop();
			priorityQueue.Pop();
			priorityQueue.Pop();
			priorityQueue.Pop();

			priorityQueue.Add(pq80);
			Assert.AreEqual(0, ((IBinaryQueueElement)pq80).Index);
			priorityQueue.Delete(pq80);
			pq80 = new PQWDElement(80);
			Assert.AreEqual(-1, ((IBinaryQueueElement)pq80).Index);

			priorityQueue.Add(pq80);
			Assert.AreEqual(pq80, priorityQueue.Peek());
			priorityQueue.Add(pq90);
			Assert.AreEqual(2, priorityQueue.Count);
			Assert.AreEqual(pq80, priorityQueue.Peek());
			Assert.AreEqual(pq80, priorityQueue.Pop());
			Assert.AreEqual(pq90, priorityQueue.Peek());
			priorityQueue.Add(pq30);
			priorityQueue.Add(pq90);
			priorityQueue.Add(pq85);
			priorityQueue.Add(pq20);
			// 90, 90, 30, 85, 20
			Assert.AreEqual(5, priorityQueue.Count);
			Assert.AreEqual(pq20, priorityQueue.Pop());
			Assert.AreEqual(pq30, priorityQueue.Pop());
			// 90, 90, 85
			Assert.AreEqual(3, priorityQueue.Count);
			priorityQueue.Add(pq50);
			priorityQueue.Add(pq35);
			// 90, 90, 85, 50, 35
			Assert.AreEqual(5, priorityQueue.Count);
			Assert.AreEqual(pq35, priorityQueue.Pop());
			Assert.AreEqual(pq50, priorityQueue.Pop());
			Assert.AreEqual(pq85, priorityQueue.Pop());
			Assert.AreEqual(pq90, priorityQueue.Pop());
			Assert.AreEqual(pq90, priorityQueue.Pop());
			Assert.AreEqual(0, priorityQueue.Count);

			priorityQueue.Add(pq35);
			priorityQueue.Add(pq50);
			priorityQueue.Add(pq20);
			priorityQueue.Add(pq85);
			priorityQueue.Add(pq30);
			priorityQueue.Add(pq90);
			priorityQueue.Add(pq80);

			priorityQueue.Delete(pq50);
			priorityQueue.Delete(pq30);
			// 35, 20, 85, 90, 80
			Assert.AreEqual(5, priorityQueue.Count);
			Assert.AreEqual(pq20, priorityQueue.Pop());
			Assert.AreEqual(pq35, priorityQueue.Pop());
			Assert.AreEqual(pq80, priorityQueue.Pop());
			Assert.AreEqual(pq85, priorityQueue.Pop());
			Assert.AreEqual(pq90, priorityQueue.Pop());
			Assert.AreEqual(0, priorityQueue.Count);
		}
		// ReSharper restore CSharpWarnings::CS1591

		class PQWDElement : IBinaryQueueElement
		{
			#region Private Variables
			int _i = -1;

			#endregion

			// ReSharper disable once MemberCanBePrivate.Local
			public int Val { get; set; }

			#region Constructor
			public PQWDElement(int val)
			{
				Val = val;
			}
			#endregion

			#region PriorityQueueElement Members
			int IBinaryQueueElement.Index
			{
				get { return _i; }
				set { _i = value; }
			}

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
		public void TestMethod1()
		{
			//
			// TODO: Add test logic here
			//
		}
	}
}
