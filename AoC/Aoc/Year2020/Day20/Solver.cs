using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Aoc.Year2020.Day20
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 20;

		private List<Image> _images = new List<Image>();

		private Dictionary<string, List<int>> _sides = new Dictionary<string, List<int>>();

		private List<Point> _seaMonsterPattern = new List<Point> { new Point(0,0), new Point(5, 0), new Point(6, 0), new Point(11, 0), new Point(12, 0), new Point(17, 0), new Point(18, 0), new Point(19,0),
			new Point(1,1), new Point(4,1), new Point(7,1), new Point(10,1), new Point(13,1), new Point(16,1), new Point(18,-1)
		};

		private class Image
		{
			public Image(int id)
			{
				Id = id;
			}

			public int Id { get; }
			public List<string> Content { get; private set; } = new List<string>();

			public string RightSide => string.Join(string.Empty, Content.Select(x => x[x.Length - 1]));

			public string LeftSide => string.Join(string.Empty, Content.Select(x => x[0]));

			public string TopSide => Content.First();

			public string BottomSide => Content.Last();


			public List<string> GetSidesWithInversion()
			{
				var sides = GetSides();
				sides.AddRange(sides.Select(x => string.Join(string.Empty, x.Reverse())).ToList());

				return sides;
			}

			public void Invert()
			{
				for (int i = 0; i < Content.Count; i++)
				{
					Content[i] = string.Join(string.Empty, Content[i].Reverse());
				}
			}

			public void Rotate()
			{
				var newContent = new List<string>();
				for (int i = Content[0].Length - 1; i >= 0; i--)
				{
					var builder = new StringBuilder();
					for(int j = 0; j < Content.Count; j++)
					{
						builder.Append(Content[j][i]);
					}
					newContent.Add(builder.ToString());
				}

				Content = newContent;
			}

			public List<string> GetSides()
			{
				var sides = new List<string>();
				sides.Add(TopSide);
				sides.Add(BottomSide);
				sides.Add(LeftSide);
				sides.Add(RightSide);

				return sides;
			}

			public void PrintMe()
			{
				foreach(var line in Content)
				{
					Console.WriteLine(line);
				}

				Console.WriteLine();
			}
		}

		public Solver()
		{
			var input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt")
				.ToList();

			Image image = null;
			foreach (var line in input)
			{
				if (line.StartsWith("Tile "))
				{
					if (image != null) _images.Add(image);					
					image = new Image(int.Parse(line.Replace("Tile ", string.Empty).Replace(":", string.Empty)));

				}
				else if (line.StartsWith(".") || line.StartsWith("#"))
				{
					image.Content.Add(line);
				}
			}

			_images.Add(image);
			_images.ForEach(image => image.GetSidesWithInversion().ForEach(x =>
			{
				if (!_sides.TryAdd(x, new List<int> { image.Id }))
				{
					_sides[x].Add(image.Id);
				}
			}));
		}

		public string SolveFirstTask()
		{
			List<int> keys = GetCornerTiles();
			return $"{(long)keys[0] * keys[1] * keys[2] * keys[3]}";
		}

		private List<int> GetCornerTiles()
		{
			var sidesWithNoNeighbours = _sides.Where(x => x.Value.Count() == 1).ToList();
			var keys = sidesWithNoNeighbours.SelectMany(s => s.Value).GroupBy(i => i).Where(i => i.Count() == 4).Select(x => x.Key).Distinct().ToList();//4x times there instead of just 2x times because of inversions
			return keys;
		}

		public string SolveSecondTask()
		{
			var cornerTiles = GetCornerTiles();
			var topLeftCornerTile = _images.Where(i => i.Id == cornerTiles.First()).First();
			cornerTiles.Remove(cornerTiles.First());
			var tilePositions = new List<List<int>>();

			while (true)
			{
				if (_sides[topLeftCornerTile.LeftSide].Count() == 1 && _sides[topLeftCornerTile.TopSide].Count() == 1) break;

				topLeftCornerTile.Rotate();
			}

			Image yPositionTile, xPositionTile = yPositionTile = topLeftCornerTile;
			var x = 0;
			var y = 0;
			while (true)
			{
				tilePositions.Add(new List<int>());
				while (true)
				{
					tilePositions[y].Add(xPositionTile.Id);

					xPositionTile = FindConnectingTileInAxis(xPositionTile, 'X');
					if (xPositionTile == null) break;

					x++;
				}

				yPositionTile = xPositionTile = FindConnectingTileInAxis(yPositionTile, 'Y'); //starting first column of next line
				if (yPositionTile == null) break;

				y++;
				x = 0;
			}

			var map = new Image(0);
			map.Content.AddRange(GenerateMap(tilePositions));
			var res = string.Empty;			

			var monsterCount = CountMonstersWithInversion(map);

			return $"{map.Content.SelectMany(m => m).Where(m => m == '#').Count() - (monsterCount * _seaMonsterPattern.Count())}";
		}

		private int CountMonstersWithInversion(Image map)
		{
			var monsterCount = CountMonsters(map);
			if (monsterCount > 0) return monsterCount;

			map.Invert();

			return CountMonsters(map);
		}

		private int CountMonsters(Image map)
		{
			for (int i = 0; i < 4; i++)
			{
				var monsterCount = 0;
				for (var y = 1; y < map.Content.Count - 1; y++)
				{
					for (var x = 0; x < map.Content[0].Length - 20; x++)
					{
						if (_seaMonsterPattern.All(m => map.Content[y + m.Y][x + m.X] == '#'))
						{
							monsterCount++;
						}
					}
				}

				if (monsterCount > 0) return monsterCount;

				map.Rotate();
			}

			return 0;
		}

		private Image FindConnectingTileInAxis(Image tile, char axis)
		{
			var connectingSide = axis == 'X' ? tile.RightSide : tile.BottomSide;
			Func<Image, bool> imageSidePicker = axis == 'X' 
				? (image) => image.LeftSide == connectingSide 
				: (image) => image.TopSide == connectingSide;

			var connectingImage = GetConnectingImage(tile, connectingSide);

			if (connectingImage == null) return null;

			RotateToPosition(connectingImage, imageSidePicker);

			return connectingImage;			
		}

		private Image GetConnectingImage(Image tile, string side)
		{
			var connectingTile = _sides[side].Where(tileId => tileId != tile.Id).Distinct().FirstOrDefault();

			return _images.FirstOrDefault(i => i.Id == connectingTile);
		}

		private void RotateToPosition(Image image, Func<Image, bool> inPosition)
		{
			if (TryAllFourSides(image, inPosition)) return;

			image.Invert();

			if (!TryAllFourSides(image, inPosition)) throw new Exception($"No matching orientationFound for image {image.Id}");
		}

		private static bool TryAllFourSides(Image image, Func<Image, bool> inPosition)
		{
			for (int i = 0; i < 4; i++)
			{
				if (inPosition(image))
				{
					return true;
				}

				image.Rotate();
			}

			return false;
		}

		public List<string> GenerateMap(List<List<int>> tilePositions)
		{
			var map = new List<string>();

			foreach (var tileLine in tilePositions)
			{				
				for(int i = 1; i < _images.First().Content[0].Length - 1; i++)
				{
					var mapLine = Enumerable.Empty<char>();
					foreach(var tileId in tileLine)
					{
						var tile = _images.First(i => i.Id == tileId);
						mapLine = mapLine.Concat(tile.Content[i].Substring(1, tile.Content[i].Length - 2));
					}

					map.Add(string.Join(string.Empty, mapLine));
				}
			}

			return map;
		}		
	}
}