﻿using System;
using System.Collections.Generic;

namespace Priority_Queue
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	A priority queue implemented as an array.  This is a pretty standard implementation. </summary>
	///
	/// <remarks>	Darrellp, 2/17/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public class BinaryPriorityQueue<TPQ> : IEnumerable<TPQ> where TPQ : IComparable
	{
		#region Private Variables
		/// <summary>
		/// Array to keep elements in.  C# lists are actually implemented as arrays.  This is contrary
		/// to everything I learned about the terms "array" and "list", but that's nonetheless the way
		/// they're implemented in the CLR Framework.
		/// </summary>
		protected readonly List<TPQ> LstHeap = new List<TPQ>();
		#endregion

		#region Properties
		///<summary>
		/// Count of items in the priority queue
		///</summary>
		public int Count
		{
			get { return LstHeap.Count; }
		}
		#endregion

		#region Public methods

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Insert a value into the priority queue. </summary>
		///
		/// <remarks>	Darrellp, 2/17/2011. </remarks>
		///
		/// <param name="val">	Value to insert. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public void Add(TPQ val)
		{
			// Add the new element to the end of the list
			LstHeap.Add(val);
			SetAt(LstHeap.Count - 1, val);

			// Move it up the tree to it's correct position
			UpHeap(LstHeap.Count - 1);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Return the maximal element in the queue. </summary>
		///
		/// <remarks>	Darrellp, 2/17/2011. </remarks>
		///
		/// <exception cref="IndexOutOfRangeException">	Trying to peek at an empty priority queue. </exception>
		///
		/// <returns>	Maximal element in the queue. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public TPQ Peek()
		{
			// If there are no elements to peek
			if (LstHeap.Count == 0)
			{
				// Throw and exception
				throw new IndexOutOfRangeException("Peeking at an empty priority queue");
			}

			// Otherwise, the root is our largest element
			// The 0'th element in the list is always the root
			return LstHeap[0];
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Remove and return the maximal element in the queue. </summary>
		///
		/// <remarks>	Darrellp, 2/17/2011. </remarks>
		///
		/// <exception cref="IndexOutOfRangeException">	Thrown when the priority queue is empty. </exception>
		///
		/// <returns>	Maximal element. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public virtual TPQ Pop()
		{
			// If There's nothing to pop
			if (LstHeap.Count == 0)
			{
				// Throw an exception
				throw new IndexOutOfRangeException("Popping an empty priority queue");
			}

			// Save away the max value in the heap
			var valRet = LstHeap[0];

			// Move the last element in the list to the now vacated first
			// Yea, and I sayeth unto you, the last shall be first...
			SetAt(0, LstHeap[LstHeap.Count - 1]);

			// Drop the now redundant last item
			LstHeap.RemoveAt(LstHeap.Count - 1);

			// Move the top item down the tree to its proper position
			DownHeap(0);

			// Return the element we removed
			return valRet;
		}
		#endregion

		#region Virtual methods

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Sets an element in the list we keep our heap elements in </summary>
		///
		/// <remarks>
		/// This is the only way elements should be inserted into LstHeap.  This ensures, among other things,
		/// that the elements in a queue with deletions always have their held indices up to date.
		/// Darrellp, 2/17/2011. </remarks>
		///
		/// <param name="i">	The index into LstHeap. </param>
		/// <param name="val">	The value to be set. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected virtual void SetAt(int i, TPQ val)
		{
			LstHeap[i] = val;
		}
		#endregion

		#region Heapifying

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Queries if a given right son exists. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	The index of the parent. </param>
		///
		/// <returns>	true if the right son exists. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected bool RightSonExists(int i)
		{
			return RightChildIndex(i) < LstHeap.Count;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Queries if a given left son exists. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	The index of the parent. </param>
		///
		/// <returns>	true if the left son exists. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected bool LeftSonExists(int i)
		{
			return LeftChildIndex(i) < LstHeap.Count;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Index of parent node. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	Child's index. </param>
		///
		/// <returns>	Index to the parent. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected int ParentIndex(int i)
		{
			return (i - 1) / 2;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Index of left child's node. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	Parent's index. </param>
		///
		/// <returns>	Index to the left child. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected int LeftChildIndex(int i)
		{
			return 2 * i + 1;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Index of right child's node. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	Parent's index. </param>
		///
		/// <returns>	Index to the right child. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected int RightChildIndex(int i)
		{
			return 2 * (i + 1);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Array value at index i. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	The index. </param>
		///
		/// <returns>	Array value at i. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected TPQ ArrayVal(int i)
		{
			return LstHeap[i];
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Returns the parent. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	The index of the child. </param>
		///
		/// <returns>	The parent. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected TPQ Parent(int i)
		{
			return LstHeap[ParentIndex(i)];
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Returns the left child. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	The index of the parent. </param>
		///
		/// <returns>	The left child. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected TPQ Left(int i)
		{
			return LstHeap[LeftChildIndex(i)];
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Returns the right child. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	The index of the parent. </param>
		///
		/// <returns>	The right child. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected TPQ Right(int i)
		{
			return LstHeap[RightChildIndex(i)];
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Swaps two elements of the priority queue. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <param name="i">	The index of the first element. </param>
		/// <param name="j">	The index of the second element. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected void Swap(int i, int j)
		{
			var valHold = ArrayVal(i);
			SetAt(i, LstHeap[j]);
			SetAt(j, valHold);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Move an element up the heap to it's proper position </summary>
		///
		/// <remarks>	Darrellp, 2/17/2011. </remarks>
		///
		/// <param name="i">	The index of the element to move. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected void UpHeap(int i)
		{
			// While we're not the root and our parent is smaller than we are
			while (i > 0 && ArrayVal(i).CompareTo(Parent(i)) > 0)
			{
				// Swap us with our parents
				Swap(i, ParentIndex(i));
				i = ParentIndex(i);
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Move an element down the heap to it's proper position. </summary>
		///
		/// <remarks>	Darrellp, 2/17/2011. </remarks>
		///
		/// <param name="i">	The index of the element to move. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected void DownHeap(int i)
		{
			// Until we've reached our final position
			while (i >= 0)
			{
				// Initialize
				var iContinue = -1;

				// If we have a right son and he is larger than us
				if (RightSonExists(i) && Right(i).CompareTo(ArrayVal(i)) > 0)
				{
					// Arrange to swap us with the larger of our two children
					iContinue = Left(i).CompareTo(Right(i)) < 0 ? RightChildIndex(i) : LeftChildIndex(i);
				}
				// Else if we have a left son and he is larger than us
				else if (LeftSonExists(i) && Left(i).CompareTo(ArrayVal(i)) > 0)
				{
					// Arrange to swap with him
					iContinue = LeftChildIndex(i);
				}

				// If we found a node to swap with
				if (iContinue >= 0 && iContinue < LstHeap.Count)
				{
					// Make the swap
					Swap(i, iContinue);
				}

				// Continue on down the tree if we made a swap
				i = iContinue;
			}
		}
		#endregion

		#region Validation
		public virtual bool FValidate()
		{
			return LstHeap.Count == 0 || FValidate(0);
		}

		bool FValidate(int iRoot)
		{
			var valRoot = LstHeap[iRoot];

			if (LeftSonExists(iRoot))
			{
				if (valRoot.CompareTo(Left(iRoot)) < 0)
				{
					return false;
				}
				if (!FValidate(LeftChildIndex(iRoot)))
				{
					return false;
				}
			}
			if (RightSonExists(iRoot))
			{
				if (valRoot.CompareTo(Right(iRoot)) < 0)
				{
					return false;
				}
				if (!FValidate(LeftChildIndex(iRoot)))
				{
					return false;
				}
			}
			return true;
		}
		#endregion

		#region IEnumerable<T> Members

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Gets an enumerator for the items in the queue. </summary>
		///
		/// <remarks>	Darrellp, 2/21/2011. </remarks>
		///
		/// <returns>	The enumerator. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected IEnumerator<TPQ> GetEnumerator()
		{
			return LstHeap.GetEnumerator();
		}

		IEnumerator<TPQ> IEnumerable<TPQ>.GetEnumerator()
		{
			return GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
