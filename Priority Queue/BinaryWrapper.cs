using System;

namespace Priority_Queue
{
	public class BinaryWrapper<TPQ> : IComparable where TPQ : IComparable
	{
		public TPQ Attr { get; set; }
		public int Index { get; set; }

		public BinaryWrapper(TPQ attr)
		{
			Attr = attr;
			Index = -1;
		}

		public int CompareTo(object obj)
		{
			var other = obj as BinaryWrapper<TPQ>;
			if (other == null)
			{
				throw new ArgumentException("Different types in FibonacciWrapper<TPQ>.CompareTo()");
			}
			return Attr.CompareTo(other.Attr);
		}

		public override string ToString()
		{
			return "[" + Attr.ToString() + "]";
		}
	}
}
