using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc.Year2020.Day1
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 1;

		private const int SearchedSum = 2020;
		private List<int> Expenses { get; set; }

		public Solver()
		{
			Expenses = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt")
				.Select(e => int.Parse(e))
				.ToList();
		}

		public string SolveFirstTask()
		{
			var length = Expenses.Count;

			for(int i = 0; i < length - 1; i++)
			{
				for(int j = 1 + i; j < length; j++)
				{
					if (Expenses[i] + Expenses[j] == SearchedSum)
					{
						return $"{Expenses[i] * Expenses[j]}";
					}
				}
			}

			throw new ApplicationException("Not solved :-(");
		}

		public string SolveSecondTask()
		{
			var length = Expenses.Count;

			for (int i = 0; i < length - 2; i++)
			{
				for (int j = 1 + i; j < length - 1; j++)
				{
					for(int k = 1 + j; k < length; k++)
					{
						if (Expenses[i] + Expenses[j] + Expenses[k] == SearchedSum)
						{
							return $"{Expenses[i] * Expenses[j] * Expenses[k]}";
						}
					}					
				}
			}

			throw new ApplicationException("Not solved :-(");
		}
	}
}
