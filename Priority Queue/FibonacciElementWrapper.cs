using System;

namespace Priority_Queue
{
	internal class FibonacciElementWrapper<TPQ> where TPQ : IComparable
	{
		public TPQ Attr { get; set; }
		public FibonacciElementWrapper<TPQ> FirstChild { get; set; }
		public FibonacciElementWrapper<TPQ> Parent { get; set; }
		public FibonacciElementWrapper<TPQ> LeftSibling { get; set; }
		public FibonacciElementWrapper<TPQ> RightSibling { get; set; }
		public int Degree { get; set; }
		public bool Marked { get; set; }

		public FibonacciElementWrapper(TPQ attr)
		{
			Attr = attr;
			Degree = 0;
			FirstChild = null;
			Marked = false;
			Parent = null;
			LeftSibling = RightSibling = this;
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

		public override string ToString()
		{
			return "[" + Attr.ToString() + "]";
		}
	}
}
