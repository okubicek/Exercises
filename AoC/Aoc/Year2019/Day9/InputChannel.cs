using Aoc.Year2019.OpCodeComputer;
using System.Collections.Generic;

namespace Aoc.Year2019.Day9
{
	public class InputChannel : IInputChannel
	{
		private Stack<long> _stack;

		public InputChannel(long value)
		{
			_stack = new Stack<long>(new[] { value });
		}

		public long GetNext()
		{
			return _stack.Pop();
		}
	}
}
