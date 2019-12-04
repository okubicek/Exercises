using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.Day1
{
	public class Solver : IPuzzleSolver
	{
		private List<string> Input { get; }

		public Solver()
		{
			Input = InputFileReader.GetInput(@"Year2019/Inputs/Day1.txt");
		}

		public int Day => 1;

		public string SolveFirstTask()
		{
			var sum = Input.Select(x => CalculateFuel(int.Parse(x))).Sum();
			return sum.ToString();
		}

		public string SolveSecondTask()
		{
			var sum = Input.Select(x => CalculateTotalFuel(int.Parse(x))).Sum();
			return sum.ToString();
		}

		public int CalculateTotalFuel(int val)
		{
			var fuel = CalculateFuel(val);
			return fuel <= 0 ? 0 : fuel + CalculateTotalFuel(fuel);
		}

		private static int CalculateFuel(int val)
		{
			return (val / 3) - 2;
		}
	}
}
