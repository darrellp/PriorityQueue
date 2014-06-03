using System;
using System.Collections.Generic;
using System.Linq;

namespace Priority_Queue
{
	public static class FibonacciValidation<TPQ> where TPQ : IComparable
	{
		// ReSharper disable once StaticFieldInGenericType
		internal static int ElementCount;

		// Providing this as a place to set a breakpoint
		internal static bool False()
		{
			return false;
		}

		internal static bool IsParentValid(FibonacciElementWrapper<TPQ> parent)
		{
			if (parent == null)
			{
				return true;
			}
			ElementCount++;
			if (!IsLinkedListValid(parent.FirstChild))
			{
				return False();
			}

			return FibonacciPriorityQueue<TPQ>.EnumerateLinkedList(parent.FirstChild).All(IsParentValid) || False();
		}

		internal static bool IsLinkedListValid(FibonacciElementWrapper<TPQ> list)
		{
			if (list == null)
			{
				return true;
			}
			var cur = list;
			var nextSibling = list.RightSibling;

			var vals = new HashSet<FibonacciElementWrapper<TPQ>>();
			while (true)
			{
				if (vals.Contains(cur))
				{
					if (cur != list)
					{
						return False();
					}
					break;
				}
				vals.Add(cur);

				cur = nextSibling;
				if (nextSibling == null)
				{
					return False();
				}
				nextSibling = nextSibling.RightSibling;
			}
			var count = vals.Count;
			vals.Clear();
			cur = list;
			nextSibling = list.LeftSibling;
			while (true)
			{
				if (vals.Contains(cur))
				{
					if (cur != list)
					{
						return False();
					}
					break;
				}
				vals.Add(cur);

				cur = nextSibling;
				if (nextSibling == null)
				{
					return False();
				}
				nextSibling = nextSibling.LeftSibling;
			}
			return vals.Count == count;
		}
	}
}