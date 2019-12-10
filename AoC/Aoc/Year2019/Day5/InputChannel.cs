using Aoc.Year2019.OpCodeComputer;
using System.Collections.Generic;

namespace Aoc.Year2019.Day5
{
	public class InputChannel : IInputChannel
	{
		private Stack<int> _stack;

		public InputChannel(int value)
		{
			_stack = new Stack<int>(new[] { value });
		}

		public int GetNext()
		{
			return _stack.Pop();
		}
	}
}
