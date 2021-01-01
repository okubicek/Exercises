using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Aoc.Year2020.Day24
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 24;

		private List<string> _tileList;

		private HashSet<Point> _blackTiles = new HashSet<Point>();

		private List<string> _directionList = new List<string> { "e", "w", "se", "ne", "sw", "nw" };

		public Solver()
		{
			_tileList = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt").ToList();			
		}

		public string SolveFirstTask()
		{
			foreach(var tile in _tileList)
			{
				var currentCoor = new Point(0, 0);
				for (int i = 0; i < tile.Length; i++)
				{
					string direction;
					if (tile[i] == 'e' || tile[i] == 'w')
					{
						direction = tile[i].ToString();
					}else
					{
						direction = tile.Substring(i, 2);
						i++;
					}

					currentCoor = ApplyDirection(direction, currentCoor);
				}

				if (_blackTiles.Contains(currentCoor))
				{
					_blackTiles.Remove(currentCoor);
				}else
				{
					_blackTiles.Add(currentCoor);
				}
			}

			return $"{_blackTiles.Count}";
		}

		public Point ApplyDirection(string direction, Point currentCoor)
		{
			return direction switch
			{
				"e" => new Point(currentCoor.X + 2, currentCoor.Y),
				"w" => new Point(currentCoor.X - 2, currentCoor.Y),
				"nw" => new Point(currentCoor.X - 1, currentCoor.Y + 1),
				"sw" => new Point(currentCoor.X - 1, currentCoor.Y - 1),
				"ne" => new Point(currentCoor.X + 1, currentCoor.Y + 1),
				"se" => new Point(currentCoor.X + 1, currentCoor.Y - 1),
			};
		}

		public string SolveSecondTask()
		{						
			for(int i = 0; i < 100; i++)
			{
				var checkedTiles = new HashSet<Point>();
				var nextRoundState = new HashSet<Point>();

				foreach (var blackTile in _blackTiles)
				{
					var tiles = new List<Point> { blackTile };
					tiles.AddRange(_directionList.Select(d => ApplyDirection(d, blackTile)));
					
					foreach(var tile in tiles)
					{
						if (!checkedTiles.Contains(tile) && ShouldBeBlack(tile, _blackTiles))
						{
							nextRoundState.Add(tile);
						}

						checkedTiles.Add(tile);
					}
				}

				_blackTiles = nextRoundState;
			}

			return $"{_blackTiles.Count}";
		}

		private bool ShouldBeBlack(Point tile, HashSet<Point> blackTiles)
		{
			var blackNeighbourCount = _directionList.Select(d => ApplyDirection(d, tile))
				.Where(nt => blackTiles.Contains(nt))
				.Count();

			return blackTiles.Contains(tile) ? blackNeighbourCount == 1 || blackNeighbourCount == 2 : blackNeighbourCount == 2;
		}
	}
}
