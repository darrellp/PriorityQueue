using System;

namespace Priority_Queue
{
	internal class FibonacciWrapper<TPQ> where TPQ : IComparable
	{
		public TPQ Attr { get; set; }
		public FibonacciWrapper<TPQ> FirstChild { get; set; }
		public FibonacciWrapper<TPQ> Parent { get; set; }
		public FibonacciWrapper<TPQ> LeftSibling { get; set; }
		public FibonacciWrapper<TPQ> RightSibling { get; set; }
		public int Degree { get; set; }
		public bool Marked { get; set; }
		internal bool InfinitelyNegative { get; set; }

		public FibonacciWrapper(TPQ attr)
		{
			Attr = attr;
			Degree = 0;
			FirstChild = null;
			Marked = false;
			Parent = null;
			LeftSibling = RightSibling = this;
			InfinitelyNegative = false;
		}

		public int CompareTo(object obj)
		{
			var other = obj as FibonacciWrapper<TPQ>;
			if (other == null)
			{
				throw new ArgumentException("Different types in FibonacciWrapper<TPQ>.CompareTo()");
			}
			// Infinitely negative values are always smaller than other values
			if (InfinitelyNegative)
			{
				return -1;
			}
			if (other.InfinitelyNegative)
			{
				return 1;
			}
			return Attr.CompareTo(other.Attr);
		}

		public override string ToString()
		{
			return "[" + Attr.ToString() + "]";
		}
	}
}
