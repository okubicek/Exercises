using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Aoc.Year2019.Day8
{
	[Aoc(Day=Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 8;

		private string _input;

		public Solver()
		{
			_input = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt")[0];
		}

		public string SolveFirstTask()
		{
			var size = new Size(25,6);

			var min = int.MaxValue;
			var minLayer = string.Empty;

			var layers = ConvertInputToLayers(size);
			
			foreach(var layer in layers)
			{ 
				var count0 = CountNumberOfTimesCharacterIsPresent(layer, '0');
				if (min > count0)
				{
					min = count0;
					minLayer = layer;
				}
			}

			return (CountNumberOfTimesCharacterIsPresent(minLayer, '1') * 
				CountNumberOfTimesCharacterIsPresent(minLayer, '2'))
				.ToString();
		}

		private List<string> ConvertInputToLayers(Size size)
		{
			var numberOfPixelsInLayer = size.Height * size.Width;
			var layers = new List<string>();

			for (int i = 0; i < _input.Length; i = i + numberOfPixelsInLayer)
			{
				var layer = _input.Substring(i, numberOfPixelsInLayer);
				layers.Add(layer);
			}

			return layers;
		}

		public int CountNumberOfTimesCharacterIsPresent(string str, char character)
		{
			return str.Where(x => x == character).Count();
		}

		public string SolveSecondTask()
		{
			var size = new Size(25,6);
			var layers = ConvertInputToLayers(size);

			var output = new StringBuilder();

			for(int i = 0; i < size.Height * size.Width; i++)
			{
				var outputBit = '2';
				layers.ForEach(layer =>
				{
					if (outputBit != '1' && outputBit != '0')
					{
						outputBit = layer[i];
					}
				});

				if (i % size.Width == 0)
				{
					output.Append(Environment.NewLine);
				}

				output.Append(outputBit == '0' ? ' ' : '-');
			}

			return output.ToString();
		}
	}
}
