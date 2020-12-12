using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc.Year2020.Day11
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private class Point {
			public Point(int x, int y) { X = x; Y = y; }

			public int X { get; set; }

			public int Y { get; set; }
		}

		private const int Day = 11;

		private List<string> _seatingPlan;

		private List<Point> _adjacentSeatCoordinates = new List<Point> { new Point(-1, 1), new Point(0, 1), new Point(1, 1), new Point(-1, 0), 
														new Point(1, 0), new Point(-1, -1), new Point(0, -1), new Point(1, -1) };

		public Solver()
		{
			_seatingPlan = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");
		}

		public string SolveFirstTask()
		{
			List<string> lastPlan = DetermineFinalSeatingPlan(DetermineState);

			var count = lastPlan.SelectMany(x => x).Where(x => x == '#').Count();
			return $"{count}";
		}

		public string SolveSecondTask()
		{
			List<string> lastPlan = DetermineFinalSeatingPlan(DetermineStateSecondMethod);

			var count = lastPlan.SelectMany(x => x).Where(x => x == '#').Count();
			return $"{count}";
		}

		private List<string> DetermineFinalSeatingPlan(Func<List<string>, Point, char> determineState)
		{
			var lastPlan = _seatingPlan;
			var seatingPlanChanged = true;


			while (seatingPlanChanged)
			{
				var newPlan = new List<string>();
				seatingPlanChanged = false;
				for (int i = 0; i < _seatingPlan.Count; i++)
				{
					var builder = new StringBuilder();
					for (int j = 0; j < _seatingPlan[0].Length; j++)
					{
						var seatState = determineState(lastPlan, new Point(j, i));
						if (lastPlan[i][j] != seatState)
						{
							seatingPlanChanged = true;
						}

						builder.Append(seatState);
					}

					newPlan.Add(builder.ToString());
				}

				lastPlan = newPlan;
			}

			return lastPlan;
		}

		private char DetermineState(List<string> previousPlan, Point point)
		{
			return previousPlan[point.Y][point.X] switch
			{
				'.' => '.',
				'L' => OccupiedAdjacentSeats(previousPlan, point) == 0 ? '#' : 'L',
				'#' => OccupiedAdjacentSeats(previousPlan, point) < 4 ? '#' : 'L'
			};
		}

		private char DetermineStateSecondMethod(List<string> seatingPlan, Point point)
		{
			return seatingPlan[point.Y][point.X] switch
			{
				'.' => '.',
				'L' => OccupiedAdjacentSeatsSecondMethod(seatingPlan, point) == 0 ? '#' : 'L',
				'#' => OccupiedAdjacentSeatsSecondMethod(seatingPlan, point) < 5 ? '#' : 'L'
			};
		}

		private int OccupiedAdjacentSeats(List<string> seatingPlan, Point point)
		{			
			return _adjacentSeatCoordinates
				.Select(p => new Point(p.X + point.X, p.Y + point.Y))
				.Where(p => p.X >= 0 && p.Y >= 0 && p.X < seatingPlan[0].Length && p.Y < seatingPlan.Count)
				.Where(p => seatingPlan[p.Y][p.X] == '#')
				.Count();
		}

		private int OccupiedAdjacentSeatsSecondMethod(List<string> seatingPlan, Point point)
		{
			var pointsToCheck = _adjacentSeatCoordinates.Select(p => (Diff: p, Point: new Point(p.X + point.X, p.Y + point.Y))).ToList();
			var occupiedSeats = 0;
			do
			{
				var toRemove = new List<Point>();
				foreach (var coord in pointsToCheck)
				{
					var p = coord.Point;

					if (!IsWithinBounds(seatingPlan, p) || seatingPlan[p.Y][p.X] != '.')
					{
						toRemove.Add(coord.Diff);
					}

					if (IsWithinBounds(seatingPlan, p) && seatingPlan[p.Y][p.X] == '#')
					{
						occupiedSeats++;
					}
					
					p.X += coord.Diff.X;
					p.Y += coord.Diff.Y;
				}

				pointsToCheck = pointsToCheck.Where(x => !toRemove.Any(y => x.Diff == y)).ToList();
			} while (pointsToCheck.Any());

			return occupiedSeats;
		}	

		private static bool IsWithinBounds(List<string> previousPlan, Point p)
		{
			return p.X >= 0 && p.Y >= 0 && p.X < previousPlan[0].Length && p.Y < previousPlan.Count;
		}		
	}
}
