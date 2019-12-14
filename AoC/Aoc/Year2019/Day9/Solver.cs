using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.Day9
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 9;

		private List<long> _program;

		public Solver()
		{
			_program = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0]
				.Split(',')
				.Select(x => long.Parse(x))
				.ToList();
		}

		public string SolveFirstTask()
		{
			var input = 1;

			return RunBoostProgram(input);
		}

		private string RunBoostProgram(int input)
		{
			var output = new OutputChannel();
			var computer = new OpCodeComputer.OpCodeComputer(new InputChannel(input), output);
			computer.ProcessInstructions(_program);

			return string.Join(',', output.Outputs);
		}

		public string SolveSecondTask()
		{
			var input = 2;

			return RunBoostProgram(input);
		}
	}
}
