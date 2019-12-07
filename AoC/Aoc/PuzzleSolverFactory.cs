using System;
using System.Linq;
using System.Reflection;

namespace Aoc
{
	public class PuzzleSolverFactory
	{
		public IPuzzleSolver GetSolver(int aocYear, int day)
		{
			var puzzle = Assembly.GetCallingAssembly()
				.GetTypes()
				.SingleOrDefault(
					x => x.Namespace.Contains($"Year{aocYear}") && 
					GetAocDayAttributeValue(x) == day &&
					typeof(IPuzzleSolver).IsAssignableFrom(x)
				);

			if (puzzle == null)
			{
				throw new NotImplementedException("No solver found for given day");
			}

			return Activator.CreateInstance(puzzle) as IPuzzleSolver;
		}

		private static int? GetAocDayAttributeValue(Type type)
		{
			var attribute = type.GetCustomAttribute(typeof(AocAttribute));

			if (attribute != null)
			{
				var propertyInfo = typeof(AocAttribute).GetProperties().First(x => x.Name.Equals(nameof(AocAttribute.Day)));

				return (int) propertyInfo.GetValue(attribute);
			}

			return null;
		}
	}
}