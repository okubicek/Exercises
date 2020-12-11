using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			var previousNode = 0;

			foreach(int i in _diffList[2])
			{
				variants *= GetSubVariants(_joltages.GetRange(previousNode, i - previousNode), _joltages[i]);
			};

			return $"{variants}";
		}

		private int GetSubVariants(List<int> lists, int value)
		{
			var subVariantCount = 0;
			foreach (var item in lists.Where(x => x < (value + 3)))
			{
				subVariantCount += GetSubVariants(lists.GetRange(1, lists.Count - 1), item);
			}

			return subVariantCount;
		}
	}
}
