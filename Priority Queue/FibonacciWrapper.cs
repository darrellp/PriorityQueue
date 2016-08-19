using System;

namespace Priority_Queue
{
	internal class FibonacciWrapper<TPQ> : IBinaryQueueDeletionElement
    {
		public TPQ Attr { get; set; }
		public FibonacciWrapper<TPQ> FirstChild { get; set; }
		public FibonacciWrapper<TPQ> Parent { get; set; }
		public FibonacciWrapper<TPQ> LeftSibling { get; set; }
		public FibonacciWrapper<TPQ> RightSibling { get; set; }
		public int Degree { get; set; }
		public bool Marked { get; set; }
		private readonly Func<TPQ, TPQ, int> _compare;
		internal bool InfinitelyNegative { get; set; }

		public FibonacciWrapper(TPQ attr, Func<TPQ, TPQ, int> compare = null)
		{
			Attr = attr;
			Degree = 0;
			FirstChild = null;
			Marked = false;
			Parent = null;
			LeftSibling = RightSibling = this;
			InfinitelyNegative = false;
			_compare = compare;
		}

		public int CompareTo(object obj)
		{
			var otherWrapper = obj as FibonacciWrapper<TPQ>;

			// Infinitely negative values are always smaller than other values
			if (InfinitelyNegative)
			{
				return -1;
			}
			if (otherWrapper.InfinitelyNegative)
			{
				return 1;
			}
			if (_compare != null)
			{
				return _compare(Attr, otherWrapper.Attr);
			}

			var cmpThis = Attr as IComparable;
			var cmpOther = otherWrapper.Attr as IComparable;
			if (cmpThis == null || cmpOther == null)
			{
				throw new InvalidOperationException("No comparison function and Attrs are not IComparable");
			}
			return cmpThis.CompareTo(cmpOther);
		}

		public override string ToString()
		{
			return "[" + Attr.ToString() + "]";
		}

	    public int Index { get; set; }
    }
}
