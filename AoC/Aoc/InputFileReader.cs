using System.Collections.Generic;
using System.IO;

namespace Aoc
{
	public class InputFileReader
	{
		public static List<string> GetInput(string fileName)
		{
			var streamReader = new StreamReader(fileName);
			var inputList = new List<string>();

			var line = streamReader.ReadLine();
			while (line != null)
			{
				inputList.Add(line);
				line = streamReader.ReadLine();
			}

			return inputList;
		}
	}
}
