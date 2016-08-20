using System;

namespace Priority_Queue
{
    /// <summary>
    /// The IHasCookie is very similar to the IPriorityQueueDeletionElement interface.  The
    /// main difference is that IHasCookie isn't comparable and it stores an object rather than
    /// an integer.  It's used in our typed objects to store the properly typed 
    /// </summary>
	internal interface IHasCookie
	{
        IBinaryQueueDeletionElement Cookie { get; set; }
	}

    /// <summary>
    /// Class Pqt for typed arguments to the Fibonacci queue
    /// </summary>
    /// <remarks>
    /// The priority queues are great as stands but require the user to keep track of
    /// cookies if they want to later delete or reduce the key of a value.  Often we make space in
    /// the base type to hold the cookie but this is bad on two counts.  It means a separate
    /// class from the base attribute class and it also means manipulating the returned
    /// values so they're stored and retrieved properly.  This is especially bad in base cases
    /// such as ints/doubles because now we can no longer work directly with ints and doubles but
    /// must invent new temporary classes to hold both the cookie and the value of the int/double.
    /// 
    /// Enter Pqt which is implicit conversion to and from the actual value type so that
    /// for the most part we can work as though we're working with the original types (at the
    /// expense of having a lot of implicit conversion going on).  Also, they hold the cookie
    /// in themselves to no new class is required for each type.  For the most part you can treat
    /// the Pqt types as their generic T types.  You can retrieve/save the cookies manually but
    /// there are a series of methods in FibonacciPriortyQueue in "Typed Access" region that
    /// take care of most of this for you.
    /// 
    /// One thing that has to be considered is that in the test code which generally just takes
    /// objects as arguments, an operation such as:
    ///		Assert.AreEqual(10, fpq.Pop());
    /// will probably fail because 10 is an integer and nothing about the type of the second
    /// argument of AreEqual will coerce it to convert the BaseType returned from Pop() to an integer.
    /// The proper thing to do here is just
    ///		Assert.AreEqual(10, (int)fpq.Pop());
    /// </remarks>
    /// <typeparam name="T">Type of values being stored</typeparam>
    public class Pqt<T> : IComparable, IHasCookie where T : IComparable
	{
		public IBinaryQueueDeletionElement Cookie { get; set; }
		protected T Value { get; set; }

		protected Pqt()
		{
			Value = default(T);
		}

		protected Pqt(T value)
		{
			Value = value;
		}

		public Pqt(Pqt<T> oldValue)
		{
			Value = oldValue.Value;
			Cookie = oldValue.Cookie;
		}

		public static implicit operator Pqt<T>(T value)
		{
			return new Pqt<T>(value);
		}

		public static implicit operator T(Pqt<T> value)
		{
			return value.Value;
		}

		public int CompareTo(object obj)
		{
			var other = obj as Pqt<T>;
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
}
