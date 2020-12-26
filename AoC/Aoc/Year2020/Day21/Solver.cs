using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day21
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 21;		
		
		private List<Recipe> _recipes = new List<Recipe>();

		private List<Alergen> _alergens = new List<Alergen>();

		private HashSet<string> _nonAlergenic = new HashSet<string>();

		public class Recipe
		{
			public List<string> Ingredients { get; } = new List<string>();

			public List<Alergen> Alergens { get; } = new List<Alergen>();			
		}

		public class Alergen
		{
			public string Name { get; }

			public List<Recipe> Recipes = new List<Recipe>();

			public Alergen(string name)
			{
				Name = name;
			}
		}

		public Solver()
		{
			var lines = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt")
				.ToList();

			foreach(var line in lines)
			{
				var split = line.Split(" (");
				var ingredients = split[0].Split(" ");
				var recipe = new Recipe();
				recipe.Ingredients.AddRange(ingredients);

				split[1]
					.Replace("contains ", string.Empty)
					.Replace(")", string.Empty)
					.Split(", ").ToList()
					.ForEach(x => 
				{
					var a = _alergens.FirstOrDefault(a => a.Name == x);
					if (a == null)
					{
						a = new Alergen(x);
						_alergens.Add(a);
					}

					a.Recipes.Add(recipe);
					recipe.Alergens.Add(a);
				});

				_recipes.Add(recipe);
			}
		}

		public string SolveFirstTask()
		{
			var checkedIngredients = new HashSet<string>();			
			var usageCount = 0;

			var ingredients = _recipes.SelectMany(r => r.Ingredients).Distinct();
			foreach(var ingredient in ingredients)
			{
				var alergens = _recipes.Where(r => r.Ingredients.Contains(ingredient)).SelectMany(r => r.Alergens).Distinct();
				if (!alergens.Any(a => a.Recipes.All(r => r.Ingredients.Contains(ingredient))))
				{
					usageCount += _recipes.Where(r => r.Ingredients.Contains(ingredient)).Count();
					_nonAlergenic.Add(ingredient);
				}
			}

			return $"{usageCount}";
		}

		public string SolveSecondTask()
		{
			var candidateIngredients = new List<(string alergen, List<string> ingredients)>();
			var result = new List<(string alergen, string ingredient)>();

			foreach(var alergen in _alergens)
			{
				var ingredients = alergen.Recipes.Select(r => r.Ingredients.Where(i => !_nonAlergenic.Contains(i))).ToList();
				var intersection = ingredients
									.Skip(1)
									.Aggregate(
										new HashSet<string>(ingredients.First()),
										(h, e) => { h.IntersectWith(e);	return h; }
									);
				candidateIngredients.Add((alergen.Name, intersection.ToList()));
			}

			while(candidateIngredients.Any())
			{
				var i = candidateIngredients.Where(i => i.ingredients.Count() == 1).ToList();				

				foreach(var match in i)
				{					
					result.Add((match.alergen, match.ingredients.First()));
					candidateIngredients.Remove(match);					
					foreach (var ingredients in candidateIngredients)
					{
						var toRemove = match.ingredients.First();						
						ingredients.ingredients.Remove(toRemove);
					}
				}				
			}

			result = result.OrderBy(x => x.alergen).ToList();

			var res = string.Join(",", result.Select(x => x.ingredient));
			return $"{res}";
		}
	}
}
