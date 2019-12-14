using Aoc.Year2019.OpCodeComputer;
using System;
using System.Collections.Generic;

namespace Aoc.Year2019.Day9
{
	public class OutputChannel : IOutputChannel
	{
		public List<long> Outputs { get; set; } = new List<long>();

		public void Send(long val)
		{
			Outputs.Add(val);
		}
	}
}
