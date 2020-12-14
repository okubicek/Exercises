using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Year2020.Day14
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 14;

		private List<Set> _instructions = new List<Set>();

		private Dictionary<long, long> _memory;
		private class Set
		{
			public Set(string mask)
			{
				Mask = mask;
			}

			public string Mask { get; }

			public List<(long Address, long Value)> Instructions { get; } = new List<(long Address, long Value)>();
		}

		public Solver()
		{
			var input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt");

			input.ForEach(i =>
			{
				if(i.StartsWith("mask"))
				{
					_instructions.Add(new Set(i.Replace("mask = ", string.Empty)));
				}
				else
				{
					var ins = _instructions.Last();
					var split = i.Split(" = ");
					ins.Instructions.Add((
						long.Parse(split[0].Replace("mem[", string.Empty).Replace("]", string.Empty)),
						long.Parse(split[1])));
				}
			});
		}

		public string SolveFirstTask()
		{
			_memory = new Dictionary<long, long>();

			foreach(var set in _instructions)
			{
				foreach(var ins in set.Instructions)
				{
					var number = ins.Value;
					for (int i = 0; i < 36; i++)
					{
						switch (set.Mask[35 - i])
						{
							case '1':
								number |= (long)(1UL << i);
								break;
							case '0':
								number &= (long)~(1UL << i);
								break;
						}
					}

					SetMemory(ins.Address, number);
				}
			}

			return $"{_memory.Sum(x => (decimal)x.Value)}";
		}

		private void SetMemory(long address, long value)
		{
			if (_memory.ContainsKey(address))
			{
				_memory[address] = value;
			}
			else
			{
				_memory.Add(address, value);
			}
		}

		public string SolveSecondTask()
		{
			_memory = new Dictionary<long, long>();

			foreach (var set in _instructions)
			{
				foreach (var ins in set.Instructions)
				{
					var number = ins.Address;
					var wildCardIndexes = new List<int>();
					for (int bitPos = 0; bitPos < 36; bitPos++)
					{
						switch (set.Mask[35 - bitPos])
						{
							case '1':
								number = SetBit(number, bitPos);
								break;
							case 'X':
								wildCardIndexes.Add(bitPos);
								number = ResetBit(number, bitPos);
								break;
						}
					}

					var memoryAddresses = new List<long> { number };
					wildCardIndexes.ForEach(wc => memoryAddresses.AddRange(memoryAddresses.Select(ma => SetBit(ma, wc)).ToList()));
					memoryAddresses.ForEach(ma => 
					{
						SetMemory(ma, ins.Value);
					});					
				}				
			}

			return $"{_memory.Sum(x => (decimal)x.Value)}";
		}

		private static long ResetBit(long number, int bitPos)
		{
			number &= (long)~(1UL << bitPos);
			return number;
		}

		private static long SetBit(long number, int bitPos)
		{
			number |= (long)(1UL << bitPos);
			return number;
		}
	}
}
