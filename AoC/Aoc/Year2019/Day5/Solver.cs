using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.Day5
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 5;

		private List<int> InputInstructions;

		public Solver()
		{
			Initialize();
		}

		public string SolveFirstTask()
		{
			Initialize();

			var phase = 5;
			var computer = new OpCodeComputer.OpCodeComputer(
					new InputChannel(phase),
					new OutputChannel()
				);

			computer.ProcessInstructions(InputInstructions);
			
			return string.Empty;
		}

		private void Initialize()
		{
			InputInstructions = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0]
				.Split(',')
				.Select(x => int.Parse(x))
				.ToList();
		}

		

		public string SolveSecondTask()
		{
			return string.Empty;
		}
	}
}
