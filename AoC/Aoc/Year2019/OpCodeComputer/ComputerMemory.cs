using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.OpCodeComputer
{
	public class ComputerMemory : List<long>
	{
		public ComputerMemory(IEnumerable<long> program, int memorySize) : base(memorySize)
		{
			InsertRange(0, program);
			AddRange(Enumerable.Repeat<long>(0, memorySize - program.Count()));
		}

		public long this[long i]
		{
			get { return base[(int)i]; }
			set { base[(int)i] = value; }
		}
	}
}
