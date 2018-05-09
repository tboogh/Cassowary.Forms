using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Cassoway.Forms.Layout
{
	public sealed class ConstraintCollection : ConstraintCollection<CassowaryConstraint>
	{
		
	}
	
	public class ConstraintCollection<T> : IList<T> where T : IConstraint
	{
		readonly List<T> _internalList = new List<T>();

		internal ConstraintCollection()
		{
		}

		public void Add(T item)
		{
			_internalList.Add(item);
			item.ItemChanged += OnItemChanged;
			OnItemChanged(this, EventArgs.Empty);
		}

		public void Clear()
		{
			foreach (T item in _internalList)
				item.ItemChanged -= OnItemChanged;
			_internalList.Clear();
			OnItemChanged(this, EventArgs.Empty);
		}

		public bool Contains(T item)
		{
			return _internalList.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_internalList.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _internalList.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			item.ItemChanged -= OnItemChanged;
			bool success = _internalList.Remove(item);
			if (success)
				OnItemChanged(this, EventArgs.Empty);
			return success;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		public int IndexOf(T item)
		{
			return _internalList.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			_internalList.Insert(index, item);
			item.ItemChanged += OnItemChanged;
			OnItemChanged(this, EventArgs.Empty);
		}

		public T this[int index]
		{
			get { return _internalList[index]; }
			set
			{
				if(index < _internalList.Count && index >= 0 && _internalList[index] != null)
					_internalList[index].ItemChanged -= OnItemChanged;

				_internalList[index] = value;
				value.ItemChanged += OnItemChanged;
				OnItemChanged(this, EventArgs.Empty);
			}
		}

		public void RemoveAt(int index)
		{
			T item = _internalList[index];
			_internalList.RemoveAt(index);
			item.ItemChanged -= OnItemChanged;
			OnItemChanged(this, EventArgs.Empty);
		}

		public event EventHandler ItemChanged;

		void OnItemChanged(object sender, EventArgs e)
		{
			EventHandler eh = ItemChanged;
			eh?.Invoke(this, EventArgs.Empty);
		}
	}
}