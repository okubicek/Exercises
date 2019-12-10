using Aoc.Year2019.OpCodeComputer;
using System;

namespace Aoc.Year2019.Day5
{
	public class OutputChannel : IOutputChannel
	{
		public void Send(int val)
		{
			Console.WriteLine($"Output: {val}");
		}
	}
}
