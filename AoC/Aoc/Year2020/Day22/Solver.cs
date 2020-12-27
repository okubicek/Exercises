using System.Collections.Generic;
using System.Linq;

namespace Aoc.Year2020.Day22
{
	[Aoc(Day = Day)]
	public class Solver : IPuzzleSolver
	{
		private const int Day = 22;

		private Queue<int> _playerOne;

		private Queue<int> _playerTwo;

		public Solver()
		{
			Initialize();
		}

		public string SolveFirstTask()
		{
			Play(_playerOne, _playerTwo, false);

			var winningDeck = _playerTwo.Any() ? _playerTwo : _playerOne;

			int res = CalculateScore(winningDeck);

			return $"{res}";
		}

		public string SolveSecondTask()
		{
			Initialize();
			Play(_playerOne, _playerTwo, true);

			var winningDeck = _playerTwo.Any() ? _playerTwo : _playerOne;

			int res = CalculateScore(winningDeck);

			return $"{res}";
		}

		private void Initialize()
		{
			_playerOne = new Queue<int>();
			_playerTwo = new Queue<int>();
			var input = InputFileReader.GetInput($"Year2020/Inputs/Day{Day}.txt").ToList();
			var playerId = 0;
			foreach (var line in input)
			{
				if (line.StartsWith("Player"))
				{
					playerId++;
				}
				else if (line != string.Empty)
				{
					if (playerId == 1)
					{
						_playerOne.Enqueue(int.Parse(line));
					}
					else
					{
						_playerTwo.Enqueue(int.Parse(line));
					}
				}
			}
		}		

		private int Play(Queue<int> playerOneDeck, Queue<int> playerTwoDeck, bool extendedRules)
		{
			var memory = new HashSet<string>();

			while (playerOneDeck.Any() && playerTwoDeck.Any())
			{
				var roundUid = $"{string.Join(",", playerOneDeck)},{string.Join(",", playerTwoDeck)}";

				var playerOneCard = playerOneDeck.Dequeue();
				var playerTwoCard = playerTwoDeck.Dequeue();

				int winner;
				if (memory.Contains(roundUid) && extendedRules)
				{
					winner = 1;
				}
				else if ((playerOneDeck.Count >= playerOneCard && playerTwoDeck.Count >= playerTwoCard) && extendedRules)
				{
					winner = Play(new Queue<int>(playerOneDeck.Take(playerOneCard)),
						new Queue<int>(playerTwoDeck.Take(playerTwoCard)), extendedRules);
				}
				else
				{
					winner = playerOneCard > playerTwoCard ? 1 : 2;
				}

				if (winner == 1)
				{
					playerOneDeck.Enqueue(playerOneCard);
					playerOneDeck.Enqueue(playerTwoCard);
				}
				else
				{
					playerTwoDeck.Enqueue(playerTwoCard);
					playerTwoDeck.Enqueue(playerOneCard);					
				}

				memory.Add(roundUid);
			}

			return playerOneDeck.Any() ? 1 : 2;
		}

		private static int CalculateScore(Queue<int> winningDeck)
		{
			var res = 0;
			for (int i = winningDeck.Count; i > 0; i--)
			{
				res += winningDeck.Dequeue() * i;
			}

			return res;
		}		
	}
}
