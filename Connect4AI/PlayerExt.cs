using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4AI {
	public static class PlayerExt {
		public static Player GetOpposite(this Player player) {
			if(player == Player.Player1) return Player.Player2; else return Player.Player1;
		}
		public static ConsoleColor GetColor(this Player? player) {
			if(player != Player.Player1) return ConsoleColor.Red;
			return ConsoleColor.Blue;
		}
		public static string Render(this Player? player) {
			if(player == Player.Player1) return "XXXXXX";
			if(player == Player.Player2) return "OOOOOO";
			return "      ";
		}
	}
}
