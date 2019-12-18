using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Aoc.Year2019.Day10
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 10;

		private const decimal Undefined = decimal.MaxValue;

		private List<string> AsteroidMap { get; set; }

		public Solver()
		{
			AsteroidMap = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt");
		}

		public string SolveFirstTask()
		{
			var xSize = AsteroidMap.First().Length;
			var ySize = AsteroidMap.Count;

			var asteroidCounts = new List<int>();

			for (int x = 0; x < xSize; x++)
			{
				for (int y = 0; y < ySize; y++)
				{
					if (IsBlankSpace(AsteroidMap[y][x])) continue;

					var stationCoordinates = new Point(x, y);
					var visibleAsteroidsCount = CountVisibleAsteroids(stationCoordinates, xSize, ySize);

					asteroidCounts.Add(visibleAsteroidsCount);
				}
			}

			return asteroidCounts.Max().ToString();
		}

		public int CountVisibleAsteroids(Point stationCoordinates, int searchedSpaceXSize, int searchedSpaceYSize)
		{
			var angles = new List<HashSet<decimal>> { new HashSet<decimal>(), new HashSet<decimal>() };

			for (int x = 0; x < searchedSpaceXSize; x++)
			{
				for (int y = 0; y < searchedSpaceYSize; y++)
				{
					if (IsBlankSpace(AsteroidMap[y][x]) || IsItself(ref stationCoordinates, x, y)) continue;

					var distanceToAsteroid = new Size(
							x - stationCoordinates.X,
							y - stationCoordinates.Y
						);

					decimal angle = CalculateAngleToAsteroid(distanceToAsteroid);

					var position = distanceToAsteroid.Height < 0 ? 0 : 1;
					angles[position].Add(angle);
				}
			}

			return angles[0].Count + angles[1].Count;
		}

		private static bool IsItself(ref Point stationCoordinates, int x, int y)
		{
			return x == stationCoordinates.X && y == stationCoordinates.Y;
		}

		private bool IsBlankSpace(char v)
		{
			return v != '#';
		}

		private static decimal CalculateAngleToAsteroid(Size distanceToAsteroid)
		{
			return distanceToAsteroid.Height == 0
				? Undefined // this is for undefined
				: (decimal)distanceToAsteroid.Width / distanceToAsteroid.Height;
		}

		public string SolveSecondTask()
		{
			return string.Empty;
		}
	}
}
