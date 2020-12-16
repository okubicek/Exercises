using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Year2020.Day16
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private class TicketProperty
		{
			public TicketProperty(string propertyName, IEnumerable<Range> ranges)
			{
				PropertyName = propertyName;
				Ranges = ranges;
			}

			public bool IsValidValue(int value)
			{
				return Ranges.Any(x => x.FallsWithin(value));
			}

			public string PropertyName { get; }

			public IEnumerable<Range> Ranges { get; }
		}


		private const int Day = 16;

		private List<TicketProperty> _ticketProperties = new List<TicketProperty>();
		private List<List<int>> _nearbyTickets = new List<List<int>>();

		private List<int> _myTicket;

		public Solver()
		{
			var input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");
			var mode = 0;
			foreach(var line in input)
			{				
				mode = line == "your ticket:" ? 1 : line == "nearby tickets:" ? 2 : mode;

				if (line == string.Empty || line == "your ticket:" || line == "nearby tickets:")
					continue;

				switch (mode)
				{
					case 0:
						var split = line.Split(':');
						var valueRange = split[1].Split(" or ").Select(x => new Range(int.Parse(x.Split('-')[0]), int.Parse(x.Split('-')[1])));

						_ticketProperties.Add(new TicketProperty(split[0], valueRange));
						break;
					case 1:
						_myTicket = line.Split(',').Select(x => int.Parse(x)).ToList();
						break;
					case 2:
						_nearbyTickets.Add(line.Split(',').Select(x => int.Parse(x)).ToList());
						break;
				}				
			}
			
		}

		public string SolveFirstTask()
		{
			var r = _nearbyTickets.SelectMany(x => x).Where(x => !_ticketProperties.Any(tp => tp.IsValidValue(x)));

			var rd = r.Distinct();
			
			return $"{r.Sum()}";
		}

		public string SolveSecondTask()
		{
			var validTickets = _nearbyTickets.Where(x => !x.Any(y => !_ticketProperties.Any(tp => tp.IsValidValue(y)))).ToList();			
			validTickets.Add(_myTicket);

			var possibilities = new Dictionary<string, List<int>>();
			var matchedProperties = new List<(string name, int columnIndex)>();
			foreach (var ticketProperty in _ticketProperties)
			{
				for(int i = 0; i < _myTicket.Count; i++)
				{
					if(validTickets.All(t => ticketProperty.IsValidValue(t[i])))
					{
						if (!possibilities.TryAdd(ticketProperty.PropertyName, new List<int> { i }))
							possibilities[ticketProperty.PropertyName].Add(i);
					}
				}
			}

			while(possibilities.Any())
			{
				var onlyOnePossibility = possibilities.Where(x => x.Value.Count() == 1);
				foreach(var ticketProperty in onlyOnePossibility)
				{
					var columnIndex = ticketProperty.Value.First();
					matchedProperties.Add((ticketProperty.Key, columnIndex));

					possibilities.Remove(ticketProperty.Key);

					foreach (var keyValue in possibilities)
					{
						keyValue.Value.Remove(columnIndex);
					}										
				}
			}

			long result = 1;
			matchedProperties.Where(x => x.name.StartsWith("departure")).ToList().ForEach(x => result *= _myTicket[x.columnIndex]);

			return $"{result}";
		}
	}

	public static class RangeExt
	{
		public static bool FallsWithin(this Range r, int value)
		{
			return r.End.Value >= value && r.Start.Value <= value;
		}
	}
}
