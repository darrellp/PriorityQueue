using System;

namespace Priority_Queue
{
	public class FibonacciElementWrapper<TPQ> : IFibonacciQueueElement<TPQ> where TPQ : IComparable
	{
		public TPQ Attr { get; private set; }
		public IFibonacciQueueElement<TPQ> FirstChild { get; set; }
		public IFibonacciQueueElement<TPQ> Parent { get; set; }
		public IFibonacciQueueElement<TPQ> LeftSibling { get; set; }
		public IFibonacciQueueElement<TPQ> RightSibling { get; set; }
		public int Degree { get; set; }
		public bool Marked { get; set; }

		public FibonacciElementWrapper(TPQ attr)
		{
			Attr = attr;
		}

		public int CompareTo(object obj)
		{
			var other = obj as FibonacciElementWrapper<TPQ>;
			if (other == null)
			{
				throw new ArgumentException("Different types in FibonacciElementWrapper<TPQ>.CompareTo()");
			}
			return Attr.CompareTo(other.Attr);
		}
	}
}
