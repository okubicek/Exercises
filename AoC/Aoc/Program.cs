using System;

namespace Aoc
{
	public class Program
	{
		static void Main(string[] args)
		{
			var dayToRun = 10;
			var solver = new PuzzleSolverFactory().GetSolver(2020, dayToRun);

			SolveTask(solver, 1);
			SolveTask(solver, 2);
			Console.WriteLine(string.Empty);

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
