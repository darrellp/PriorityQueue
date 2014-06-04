using System;

namespace Priority_Queue
{
	public interface IHasCookie
	{
		object Cookie { get; set; }
	}

	public class Fpqt<T> : IComparable, IHasCookie where T : IComparable
	{
		public object Cookie { get; set; }
		protected T Value { get; set; }

		protected Fpqt()
		{
			Value = default(T);
		}

		protected Fpqt(T value)
		{
			Value = value;
		}

		public Fpqt(Fpqt<T> oldValue)
		{
			Value = oldValue.Value;
			Cookie = oldValue.Cookie;
		}

		public static implicit operator Fpqt<T>(T value)
		{
			return new Fpqt<T>(value);
		}

		public static implicit operator T(Fpqt<T> value)
		{
			return value.Value;
		}

		public int CompareTo(object obj)
		{
			var other = obj as Fpqt<T>;
			if (other == null)
			{
				throw new ArgumentException("Non-FpqInt compared to FpqInt");
			}
			return Value.CompareTo(other.Value);
		}

		public override string ToString()
		{
			// ReSharper disable once SpecifyACultureInStringConversionExplicitly
			return Value.ToString();
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}

	public class FpqInt : Fpqt<int>
	{
		public FpqInt(int n) : base(n) {}

		public static implicit operator FpqInt(int value)
		{
			return new FpqInt(value);
		}

		public static implicit operator int(FpqInt value)
		{
			return value.Value;
		}
	}

	public class FpqDbl : Fpqt<double>
	{
		public FpqDbl(double n) : base(n) { }

		public static implicit operator FpqDbl(double value)
		{
			return new FpqDbl(value);
		}

		public static implicit operator double(FpqDbl value)
		{
			return value.Value;
		}
	}
}
