using System;
using System.Collections.Generic;

namespace Aoc.Year2020.Day17
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 17;

		private HashSet<Point4D> _initialOccupiedSpaces;

		private bool _checkIn4D = false;

		public Solver()
		{
			_initialOccupiedSpaces = new HashSet<Point4D>();

			var input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");

			for (int y = 0; y < input.Count; y++)
			{
				for (int x = 0; x < input[y].Length; x++)
				{
					if (input[y][x] == '#')
					{
						_initialOccupiedSpaces.Add(new Point4D(x, y, 0, 0));
					}
				}
			}
		}

		public string SolveFirstTask()
		{
			HashSet<Point4D> space = RunSimulation();

			return $"{space.Count}";
		}

		private HashSet<Point4D> RunSimulation()
		{
			var space = _initialOccupiedSpaces;
			for (int cycle = 1; cycle <= 6; cycle++)
			{
				var newSpace = new HashSet<Point4D>();

				foreach (var point in space)
				{
					ResolveStateForNextRound(true, point, newSpace, space);
					ResolveStateOfNeighborsForNextRound(point, newSpace, space);
				}

				space = newSpace;
			}

			return space;
		}

		public string SolveSecondTask()
		{
			_checkIn4D = true;
			HashSet<Point4D> space = RunSimulation();

			return $"{space.Count}";
		}

		private void ResolveStateOfNeighborsForNextRound(Point4D checkedPoint, HashSet<Point4D> newSpace, HashSet<Point4D> space)
		{
			PerformActionOnPointNeighbors(checkedPoint, (currentPoint) =>
			{
				if (!space.Contains(currentPoint))
				{
					ResolveStateForNextRound(false, currentPoint, newSpace, space);
				}				
			});
		}

		private void ResolveStateForNextRound(bool isOccupied, Point4D point, HashSet<Point4D> newSpace, HashSet<Point4D> space)
		{
			var count = CountOccupiedNeighbors(point, space);

			if ((isOccupied && (count == 2 || count == 3)) 
				|| (!isOccupied && count == 3))
			{
				newSpace.Add(point);
			}			
		}

		private int CountOccupiedNeighbors(Point4D checkedPoint, HashSet<Point4D> space)
		{
			int count = 0;
			PerformActionOnPointNeighbors(checkedPoint, (currentPoint) =>
			{
				if (space.Contains(currentPoint) && checkedPoint != currentPoint)
				{
					count++;
				}
			});

			return count;
		}

		public void PerformActionOnPointNeighbors(Point4D checkedPoint, Action<Point4D> toPerform)
		{
			for (int d4 = _checkIn4D ? checkedPoint.D4 - 1 : 0; d4 <= (_checkIn4D ? checkedPoint.D4 + 1 : 0); d4++)
			{
				for (int z = checkedPoint.Z - 1; z <= checkedPoint.Z + 1; z++)
				{
					for (int y = checkedPoint.Y - 1; y <= checkedPoint.Y + 1; y++)
					{
						for (int x = checkedPoint.X - 1; x <= checkedPoint.X + 1; x++)
						{
							var currentPoint = new Point4D(x, y, z, d4);
							toPerform(currentPoint);
						}
					}
				}
			}
		}

		public record Point4D
		{
			public Point4D(int x, int y, int z, int d4)
			{
				X = x;
				Y = y;
				Z = z;
				D4 = d4;
			}

			public int X { get; }

			public int Y { get; }

			public int Z { get; }

			public int D4 { get; }
		}
	}
}
