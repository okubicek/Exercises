using System;
using System.Collections.Generic;

namespace Aoc
{
	public class Program
	{
		static void Main(string[] args)
		{
			var solvers = new List<IPuzzleSolver> { new Year2019.Day1.Solver(),
				new Year2019.Day2.Solver(),
				new Year2019.Day3.Solver(),
			};

			foreach(var solver in solvers)
			{
				Console.WriteLine($"Day {solver.Day}");
				SolveTask(solver, 1);
				SolveTask(solver, 2);
				Console.WriteLine(string.Empty);
			}

			Console.Write("Please press any key ...");
			Console.Read();
		}

		private static void SolveTask(IPuzzleSolver solver, int taskNumber)
		{
			var result = taskNumber == 1 ? solver.SolveFirstTask() : solver.SolveSecondTask();

			if (!string.IsNullOrEmpty(result))
			{
				Console.WriteLine($"Task {taskNumber}: {result}");
			}
		}
	}
}
