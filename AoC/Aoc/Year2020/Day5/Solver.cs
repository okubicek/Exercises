using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day5
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 5;

		private List<string> _input;

		public Solver()
		{
			_input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");
		}

		public string SolveFirstTask()
		{
			var minId = _input.Select(x => GetRowId(x)).Max();
			return $"{minId}";
		}

		private int GetRowId(string x)
		{
			return Count(x.Substring(0, 7), 128) * 8 + Count(x.Substring(7, 3), 8);
		}

		public string SolveSecondTask()
		{
			var seats = _input.Select(x => GetRowId(x)).ToHashSet();

			var max = _input.Select(x => GetRowId(x)).Max();
			for (int i = 1; i < max; i++)
			{
				if (!seats.Contains(i))
				{
					if(seats.Contains(i-1) && seats.Contains(i+1))
					{
						return $"{i}";
					}	
				}
			}

			throw new Exception("No solution found :-(");
		}

		public int Count(string seatCoordinates, int max)
		{
			var res = 0;
			for (int i = 0; i < seatCoordinates.Length; i++)
			{				
				if (seatCoordinates[i] == 'B' || seatCoordinates[i] == 'R')
				{
					res = res + max / (int)Math.Pow(2, i + 1);
				}
			}

			return res;
		}
	}
}
