using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4AI {
	public struct Move {
		/// <summary>
		/// The player that made this turn.
		/// </summary>
		public Player Player;
		/// <summary>
		/// The column that this turn was made in.
		/// </summary>
		public int Column;

		public Move(Player player, int column) {
			Player = player;
			Column = column;
		}

		public override string ToString() {
			return $"{Player}, {Column}";
		}

		public static Move Random(Player p, BoardState b) {
			Random random = new Random(DateTime.Now.Millisecond);
			Move outp = new Move(p, random.Next(7));
			while(!b.IsMoveLegal(outp)) {
				outp = new Move(p, random.Next(7));
			}
			return outp;
		}
	}
}
