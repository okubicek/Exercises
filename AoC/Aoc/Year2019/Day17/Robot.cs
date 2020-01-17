using Aoc.Year2019.OpCodeComputer;
using System.Collections.Generic;
using System.Drawing;

namespace Aoc.Year2019.Day17
{
	public class Robot : IOutputChannel, IInputChannel
	{
		private Dictionary<Point, char> _map = new Dictionary<Point, char>();

		private const long NewLine = 10;

		private Point _currentPoint = new Point(0, 0);

		public void Send(long val)
		{
			if (val == NewLine)
			{
				_currentPoint = new Point(0, _currentPoint.Y + 1);
				return;
			}

			_map.Add(_currentPoint, (char) val);

			_currentPoint = new Point(_currentPoint.X + 1, _currentPoint.Y);
		}

		public Dictionary<Point, char> GetMap()
		{
			return _map;
		}

		public long GetNext()
		{
			return 0;
		}
	}
}
