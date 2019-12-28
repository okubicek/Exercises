using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Aoc.Year2019.Day15
{
	[Aoc(Day=Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 15;

		private List<long> _program;

		private Droid _droid = new Droid();

		public Solver()
		{
			_program = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0]
				.Split(',')
				.Select(x => long.Parse(x))
				.ToList();
		}

		public string SolveFirstTask()
		{
			var computer = new OpCodeComputer.OpCodeComputer(_droid, _droid);

			try
			{
				computer.ProcessInstructions(_program);
			}
			catch
			{ }

			return _droid.MovesToOxygenerator.ToString();
		}

		public string SolveSecondTask()
		{
			var areaWithoutOxygen = _droid.GetAllHallwayCoordinates();
			var startingPoint = _droid.OxygeneratorPosition;

			var steps = 0;
			var oxygenBorder = new List<Point> { startingPoint };

			while(areaWithoutOxygen.Any())
			{
				var nextStepOxygenBorder = new List<Point>();
				foreach(var coordinates in oxygenBorder)
				{
					AddCoordinatesToFloodWithOxygen(nextStepOxygenBorder, coordinates, areaWithoutOxygen);
					areaWithoutOxygen.Remove(coordinates);
				}

				steps++;

				oxygenBorder = nextStepOxygenBorder;
			}


			return (steps - 1).ToString();
		}

		private void AddCoordinatesToFloodWithOxygen(List<Point> coordinatesToFlood, Point coordinate, Dictionary<Point, char> areaWithoutOxygen)
		{
			AddIfExistsInMap(areaWithoutOxygen, coordinatesToFlood, new Point(coordinate.X + 1, coordinate.Y));
			AddIfExistsInMap(areaWithoutOxygen, coordinatesToFlood, new Point(coordinate.X - 1, coordinate.Y));
			AddIfExistsInMap(areaWithoutOxygen, coordinatesToFlood, new Point(coordinate.X, coordinate.Y + 1));
			AddIfExistsInMap(areaWithoutOxygen, coordinatesToFlood, new Point(coordinate.X, coordinate.Y - 1));
		}

		private void AddIfExistsInMap(Dictionary<Point, char> areaWithoutOxygen, List<Point> coordinatesToFlood, Point coordinate)
		{
			if (areaWithoutOxygen.ContainsKey(coordinate))
			{
				coordinatesToFlood.Add(coordinate);
			}
		}
	}
}
