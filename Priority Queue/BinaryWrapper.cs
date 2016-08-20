using System;

namespace Priority_Queue
{
	public class BinaryWrapper<BaseType> : IComparable, IBinaryQueueDeletionElement
	{
		public BaseType Attr { get; set; }
		public int Index { get; set; }
		private readonly Func<BaseType, BaseType, int> _compare;

		public BinaryWrapper(BaseType attr, Func<BaseType, BaseType, int> compare = null)
		{
			Attr = attr;
			Index = -1;
			_compare = compare;
		}

		public int CompareTo(object obj)
		{
			var otherWrapper = obj as BinaryWrapper<BaseType>;
			if (otherWrapper == null)
			{
				throw new ArgumentException("Different types in FibonacciWrapper<BaseType>.CompareTo()");
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
