using Aoc.Year2019.OpCodeComputer;
using System.Collections.Concurrent;

namespace Aoc.Year2019.Day7
{
	public class InputChannel : IInputChannel
	{
		private BlockingCollection<long> _queue;

		public InputChannel()
		{
			_queue = new BlockingCollection<long>();
		}

		public long GetNext()
		{
			return _queue.Take();
		}

		public void QueueInput(long input)
		{
			_queue.Add(input);
		}
	}
}
