using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.Day6
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private new Dictionary<string, Node> _orbitMap;

		private class Node
		{
			private Node _parent;

			private string _uid;

			private List<Node> _childs;

			public Node(string uid)
			{
				_uid = uid;
				_childs = new List<Node>();
			}

			public void AddChild(Node child)
			{
				_childs.Add(child);
			}

			public int GetNumberOfOrbits()
			{
				if (_parent == null)
				{
					return 0;
				}

				return 1 + _parent.GetNumberOfOrbits();
			}

			public void SetParent(Node parent)
			{
				if (_parent != null)
				{
					throw new ApplicationException("Parent is already set for this Node");
				}

				_parent = parent;
			}

			public int IsSantaHere(Node caller)
			{
				if (_uid.Equals("SAN")) return 1;

				if (_childs != null)
				{
					var stepsToSanta = _childs.Select(x => x.IsSantaHere(this)).Sum();
					if (stepsToSanta > 0)
					{
						return 1 + stepsToSanta;
					}
				}

				if (caller == _parent) return 0;

				return 1 + _parent.IsSantaHere(this);
			}
		}

		private const int Day = 6;

		public Solver()
		{
			_orbitMap = new Dictionary<string, Node>();
			var input = InputFileReader.GetInput($"Year2019/Inputs/Day{Day}.txt");

			BuildOrbitMap(input);
		}

		private void BuildOrbitMap(List<string> input)
		{
			foreach (var orbit in input)
			{
				var orbited = RegisterNode(orbit.Split(')')[0]);
				var orbiting = RegisterNode(orbit.Split(')')[1]);

				orbited.AddChild(orbiting);
				orbiting.SetParent(orbited);
			}
		}

		public string SolveFirstTask()
		{
			return _orbitMap.Select(x => x.Value.GetNumberOfOrbits()).Sum().ToString();
		}

		private Node RegisterNode(string objectKey)
		{
			if (!_orbitMap.ContainsKey(objectKey))
			{
				_orbitMap.Add(objectKey, new Node(objectKey));
			}

			return _orbitMap[objectKey];
		}

		public string SolveSecondTask()
		{
			var myNode = _orbitMap["YOU"];

			return (myNode.IsSantaHere(null) - 3).ToString();
		}
	}
}
