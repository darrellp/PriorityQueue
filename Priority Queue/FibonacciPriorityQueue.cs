using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Priority_Queue
{
	public class FibonacciPriorityQueue	<TPQ> : IEnumerable<TPQ> where TPQ : class, IFibonacciQueueElement<TPQ> 
	{
		#region Private Variables
		private IFibonacciQueueElement<TPQ> _min;
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
		static bool IsSingleTon(IFibonacciQueueElement<TPQ> element)
		{
			return ReferenceEquals(element, element.RightSibling);
		}

		static bool IsSingletonOrUnattached(IFibonacciQueueElement<TPQ> element)
		{
			return element.RightSibling == null || IsSingleTon(element);
		}

		static void MoveElement(IFibonacciQueueElement<TPQ> element, IFibonacciQueueElement<TPQ> to)
		{
			if (element == null)
			{
				throw new ArgumentException("element is null in MoveElement");
			}
			if (!IsSingletonOrUnattached(element))
			{
				// If it's already linked into another (non-singleton) list, unlink it
				element.LeftSibling.RightSibling = element.RightSibling;
				element.RightSibling.LeftSibling = element.LeftSibling;
			}
			if (to == null)
			{
				// If there's no list to link into, make element a singleton list
				element.LeftSibling = element.RightSibling = element;
			}
			else
			{
				// Standard link into non-null list
				element.LeftSibling = to;
				element.RightSibling = to.RightSibling;
				to.RightSibling.LeftSibling = element;
				to.RightSibling = element;
			}
		}

		static IFibonacciQueueElement<TPQ> CombineLists(IFibonacciQueueElement<TPQ> list1, IFibonacciQueueElement<TPQ> list2)
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
			return list1;
		}

		static IEnumerable<IFibonacciQueueElement<TPQ>> EnumerateLinkedList(IFibonacciQueueElement<TPQ> list)
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

		private static IFibonacciQueueElement<TPQ> RemoveFromList(IFibonacciQueueElement<TPQ> element)
		{
			if (IsSingletonOrUnattached(element))
			{
				// If we are not on a list or on a singleton list, there's nothing to do;
				return element;
			}
			element.LeftSibling.RightSibling = element.RightSibling;
			element.RightSibling.LeftSibling = element.RightSibling;
			element.LeftSibling = element.RightSibling = null;
			return element;
		}
		#endregion

		#region Priority Queue Operations
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Insert a value into the priority queue.
		/// </summary>
		/// <remarks>	Darrellp, 2/17/2011.	</remarks>
		/// <param name="val">Value to insert.</param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public void Add(TPQ val)
		{
			if (val == null)
			{
				throw new ArgumentNullException("null val in FibonacciPriorityQueue<TPQ>.Add()");
			}
			val.Degree = 0;
			val.FirstChild = null;
			val.Marked = false;
			val.Parent = null;
			if (_min == null)
			{
				_min = val;
				_min.LeftSibling = _min.RightSibling = _min;
			}
			else
			{
				MoveElement(val, _min);
				if (val.CompareTo(_min) < 0)
				{
					_min = val;
				}
			}
			Count++;
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

		public IFibonacciQueueElement<TPQ> ExtractMin()
		{
			var ret = _min;
			if (ret != null)
			{
				foreach (var child in EnumerateLinkedList(_min.FirstChild))
				{
					child.Parent = null;
				}
				CombineLists(_min, _min.FirstChild);
				if (IsSingleTon(_min))
				{
					_min = null;
				}
				else
				{
					_min = _min.RightSibling;
					Consolidate();
				}
				RemoveFromList(ret);
				Count--;
			}
			return ret;
		}

		private void Consolidate()
		{
			throw new NotImplementedException();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Peeks at the min element without deleting it.
		/// </summary>
		/// <remarks>	Darrellp - 6/1/14	</remarks>
		/// <returns>Smallest element in queue.</returns>
		/// <exception cref="System.IndexOutOfRangeException">Peeking at an empty priority queue</exception>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public IFibonacciQueueElement<TPQ> Peek()
		{
			// If there are no elements to peek
			if (_min == null)
			{
				// Throw an exception
				throw new IndexOutOfRangeException("Peeking at an empty priority queue");
			}

			return _min;
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
