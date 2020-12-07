using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day7
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 7;

		private HashSet<Bag> _bags;

		public Solver()
		{
			_bags = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt")
				.Select(line =>
				{
					var ruleSplit = line.Split("contain");

					var content = ParseBagContent(ruleSplit);

					return new Bag
					{
						Color = ruleSplit[0].Replace(" bags ", string.Empty),
						NestedBags = content
					};
				}).ToHashSet();
		}		

		public string SolveFirstTask()
		{
			var containingColors = new HashSet<string>();
			IEnumerable<string> searchedColor = new List<string> { "shiny gold" };
			var endReached = false;

			while(!endReached)
			{
				var bagsContainingColor = FindContainingBags(searchedColor).ToList();
				bagsContainingColor.ForEach(mb => containingColors.Add(mb.Color));
				endReached = !bagsContainingColor.Any();

				searchedColor = bagsContainingColor.Select(x => x.Color);
			}

			return $"{ containingColors.Count() }";
		}

		public string SolveSecondTask()
		{
			var count = CountNestedBags("shiny gold", 1);

			return $"{count - 1}"; //We have to substract shiny gold bag itself
		}

		private IEnumerable<Bag> FindContainingBags(IEnumerable<string> searchedColor)
		{
			return _bags.Where(b => b.NestedBags.Any(nb => searchedColor.Contains(nb.color)));
		}

		private static List<(string, int)> ParseBagContent(string[] ruleSplit)
		{
			var bagContent = ruleSplit[1].Split(',');
			if (char.IsNumber(bagContent[0][1]))
			{
				var content = bagContent.Select(bc =>
				{
					var item = bc.Trim().Split(" ");
					return ($"{item[1]} {item[2]}", int.Parse(item[0]));
				});

				return content.ToList();
			}

			return new List<(string, int)>();
		}

		private long CountNestedBags(string color, long count)
		{
			_bags.TryGetValue(new Bag { Color = color }, out var bag);

			var bagCount = bag.NestedBags.Any()
				? bag.NestedBags.Sum(nb => CountNestedBags(nb.color, nb.count) * count) + count
				: count;

			return bagCount;
		}
	}

	public class Bag
	{
		public string Color { get; set; }

		public List<(string color, int count)> NestedBags { get; set; } = new List<(string color, int count)>();

		public override bool Equals(object obj)
		{
			return obj is Bag bag &&
				   Color == bag.Color;
		}

		public override int GetHashCode()
		{
			return System.HashCode.Combine(Color);
		}
	}
}