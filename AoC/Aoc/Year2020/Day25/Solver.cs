namespace Aoc.Year2020.Day25
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 25;

		private const int _doorPublicKey = 15335876;

		private const int _cardPublicKey = 15086442;

		private const int _divider = 20201227;

		public string SolveFirstTask()
		{
			var value = 1L;
			int loopSize = 0;
			while(value != _doorPublicKey)
			{
				loopSize++;
				value = Tranform(7, value);
			}

			value = 1;
			for(int i = 0; i < loopSize; i++)
			{
				value = Tranform(_cardPublicKey, value);
			}

			return $"{value}";
		}

		private long Tranform(long subject, long value)
		{
			value *= subject;

			return value % _divider;
		}

		public string SolveSecondTask()
		{
			throw new System.NotImplementedException();
		}
	}
}
