using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Aoc.Helpers
{
	public class ToConsoleDrawer
	{
		public static void DrawFromDictionary(Dictionary<Point, char> dict, char defaultValue)
		{
			var minX = dict.Select(c => c.Key.X).Min();
			var minY = dict.Select(c => c.Key.Y).Min();
			var xSize = dict.Select(c => c.Key.X).Max() - minX;
			var ySize = dict.Select(c => c.Key.Y).Max() - minY;

			var output = new StringBuilder();
			for (int y = 0; y <= ySize; y++)
			{
				for (int x = 0; x <= xSize; x++)
				{
					output.Append(
						dict.TryGetValue(new Point(x + minX, y + minY), out char type) ? type : defaultValue);					
				}

				output.Append(Environment.NewLine);
			}

			Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
			Console.WriteLine(output);
		}
	}
}
