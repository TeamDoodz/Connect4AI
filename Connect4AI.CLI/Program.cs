using System;
using System.Threading;
using Connect4AI.AI;

namespace Connect4AI.CLI {
	// POSSIBLE OPTIMIZATIONS:
	// Multithreading - Will make the AI ~80% faster at worst, even faster if I allocate more threads
	// Pre-sort moves - Increases effects of alpha-beta pruning, but requires extensive knowledge of the game (that I don't have)
	// 
	public static class Program {
		private static Move Execute(this PlayerType player, Player p, BoardState board) {
			switch(player) {
				case PlayerType.Human: return new Move(p, int.Parse(Console.ReadLine()));
				case PlayerType.Minimax: return Minimax.GetBestMove(board, p, 6);
				case PlayerType.Random: return Move.Random(p,board);
			}
			return new Move();
		}
		private static PlayerType Get(string message) {
			Console.WriteLine(message);
			Console.WriteLine("1) Human");
			Console.WriteLine("2) Minimax");
			Console.WriteLine("3) Random");
			string choice = Console.ReadLine();
			if(choice == "1") return PlayerType.Human;
			if(choice == "2") return PlayerType.Minimax;
			if(choice == "3") return PlayerType.Random;
			return PlayerType.Minimax;
		}
		public static void Main(string[] args) {
			PlayerType Player1 = Get("Choose player 1");
			PlayerType Player2 = Get("Choose player 2");

			BoardState board = new BoardState();
			RenderingManager.Render(board);
			//while(true) {
				while(board.HasPlayerWon() == null) {
					board.ApplyTurn(Player1.Execute(Player.Player1, board));
					RenderingManager.Render(board);

					if(board.HasPlayerWon() != null) break;

					board.ApplyTurn(Player2.Execute(Player.Player2, board));
					RenderingManager.Render(board);
				}
			//}
		}
	}
}