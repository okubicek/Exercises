using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Year2020.Day9
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 9;

		private List<long> _numbers;

		public Solver()
		{
			_numbers = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt")
				.Select(i => long.Parse(i))
				.ToList();

		}

		public string SolveFirstTask()
		{
			return $"{FindOutlier().value}";
		}

		public string SolveSecondTask()
		{
			var (maxIndex, sum) = FindOutlier();

			for (int i = 0; i < maxIndex; i++)
			{
				for (int j = maxIndex - i; j > 0; j--)
				{
					var range = _numbers.GetRange(i, j);
					if (range.Sum() == sum)
					{
						return $"{range.Min() + range.Max()}";
					}
				}
			}

			throw new Exception("Solution not found :-(");
		}

		private (int index, long value) FindOutlier()
		{
			var start = 25;
			for (int i = start; i < _numbers.Count; i++)
			{
				var range = _numbers.GetRange(i - start, start);

				if (!FindMatch(range, _numbers[i]))
				{
					return (i, _numbers[i]);
				}
			}

			throw new Exception("Solution not found :-(");
		}		

		private bool FindMatch(List<long> toScan, long searchedSum)
		{
			var length = toScan.Count;			

			for (int i = 0; i < length - 1; i++)
			{
				for (int j = 1 + i; j < length; j++)
				{
					if (toScan[i] + toScan[j] == searchedSum)
					{
						return true;
					}
				}				
			}

			return false;
		}		
	}
}
