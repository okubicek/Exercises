using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc.Year2020.Day4
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 4;

		private List<List<(string key, string value)>> _input = new List<List<(string key, string value)>>();
		private List<string> _eyeColors = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

		public Solver()
		{
			var input =  InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");
			var interim = new List<(string key, string value)>();

			foreach(var line in input)
			{
				if (line == string.Empty)
				{
					_input.Add(interim);
					interim = new List<(string key, string value)>();					
				}else
				{
					line.Split(" ").ToList().ForEach(l =>
					{
						var split = l.Split(":");
						interim.Add((split[0], split[1]));
					});
				}

			}

			_input.Add(interim);
		}

		public string SolveFirstTask()
		{
			var res = _input.Where(i => i.Count(k => k.key != "cid") == 7).Count();
			return $"{res}";
		}

		public string SolveSecondTask()
		{
			var res = _input.Where(i => i.Count(k => k.key != "cid") == 7).Where(i => IsValid(i)).Count();
			return $"{res}";
		}

		private bool IsValid(List<(string key, string value)> passportInfo)
		{
			foreach(var info in passportInfo)
			{
				if(!IsValid(info))
				{
					return false;
				}
			}

			return true;
		}

		private bool IsValid((string key, string value) info)
		{
			return (info.key) switch
			{
				"cid" => true,
				"byr" => info.value.Length == 4 && int.TryParse(info.value, out var dob) ? dob >= 1920 && dob <= 2002 : false,
				"iyr" => info.value.Length == 4 && int.TryParse(info.value, out var dob) ? dob >= 2010 && dob <= 2020 : false,
				"eyr" => info.value.Length == 4 && int.TryParse(info.value, out var dob) ? dob >= 2020 && dob <= 2030 : false,
				"hgt" => IsValidHeigth(info.value),
				"ecl" => _eyeColors.Contains(info.value),
				"hcl" => IsValidHairColor(info.value),
				"pid" => IsValidPassport(info.value)
			};
		}

		private bool IsValidPassport(string value)
		{			
			return int.TryParse(value, out _) && value.Length == 9;
		}

		private bool IsValidHeigth(string height)
		{
			var unit = height.Substring(height.Length - 2, 2);

			if (unit == "cm")
			{
				return int.TryParse(height.Substring(0, height.Length - 2), out var value) ? value >= 150 && value <= 193 : false;
			}else if(unit == "in")
			{
				return int.TryParse(height.Substring(0, height.Length - 2), out var value) ? value >= 59 && value <= 76 : false;
			}

			return false;
		}

		private bool IsValidHairColor(string HairColor)
		{
			return HairColor[0] == '#' && Regex.IsMatch(HairColor.Substring(1), "[a-f0-9]");
		}
	}
}
