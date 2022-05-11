using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect4AI.AI;

namespace Connect4AI.CLI {
	public static class RenderingManager {
		public static ConsoleColor BorderColor = ConsoleColor.White;
		public static ConsoleColor SelectColor = ConsoleColor.Yellow;

		public static void Render(BoardState board) {
			//Console.Clear();
			Console.WriteLine("╠══════╬══════╬══════╬══════╬══════╬══════╬══════╬");
			for(int y = board.MaxHeight; y >= 0; y--) {
				for(int pipis = 0; pipis < 3; pipis++) {
					Console.Write("║");
					for(int x = 0; x < board.Board.Length; x++) {
						Player? player = board[x, y];
						Console.ForegroundColor = player.GetColor();
						Console.Write($"{player.Render()}");
						Console.ForegroundColor = ConsoleColor.White;
						Console.Write($"║");
					}
					Console.WriteLine();
				}
				Console.WriteLine("╠══════╬══════╬══════╬══════╬══════╬══════╬══════╬");
			}
			try {
				Console.WriteLine($"Immediate score of current board for player {Player.Player1}: {board.GetScore(Player.Player1, 0, Player.Player1)}");
				Console.WriteLine($"Immediate score of current board for player {Player.Player2}: {board.GetScore(Player.Player2, 0, Player.Player2)}");
				Console.WriteLine($"Highest match for player {Player.Player1}: {board.GetAllMatches(Player.Player1).Max()}");
				Console.WriteLine($"Highest match for player {Player.Player2}: {board.GetAllMatches(Player.Player2).Max()}");
			} catch(Exception e) {
				Console.WriteLine(e.GetType().Name);
			}
		}
	}
}
