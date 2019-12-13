using Aoc.Year2019.OpCodeComputer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aoc.Year2019.Day7
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 7;

		private List<int> Program;

		public Solver()
		{
			Program = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0]
				.Split(',')
				.Select(x => int.Parse(x))
				.ToList();
		}

		public string SolveFirstTask()
		{
			var phaseCombinations = GetAllPossibleStringCombinations(new List<string> { "0", "1", "2", "3", "4" });
			var results = new Dictionary<int, string>();

			foreach (var phase in phaseCombinations)
			{
				var output = GetOutputToThrusters(phase);

				results.Add(output, phase);
			}

			return results.Select(x => x.Key).Max().ToString();
		}

		private static List<string> GetAllPossibleStringCombinations(IEnumerable<string> numbers)
		{
			var count = numbers.Count();

			if (count == 1)
			{
				return new List<string> { numbers.First() };
			}

			if (count == 2)
			{
				var r = numbers.ToList();
				return new List<string> {
					$"{r[0]}{r[1]}",
					$"{r[1]}{r[0]}"
				};
			}

			var combinations = new List<string>();
			foreach(var num in numbers.ToList())
			{
				var res = GetAllPossibleStringCombinations(numbers.Where(x => x != num));
				combinations.AddRange(res.Select(x => x + num));
			}

			return combinations;
		}

		private int GetOutputToThrusters(string phases)
		{
			var output = new ThrusterOutput(0);
			var outputChannel = new OutputChannel(output);
			var inputChannel = new InputChannel();
			var computer = new OpCodeComputer.OpCodeComputer(inputChannel, outputChannel);

			for (var i = 0; i < 5; i++)
			{
				var pj = phases[i];
				inputChannel.QueueInput(int.Parse(phases.Substring(i, 1)));
				inputChannel.QueueInput(output.Value);

				computer.ProcessInstructions(Program);
			}

			return output.Value;
		}

		public string SolveSecondTask()
		{
			var phaseCombinations = GetAllPossibleStringCombinations(new List<string> { "5", "6", "7", "8", "9" });
			var results = new Dictionary<int, string>();

			foreach (var phase in phaseCombinations)
			{
				var output = GetOutputToThrustersWithFeedback(phase);

				results.Add(output, phase);
			}

			return results.Select(x => x.Key).Max().ToString();
		}

		private int GetOutputToThrustersWithFeedback(string phase)
		{
			var inputChannels = new List<IInputChannel>();
			var tasks = new List<Task>();

			var inputChannel = new InputChannel();

			for (var i = 0; i < 5; i++)
			{
				inputChannel.QueueInput(i);
				var outputChannel = new LinkedOutputChannel(inputChannel);

				inputChannels.Add(inputChannel);

				var t = Task.Run(() =>
				{
					var computer = new OpCodeComputer.OpCodeComputer(inputChannel, outputChannel);
					computer.ProcessInstructions(Program);
				});
			}

			Task.WaitAll(Task.WhenAll(tasks));

			return inputChannels.First().GetNext();
		}
	}
}
