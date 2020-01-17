using Aoc.Helpers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Aoc.Year2019.Day17
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 17;

		private List<long> _program;

		private Dictionary<Point, char> Map;

		private const char Path = '#';

		public Solver()
		{
			_program = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0]
				.Split(',')
				.Select(x => long.Parse(x))
				.ToList();
		}

		public string SolveFirstTask()
		{
			var robot = new Robot();
			var computer = new OpCodeComputer.OpCodeComputer(null, robot);

			computer.ProcessInstructions(_program);

			Map = robot.GetMap();
			var intersections = GetIntersections(Map).ToList();

			ToConsoleDrawer.DrawFromDictionary(Map, ' ');

			return intersections.Select(coor => coor.X * coor.Y).Sum().ToString();
		}

		public string SolveSecondTask()
		{
			var robot = new Robot();
			var computer = new OpCodeComputer.OpCodeComputer(null, robot);

			_program[0] = 2;
			computer.ProcessInstructions(_program);

			return string.Empty;
		}

		private IEnumerable<Point> GetIntersections(Dictionary<Point, char> map)
		{
			var path = map.Where(x => x.Value == Path).Select(x => x.Key).ToList();
			var pathMap = path.ToHashSet();

			foreach (var p in path)
			{
				if (pathMap.Contains(new Point(p.X + 1, p.Y)) &&
					pathMap.Contains(new Point(p.X - 1, p.Y)) &&
					pathMap.Contains(new Point(p.X, p.Y + 1)) &&
					pathMap.Contains(new Point(p.X, p.Y - 1)))
				{
					yield return p;
				}
			}
		}
	}
}
