using System;

namespace Priority_Queue
{
	public class BinaryWrapper<TPQ> : IComparable, IBinaryQueueDeletionElement
	{
		public TPQ Attr { get; set; }
		public int Index { get; set; }
		private readonly Func<TPQ, TPQ, int> _compare;

		public BinaryWrapper(TPQ attr, Func<TPQ, TPQ, int> compare = null)
		{
			Attr = attr;
			Index = -1;
			_compare = compare;
		}

		public int CompareTo(object obj)
		{
			var otherWrapper = obj as BinaryWrapper<TPQ>;
			if (otherWrapper == null)
			{
				throw new ArgumentException("Different types in FibonacciWrapper<TPQ>.CompareTo()");
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
	}
}
