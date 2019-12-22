using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.Day14
{
	[Aoc(Day=Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 14;

		private List<string> _reactionList;

		public Solver()
		{
			_reactionList = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt");
		}

		public struct Resource
		{
			public Resource(int quantity, Product type)
			{
				Quantity = quantity;
				Product = type;
			}

			public int Quantity { get; }

			public Product Product { get; }
		}

		public class Product
		{
			public Product(string type)
			{
				Type = type;
			}

			public string Type { get; private set; }

			public int MinProducableQuantity { get; private set; }

			public List<Resource> Resources { get; private set; }

			public void AddResources(List<Resource> resources)
			{
				Resources = resources;
			}

			public void SetProductQuantity(int quantity)
			{
				MinProducableQuantity = quantity;
			}
		}

		public string SolveFirstTask()
		{
			Dictionary<string, Product> recipes = BuildRecipePipeline();

			var amountOfOre = GetAmountOfOre(1, recipes);

			return amountOfOre.ToString();
		}

		public string SolveSecondTask()
		{
			Dictionary<string, Product> recipes = BuildRecipePipeline();

			var amountOfOrePerOneFuelUnit = GetAmountOfOre(1, recipes);

			var availableOre = 1000000000000;

			var fuelToTry = (availableOre / amountOfOrePerOneFuelUnit) + 1;
			var stepSize = fuelToTry;

			var tried = new HashSet<long>();

			while (!tried.Contains(fuelToTry))
			{
				var ore = GetAmountOfOre(fuelToTry, recipes);
				tried.Add(fuelToTry);

				var modifier = ore < availableOre ? 1 : -1;

				fuelToTry += modifier * stepSize;

				stepSize = stepSize == 1 ? 1 : stepSize / 2;
			}

			return GetAmountOfOre(fuelToTry, recipes) > availableOre ? (fuelToTry - 1).ToString() : fuelToTry.ToString();
		}

		private static long GetAmountOfOre(long fuelRequirement, Dictionary<string, Product> recipes)
		{
			var productionQueue = new Queue<(long requiredQuantity, Product product)>();
			productionQueue.Enqueue((fuelRequirement, recipes["FUEL"]));
			var excessStorage = new Dictionary<string, long>();

			long amountOfOre = 0;

			while (productionQueue.Any())
			{
				(var quantity, var productToProduce) = productionQueue.Dequeue();

				if (IsOre(productToProduce))
				{
					amountOfOre += quantity;
				}
				else
				{
					if (excessStorage.TryGetValue(productToProduce.Type, out var volume))
					{
						var amountToTakeFromStorage = quantity > volume ? volume : quantity;
						quantity -= amountToTakeFromStorage;
						excessStorage[productToProduce.Type] -= amountToTakeFromStorage;
					}

					var excess = CalculateExcess(quantity, productToProduce);

					var resourceMultiplier = (excess + quantity) / productToProduce.MinProducableQuantity;
					excessStorage.TryAdd(productToProduce.Type, 0);
					excessStorage[productToProduce.Type] += excess;

					productToProduce.Resources
						.ForEach(res => productionQueue.Enqueue((res.Quantity * resourceMultiplier, res.Product)));
				}
			}

			return amountOfOre;
		}

		private Dictionary<string, Product> BuildRecipePipeline()
		{
			var products = new Dictionary<string, Product>();

			foreach (var reaction in _reactionList)
			{
				var resourcesAndProduct = reaction.Split("=>");

				var resourcesInfo = resourcesAndProduct[0].Trim().Split(",");
				var requiredResources = new List<Resource>();
				foreach (var resource in resourcesInfo)
				{
					var quantity = GetQuantity(resource);
					var type = resource.Trim().Split(" ")[1];

					products.TryAdd(type, new Product(type));
					var node = products[type];

					requiredResources.Add(new Resource(quantity, node));
				}

				var productInfo = resourcesAndProduct[1].Trim().Split(" ");
				var productType = productInfo[1].Trim();

				products.TryAdd(productType, new Product(productType));
				products[productType].AddResources(requiredResources);
				products[productType].SetProductQuantity(int.Parse(productInfo[0]));
			}

			return products;
		}

		private static long CalculateExcess(long quantity, Product productToProduce)
		{
			var excess = quantity % productToProduce.MinProducableQuantity;
			excess = excess != 0 ? productToProduce.MinProducableQuantity - excess : 0;
			return excess;
		}

		private static bool IsOre(Product productToProduce)
		{
			return productToProduce.Type == "ORE";
		}

		private static int GetQuantity(string resource)
		{
			return int.Parse(resource.Trim().Split(" ")[0]);
		}		
	}
}
