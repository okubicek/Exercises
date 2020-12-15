using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Year2020.Day15
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 15;

		private List<int> _input;

		public Solver()
		{
			_input = new List<int> { 14, 8, 16, 0, 1, 17 };
		}

		public string SolveFirstTask()
		{
			var turnToCheck = 30000000;
			int lastNumberSpoken = FindTurnValue(2020);

			return $"{lastNumberSpoken}";
		}


		public string SolveSecondTask()
		{
			int lastNumberSpoken = FindTurnValue(30000000);

			return $"{lastNumberSpoken}";
		}

		private int FindTurnValue(int turnToCheck)
		{
			int turn = _input.Count + 1;
			var gameMemory = Initialize();

			var lastNumberSpoken = _input.Last();
			while (turn <= turnToCheck)
			{
				var newNumberSpoken = gameMemory.TryGetValue(lastNumberSpoken, out var pastTurn) ? (turn - 1) - pastTurn : 0;
				PutToMemory(gameMemory, lastNumberSpoken, turn - 1);

				lastNumberSpoken = newNumberSpoken;
				turn++;
			}

			return lastNumberSpoken;
		}

		private Dictionary<int, int> Initialize()
		{
			var memory = new Dictionary<int, int>();

			for(int i = 1; i < _input.Count; i++)
			{
				PutToMemory(memory, _input[i-1], i);
			}

			return memory;
		}

		private void PutToMemory(Dictionary<int, int> memory, int val, int turn)
		{
			if (!memory.TryAdd(val, turn))
			{
				memory[val] = turn;
			}
		}
	}
}
