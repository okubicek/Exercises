using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day13
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 13;

		private List<string> _input;

		private int _earliestDeparture;

		private List<(int busId, int position)> _busses = new List<(int busId, int position)>();

		public Solver()
		{
			_input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");
			_earliestDeparture = int.Parse(_input[0]);
			var busses = _input[1].Split(',');

			for (int i = 0; i < busses.Length; i++)
			{
				if (busses[i] != "x")
				{
					_busses.Add((int.Parse(busses[i]), i));
				}
			}
		}

		public string SolveFirstTask()
		{
			var departureTime = _earliestDeparture + 1;
			while(true)
			{
				foreach(var bus in _busses)
				{
					if (departureTime % bus.busId == 0)
					{
						return $"{bus.busId * (departureTime - _earliestDeparture)}";
					}
				}

				departureTime++;
			}
		}

		public string SolveSecondTask()
		{
			long step, time = step = _busses[0].busId;

			for (int i = 1; i < _busses.Count; i++)
			{
				var currentBusSelection = _busses.Take(i + 1);
				while (currentBusSelection.Any(b => (time + b.position) % b.busId != 0))
				{
					time += step;
				}

				step *= _busses[i].busId;
			}

			return $"{time}";			
		}
	}
}
