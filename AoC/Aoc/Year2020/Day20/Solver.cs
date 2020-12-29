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

		private List<Point> _seaMonsterPattern = new List<Point> { new Point(0,0), new Point(1,1), new Point(5, 1), new Point(6, 1), new Point(11, 1), new Point(12, 1), new Point(17, 1), new Point(18, 1), new Point(19,1),
			new Point(4,1), new Point(7,1), new Point(10,1), new Point(13,1), new Point(16,1), new Point(18,-1)
		};

		private class Image
		{
			public Image(int id)
			{
				Id = id;
			}

			public int Id { get; }
			public List<string> Content { get; private set; } = new List<string>();

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
				sides.Add(Content.First());
				sides.Add(Content.Last());
				sides.Add(string.Join(string.Empty, Content.Select(x => x[0])));
				sides.Add(string.Join(string.Empty, Content.Select(x => x[x.Length - 1])));

				return sides;
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
			var uniqueSides = _sides.Where(x => x.Value.Count() == 1).ToList();
			var keys = uniqueSides.SelectMany(s => s.Value).GroupBy(i => i).Where(i => i.Count() == 4).Select(x => x.Key).Distinct().ToList();
			return $"{(long)keys[0] * keys[1] * keys[2] * keys[3]}";
		}

		public string SolveSecondTask()
		{
			var map = new Image(0);
			map.Content.AddRange(GenerateMap(null));
			var res = string.Empty;

			for (int i = 0; i < 4; i++)
			{
				var monsterCount = 0;
				for (int y = 1; y < map.Content.Count - 1; y++)
				{
					for (int x = 0; x < map.Content[0].Length - 20; x++)
					{
						if (_seaMonsterPattern.All(m => map.Content[y + m.Y][x + m.X] == '#'))
						{
							monsterCount++;
						}
					}
				}

				map.Rotate();
				res += $"{monsterCount}-";
			}

			return res;
		}

		public List<string> GenerateMap(List<List<int>> tilePositions)
		{
			var map = new List<string>();

			foreach (var tileLine in tilePositions)
			{
				var tiles = _images.Where(i => tileLine.Contains(i.Id));

				for(int i = 1; i < tiles.First().Content[0].Length - 1; i++)
				{
					var mapLine = string.Empty;
					foreach(var tile in tiles)
					{
						map.Add(tile.Content[i].Substring(1, tile.Content[i].Length - 2));
					}

					map.Add(mapLine);
				}
			}

			return map;
		}		
	}
}
