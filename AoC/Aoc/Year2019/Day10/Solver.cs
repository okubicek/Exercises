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

		private List<char[]> AsteroidMap { get; set; }

		public Solver()
		{
			AsteroidMap = InputFileReader
				.GetInput($"Year2019/Inputs/Day{Day}.txt")
				.Select(x => x.ToCharArray())
				.ToList();
		}

		public string SolveFirstTask()
		{
			var stationDetails = DetermineStationPosition();

			return stationDetails.visibleAsteroids.ToString();
		}

		private (Point coordinates, int visibleAsteroids) DetermineStationPosition()
		{
			var xSize = AsteroidMap.First().Length;
			var ySize = AsteroidMap.Count;

			var asteroidCounts = new Dictionary<Point, int>();

			for (int y = 0; y < ySize; y++)
			{
				for (int x = 0; x < xSize; x++)
				{
					if (IsBlankSpace(AsteroidMap[y][x])) continue;

					var stationCoordinates = new Point(x, y);
					var closestAsteroids = DetermineClosestAsteroids(stationCoordinates, xSize, ySize);

					asteroidCounts.TryAdd(stationCoordinates, closestAsteroids.Count);
				}
			}

			var max = asteroidCounts.Select(x => x.Value).Max();
			var coordinate = asteroidCounts.Where(x => x.Value == max).First();
			return (coordinate.Key, max);
		}

		public List<(decimal angle, Point coordinates, decimal distance)> DetermineClosestAsteroids(Point stationCoordinates, int searchedSpaceXSize, int searchedSpaceYSize)
		{
			var angles = new Dictionary<decimal, (Point coordinates, decimal distance)>();

			for (int y = 0; y < searchedSpaceYSize; y++)
			{
				for (int x = 0; x < searchedSpaceXSize; x++)
				{
					if (IsBlankSpace(AsteroidMap[y][x]) || IsItself(ref stationCoordinates, x, y)) continue;

					var distanceToAsteroid = new Size(
							x - stationCoordinates.X,
							stationCoordinates.Y - y
						);

					var (angle, absoluteDistance) = CalculateAngleAndDistanceToAsteroid(distanceToAsteroid);
					angle = Math.Round(angle, 5, MidpointRounding.AwayFromZero);

					var currentAsteroid = (new Point(x, y), absoluteDistance);
					if (!angles.TryAdd(angle, currentAsteroid))
					{
						angles[angle] = angles[angle].distance > absoluteDistance ? currentAsteroid : angles[angle];						
					}
				}
			}

			return angles
				.Select(x => (x.Key, x.Value.coordinates, x.Value.distance))
				.ToList();
		}

		private static bool IsItself(ref Point stationCoordinates, int x, int y)
		{
			return x == stationCoordinates.X && y == stationCoordinates.Y;
		}

		private bool IsBlankSpace(char v)
		{
			return v != '#';
		}

		private static (decimal radius, decimal distance) CalculateAngleAndDistanceToAsteroid(Size distanceToAsteroid)
		{
			int quadrant = DetermineQuadrant(distanceToAsteroid);

			var c = (decimal) Math.Sqrt(Math.Pow(distanceToAsteroid.Height, 2) + Math.Pow(distanceToAsteroid.Width, 2));

			switch(quadrant)
			{
				case 0:
				case 2:
					return (Math.Abs(distanceToAsteroid.Width) / c + quadrant, c);
				case 1:
				case 3:
					return (Math.Abs(distanceToAsteroid.Height) / c + quadrant, c );
			}

			throw new NotImplementedException("Invalid quadrant");
		}

		private static int DetermineQuadrant(Size distanceToAsteroid)
		{
			if (distanceToAsteroid.Width >= 0 && distanceToAsteroid.Height >= 0) return 0;
			if (distanceToAsteroid.Width >= 0 && distanceToAsteroid.Height < 0) return 1;
			if (distanceToAsteroid.Width < 0 && distanceToAsteroid.Height <= 0) return 2;			

			return 3;
		}

		public string SolveSecondTask()
		{
			var xSize = AsteroidMap.First().Length;
			var ySize = AsteroidMap.Count;

			var stationDetails = DetermineStationPosition();

			var asteroids = DetermineClosestAsteroids(stationDetails.coordinates, xSize, ySize);
			Point winner = new Point();
			var count = 1;

			while(asteroids.Any())
			{
				asteroids = asteroids.OrderBy(x => x.angle).ToList();
				foreach (var asteroid in asteroids)
				{
					if (count == 200)
					{
						winner = asteroid.coordinates;
					}

					ZapAsteroidWithLaser(asteroid.coordinates);
					count++;
				}

				asteroids = DetermineClosestAsteroids(stationDetails.coordinates, xSize, ySize);
			}

			return (winner.X * 100 + winner.Y).ToString();
		}

		private void ZapAsteroidWithLaser(Point coordinates)
		{
			AsteroidMap[coordinates.Y][coordinates.X] = '.';
		}
	}
}
