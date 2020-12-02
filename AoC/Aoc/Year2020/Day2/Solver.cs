using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day2
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 2;

		private List<CorporatePassword> _passwords;

		public Solver()
		{
			_passwords = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt")
				.Select(x => {
					var parts = x.Split(' ');
					var policy = parts[0].Split('-');
					return new CorporatePassword
					{
						Letter = parts[1].Replace(":", string.Empty)[0],
						PolicyField2 = int.Parse(policy[0]),
						PolicyField1 = int.Parse(policy[1]),
						Password = parts[2]
					}; 
				}).ToList();
		}

		public string SolveFirstTask()
		{
			var validOnes = _passwords.Count(x => x.IsFirstPolicyValid());
			return $"{validOnes}";
		}

		public string SolveSecondTask()
		{
			var validOnes = _passwords.Count(x => x.IsSecondPolicyValid());
			return $"{validOnes}";
		}
	}

	public class CorporatePassword
	{
		public string Password { get; init; }

		public char Letter { get; init; }

		public int PolicyField1 { get; init; }

		public int PolicyField2 { get; init; }

		public bool IsFirstPolicyValid()
		{
			var numOfMatches = Password.Count(x => x == Letter);
			return numOfMatches >= PolicyField2 && numOfMatches <= PolicyField1 ? true : false;			
		}

		public bool IsSecondPolicyValid()
		{
			return Password[PolicyField1 - 1] == Letter ^ Password[PolicyField2 - 1] == Letter 
				? true 
				: false;			
		}
	}
}
