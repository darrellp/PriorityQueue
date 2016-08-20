using System;

namespace Priority_Queue
{
    public class FibonacciTypedQueue<BaseType> : FibonacciPriorityQueue<Pqt<BaseType>> where BaseType : IComparable
    {
        public FibonacciTypedQueue(Func<Pqt<BaseType>, Pqt<BaseType>, int> compare = null) : base(compare) {}

        public new Pqt<BaseType> Add(Pqt<BaseType> value)
        {
            value.Cookie = base.Add(value);
            return value;
        }

        public void Delete(Pqt<BaseType> value)
        {
            Delete(value.Cookie);
        }

        public void DecreaseKey(Pqt<BaseType> oldValue, Pqt<BaseType> newValue)
        {
            newValue.Cookie = oldValue.Cookie;
            base.DecreaseKey(oldValue.Cookie, newValue);
        }

    }
}