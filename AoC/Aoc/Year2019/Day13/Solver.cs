using Aoc.Year2019.OpCodeComputer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Aoc.Year2019.Day13
{
	[Aoc(Day=Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 13;

		private List<long> _program;

		public enum TileType
		{
			Empty,
			Wall,
			Block,
			HorizontalPaddle,
			Ball
		}

		public enum ArcadeState
		{
			WaitingForX,
			WaitingForY,
			WaitingForType
		}

		public class Arcade : IInputChannel, IOutputChannel
		{
			public Dictionary<Point, TileType> Monitor { get; private set; } = new Dictionary<Point, TileType>();

			private ArcadeState _state = ArcadeState.WaitingForX;

			public int Score { get; private set; }

			private Point _ballPosition;

			private Point _paddlePosition;

			private int _ballDirection;

			private int _interimX;

			private int _interimY;

			public long GetNext()
			{			
				var _estimatedImpactX = (_ballPosition.X + (_paddlePosition.Y - _ballPosition.Y) * _ballDirection) - _ballDirection;

				if (_estimatedImpactX > _paddlePosition.X)
				{
					return 1;
				}
				else if(_estimatedImpactX == _paddlePosition.X)
				{
					return 0;
				}
				else
				{
					return -1;
				}
			}

			private void Draw()
			{
				var xSize = Monitor.Select(c => c.Key.X).Max();
				var ySize = Monitor.Select(c => c.Key.Y).Max();

				var output = new StringBuilder();
				for (int y = 0; y <= ySize; y++)
				{
					for (int x = 0; x <= xSize; x++)
					{
						if (Monitor.TryGetValue(new Point(x, y), out TileType type))
						{
							switch (type)
							{
								case TileType.Empty:
									output.Append(" ");
									break;
								case TileType.Ball:
									output.Append("*");
									break;
								case TileType.Block:
									output.Append(".");
									break;
								case TileType.HorizontalPaddle:
									output.Append("-");
									break;
								case TileType.Wall:
									output.Append("|");
									break;
							}
						}
					}

					output.Append(Environment.NewLine);
				}

				Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
				Console.WriteLine(output);
				Thread.Sleep(100);
			}			

			public void Send(long val)
			{
				var input = (int)val;
				switch(_state)
				{
					case ArcadeState.WaitingForX:
						_interimX = input;
						_state = ArcadeState.WaitingForY;
						break;
					case ArcadeState.WaitingForY:
						_interimY = input;
						_state = ArcadeState.WaitingForType;
						break;
					case ArcadeState.WaitingForType:
						if (_interimX == -1)
						{
							Score = input;
						}else
						{
							var tileType = (TileType)input;
							var tilePosition = new Point(_interimX, _interimY);

							if (Monitor.ContainsKey(tilePosition))
							{
								Monitor[tilePosition] = tileType;
							}
							else
							{
								Monitor.Add(new Point(_interimX, _interimY), tileType);
							}

							if (tileType == TileType.Ball)
							{
								_ballDirection = tilePosition.X - _ballPosition.X > 0 ? 1 : -1;
								_ballPosition = tilePosition;
								//Draw();
							}
							else if(tileType == TileType.HorizontalPaddle)
							{
								_paddlePosition = tilePosition;
							}
						}

						_state = ArcadeState.WaitingForX;
						break;
				}
			}
		}

		public Solver()
		{
			_program = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0]
				.Split(',')
				.Select(x => long.Parse(x))
				.ToList();
		}

		public string SolveFirstTask()
		{
			var arcade = new Arcade();
			var computer = new OpCodeComputer.OpCodeComputer(arcade, arcade);

			computer.ProcessInstructions(_program);

			return arcade.Monitor
				.Where(x => x.Value == TileType.Block)
				.Count()
				.ToString();
		}

		public string SolveSecondTask()
		{
			_program[0] = 2;

			var arcade = new Arcade();
			var computer = new OpCodeComputer.OpCodeComputer(arcade, arcade);

			computer.ProcessInstructions(_program);

			return arcade.Score.ToString();
		}
	}
}
