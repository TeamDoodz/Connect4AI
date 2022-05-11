using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4AI {
	// BuT iCLoNeAbLe Is BAd bitch STFU!!!!!!!
	public class BoardState : ICloneable {
		public List<Player>[] Board { get; private set; }
		public readonly int MaxHeight;

		public BoardState(int width = 7, int height = 6) {
			Board = new List<Player>[width];
			//TODO: This for loop might not be neccesary
			for(int i = 0; i < width; i++) {
				Board[i] = new List<Player>();
			}
			MaxHeight = height - 1;
		}

		public Player? this[int x, int y] {
			get {
				if(x >= Board.Length || y > MaxHeight) throw new IndexOutOfRangeException();
				List<Player> column = Board[x];
				if(column.Count > y) return column[y]; else return null;
			}
		}

		/// <summary>
		/// The player who will place the next piece.
		/// </summary>
		public Player NextPlayer => GetAllTurnsBy(Player.Player1) < GetAllTurnsBy(Player.Player2)? Player.Player1 : Player.Player2;

		public void ApplyTurn(Move move) {
			if(move.Column >= Board.Length) throw new ArgumentOutOfRangeException(nameof(move), "Input move accessed a nonexistent column.");
			if(Board[move.Column].Count > MaxHeight) throw new InvalidMoveException($"Column {move.Column} is already filled.");
			Board[move.Column].Add(move.Player);
		}

		public bool IsMoveLegal(Move move) {
			if(move.Column >= Board.Length) return false;
			if(Board[move.Column].Count > MaxHeight) return false;
			return true;
		}

		public int GetAllTurnsBy(Player player) {
			int res = 0;
			foreach(var column in Board) {
				foreach(var space in column) {
					if(space == player) res++;
				}
			}
			return res;
		}

		public Move[] GetPossibleMovesFor(Player player) {
			List<Move> moves = new List<Move>();
			for(int i = 0; i < Board.Length; i++) {
				Move move = new Move(player,i);
				if(IsMoveLegal(move)) moves.Add(move);
			}
			return moves.ToArray();
		}

		public IEnumerable<int> GetAllMatches(Player player) {
			// Vertical matches
			{
				for(int x = 0; x < Board.Length; x++) {
					int length = 0;
					for(int y = 0; y <= MaxHeight; y++) {
						if(this[x, y] == player) {
							length++;
						} else {
							if(length > 0) {
								yield return length;
								length = 0;
							}
						}
					}
					if(length > 0) yield return length;
				}
			}
			// Horizontal matches
			{
				for(int y = 0; y <= MaxHeight; y++) {
					int length = 0;
					for(int x = 0; x < Board.Length; x++) {
						if(this[x, y] == player) {
							length++;
						} else {
							if(length > 0) {
								yield return length;
								length = 0;
							}
						}
					}
					if(length > 0) yield return length;
				}
			}
			// Right diagonal matches
			{
				List<(int, int)> checkedSpaces = new List<(int, int)>();
				for(int x = 0; x < Board.Length; x++) {
					for(int y = 0; y <= MaxHeight; y++) {
						if(!checkedSpaces.Contains((x,y))) {
							int num = 0;
							try {
								if(this[x,y] == player) { 
									num++;
									checkedSpaces.Add((x, y));
									if(this[x+1, y+1] == player) {
										num++;
										checkedSpaces.Add((x+1, y+1));
										if(this[x + 2, y + 2] == player) {
											num++;
											checkedSpaces.Add((x + 2, y + 2));
											if(this[x + 3, y + 3] == player) {
												num++;
												checkedSpaces.Add((x + 3, y + 3));
											}
										}
									}
								}
							} catch(IndexOutOfRangeException) { }
							if(num > 0) yield return num;
						}
					}
				}
			}
			// Left diagonal matches
			{
				List<(int, int)> checkedSpaces = new List<(int, int)>();
				for(int x = Board.Length - 1; x >= 0; x--) {
					for(int y = MaxHeight; y >= 0; y--) {
						if(!checkedSpaces.Contains((x, y))) {
							int num = 0;
							try {
								if(this[x, y] == player) {
									num++;
									checkedSpaces.Add((x, y));
									if(this[x - 1, y + 1] == player) {
										num++;
										checkedSpaces.Add((x - 1, y + 1));
										if(this[x - 2, y + 2] == player) {
											num++;
											checkedSpaces.Add((x - 2, y + 2));
											if(this[x - 3, y + 3] == player) {
												num++;
												checkedSpaces.Add((x - 3, y + 3));
											}
										}
									}
								}
							} catch(IndexOutOfRangeException) { }
							if(num > 0) yield return num;
						}
					}
				}
			}
		}

		public Player? HasPlayerWon() {
			//Stopwatch sw = new Stopwatch();
			//sw.Start();
			Player? res = null;
			if(GetAllMatches(Player.Player1).Contains(4)) res = Player.Player1;
			if(GetAllMatches(Player.Player2).Contains(4)) res = Player.Player2;
			//sw.Stop();
			//Console.WriteLine($"HasPlayerWon method executed in {sw.ElapsedMilliseconds} ms.");
			return res;
		}

		public object Clone() {
			var outp = new BoardState(Board.Length, MaxHeight + 1);
			List<Player>[] copied = new List<Player>[Board.Length];
			for(int i = 0; i < Board.Length; i++) {
				copied[i] = new List<Player>(Board[i]);
			}
			outp.Board = copied;
			return outp;
		}
	}
}
