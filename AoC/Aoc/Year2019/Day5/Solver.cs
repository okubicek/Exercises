using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.Day5
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 5;

		private List<int> _memory;

		private InputSource _input;

		private Output _output;

		private class InputSource
		{
			private Stack<int> _stack = new Stack<int>(new[] { 5 });

			public int GetNext()
			{
				return _stack.Pop();
			}
		}

		private class Output
		{
			public void Send(int val)
			{
				Console.WriteLine($"Output: {val}");
			}
		}

		public Solver()
		{
			_output = new Output();
			Initialize();
		}

		public string SolveFirstTask()
		{
			Initialize();
			var length = _memory.Count;
			int ptr = 0;

			while(ptr < length && _memory[ptr] != 99)
			{
				var code = _memory[ptr];

				var opCode = code % 10;
				var nounMode = GetMode((code / 100) % 10);
				var verbMode = GetMode((code / 1000) % 10);
				
				ptr = ProcessOpcodeAndMove(opCode, nounMode, verbMode, ptr);
			}

			return string.Empty;
		}

		private void Initialize()
		{
			_input = new InputSource();
			_memory = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0]
				.Split(',')
				.Select(x => int.Parse(x))
				.ToList();
		}

		private enum Mode
		{
			Position,
			Value
		}

		private Mode GetMode(int code)
		{
			return code == 1 ? Mode.Value : Mode.Position;
		}

		private int GetValueByMode(Mode mode, int value)
		{
			switch(mode)
			{
				case Mode.Position:
					return _memory[value];
				case Mode.Value:
					return value;
				default:
					throw new NotImplementedException($"Unknown mode {mode}");
			}
		}

		private int ProcessOpcodeAndMove(int opcode, Mode nounMode, Mode verbMode, int ptr)
		{
			switch (opcode)
			{
				case 1:
					_memory[_memory[ptr + 3]] = GetValueByMode(nounMode, _memory[ptr + 1]) + GetValueByMode(verbMode, _memory[ptr + 2]);
					return ptr + 4;
				case 2:
					_memory[_memory[ptr + 3]] = GetValueByMode(nounMode, _memory[ptr + 1]) * GetValueByMode(verbMode, _memory[ptr + 2]);
					return ptr + 4;
				case 3:
					_memory[_memory[ptr + 1]] = _input.GetNext();
					return ptr + 2;
				case 4:
					Console.WriteLine($"Output: {GetValueByMode(nounMode, _memory[ptr + 1])}");
					return ptr + 2;
				case 5:
					if (GetValueByMode(nounMode, _memory[ptr + 1]) != 0)
					{
						return GetValueByMode(verbMode, _memory[ptr + 2]);
					}
					
					return ptr + 3;
				case 6:
					if (GetValueByMode(nounMode, _memory[ptr + 1]) == 0)
					{
						return GetValueByMode(verbMode, _memory[ptr + 2]);
					}

					return ptr + 3;
				case 7:					
					_memory[_memory[ptr + 3]] = GetValueByMode(nounMode, _memory[ptr + 1]) < GetValueByMode(verbMode, _memory[ptr + 2])
							?1
							:0;
					return ptr + 4;
				case 8:
					_memory[_memory[ptr + 3]] = GetValueByMode(nounMode, _memory[ptr + 1]) == GetValueByMode(verbMode, _memory[ptr + 2])
							? 1
							: 0;
					return ptr + 4;
				default:
					throw new NotImplementedException($"Invalid value {opcode}");
			}
		}

		public string SolveSecondTask()
		{
			return string.Empty;
		}
	}
}
