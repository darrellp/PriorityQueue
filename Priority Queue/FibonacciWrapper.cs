using System;

namespace Priority_Queue
{
    public class FibonacciWrapper<BaseType> : IBinaryQueueDeletionElement
    {
		public BaseType Attr { get; set; }
		public FibonacciWrapper<BaseType> FirstChild { get; set; }
		public FibonacciWrapper<BaseType> Parent { get; set; }
		public FibonacciWrapper<BaseType> LeftSibling { get; set; }
		public FibonacciWrapper<BaseType> RightSibling { get; set; }
		public int Degree { get; set; }
		public bool Marked { get; set; }
		private readonly Func<BaseType, BaseType, int> _compare;
		internal bool InfinitelyNegative { get; set; }

		public FibonacciWrapper(BaseType attr, Func<BaseType, BaseType, int> compare = null)
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
			var otherWrapper = obj as FibonacciWrapper<BaseType>;

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
