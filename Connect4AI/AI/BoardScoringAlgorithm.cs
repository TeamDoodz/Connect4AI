using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4AI.AI {
	public static class BoardScoringAlgorithm {
		public const int HIGH_VAL = 10000;
		public const int LOW_VAL = -9999; // prioritize winning over preventing loss by a small amount

		// i have no idea how this works, but apparently prioritising putting pieces in the center seems to work as an eval function.
		private static int[,] evaluationTable = new int[,] {{3 , 4 , 5 , 7 , 5 , 4 , 3},
															{4 , 6 , 8 , 10, 8 , 6 , 4},
															{5 , 8 , 11, 13, 11, 8 , 5},
															{5 , 8 , 11, 13, 11, 8 , 5},
															{4 , 6 , 8 , 10, 8 , 6 , 4},
															{3 , 4 , 5 , 7 , 5 , 4 , 3}};
		/// <param name="player">The player that went last.</param>
		/// <param name="perspective">Score from the perspective of this player.</param>
		/// <param name="alpha">The alpha for alpha-beta pruning. Leave null to disable.</param>
		public static int GetScore(this BoardState board, Player player, int depth, Player perspective, bool ChooseMax = false, int? alpha = null) {
			int StaticEval() {
				int outp = 0;
				/*
				int currentPlayerScore = 0;
				foreach(var match in board.GetAllMatches(player)) {
					if(match == 2) currentPlayerScore += 1;
					if(match == 3) currentPlayerScore += 4;
					// execution should never get to match == 4, but lets add a case for it just in case
					if(match == 4) currentPlayerScore += 100;
				}
				int opposingPlayerScore = 0;
				foreach(var match in board.GetAllMatches(player.GetOpposite())) {
					if(match == 2) opposingPlayerScore += 1;
					if(match == 3) opposingPlayerScore += 6;
					// execution should never get to match == 4, but lets add a case for it just in case
					if(match == 4) opposingPlayerScore += 100;
				}
				selfScore = currentPlayerScore - opposingPlayerScore;
				*/
				for(int x = 0; x < board.Board.Length; x++) {
					for(int y = 0; y <= board.MaxHeight; y++) {
						try {
							if(board[x, y] == perspective)  outp += evaluationTable[x, y];
							if(board[x, y] == perspective.GetOpposite()) outp -= evaluationTable[x, y];
						} catch(IndexOutOfRangeException) {
							//Console.WriteLine($"Position ({x},{y}) was out of bounds.");
						}
					}
				}
				return outp + 128;
			}

			bool DoAlphaBeta = alpha != null;
			int a = alpha ?? 0;

			{
				Player? won = board.HasPlayerWon();
				if(won != null) {
					//Console.WriteLine($"Returning early because {won} wins (is good: {won == perspective})");
					if(won == perspective) return HIGH_VAL; else return LOW_VAL;
				}
			}

			if(depth == 0) {
				return StaticEval();
			}

			int outp = ChooseMax ? int.MinValue: int.MaxValue;
			foreach(var move in board.GetPossibleMovesFor(player.GetOpposite())) {
				BoardState boardClone = (BoardState)board.Clone();
				boardClone.ApplyTurn(move);
				int score = boardClone.GetScore(player.GetOpposite(), depth - 1, perspective, !ChooseMax, DoAlphaBeta ? outp : null);
				if(ChooseMax) {
					if(DoAlphaBeta) if(score >= a) return HIGH_VAL;
					if(score == HIGH_VAL) return score;
					if(score > outp) outp = score;
				} else {
					if(DoAlphaBeta) if(score <= a) return LOW_VAL;
					if(score == LOW_VAL) return score;
					if(score < outp) outp = score;
				}
			}
			return outp;
		}

		/// <param name="player">The player that went last.</param>
		/// <param name="perspective">Score from the perspective of this player.</param>
		public static void GetScore(this BoardState board, Player player, int depth, Player perspective, out int score, bool chooseMax = false) {
			score = GetScore(board, player, depth, perspective, chooseMax);
		}
	}
}
