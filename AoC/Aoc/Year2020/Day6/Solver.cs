using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day6
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 6;

		private List<string> _input = new List<string>();

		public Solver()
		{
			_input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");			
		}

		private List<string> GroupAnswersEverybodyAnsweredYes(List<string> input)
		{
			var result = new List<string>();
			string groupAnswers = null;

			foreach (var answers in input)
			{								
				if (answers == string.Empty)
				{
					result.Add(groupAnswers);
					groupAnswers = null;
				}else
				{
					groupAnswers = groupAnswers == null
						? answers
						: string.Concat(groupAnswers.Intersect(answers));
				}
			}

			result.Add(groupAnswers);

			return result;
		}

		private List<HashSet<char>> GroupAnswersSomebodyAnsweredYes(List<string> input)
		{
			var result = new List<HashSet<char>>();
			var groupAnswers = new HashSet<char>();

			foreach (var answers in input)
			{
				if (answers == string.Empty)
				{
					result.Add(groupAnswers);
					groupAnswers = new HashSet<char>();
				}
				else
				{
					foreach (var answer in answers)
					{
						groupAnswers.Add(answer);
					}
				}
			}

			result.Add(groupAnswers);

			return result;
		}

		public string SolveFirstTask()
		{
			var answers = GroupAnswersSomebodyAnsweredYes(_input);
			var res = answers.Select(x => x.Count()).Sum();
			return $"{res}";
		}


		public string SolveSecondTask()
		{
			var answers = GroupAnswersEverybodyAnsweredYes(_input);
			var res = answers.Select(x => x.Length).Sum();
			return $"{res}";
		}
	}
}
