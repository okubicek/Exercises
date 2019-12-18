using Aoc.Year2019.OpCodeComputer;
using System.Collections.Generic;
using System.Drawing;

namespace Aoc.Year2019.Day11
{
	public class Robot : IOutputChannel, IInputChannel
	{
		public Robot(bool inputFieldColor)
		{
			PaintedFields.Add("0_0", inputFieldColor);
		}

		private enum State
		{
			Moving,
			Painting
		}

		private enum Directions
		{
			Up = 0,
			Left = 1,
			Down = 2,
			Right = 3
		}

		public Dictionary<string, bool> PaintedFields { get; } = new Dictionary<string, bool>();

		private Point _currentPosition = new Point(0, 0);

		private State _state = State.Painting;

		private Directions _direction = Directions.Up;

		public long GetNext()
		{
			return GetColorUnderRobot();
		}

		private long GetColorUnderRobot()
		{
			var key = BuildKey();
			if (PaintedFields.ContainsKey(key))
			{
				return PaintedFields[key] ? 1 : 0;
			}

			return 0;
		}

		public void Send(long val)
		{
			ProcessCommands(val);
		}

		private void ProcessCommands(long val)
		{
			if (_state == State.Painting)
			{
				Paint(val);
				_state = State.Moving;
			}
			else
			{
				ChangeDirection(val);
				Move();
				_state = State.Painting;
			}
		}

		private void Paint(long val)
		{
			var key = BuildKey();
			var isWhite = val == 1;

			if (!PaintedFields.TryAdd(key, isWhite))
			{
				PaintedFields[key] = isWhite;
			}
		}

		private string BuildKey()
		{
			return $"{_currentPosition.X}_{_currentPosition.Y}";
		}

		private void ChangeDirection(long val)
		{
			var modifier = val == 0 ? 1 : -1;
			if ((int)_direction + modifier < 0)
			{
				_direction = Directions.Right;
			}
			else if ((int)_direction + modifier > 3)
			{
				_direction = Directions.Up;
			}
			else
			{
				_direction = _direction + modifier;
			}
		}

		private void Move()
		{
			switch (_direction)
			{
				case Directions.Down:
					_currentPosition.Y--;
					break;
				case Directions.Up:
					_currentPosition.Y++;
					break;
				case Directions.Left:
					_currentPosition.X--;
					break;
				case Directions.Right:
					_currentPosition.X++;
					break;
			}
		}
	}
}
