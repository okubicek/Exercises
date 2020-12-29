using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc.Year2020.Day23
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 23;

		private RingList _ringList;

		private List<int> _input;

		public Solver()
		{
			_input = "193467258".Select(x => int.Parse(x.ToString())).ToList();			
		}

		public string SolveFirstTask()
		{
			_ringList = new RingList(_input);
			_ringList.MoveToNext();
			RunGame(100);

			return GetFirstTaskResult();
		}

		private void RunGame(int repeats)
		{
			for (int i = 1; i <= repeats; i++)
			{
				var pickup = _ringList.PickupElements(3);

				var destination = GetDestination(pickup.Select(x => x.Value));
				_ringList.InsertAfter(destination, pickup);
				_ringList.MoveToNext();
			}
		}

		public string GetFirstTaskResult()
		{
			_ringList.SetCurrent(1);
			var builder = new StringBuilder();
			do
			{
				builder.Append(_ringList.Current);
				_ringList.MoveToNext();				
			} while (_ringList.Current != 1);

			return builder.ToString().Remove(0, 1);
		}

		public string SolveSecondTask()
		{
			_input.AddRange(Enumerable.Range(_input.Max() + 1, 1000000 - _input.Count).ToList());
			
			_ringList = new RingList(_input);
			_ringList.MoveToNext();
			RunGame(10000000);

			_ringList.SetCurrent(1);		

			return $"{(long)_ringList.MoveToNext() * _ringList.MoveToNext()}";
		}

		private int GetDestination(IEnumerable<int> ringfencedItems)
		{
			var destination = _ringList.Current - 1;
			while (destination < _ringList.Min || ringfencedItems.Contains(destination))
			{
				destination = destination - 1 >= _ringList.Min ? destination - 1 : _ringList.Max;
			}

			return destination;
		}

		private class RingList
		{
			Dictionary<int, Item> _items = new Dictionary<int, Item>();

			private Item _current;

			public int Current => _current.Value;

			public int Max { get; }

			public int Min { get; }

			public RingList(IEnumerable<int> initValues)
			{
				foreach(var val in initValues)
				{
					Add(val);
				}

				Max = _items.Max(x => x.Key);
				Min = _items.Min(x => x.Key);
			}			

			public void Add(int value)
			{
				var newItem = Add(_current, value);
				_current = newItem;
			}

			public void InsertAfter(int label, IEnumerable<Item> values)
			{
				var attachPoint = _items[label];
				values.Last().Next = attachPoint.Next;
				attachPoint.Next = values.First();
			}

			public IEnumerable<Item> PickupElements(int count)
			{
				var pickedup = new List<Item>();
				for (int i = 0; i < count; i++)
				{
					var picked = _current.Next;
					_current.Next = picked.Next;				
					pickedup.Add(picked);					
				}

				return pickedup;
			}

			public void SetCurrent(int label)
			{
				_current = _items[label];
			}

			public int MoveToNext()
			{
				_current = _current.Next;

				return Current;
			}

			private Item Add(Item attachPoint, int value)
			{
				var newItem = new Item(value);

				if (attachPoint != null)
				{
					newItem.Next = attachPoint.Next;					
					attachPoint.Next = newItem;
				}

				_items.Add(value, newItem);

				return newItem;
			}

			public class Item
			{
				public Item(int value)
				{
					Value = value;
					Next = this;					
				}

				public int Value { get; }

				public Item Next { get; set; }				
			}
		}		
	}
}
