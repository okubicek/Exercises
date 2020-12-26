using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day19
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private interface IRule
		{
			(bool isValid, List<string> validCombinations) Matches(string toValidate, List<string> possibleCombinations);
		}

		private class FixedCharacterRule : IRule
		{
			public FixedCharacterRule(string input)
			{
				_character = input.Replace("\"", string.Empty)[0];
			}

			private char _character;

			public (bool isValid, List<string> validCombinations) Matches(string toValidate, List<string> possibleCombinations)
			{
				var add = possibleCombinations.Select(x => x + _character);
				var validCombinations = add.Where(a => toValidate.StartsWith(a)).ToList();

				return (validCombinations.Any(), validCombinations);
			}
		}

		private class PositionRule : IRule
		{
			public List<List<int>> _ruleSets { get; } = new List<List<int>>();			

			private Dictionary<int, IRule> _ruleBook;

			public PositionRule(string input, Dictionary<int, IRule> ruleBook)
			{
				if (input.Contains('|'))
				{
					var split = input.Split(" | ").ToList();
					split.ForEach(x =>
					{
						PopulateRules(x);
					});
				}
				else
				{
					PopulateRules(input);
				}

				_ruleBook = ruleBook;
			}

			private void PopulateRules(string input)
			{
				var set = new List<int>();
				input.Split(" ").ToList().ForEach(x => set.Add(int.Parse(x)));				

				_ruleSets.Add(set);
			}

			public (bool isValid, List<string> validCombinations) Matches(string toValidate, List<string> possibleCombinations)
			
			{
				var validCombinations = new List<string>();
				for (var i = 0; i < _ruleSets.Count; i++)
				{
					var res = EvaluateRuleSet(toValidate, possibleCombinations, i);
					if (res.isValid)
					{
						validCombinations.AddRange(res.validCombinations);
					}
				}

				return (validCombinations.Any(), validCombinations);
			}

			private (bool isValid, List<string> validCombinations) EvaluateRuleSet(string toValidate, List<string> possibleCombinations, int ruleSetId)
			{
				var tempIndex = possibleCombinations;
				foreach (var ruleId in _ruleSets[ruleSetId])
				{
					var res = _ruleBook[ruleId].Matches(toValidate, tempIndex);
					if (!res.isValid)
					{
						return (false, null);
					}

					tempIndex = res.validCombinations;
				}

				return (true, tempIndex);
			}
		}

		private const int Day = 19;		

		private Dictionary<int, IRule> _ruleBook = new Dictionary<int, IRule>();

		private List<string> _messages = new List<string>();

		public Solver()
		{
			var switchToMessagePopulation = false;
			var input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt")
				.ToList();

			foreach(var line in input)
			{
				if (line == string.Empty)
				{
					switchToMessagePopulation = true;
					continue;
				}

				if (switchToMessagePopulation)
				{
					_messages.Add(line);
				}else
				{
					var split = line.Split(": ");
					_ruleBook.Add(int.Parse(split[0]), line.Contains("\"") ? new FixedCharacterRule(split[1]) : new PositionRule(split[1], _ruleBook));
				}
			}
		}

		public string SolveFirstTask()
		{
			int validOnes = Solve();

			return $"{validOnes}";
		}

		public string SolveSecondTask()
		{
			_ruleBook[8] = new PositionRule("42 | 42 8", _ruleBook);
			_ruleBook[11] = new PositionRule("42 31 | 42 11 31", _ruleBook);

			int validOnes = Solve();

			return $"{validOnes}";
		}

		private int Solve()
		{
			int validOnes = 0;
			foreach (var message in _messages)
			{
				var res = _ruleBook[0].Matches(message, new List<string> { string.Empty });

				if (res.isValid && res.validCombinations.Any(x => x.Length == message.Length))
				{
					validOnes++;
				}
			}

			return validOnes;
		}
	}
}
