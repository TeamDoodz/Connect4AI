using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connect4AI.AI {
	public static class Minimax {
		public static Move GetBestMove(BoardState board, Player player, int depth = 5, bool doThreading = false) {
			Stopwatch sw = new Stopwatch();
			sw.Start();

			bool AllSame = true;
			(Move,int)? bestMove = null;

			List<Move> moves = board.GetPossibleMovesFor(player).ToList();
			/*
			moves.Sort((a, b) => {
				int aScore = 0;
				int bScore = 0;
				{
					BoardState clonedBoard = (BoardState)board.Clone();
					clonedBoard.ApplyTurn(a);
					aScore = clonedBoard.GetScore(player, 0, player);
				}
				{
					BoardState clonedBoard = (BoardState)board.Clone();
					clonedBoard.ApplyTurn(b);
					bScore = clonedBoard.GetScore(player, 0, player);
				}
				return aScore - bScore;
			});
			moves.Reverse();
			*/
			foreach(Move move in moves) {
				if(doThreading) {
					throw new NotImplementedException();
				} else {
					GetMove(board, player, depth, move, (bestMove ?? (new Move(), int.MinValue)).Item2, (x) => {
						if(bestMove != null && x.Item2 != bestMove?.Item2) AllSame = false;
						if(bestMove == null || x.Item2 > bestMove?.Item2) {
							bestMove = (x.Item1, x.Item2);
						}
					});
				}
			}

			// If the AI believes all moves are equally valued, have it think progressively shorter and shorter until it makes up its mind.
			if(AllSame && depth != 0) {
				return GetBestMove(board, player, depth-1);
			}

			sw.Stop();
			Console.WriteLine($"Found move in {sw.ElapsedMilliseconds} ms");

			return bestMove.Value.Item1;

			static void GetMove(BoardState board, Player player, int depth, Move move, int alpha, Action<(Move, int)> callback) {
				BoardState testBoard = (BoardState)board.Clone();
				testBoard.ApplyTurn(move);

				int score = testBoard.GetScore(player, depth, player, false, alpha);
				Console.WriteLine($"{move}, {score}");
				callback((move,score));
			}
		}
	}
}
