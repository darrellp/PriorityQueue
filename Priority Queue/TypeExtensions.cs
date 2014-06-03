using System;

namespace Priority_Queue
{
	public class FpqInt : IComparable
	{
		public object Cookie { get; set; }
		public int Value { get; set; }

		public FpqInt(int value)
		{
			Value = value;
		}

		public static implicit operator FpqInt(int value)
		{
			return new FpqInt(value);
		}

		public static implicit operator int(FpqInt value)
		{
			return value.Value;
		}

		public int CompareTo(object obj)
		{
			var other = obj as FpqInt;
			if (other == null)
			{
				throw new ArgumentException("Non-FpqInt compared to FpqInt");
			}
			return Value.CompareTo(other.Value);
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}

	public static class FpqExtensions
	{
		public static FpqInt AddInt(this FibonacciPriorityQueue<FpqInt> cur, int n)
		{
			var insert = new FpqInt(n);
			insert.Cookie = cur.Add(insert);
			return insert;
		}

		public static int ExtractMinInt(this FibonacciPriorityQueue<FpqInt> cur)
		{
			return cur.ExtractMin();
		}

		public static int PeekInt(this FibonacciPriorityQueue<FpqInt> cur)
		{
			return cur.Peek();
		}
	}
}
