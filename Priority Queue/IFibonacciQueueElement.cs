using System;

namespace Priority_Queue
{
	/// <summary>
	/// Interface IFibonacciQueueElement
	/// </summary>
	/// <remarks>
	/// Interface that elements in a fibonacci priority queue must satisfy
	/// </remarks>
	public interface IFibonacciQueueElement<TPQ> : IComparable
	{
		TPQ Attr { get; }

		// First child in the child list
		IFibonacciQueueElement<TPQ> FirstChild { get; set; }

		// Parent
		IFibonacciQueueElement<TPQ> Parent { get; set; }

		// Siblings at the same level of the tree (or forest in the case of roots
		IFibonacciQueueElement<TPQ> LeftSibling { get; set; }
		IFibonacciQueueElement<TPQ> RightSibling { get; set; }

		// Count in child list
		int Degree { get; set; }

		// Marked in the algorithms
		bool Marked { get; set; }
	}
}
