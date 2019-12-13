using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aoc.Year2019.OpCodeComputer
{
	public class OpCodeComputer
	{
		private List<int> _memory;

		private IInputChannel _input;

		private IOutputChannel _output;

		public OpCodeComputer(IInputChannel input, 
			IOutputChannel output)
		{
			_input = input;
			_output = output;
		}

		private enum Mode
		{
			Position,
			Value
		}

		public void ProcessInstructions(IEnumerable<int> instructionSet)
		{
			FillMemoryWithInstructions(instructionSet);

			var length = _memory.Count;
			int ptr = 0;

			while (ptr < length && _memory[ptr] != 99)
			{
				var code = _memory[ptr];

				var opCode = code % 10;
				var nounMode = GetMode((code / 100) % 10);
				var verbMode = GetMode((code / 1000) % 10);

				ptr = ProcessOpcodeAndMove(opCode, nounMode, verbMode, ptr);
			}
		}

		private void FillMemoryWithInstructions(IEnumerable<int> instructionSet)
		{
			_memory = instructionSet.ToList();
		}

		private Mode GetMode(int code)
		{
			return code == 1 ? Mode.Value : Mode.Position;
		}

		private int GetValueByMode(Mode mode, int value)
		{
			switch (mode)
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
					_output.Send(GetValueByMode(nounMode, _memory[ptr + 1]));
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
							? 1
							: 0;
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
	}
}
