using Aoc.Year2019.OpCodeComputer;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Aoc.Year2019.Day7
{
	public class InputChannel : IInputChannel
	{
		private BlockingCollection<int> _queue;

		public InputChannel()
		{
			_queue = new BlockingCollection<int>();
		}

		public int GetNext()
		{
			return _queue.Take();
		}

		public void QueueInput(int input)
		{
			_queue.Add(input);
		}
	}
}
