using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Year2020.Day12
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 12;

		private List<string> _input;		

		public Solver()
		{
			_input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");
		}

		public string SolveFirstTask()
		{
			var currentPosition = new Point(0, 0);
			var facingDirection = 'E';

			foreach (var instruction in _input)
			{
				(facingDirection,currentPosition) = ExecuteInstruction(instruction, currentPosition, facingDirection);
			}

			return $"{Math.Abs(currentPosition.X) + Math.Abs(currentPosition.Y)}";
		}

		public string SolveSecondTask()
		{
			var waypoint = new Point(10, 1);
			var currentPosition = new Point(0, 0);
			var currentDirection = 'E';

			foreach (var instruction in _input)
			{
				int instructionValue = ParseInstructionValue(instruction);				

				switch(instruction[0])
				{
					case 'W':
					case 'S':
					case 'N':
					case 'E':
						waypoint = Move(instruction[0], waypoint, instructionValue);
						break;
					case 'F':
						currentPosition = new Point(currentPosition.X + instructionValue * waypoint.X, currentPosition.Y + instructionValue * waypoint.Y);
						break;
					case 'R':
					case 'L':
						(currentDirection, waypoint) = RotateWaypoint(instruction[0], instructionValue, currentDirection, waypoint);
						break;
				}
			}

			return $"{Math.Abs(currentPosition.X) + Math.Abs(currentPosition.Y)}";
		}

		private (char, Point) RotateWaypoint(char instruction, int instructionValue, char currentDirection, Point waypoint)
		{
			//1st quadrant = E, 2nd quadrant = S, 3rd quadrant = W, 4th quadrant = N
			var rotationDiff = new Dictionary<char, Point> { { 'E', new Point(1, 1) }, { 'S', new Point(1, -1) }, { 'W', new Point(-1, -1) }, { 'N', new Point(-1, 1) } };			

			for (int i = 0; i < instructionValue/90; i++)
			{
				currentDirection = Turn(instruction, instructionValue, currentDirection);
				var coordinageChange = rotationDiff[currentDirection];

				waypoint = new Point(Math.Abs(waypoint.Y) * coordinageChange.X, Math.Abs(waypoint.X) * coordinageChange.Y);
			}

			return (currentDirection, waypoint);
		}

		public (char, Point) ExecuteInstruction(string instruction, Point currentPosition, char facingDirection)
		{
			int instructionValue = ParseInstructionValue(instruction);			

			switch (instruction[0])
			{
				case 'F':
					return (facingDirection, Move(facingDirection, currentPosition, instructionValue));
				case 'N':
				case 'S':
				case 'E':
				case 'W':
					return (facingDirection, Move(instruction[0], currentPosition, instructionValue));
				case 'R':
				case 'L':
					facingDirection = Turn(instruction[0], instructionValue, facingDirection);
					break;
			}

			return (facingDirection, currentPosition);
		}

		private static int ParseInstructionValue(string instruction)
		{
			return int.Parse(instruction.Substring(1, instruction.Length - 1));
		}

		private char Turn(char direction, int numericValue, char currentDirection)
		{
			var _sides = "ESWN";
			var index = _sides.IndexOf(currentDirection);
			
			for (int i = 0; i < numericValue / 90; i++)
			{
				if (direction == 'R')
				{
					index = index == 3 ? 0 : index + 1;
				}
				else
				{
					index = index == 0 ? 3 : index - 1;
				}
			}

			return _sides[index];
		}

		private Point Move(char direction, Point currentPosition, int distanceToMove)
		{
			return direction switch
			{
				'W' => new Point(currentPosition.X - distanceToMove, currentPosition.Y),
				'E' => new Point(currentPosition.X + distanceToMove, currentPosition.Y),
				'N' => new Point(currentPosition.X, currentPosition.Y + distanceToMove),
				'S' => new Point(currentPosition.X, currentPosition.Y - distanceToMove),
			};
		}		
	}
}
