using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day20
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 20;

		private List<Image> _images = new List<Image>();

		private Dictionary<string, List<int>> _sides = new Dictionary<string, List<int>>();

		private class Image
		{
			public Image(int id)
			{
				Id = id;
			}

			public int Id { get; }
			public List<string> Content { get; } = new List<string>();

			public List<string> GetSidesWithInversion()
			{
				var sides = GetSides();
				sides.AddRange(sides.Select(x => string.Join(string.Empty, x.Reverse())).ToList());

				return sides;
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
			return $"";
		}
	}
}
