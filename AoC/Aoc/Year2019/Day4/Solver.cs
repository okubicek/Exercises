namespace Aoc.Year2019.Day4
{
	public class Solver : IPuzzleSolver
	{
		public int Day => 4;

		private int _min = 254032;
		private int _max = 789860;

		public string SolveFirstTask()
		{
			var conditionFullfilledCounter = 0;

			for (int i = _min; i <= _max; i++)
			{
				var numstr = i.ToString();
				if (HasTwoMatchingDigits(numstr) && NumbersNeverDecrease(numstr))
				{
					conditionFullfilledCounter++;
				}
			}

			return conditionFullfilledCounter.ToString();
		}

		public string SolveSecondTask()
		{
			var conditionFullfilledCounter = 0;

			for (int i = _min; i <= _max; i++)
			{
				var numstr = i.ToString();
				if (HasTwoMatchingDigits(numstr) && NumbersNeverDecrease(numstr) && HasAtLeastSingleGroupWithTwoDigits(numstr))
				{
					conditionFullfilledCounter++;
				}
			}

			return conditionFullfilledCounter.ToString();
		}

		private bool HasTwoMatchingDigits(string number)
		{
			for (int i = 0; i < number.Length - 1; i++)
			{
				if (number[i] == number[i + 1])
				{
					return true;
				}
			}

			return false;
		}

		private bool NumbersNeverDecrease(string number)
		{
			for(int i = 0; i < number.Length - 1; i++)
			{
				if(number[i] > number[i+1])
				{
					return false;
				}
			}

			return true;
		}

		private bool HasAtLeastSingleGroupWithTwoDigits(string numstr)
		{
			var counter = 0;

			for (int i = 0; i < numstr.Length - 1; i++)
			{
				if (numstr[i] == numstr[i + 1])
				{
					counter++;
				}
				else
				{
					if (counter == 1)
					{
						return true;
					}

					counter = 0;
				}
			}

			return counter == 1;
		}
	}
}
