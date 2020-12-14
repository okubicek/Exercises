using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Year2020.Day13
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 13;

		private List<string> _input;

		private int _earliestDeparture;

		private List<int> _busses;

		public Solver()
		{
			_input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");
			_earliestDeparture = int.Parse(_input[0]);
			_busses = _input[1].Split(',').Where(x => x != "x").Select(x => int.Parse(x)).ToList();
		}

		public string SolveFirstTask()
		{
			var departureTime = _earliestDeparture + 1;
			while(true)
			{			
				foreach(var bus in _busses)
				{
					if (departureTime % bus == 0)
					{
						return $"{bus * (departureTime - _earliestDeparture)}";
					}					
				}

				departureTime++;
			}
		}

		public string SolveSecondTask()
		{			
			return $"";
		}
	}
}
