using Aoc.Helpers;
using Aoc.Year2019.OpCodeComputer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Aoc.Year2019.Day15
{
	public class Droid : IInputChannel, IOutputChannel
	{
		private const char Wall = '#';
		private const char Hallway = '.';
		private const char Oxygenerator = 'X';

		private int _movementCount;

		public int MovesToOxygenerator;

		private Dictionary<Point, char> _map = new Dictionary<Point, char> { { new Point(0, 0), Hallway } };

		private Point _currentPosition = new Point(0, 0);

		private Direction _lastDirectionCommand;

		public Point OxygeneratorPosition { get; private set; }

		private Stack<Direction> _backTrace = new Stack<Direction>();

		private bool _backtracing;

		private List<Direction> _directionOrder = new List<Direction> {
			Direction.West,
			Direction.North,
			Direction.East,
			Direction.South
			//Direction.East,
			//Direction.South,
			//Direction.West,
			//Direction.North
		};

		public Dictionary<Point, char> GetAllHallwayCoordinates()
		{
			return _map.Where(x => x.Value == Hallway).ToDictionary(x => x.Key, x => x.Value);
		}

		public long GetNext()
		{
			var nextDirection = GetNextDirection();

			if (nextDirection == Direction.NotSpecified)
			{
				if (_backTrace.TryPop(out var dir))
				{
					nextDirection = dir;
					_backtracing = true;
				}
				else
				{
					ToConsoleDrawer.DrawFromDictionary(_map, ' ');
					throw new Exception("Whole Map has been scanned");
				}
			}

			_lastDirectionCommand = nextDirection;
			return (long)nextDirection;
		}

		public void Send(long val)
		{
			var newCoordinates = NewCoordinates(_lastDirectionCommand);
			char mapSymbol;

			switch ((StatusCodes)val)
			{
				case StatusCodes.Moved:
					//_map[_currentPosition] = Hallway;
					//_map.TryAdd(newCoordinates, '|');
					//_map[newCoordinates] = '|';
					//ToConsoleDrawer.DrawFromDictionary(_map, ' ');
					//Thread.Sleep(100);
					Move(newCoordinates);

					mapSymbol = Hallway;
					break;
				case StatusCodes.WallHit:
					mapSymbol = Wall;
					break;
				case StatusCodes.OxygenFound:
					Move(newCoordinates);
					OxygeneratorPosition = newCoordinates;
					mapSymbol = Oxygenerator;
					MovesToOxygenerator = _movementCount;
					break;
				default:
					throw new NotImplementedException($"{val.ToString()} is not supported");
			}

			_map.TryAdd(newCoordinates, mapSymbol);
		}

		private void Move(Point newCoordinates)
		{
			if (_backtracing)
			{
				_movementCount--;
				_backtracing = false;
			}
			else
			{
				_backTrace.Push(GetOppositeDirection(_lastDirectionCommand));
				_movementCount++;
			}

			_currentPosition = newCoordinates;
		}

		private Point NewCoordinates(Direction direction)
		{
			switch (direction)
			{
				case Direction.North:
					return new Point(_currentPosition.X, _currentPosition.Y + 1);
				case Direction.South:
					return new Point(_currentPosition.X, _currentPosition.Y - 1);
				case Direction.West:
					return new Point(_currentPosition.X - 1, _currentPosition.Y);
				case Direction.East:
					return new Point(_currentPosition.X + 1, _currentPosition.Y);				
			}

			throw new ArgumentException($"Direction {direction.ToString()} is not supported");
		}

		private Direction GetOppositeDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.North:
					return Direction.South;
				case Direction.South:
					return Direction.North;
				case Direction.West:
					return Direction.East;
				case Direction.East:
					return Direction.West;
			}

			throw new ArgumentException($"Direction {direction.ToString()} is not supported");
		}

		private Direction GetNextDirection()
		{
			foreach(var direction in _directionOrder)
			{
				if (!_map.ContainsKey(NewCoordinates(direction)))
				{
					return direction;
				}
			}

			return Direction.NotSpecified;
		}
	}
}
