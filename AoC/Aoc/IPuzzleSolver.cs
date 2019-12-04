namespace Aoc
{
	public interface IPuzzleSolver
	{
		int Day { get; }

		string SolveFirstTask();

		string SolveSecondTask();
	}
}
