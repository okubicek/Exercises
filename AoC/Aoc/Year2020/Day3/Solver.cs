using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Aoc.Year2020.Day3
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 3;

		private List<string> _input;

		public Solver()
		{
			_input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");
		}

		public string SolveFirstTask()
		{
			var trees = CountNumberOfTrees(3, 1);

			return $"{trees}";
		}

		public string SolveSecondTask()
		{
			var slopes = new List<(int x, int y)> {
				(1,1),
				(3,1),
				(5,1),
				(7,1),
				(1,2)
			};

			long prob = 1;
			foreach(var slope in slopes)
			{
				prob = prob * (long)CountNumberOfTrees(slope.x, slope.y);
			}

			return $"{prob}";
		}

		public int CountNumberOfTrees(int deltaX, int deltaY)
		{
			var y = deltaY;
			var x = 1;
			var trees = 0;
			var length = _input.First().Length;

			do 
			{
				x = x + deltaX;				

				var positionX = x % length;
				if (positionX == 0)
					positionX = length;

				if (_input[y][positionX - 1] == '#')
				{
					trees++;
				}

				y = y + deltaY;
			}while(y < _input.Count);

			return trees;
		}
	}
}
