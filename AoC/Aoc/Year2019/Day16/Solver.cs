using System;
using System.Text;

namespace Aoc.Year2019.Day16
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 16;

		private string _sequence;

		private int[] _basePattern = new int[] { 0, 1, 0, -1 };

		public Solver()
		{
			_sequence = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0];
		}

		public string SolveFirstTask()
		{
			var sequence = CalculateSequence(100, _sequence);

			return sequence.Substring(0, 8);
		}

		public string SolveSecondTask()
		{
			var startingSequence = "".PadLeft(10000, '.').Replace(".", _sequence);

			var toSkip = int.Parse(startingSequence.Substring(0, 7));
			var seq = startingSequence.Substring(toSkip, startingSequence.Length - toSkip);

			var sequenceBuilder = new StringBuilder();
			var value = 0;
			for(int i = 0; i < 100; i++)
			{
				for (int j = seq.Length - 1; j >= 0; j--)
				{
					value += (int)char.GetNumericValue(seq[j]);
					sequenceBuilder.Append(value % 10);
				}

				value = 0;
				seq = Reverse(sequenceBuilder.ToString());
				sequenceBuilder.Clear();
			}

			return seq.Substring(0, 8); ;
		}

		private static string Reverse(string sequence)
		{
			var toReverse = sequence.ToCharArray();
			Array.Reverse(toReverse);

			var seq = new string(toReverse);
			return seq;
		}

		private string CalculateSequence(int phases, string sequence)
		{
			var newSequenceBuilder = new StringBuilder();

			for (var i = 0; i < phases; i++)
			{
				for (var j = 1; j <= sequence.Length; j++)
				{
					newSequenceBuilder.Append(GetSequenceValue(j, sequence));
				}

				sequence = newSequenceBuilder.ToString();
				newSequenceBuilder.Clear();
			}

			return sequence;
		}

		private string GetSequenceValue(int repeats, string sequence)
		{
			var sum = 0;
			for (int i = 0; i < sequence.Length; i++)
			{
				sum += (int)char.GetNumericValue(sequence[i]) * GetBasePattern(i, repeats);
			}

			return (Math.Abs(sum) % 10).ToString();
		}		

		private int GetBasePattern(int position, int repeats)
		{
			var sequenceLength = repeats * _basePattern.Length;
			var positionWithinSequence = (position + 1) % sequenceLength;
			return _basePattern[positionWithinSequence / repeats];
		}
	}
}
