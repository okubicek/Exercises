using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Aoc.Year2019.Day3
{
	public class Solver : IPuzzleSolver
	{
		private Dictionary<string, int> _firstCablePath = new Dictionary<string, int>();
		private Dictionary<string, int> _secondCablePath = new Dictionary<string, int>();
		private HashSet<string> _intersections = new HashSet<string>();

		public int Day => 3;

		public Solver()
		{
			var input = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt");

			TraceBothCables(input);
		}

		public string SolveFirstTask()
		{
			return _intersections.Select(x => CalculateDistance(x)).Min().ToString();
		}

		public string SolveSecondTask()
		{
			return _intersections.Select(x => _firstCablePath[x] + _secondCablePath[x]).Min().ToString();
		}

		private void TraceBothCables(List<string> input)
		{
			Action<Point, int> traceFirstCableAction = (coor, step) => _firstCablePath.TryAdd(StringifyCoordinates(coor), step);
			Action<Point, int> findIntersectionsWithSecondCableAction = (x, step) =>
			{
				var coor = StringifyCoordinates(x);
				_secondCablePath.TryAdd(coor, step);
				if (_firstCablePath.ContainsKey(coor))
				{
					_intersections.Add(coor);
				}
			};

			TraceCable(traceFirstCableAction, input[0].Split(','));
			TraceCable(findIntersectionsWithSecondCableAction, input[1].Split(','));
		}

		private int CalculateDistance(string coordinates)
		{
			var xy = coordinates.Split('_');

			return Math.Abs(int.Parse(xy[0])) + Math.Abs(int.Parse(xy[1]));
		}

		private string StringifyCoordinates(Point p)
		{
			return $"{p.X}_{p.Y}";
		}

		private void TraceCable(Action<Point, int> executeTraceAction, string[] input)
		{
			var coordinates = new Point(0, 0);

			var step = 0;

			foreach (var instruction in input)
			{
				var direction = instruction[0];
				var numOfIters = int.Parse(instruction.Substring(1));

				for (int i = 0; i < numOfIters; i++)
				{
					step++;
					coordinates = ApplyInstructionToCoordinates(coordinates, instruction);
					executeTraceAction(coordinates, step);
				}
			}
		}

		private static Point ApplyInstructionToCoordinates(Point coordinates, string instruction)
		{
			switch (instruction[0])
			{
				case 'U':
					coordinates.Y++;
					break;
				case 'D':
					coordinates.Y--;
					break;
				case 'L':
					coordinates.X--;
					break;
				case 'R':
					coordinates.X++;
					break;
			}

			return coordinates;
		}
	}
}
