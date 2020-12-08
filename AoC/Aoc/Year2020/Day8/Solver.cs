using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Year2020.Day8
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 8;

		private List<(string instruction, int value)> _instructions;

		public Solver()
		{
			_instructions = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt")
				.Select(i => {
					var split = i.Split(' ');

					return (split[0], int.Parse(split[1]));
				}).ToList();
				
		}

		public string SolveFirstTask()
		{
			var res = ExecuteProgram();

			return $"{res.acc}";
		}

		private (int acc, bool loopDetected) ExecuteProgram()
		{
			var executedInstructions = new HashSet<int>();
			var acc = 0;

			int index = 0;
			while (index < _instructions.Count)
			{
				executedInstructions.Add(index);
				var instruction = _instructions[index];

				switch (instruction.instruction)
				{
					case "acc":
						acc = acc + instruction.value;
						index++;
						break;
					case "jmp":
						index += instruction.value;
						break;
					default:
						index++;
						break;
				};

				if (executedInstructions.Contains(index))
				{
					return (acc, true);
				}
			}

			return (acc, false);
		}

		public string SolveSecondTask()
		{
			IEnumerable<int> instructionsToTryToModify = GetInstructionsToModify();
			foreach(var instructionIndex in instructionsToTryToModify)
			{
				_instructions[instructionIndex] = InstructionReplacement(instructionIndex);

				var res = ExecuteProgram();
				if (!res.loopDetected)
					return $"{res.acc}";

				_instructions[instructionIndex] = InstructionReplacement(instructionIndex);
			}

			throw new Exception("Solution not found :-(");
		}

		private (string, int) InstructionReplacement(int instructionIndex)
		{
			var ins = _instructions[instructionIndex];
			return ins.instruction == "jmp" 
				? ( "nop", ins.value ) 
				: ins.instruction == "nop" 
					? ("jmp", ins.value) 
					: ins;
		}

		private IEnumerable<int> GetInstructionsToModify()
		{
			for (int i = 0; i < _instructions.Count; i++)
			{
				if (_instructions[i].instruction == "nop" || _instructions[i].instruction == "jmp")
				{
					yield return i;
				}
			}
		}
	}
}
