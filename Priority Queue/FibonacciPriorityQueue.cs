using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Priority_Queue
{
	public class FibonacciPriorityQueue<TPQ> : IEnumerable<TPQ> where TPQ : IComparable
	{
		#region Private Variables
		private FibonacciElementWrapper<TPQ> _min;
		#endregion

		#region Properties
		///<summary>
		/// Count of items in the priority queue
		///</summary>
		public int Count { get; set; }
		#endregion

		#region Constructor
		public FibonacciPriorityQueue()
		{
			Count = 0;
		}
		#endregion

		#region Utility functions
		static bool IsSingleTon(FibonacciElementWrapper<TPQ> element)
		{
			return ReferenceEquals(element, element.RightSibling);
		}

		static bool IsSingletonOrUnattached(FibonacciElementWrapper<TPQ> element)
		{
			return element.RightSibling == null || IsSingleTon(element);
		}

		static FibonacciElementWrapper<TPQ> CombineLists(FibonacciElementWrapper<TPQ> list1, FibonacciElementWrapper<TPQ> list2)
		{
			if (list1 == null)
			{
				return list2;
			}
			if (list2 == null)
			{
				return list1;
			}

			var list1Next = list1.RightSibling;
			list1.RightSibling = list2.RightSibling;
			list2.RightSibling.LeftSibling = list1;
			list1Next.LeftSibling = list2;
			list2.RightSibling = list1Next;
			ThrowBadList(list1);
			return list1;
		}

		static IEnumerable<FibonacciElementWrapper<TPQ>> EnumerateLinkedList(FibonacciElementWrapper<TPQ> list)
		{
			if (list == null)
			{
				yield break;
			}
			var cur = list;
			var nextSibling = list.RightSibling;

			do
			{
				yield return cur;
				cur = nextSibling;
				if (nextSibling == null)
				{
					yield break;
				}
				nextSibling = nextSibling.RightSibling;
			} while (!ReferenceEquals(cur, list));
		}

		private static FibonacciElementWrapper<TPQ> RemoveFromList(FibonacciElementWrapper<TPQ> element)
		{
			if (IsSingletonOrUnattached(element))
			{
				// If we are not on a list or on a singleton list, there's nothing to do;
				return element;
			}
			var oldList = element.LeftSibling;
			element.LeftSibling.RightSibling = element.RightSibling;
			element.RightSibling.LeftSibling = element.LeftSibling;

			// Turn element into a singleton list
			element.LeftSibling = element.RightSibling = element;
			ThrowBadList(oldList);
			return element;
		}

		private void Consolidate()
		{
			var degreeToRoot = new FibonacciElementWrapper<TPQ>[64];
			var rootList = EnumerateLinkedList(_min).ToList();

			foreach (var element in rootList)
			{
				var smallerRoot = element;
				var curDegree = element.Degree;
				while (degreeToRoot[curDegree] != null)
				{
					var largerRoot = degreeToRoot[curDegree];
					if (smallerRoot.CompareTo(largerRoot) > 0)
					{
						var swapT = smallerRoot;
						smallerRoot = largerRoot;
						largerRoot = swapT;
					}
					HeapLink(largerRoot, smallerRoot);
					degreeToRoot[curDegree] = null;
					curDegree++;
				}
				degreeToRoot[curDegree] = smallerRoot;
			}
			_min = null;
			foreach (var root in degreeToRoot.Where(elm => elm != null).Select(RemoveFromList))
			{
				if (_min == null)
				{
					_min = root;
				}
				else
				{
					CombineLists(_min, root);
					if (root.Attr.CompareTo(_min.Attr) < 0)
					{
						_min = root;
					}
				}
			}
		}

		private void HeapLink(FibonacciElementWrapper<TPQ> newChild, FibonacciElementWrapper<TPQ> newParent)
		{
			RemoveFromList(newChild);
			if (newParent.FirstChild == null)
			{
				newParent.FirstChild = newChild;
			}
			else
			{
				CombineLists(newParent.FirstChild, newChild);
			}
			newParent.Degree++;
			newChild.Marked = false;
			newChild.Parent = newParent;
		}

		[Conditional("DEBUG")]
		private static void ThrowBadList(FibonacciElementWrapper<TPQ> list)
		{
			if (!IsLinkedListValid(list))
			{
				throw new InvalidOperationException("Bad list");
			}
		}

		[Conditional("DEBUG")]
		private static void ThrowBadFpq(FibonacciPriorityQueue<TPQ> fpq)
		{
			if (!fpq.Validate())
			{
				throw new InvalidOperationException("Bad Fibonacci Priority Queue");
			}
		}

		// Providing this as a place to set a breakpoint
		static bool False()
		{
			return false;
		}

		private static bool IsParentValid(FibonacciElementWrapper<TPQ> parent)
		{
			if (parent == null)
			{
				return true;
			}
			_elementCount++;
			if (!IsLinkedListValid(parent.FirstChild))
			{
				return False();
			}

			return EnumerateLinkedList(parent.FirstChild).All(IsParentValid) || False();
		}

		// ReSharper disable once StaticFieldInGenericType
		private static int _elementCount;

		public bool Validate()
		{
			_elementCount = 0;
			if (!IsLinkedListValid(_min))
			{
				return False();
			}
			if (EnumerateLinkedList(_min).Any(parent => !IsParentValid(parent)))
			{
				return False();
			}
			if (_elementCount != Count)
			{
				return False();
			}
			return true;
		}

		private static bool IsLinkedListValid(FibonacciElementWrapper<TPQ> list)
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
		#endregion

		#region Priority Queue Operations
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Insert an FibonacciElementWrapper value into the priority queue.
		/// </summary>
		/// <remarks>	Darrellp, 2/17/2011.	</remarks>
		/// <param name="val">Value to insert.</param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		private void Add(FibonacciElementWrapper<TPQ> val)
		{
			if (_min == null)
			{
				_min = val;
				_min.LeftSibling = _min.RightSibling = _min;
			}
			else
			{
				//MoveElement(val, _min);
				CombineLists(_min, val);
				if (val.CompareTo(_min) < 0)
				{
					_min = val;
				}
			}
			Count++;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Insert a value into the priority queue of type TPQ.
		/// </summary>
		/// <remarks>	Darrellp, 2/17/2011.	</remarks>
		/// <param name="attr">Value to insert.</param>
		/// <returns>Cookie to use to reference the object later</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public Object Add(TPQ attr)
		{
			var wrapper = new FibonacciElementWrapper<TPQ>(attr);
			Add(wrapper);
			return wrapper;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Unions the specified heap with our heap and returns the result.
		/// </summary>
		/// <remarks>	Both original heaps are destroyed	</remarks>
		/// <param name="heap">The heap to be unioned in.</param>
		/// <returns>Union of the two heaps</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public FibonacciPriorityQueue<TPQ> Union(FibonacciPriorityQueue<TPQ> heap)
		{
			var ret = new FibonacciPriorityQueue<TPQ>();
			var ourMin = _min;
			var theirMin = heap._min;

			ret._min = CombineLists(_min, heap._min);
			if (ourMin != null && theirMin != null && ourMin.CompareTo(theirMin) > 0)
			{
				ret._min = theirMin;
			}
			ret.Count = Count + heap.Count;
			return ret;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Extracts and deletes the minimum value from the priority queue.
		/// </summary>
		/// <remarks>	Darrellp - 6/1/14	</remarks>
		/// <param name="fNoMin">No current minimum if set to <c>true</c>.</param>
		/// <returns> Minimum value of type TPQ.</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public TPQ ExtractMin(out bool fNoMin)
		{
			var ret = _min;
			if (_min == null)
			{
				fNoMin = true;
				return default(TPQ);
			}

			fNoMin = false;
			foreach (var child in EnumerateLinkedList(_min.FirstChild))
			{
				child.Parent = null;
			}
			CombineLists(_min, _min.FirstChild);
			var newMin = _min.RightSibling;
			var fSingletonMin = IsSingleTon(_min);
			RemoveFromList(_min);
			if (fSingletonMin)
			{
				_min = null;
			}
			else
			{
				_min = newMin;
				Consolidate();
			}
			Count--;
			return ret == null? default(TPQ) : ret.Attr;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Extracts and deletes the minimum value from the priority queue.
		/// </summary>
		/// <remarks>	Darrellp - 6/1/14	</remarks>
		/// <returns> Minimum value of type TPQ - TPQ's default if there is no current minimum.</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public TPQ ExtractMin()
		{
			bool fNoMin;
			var ret = ExtractMin(out fNoMin);
			ThrowBadFpq(this);
			return ret;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Peeks at the min element without deleting it.
		/// </summary>
		/// <remarks>	Darrellp - 6/1/14	</remarks>
		/// <param name="fNoMin">No minimum if set to <c>true</c>.</param>
		/// <returns>Smallest element in queue or default(TPQ) if no smallest element.</returns>
		/// <exception cref="System.IndexOutOfRangeException">Peeking at an empty priority queue</exception>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public TPQ Peek(out bool fNoMin)
		{
			fNoMin = _min == null;
			return fNoMin ? default(TPQ) : _min.Attr;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Peeks at the min element without deleting it.
		/// </summary>
		/// <remarks> This is just a convenience routine  - Darrellp - 6/1/14	</remarks>
		/// <returns>Smallest element in queue or default(TPQ) if no smallest element.</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public TPQ Peek()
		{
			bool fNoMin;
			return Peek(out fNoMin);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Decreases the key for an element.
		/// </summary>
		/// <remarks>	Darrellp - 6/3/14	</remarks>
		/// <param name="xObj">The cookie representing the element.</param>
		/// <param name="newValue">The new smaller value.</param>
		/// <exception cref="System.ArgumentException">
		/// DecreaseKey recieved invalid cookie
		/// or
		/// Key value passed to DecreaseKey greater than current value
		/// </exception>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public void DecreaseKey(object xObj, TPQ newValue)
		{
			var element = xObj as FibonacciElementWrapper<TPQ>;
			if (element == null)
			{
				throw new ArgumentException("DecreaseKey recieved invalid cookie");
			}
			if (element.Attr.CompareTo(newValue) == 0)
			{
				return;
			}
			if (element.Attr.CompareTo(newValue) < 0)
			{
				throw new ArgumentException("Key value passed to DecreaseKey greater than current value");
			}
			element.Attr = newValue;
			var parent = element.Parent;
			if (parent != null && parent.Attr.CompareTo(newValue) > 0)
			{
				Cut(element, parent);
				CascadingCut(parent);
			}
			if (element.Attr.CompareTo(_min.Attr) < 0)
			{
				_min = element;
			}
		}

		private void CascadingCut(FibonacciElementWrapper<TPQ> element)
		{
			var parent = element.Parent;
			if (parent != null)
			{
				if (!element.Marked)
				{
					element.Marked = true;
				}
				else
				{
					Cut(element, parent);
					CascadingCut(parent);
				}
			}
		}

		private void Cut(FibonacciElementWrapper<TPQ> element, FibonacciElementWrapper<TPQ> parent)
		{
			if (ReferenceEquals(parent.FirstChild, element))
			{
				parent.FirstChild = IsSingleTon(element) ? null : element.RightSibling;
			}
			element = RemoveFromList(element);
			parent.Degree--;
			CombineLists(_min, element);
			element.Parent = null;
			element.Marked = false;
		}
		#endregion

		#region IEnumerable members
		public IEnumerator<TPQ> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}
}
