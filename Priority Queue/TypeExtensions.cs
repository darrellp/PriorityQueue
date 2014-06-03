using System;
using System.Net;

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

		public FpqInt(FpqInt oldValue)
		{
			Value = oldValue.Value;
			Cookie = oldValue.Cookie;
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

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}

	public static class FpqExtensions
	{
		public static FpqInt AddInt(this FibonacciPriorityQueue<FpqInt> fpq, int n)
		{
			var insert = new FpqInt(n);
			insert.Cookie = fpq.Add(insert);
			return insert;
		}

		public static int ExtractMinInt(this FibonacciPriorityQueue<FpqInt> fpq)
		{
			return fpq.ExtractMin();
		}

		public static int PeekInt(this FibonacciPriorityQueue<FpqInt> fpq)
		{
			return fpq.Peek();
		}

		public static void DecreaseKeyInt(this FibonacciPriorityQueue<FpqInt> fpq, FpqInt oldValue, FpqInt newValue)
		{
			newValue.Cookie = oldValue.Cookie;
			fpq.DecreaseKey(oldValue.Cookie, newValue);
		}
	}
}
