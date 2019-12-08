using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.Day2
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private string Input { get; }

		private const int Day = 2;

		public Solver()
		{
			Input = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0];
		}

		public string SolveFirstTask()
		{
			var inputs = Input.Split(',').Select(x => int.Parse(x)).ToList();
			inputs[1] = 12;
			inputs[2] = 2;

			PopulateMemory(inputs);

			return inputs[0].ToString();
		}

		public string SolveSecondTask()
		{
			for (int i = 0; i <= 99; i++)
			{
				for (int j = 0; j <= 99; j++)
				{
					var inputs = Input.Split(',').Select(x => int.Parse(x)).ToList();
					inputs[1] = i;
					inputs[2] = j;

					PopulateMemory(inputs);
					if (inputs[0] == 19690720)
					{
						return (i * 100 + j).ToString();
					}
				}
			}

			throw new System.ApplicationException($"No solution found");
		}

		private void PopulateMemory(List<int> inputs)
		{
			var length = inputs.Count;

			var iterationCount = 0;

			for (int x = 0; x < length && inputs[x] != 99; x = x + 4)
			{
				var firstParam = inputs[inputs[x + 1]];
				var secondParam = inputs[inputs[x + 2]];
				inputs[inputs[x + 3]] = ProcessOpcode(inputs[x], firstParam, secondParam);
				iterationCount++;
			}
		}

		public int ProcessOpcode(int opcode, int firstParam, int secondParam)
		{
			switch (opcode)
			{
				case 1:
					return firstParam + secondParam;
				case 2:
					return firstParam * secondParam;
				default:
					throw new System.ApplicationException($"Invalid value {opcode}");
			}
		}
	}
}
