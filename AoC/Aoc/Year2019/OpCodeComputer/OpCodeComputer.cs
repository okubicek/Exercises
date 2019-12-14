using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.OpCodeComputer
{
	public class OpCodeComputer
	{
		private ComputerMemory _memory;

		private IInputChannel _input;

		private IOutputChannel _output;

		private long _relativeBase = 0;

		public OpCodeComputer(IInputChannel input, 
			IOutputChannel output)
		{
			_input = input;
			_output = output;
		}

		private enum Mode
		{
			Position,
			Value,
			Relative
		}

		public void ProcessInstructions(IEnumerable<long> program)
		{
			FillMemoryWithProgram(program);

			var programLength = _memory.Count;
			int ptr = 0;

			while (ptr < programLength && _memory[ptr] != 99)
			{
				var code = _memory[ptr];

				var opCode = code % 10;
				var nounMode = GetMode((code / 100) % 10);
				var verbMode = GetMode((code / 1000) % 10);
				var resMode = GetMode((code / 10000) % 10);

				ptr = ProcessOpcodeAndMove(opCode, nounMode, verbMode, resMode, ptr);
			}
		}

		private void FillMemoryWithProgram(IEnumerable<long> program)
		{
			_memory = new ComputerMemory(program, 100000);
		}

		private Mode GetMode(long code)
		{
			switch(code)
			{
				case 0:
					return Mode.Position;
				case 1:
					return Mode.Value;
				case 2:
					return Mode.Relative;
				default:
					throw new NotImplementedException($"Mode {code} is not supported");
			}
		}

		private long GetValueByMode(Mode mode, long value)
		{
			switch (mode)
			{
				case Mode.Position:
					return _memory[value];
				case Mode.Value:
					return value;
				case Mode.Relative:
					return _memory[value + _relativeBase];
				default:
					throw new NotImplementedException($"Unknown mode {mode}");
			}
		}

		private int ProcessOpcodeAndMove(long opcode, Mode nounMode, Mode verbMode, Mode resMode, int ptr)
		{
			switch (opcode)
			{
				case 1:
					SetMemory(_memory[ptr + 3], resMode ,GetValueByMode(nounMode, _memory[ptr + 1]) + GetValueByMode(verbMode, _memory[ptr + 2]));
					return ptr + 4;
				case 2:
					SetMemory(_memory[ptr + 3], resMode, GetValueByMode(nounMode, _memory[ptr + 1]) * GetValueByMode(verbMode, _memory[ptr + 2]));
					return ptr + 4;
				case 3:
					SetMemory(_memory[ptr + 1], nounMode, _input.GetNext());
					return ptr + 2;
				case 4:
					_output.Send(GetValueByMode(nounMode, _memory[ptr + 1]));
					return ptr + 2;
				case 5:
					if (GetValueByMode(nounMode, _memory[ptr + 1]) != 0)
					{
						return (int)GetValueByMode(verbMode, _memory[ptr + 2]);
					}

					return ptr + 3;
				case 6:
					if (GetValueByMode(nounMode, _memory[ptr + 1]) == 0)
					{
						return (int)GetValueByMode(verbMode, _memory[ptr + 2]);
					}

					return ptr + 3;
				case 7:
					SetMemory(_memory[ptr + 3], resMode ,GetValueByMode(nounMode, _memory[ptr + 1]) < GetValueByMode(verbMode, _memory[ptr + 2])
							? 1
							: 0);
					return ptr + 4;
				case 8:
					SetMemory(_memory[ptr + 3], resMode, GetValueByMode(nounMode, _memory[ptr + 1]) == GetValueByMode(verbMode, _memory[ptr + 2])
							? 1
							: 0);
					return ptr + 4;
				case 9:
					_relativeBase += GetValueByMode(nounMode, _memory[ptr + 1]);
					return ptr + 2;
				default:
					throw new NotImplementedException($"Invalid value {opcode}");
			}
		}

		private void SetMemory(long address, Mode mode, long value)
		{
			address = mode == Mode.Relative ? address + _relativeBase : address;
			_memory[address] = value;
		}
	}
}
