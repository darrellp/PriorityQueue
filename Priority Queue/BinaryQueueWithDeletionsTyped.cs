using System;
using System.Linq;

namespace Priority_Queue
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>	Priority queue with deletions. </summary>
    ///
    /// <remarks>
    /// This is the same as the priority queue but the elements stored in it must satisfy the
    /// IPriorityQueueElement interface.  This allows us to store and retrieve our local indices in
    /// the objects so when we go to delete them we can query them for their index and then remove
    /// the element at that index.
    /// 	
    /// Darrellp, 2/17/2011. 
    /// </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class BinaryQueueWithDeletions<BaseType> : BinaryPriorityQueue<BaseType> where BaseType : class, IComparable, IBinaryQueueDeletionElement
    {
        #region Public overrides

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>	Removes and returns the largest object. </summary>
        ///
        /// <remarks>	Darrellp, 2/19/2011. </remarks>
        ///
        /// <returns>	The previous largest object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public new BaseType Pop(out bool fNoMin)
        {
            var valRet = base.Pop(out fNoMin);

            // When an element is removed from the heap, it's index must be reset.
            valRet.Index = -1;
            return fNoMin ? default(BaseType) : valRet;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Pops smallest element from the stack.
        /// </summary>
        /// <remarks>	Darrellp - 6/4/14	</remarks>
        /// <returns>Smallest element</returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public new BaseType Pop()
        {
            bool fNoMin;
            return Pop(out fNoMin);
        }

        public new BaseType Peek(out bool fNoMin)
        {
            var valRet = base.Peek(out fNoMin);
            return fNoMin ? default(BaseType) : valRet;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Peeks at the smallest element without removing it.
        /// </summary>
        /// <remarks>	Darrellp - 6/4/14	</remarks>
        /// <returns>   Smallest element or default(TPQ) if no smallest element.</returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public new BaseType Peek()
        {
            bool fNoMin;
            return Peek(out fNoMin);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ///  <summary>	Delete a value from the heap. </summary>
        /// 
        ///  <remarks>	Darrellp, 2/17/2011. </remarks>
        /// 
        /// <param name="valObj">Cookie for object to be deleted</param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Delete(Object valObj)
        {
            var val = valObj as BaseType;

            if (val == null)
            {
                throw new ArgumentException("Invalid type passed to Delete");
            }
            // Retrieve the item's index
            var i = val.Index;

            // Does it have a valid index?
            if (i >= 0)
            {
                // Reset the index on the item
                val.Index = -1;

                // Swap the item to be deleted for the last element in the array
                Swap(i, LstHeap.Count - 1);

                // Drop the last element of the array
                // This is the element we just placed there - i.e., the one to be deleted
                LstHeap.RemoveAt(LstHeap.Count - 1);

                // Do we need to make further adjustments
                // If this was already the last element of the array, we're done.  If
                // not then we have to make further adjustments
                if (i < LstHeap.Count)
                {
                    // Are we larger than our parent?
                    if (i != 0 && LstHeap[i].CompareTo(Parent(i)) < 0)
                    {
                        // Move us up the tree
                        UpHeap(i);
                    }
                    else
                    {
                        // Move us down the tree
                        DownHeap(i);
                    }
                }
            }
        }
        #endregion

        #region Private overrides
        public override bool FValidate()
        {
            return !LstHeap.Where((t, iVal) => t.Index != iVal).Any() && base.FValidate();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>	
        /// This override is the magic that makes the deletions work by keeping track of the index a
        /// particular element is moved to. 
        /// </summary>
        ///
        /// <remarks>	Darrellp, 2/17/2011. </remarks>
        ///
        /// <param name="i">	The index. </param>
        /// <param name="val">	The value. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        protected override void SetAt(int i, BaseType val)
        {
            base.SetAt(i, val);
            val.Index = i;
        }
        #endregion

    }

    public class BinaryQueueWithDeletionsTyped<BaseType> : BinaryPriorityQueue<BinaryWrapper<BaseType>> where BaseType : IComparable
	{
		#region Public overrides

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Removes and returns the largest object. </summary>
		///
		/// <remarks>	Darrellp, 2/19/2011. </remarks>
		///
		/// <returns>	The previous largest object. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public new BaseType Pop(out bool fNoMin)
		{
			var valRet = base.Pop(out fNoMin);

			// When an element is removed from the heap, it's index must be reset.
			valRet.Index = -1;
			return fNoMin ? default(BaseType) : valRet.Attr;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Pops smallest element from the stack.
		/// </summary>
		/// <remarks>	Darrellp - 6/4/14	</remarks>
		/// <returns>Smallest element</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public new BaseType Pop()
		{
			bool fNoMin;
			return Pop(out fNoMin);
		}

		public new BaseType Peek(out bool fNoMin)
		{
			var valRet = base.Peek(out fNoMin);
			return fNoMin ? default(BaseType) : valRet.Attr;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Peeks at the smallest element without removing it.
		/// </summary>
		/// <remarks>	Darrellp - 6/4/14	</remarks>
		/// <returns>Smallest element or default(TPQ) if no smallest element.</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public new BaseType Peek()
		{
			bool fNoMin;
			return Peek(out fNoMin);
		}

		public object Add(BaseType val)
		{
			var wrapper = new BinaryWrapper<BaseType>(val);
			Add(wrapper);
			return wrapper;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		///  <summary>	Delete a value from the heap. </summary>
		/// 
		///  <remarks>	Darrellp, 2/17/2011. </remarks>
		/// 
		/// <param name="valObj">Cookie for object to be deleted</param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public void Delete(Object valObj)
		{
		    var val = valObj as BinaryWrapper<BaseType>;

            if (val == null)
            {
                throw new ArgumentException("Invalid type passed to Delete");
            }
            // Retrieve the item's index
            var i = val.Index;

			// Does it have a valid index?
			if (i >= 0)
			{
				// Reset the index on the item
				val.Index = -1;

				// Swap the item to be deleted for the last element in the array
				Swap(i, LstHeap.Count - 1);

				// Drop the last element of the array
				// This is the element we just placed there - i.e., the one to be deleted
				LstHeap.RemoveAt(LstHeap.Count - 1);

				// Do we need to make further adjustments
				// If this was already the last element of the array, we're done.  If
				// not then we have to make further adjustments
				if (i < LstHeap.Count)
				{
					// Are we larger than our parent?
					if (i != 0 && LstHeap[i].CompareTo(Parent(i)) < 0)
					{
						// Move us up the tree
						UpHeap(i);
					}
					else
					{
						// Move us down the tree
						DownHeap(i);
					}
				}
			}
		}
		#endregion

		#region Private overrides
		public override bool FValidate()
		{
			return !LstHeap.Where((t, iVal) => t.Index != iVal).Any() && base.FValidate();
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	
		/// This override is the magic that makes the deletions work by keeping track of the index a
		/// particular element is moved to. 
		/// </summary>
		///
		/// <remarks>	Darrellp, 2/17/2011. </remarks>
		///
		/// <param name="i">	The index. </param>
		/// <param name="val">	The value. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		protected override void SetAt(int i, BinaryWrapper<BaseType> val)
		{
			base.SetAt(i, val);
			val.Index = i;
		}
		#endregion

		#region Typed Input
		// See fpqt.cs for more info on typed input

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Adds a typed input value.
		/// </summary>
		/// <remarks>	Darrellp - 6/4/14	</remarks>
		/// <param name="n">The typed input to add.</param>
		/// <returns>Typed input with cookie properly set.</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public BaseType AddTyped(BaseType n)
		{
			((IHasCookie)n).Cookie = (IBinaryQueueDeletionElement)Add(n);
			return n;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Deletes the typed input value.
		/// </summary>
		/// <remarks>	Darrellp - 6/4/14	</remarks>
		/// <param name="value">The value to be deleted.</param>
		////////////////////////////////////////////////////////////////////////////////////////////////////
		public void DeleteTyped(BaseType value)
		{
			Delete(((IHasCookie)value).Cookie);
			// Cookie is no longer valid
			((IHasCookie) value).Cookie = null;
		}
		#endregion

	}
}
