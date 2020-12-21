using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day19
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private interface IRule
		{
			(bool isValid, int index) Matches(string toValidate, int index);
		}

		private class FixedCharacterRule : IRule
		{
			public FixedCharacterRule(string input)
			{
				_character = input.Replace("\"", string.Empty)[0];
			}

			private char _character;

			public (bool isValid, int index) Matches(string toValidate, int index)
			{
				return index < toValidate.Length ? (_character == toValidate[index], index + 1) : (false, index);
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

			public (bool isValid, int index) Matches(string toValidate, int index)
			{				
				for (var i = 0; i < _ruleSets.Count; i++)
				{
					var res = EvaluateRuleSet(toValidate, index, i);
					if (res.isValid)
					{
						return (true, res.index);
					}
				}

				return (false, index);
			}

			private (bool isValid, int index) EvaluateRuleSet(string toValidate, int index, int i)
			{
				var tempIndex = index;
				foreach (var ruleId in _ruleSets[i])
				{
					var res = _ruleBook[ruleId].Matches(toValidate, tempIndex);
					if (!res.isValid)
					{
						return (false, res.index);
					}

					tempIndex = res.index;
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
			int validOnes = 0;
			foreach(var message in _messages)
			{
				var res = _ruleBook[0].Matches(message, 0);

				if (res.isValid && res.index == message.Length)
				{
					validOnes++;
				}
			}

			return $"{validOnes}";
		}

		public string SolveSecondTask()
		{
			int validOnes = 0;
			_ruleBook[8] = new PositionRule("42 | 42 8", _ruleBook);
			_ruleBook[11] = new PositionRule("42 31 | 42 11 31", _ruleBook);
			foreach (var message in _messages)
			{
				var res = _ruleBook[0].Matches(message, 0);

				if (res.isValid && res.index == message.Length)
				{
					validOnes++;
				}
			}

			return $"{validOnes}";
		}
	}
}
