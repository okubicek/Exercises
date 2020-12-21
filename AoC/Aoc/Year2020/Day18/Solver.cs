using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day18
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 18;

		private List<string> _input;		

		public Solver()
		{
			_input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");
		}

		public string SolveFirstTask()
		{
			long res = 0;
			foreach(var formula in _input)
			{
				var i = -1;
				res += Interpret(formula, ref i, EvaluateLeftToRight);
			}			

			return $"{res}";
		}

		public string SolveSecondTask()
		{
			long res = 0;
			foreach (var formula in _input)
			{
				var i = -1;
				res += Interpret(formula, ref i, EvaluateWithPlusPrecedence);
			}

			return $"{res}";
		}

		private int GetNumber(string line, ref int i)
		{
			var str = "";
			do
			{
				str += line[i];
				i++;
			} while (i < line.Length && char.IsDigit(line[i]));

			i--;

			return int.Parse(str);
		}

		public long Interpret(string line, ref int i, Func<List<long>, string, long> evaluate)
		{
			string operands = string.Empty;
			var numbers = new List<long>();
			i++;
			do
			{
				var current = line[i];

				if (char.IsDigit(current))
				{
					numbers.Add(GetNumber(line, ref i));
				}

				if (current == '+' || current == '*')
				{
					operands += current;
				}
				else if (current == '(')
				{
					numbers.Add(Interpret(line, ref i, evaluate));
				}
				else if (current == ')')
				{
					return evaluate(numbers, operands);
				}

				i++;
			} while (i < line.Length);

			return evaluate(numbers, operands);
		}

		public long EvaluateWithPlusPrecedence(List<long> numbers, string operands)
		{			
			while(operands.IndexOf('+') != -1)
			{
				var index = operands.IndexOf('+');
				numbers[index] = numbers[index] + numbers[index + 1];
				numbers.RemoveAt(index + 1);
				operands = operands.Remove(index, 1);
			}

			return string.IsNullOrEmpty(operands) ? numbers.First() : Multiply(numbers);
		}

		public long EvaluateLeftToRight(List<long> numbers, string operands)
		{
			while(operands.Any())
			{
				switch (operands[0])
				{
					case '+':
						numbers[0] = numbers[0] + numbers[1];
						break;
					case '*':
						numbers[0] = numbers[0] * numbers[1];
						break;
				}
				operands = operands.Remove(0, 1);
				numbers.RemoveAt(1);
			}

			return numbers.First();
		}

		public long Multiply(List<long> nums)
		{
			long res = 1;
			foreach(var num in nums)
			{
				res *= num;
			}

			return res;
		}
	}
}
