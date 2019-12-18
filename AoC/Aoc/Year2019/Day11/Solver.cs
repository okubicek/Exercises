using Aoc.Year2019.OpCodeComputer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Aoc.Year2019.Day11
{
	[Aoc(Day=Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 11;

		private List<long> _program;

		public Solver()
		{
			_program = _program = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0]
				.Split(',')
				.Select(x => long.Parse(x))
				.ToList();
		}		

		public string SolveFirstTask()
		{
			var robot = new Robot(false);
			var computer = new OpCodeComputer.OpCodeComputer(robot, robot);

			computer.ProcessInstructions(_program);

			return robot.PaintedFields.Count.ToString();
		}

		public string SolveSecondTask()
		{
			var robot = new Robot(true);
			var computer = new OpCodeComputer.OpCodeComputer(robot, robot);

			computer.ProcessInstructions(_program);

			var xCoordinates = robot.PaintedFields.Select(x => int.Parse(x.Key.Split('_')[0]));
			var yCoordinates = robot.PaintedFields.Select(x => int.Parse(x.Key.Split('_')[1]));
			var xSize = Math.Abs(xCoordinates.Max()) + Math.Abs(xCoordinates.Min()) + 1;
			var ySize = Math.Abs(yCoordinates.Max()) + Math.Abs(yCoordinates.Min()) + 1;

			int minX = xCoordinates.Min(), minY = yCoordinates.Min();
			var builder = new StringBuilder();
			for (int x = 0; x < xSize; x++)
			{
				for (int y = 0; y < ySize; y++)
				{
					var key = $"{x + minX}_{y + minY}";
					if(robot.PaintedFields.TryGetValue(key, out bool color))
					{
						builder.Append(color ? '#': '.');
					}
				}

				builder.Append(Environment.NewLine);
			}

			return builder.ToString();
		}
	}
}
