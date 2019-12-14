using Aoc.Year2019.OpCodeComputer;

namespace Aoc.Year2019.Day7
{
	public class LinkedOutputChannel : IOutputChannel
	{
		private InputChannel _input;

		public LinkedOutputChannel(InputChannel input)
		{
			_input = input;
		}

		public void Send(long val)
		{
			_input.QueueInput(val);
		}
	}
}
