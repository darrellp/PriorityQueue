using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Priority_Queue
{
	public class FibonacciPriorityQueue<TPQ> : IEnumerable<TPQ> where TPQ : IComparable
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

		private static void RemoveFromList(IFibonacciQueueElement<TPQ> element)
		{
			if (IsSingletonOrUnattached(element))
			{
				// If we are not on a list or on a singleton list, there's nothing to do;
				return;
			}
			element.LeftSibling.RightSibling = element.RightSibling;
			element.RightSibling.LeftSibling = element.RightSibling;

			// Turn element into a singleton list
			element.LeftSibling = element.RightSibling = element;
			return;
		}

		private void Consolidate()
		{
			var degreeToRoot = new IFibonacciQueueElement<TPQ>[64];
			var rootList = EnumerateLinkedList(_min).ToList();

			foreach (var element in rootList)
			{
				var smallerRoot = element;
				var curDegree = element.Degree;
				while (degreeToRoot[curDegree] != null)
				{
					smallerRoot = element;
					var largerRoot = degreeToRoot[curDegree];
					if (smallerRoot.CompareTo(largerRoot) > 0)
					{
						smallerRoot = degreeToRoot[curDegree];
						largerRoot = element;
					}
					degreeToRoot[curDegree] = null;
					HeapLink(largerRoot, smallerRoot);
					curDegree++;
				}
				degreeToRoot[curDegree] = smallerRoot;
			}
			_min = null;
			foreach (var root in degreeToRoot.Where(elm => elm != null))
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

		private void HeapLink(IFibonacciQueueElement<TPQ> newChild, IFibonacciQueueElement<TPQ> newParent)
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
		}
		#endregion

		#region Priority Queue Operations
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Insert an IFibonacciQueueElement value into the priority queue.
		/// </summary>
		/// <remarks>	Darrellp, 2/17/2011.	</remarks>
		/// <param name="val">Value to insert.</param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		private void Add(IFibonacciQueueElement<TPQ> val)
		{
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
		///  Insert a value into the priority queue of type TPQ.
		/// </summary>
		/// <remarks>	Darrellp, 2/17/2011.	</remarks>
		/// <param name="attr">Value to insert.</param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public void Add(TPQ attr)
		{
			Add(new FibonacciElementWrapper<TPQ>(attr));
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
			return ExtractMin(out fNoMin);
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
