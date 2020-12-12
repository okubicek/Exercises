using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day10
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 10;

		private List<int> _joltages;		

		private List<List<int>>  _diffList = new List<List<int>> { new List<int>(), new List<int>(), new List<int>() };

		public Solver()
		{
			_joltages = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt")
				.Select(i => int.Parse(i))
				.OrderBy(i => i)
				.ToList();
		}

		public string SolveFirstTask()
		{
			_joltages.Add(_joltages.Last() + 3);
			_joltages.Insert(0, 0);
			
			for(int i = 0; i < _joltages.Count() - 1; i++)
			{
				var diff = _joltages[i + 1] - _joltages[i];
				_diffList[diff-1].Add(i);
			}

			return $"{_diffList[0].Count() * _diffList[2].Count()}";
		}

		public string SolveSecondTask()
		{
			long variants = 1;
			var previousNode = -1;

			foreach(int i in _diffList[2])
			{
				variants *= GetSubVariants(_joltages.GetRange(previousNode + 2, i - (previousNode + 1)), _joltages[previousNode + 1]);
				previousNode = i;
			};

			return $"{variants}";
		}

		private int GetSubVariants(List<int> joltages, int joltage)
		{
			if (joltages.Count < 2)
			{
				return 1;
			}

			var subVariantCount = 0;
			for(var i = 0; i < joltages.Count && joltages[i] <= (joltage + 3); i++)			
			{
				subVariantCount += GetSubVariants(joltages.GetRange(i + 1, joltages.Count - (i + 1)), joltages[i]);
			}

			return subVariantCount;
		}
	}
}
