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
		public void TestTemp()
		{
			var pq = new BinaryQueueWithDeletions<int>();
			pq.Add(30);
			Assert.AreEqual(30, pq.Peek());
			var val = pq.Pop();
			Assert.AreEqual(30, val);
			var cookie = pq.Add(30);
			pq.Add(40);
			pq.Delete(cookie);
			Assert.AreEqual(1, pq.Count);
			Assert.AreEqual(40, pq.Peek());

			var pqt = new BinaryQueueWithDeletions<Pqt<int>>();
			pqt.AddTyped(30);
			Assert.AreEqual(30, (int)pqt.Peek());
			var valt = pqt.Pop();
			Assert.AreEqual(30, (int)valt);
			valt = pqt.AddTyped(30);
			pqt.AddTyped(40);
			pqt.DeleteTyped(valt);
			Assert.AreEqual(1, pqt.Count);
			Assert.AreEqual(40, (int)pqt.Peek());
		}
		[TestMethod]
		public void TestPQWithDeletions()
		{
			var pq = new BinaryQueueWithDeletions<Pqt<int>>();

			pq.AddTyped(40);
			pq.AddTyped(90);
			pq.AddTyped(20);
			var pq30 = pq.AddTyped(30);
			pq.AddTyped(35);
			pq.AddTyped(50);
			pq.AddTyped(85);
			pq.DeleteTyped(pq30);
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

			var pq80 = pq.AddTyped(80);
			Assert.AreEqual(0, ((BinaryWrapper<Pqt<int>>)(pq80.Cookie)).Index);
			pq.DeleteTyped(pq80);
			Assert.IsNull(pq80.Cookie);

			pq.AddTyped(80);
			Assert.AreEqual(80, (int)pq.Peek());
			pq.AddTyped(90);
			Assert.AreEqual(2, pq.Count);
			Assert.AreEqual(80, (int)pq.Peek());
			Assert.AreEqual(80, (int)pq.Pop());
			Assert.AreEqual(90, (int)pq.Peek());
			pq.AddTyped(30);
			pq.AddTyped(90);
			pq.AddTyped(85);
			pq.AddTyped(20);
			// 90, 90, 30, 85, 20
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(20, (int)pq.Pop());
			Assert.AreEqual(30, (int)pq.Pop());
			// 90, 90, 85
			Assert.AreEqual(3, pq.Count);
			pq.AddTyped(50);
			pq.AddTyped(35);
			// 90, 90, 85, 50, 35
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(35, (int)pq.Pop());
			Assert.AreEqual(50, (int)pq.Pop());
			Assert.AreEqual(85, (int)pq.Pop());
			Assert.AreEqual(90, (int)pq.Pop());
			Assert.AreEqual(90, (int)pq.Pop());
			Assert.AreEqual(0, pq.Count);

			pq.AddTyped(35);
			var pq50 = pq.AddTyped(50);
			pq.AddTyped(20);
			pq.AddTyped(85);
			pq30 = pq.AddTyped(30);
			pq.AddTyped(90);
			pq.AddTyped(80);

			pq.DeleteTyped(pq50);
			pq.DeleteTyped(pq30);
			// 35, 20, 85, 90, 80
			Assert.AreEqual(5, pq.Count);
			Assert.AreEqual(20, (int)pq.Pop());
			Assert.AreEqual(35, (int)pq.Pop());
			Assert.AreEqual(80, (int)pq.Pop());
			Assert.AreEqual(85, (int)pq.Pop());
			Assert.AreEqual(90, (int)pq.Pop());
			Assert.AreEqual(0, pq.Count);
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
