using Aoc.Year2019.OpCodeComputer;

namespace Aoc.Year2019.Day7
{
	public class OutputChannel : IOutputChannel
	{
		private ThrusterOutput _output;

		public OutputChannel(ThrusterOutput output)
		{
			_output = output;
		}

		public void Send(long val)
		{
			_output.Value = val;
		}
	}
}
