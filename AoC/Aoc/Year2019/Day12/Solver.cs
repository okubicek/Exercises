using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2019.Day12
{
	[Aoc(Day = 12)]
	public class Solver : IPuzzleSolver
	{
		public class Position
		{
			public Position(int x, int y, int z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			public int X { get; set; }

			public int Y { get; set; }

			public int Z { get; set; }
		}

		public class Velocity
		{
			public Velocity(int x, int y, int z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			public int X { get; set; }

			public int Y { get; set; }

			public int Z { get; set; }
		}

		public class Moon
		{
			private Position _position;

			private Velocity _velocity;

			public Moon(Position initialPosition, Velocity initialVelocity)
			{
				_position = initialPosition;
				_velocity = initialVelocity;
			}

			public void ApplyGravity(Position otherMoonPosition)
			{
				_velocity.X += CalculateGravityIncrement(otherMoonPosition.X, _position.X);
				_velocity.Y += CalculateGravityIncrement(otherMoonPosition.Y, _position.Y);
				_velocity.Z += CalculateGravityIncrement(otherMoonPosition.Z, _position.Z);
			}

			public void ApplyVelocity()
			{
				_position.X += _velocity.X;
				_position.Y += _velocity.Y;
				_position.Z += _velocity.Z;
			}

			public Position GetPosition() => _position;

			public Velocity GetVelocity () => _velocity;

			public long GetTotalEnergy()
			{
				return (Math.Abs(_velocity.X) + Math.Abs(_velocity.Y) + Math.Abs(_velocity.Z)) * 
					(Math.Abs(_position.X) + Math.Abs(_position.Y) + Math.Abs(_position.Z));
			}

			private int CalculateGravityIncrement(int otherMoonAxisPosition, int currentMoonAxisPosition)
			{
				if (otherMoonAxisPosition > currentMoonAxisPosition)
				{
					return 1;
				}

				if (otherMoonAxisPosition < currentMoonAxisPosition)
				{
					return -1;
				}

				return 0;
			}
		}

		public string SolveFirstTask()
		{
			List<Moon> moons = GetInitConstelation();

			var moonCount = moons.Count;
			for (int step = 0; step < 1000; step++)
			{
				SimulateOneStepInUniverse(moons, moonCount);				
			}

			return moons.Sum(x => x.GetTotalEnergy()).ToString();
		}

		private static void SimulateOneStepInUniverse(List<Moon> moons, int moonCount)
		{
			for (int checkedMoonIndex = 0; checkedMoonIndex < moonCount; checkedMoonIndex++)
			{
				for (int otherMoonIndex = 0; otherMoonIndex < moonCount; otherMoonIndex++)
				{
					if (otherMoonIndex != checkedMoonIndex)
					{
						moons[checkedMoonIndex].ApplyGravity(moons[otherMoonIndex].GetPosition());
					}
				}
			}

			moons.ForEach(x => { x.ApplyVelocity(); });
		}

		private static List<Moon> GetInitConstelation()
		{
			return new List<Moon>
			{
				new Moon(new Position(19, -10, 7), new Velocity(0, 0, 0)),
				new Moon(new Position(1, 2, -3), new Velocity(0, 0, 0)),
				new Moon(new Position(14, -4, 1), new Velocity(0, 0, 0)),
				new Moon(new Position(8, 7, -6), new Velocity(0, 0, 0))
				//new Moon(new Position(-8, -10, 0), new Velocity(0, 0, 0)),
				//new Moon(new Position(5, 5, 10), new Velocity(0, 0, 0)),
				//new Moon(new Position(2, -7, 3), new Velocity(0, 0, 0)),
				//new Moon(new Position(9, -8, -3), new Velocity(0, 0, 0))
			};
		}

		public string SolveSecondTask()
		{
			var cycleRepeatPerAxis = new List<(char axis, int step)>();
			List<Moon> moons = GetInitConstelation();

			var uniqueCombinations = new Dictionary<char, Dictionary<string, int>>{
				{ 'x', new Dictionary<string, int>() },
				{ 'y', new Dictionary<string, int>() },
				{ 'z', new Dictionary<string, int>() }
			};

			var step = 1;

			var functions = new List<(char axis, Func<Position, int> GetPosition, Func<Velocity, int> GetVelocity)>
			{
				('x', (pos) => pos.X, (vel) => vel.X),
				('y', (pos) => pos.Y, (vel) => vel.Y),
				('z', (pos) => pos.Z, (vel) => vel.Z)
			};

			while (cycleRepeatPerAxis.Count < 3)
			{
				SimulateOneStepInUniverse(moons, moons.Count);

				foreach (var f in functions)
				{
					
					var key = string.Join("_", moons.Select(x => $"{f.GetPosition(x.GetPosition())}_{f.GetVelocity(x.GetVelocity())}"));
					if (!uniqueCombinations[f.axis].ContainsKey(key))
					{
						uniqueCombinations[f.axis].Add(key, step);
					}
					else
					{
						if (!cycleRepeatPerAxis.Any(x => x.axis == f.axis))
						{
							cycleRepeatPerAxis.Add((f.axis, step));
						}
					}
				}

				step++;
			}

			var minRepeat = (long)cycleRepeatPerAxis.Select(x => x.step).Max() - 1;			

			long stepIteration = minRepeat;
			while(cycleRepeatPerAxis.Any(x => stepIteration % (x.step - 1) != 0))
			{
				stepIteration += minRepeat;
			}

			return stepIteration.ToString();
		}
	}
}
