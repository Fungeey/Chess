using Chess.Source.Pieces;
using Microsoft.Xna.Framework;

namespace Chess.Source.PlayField {
	struct Cell {
		public readonly Point position;
		public readonly Piece piece;

		public Cell(Point position, Pieces.Piece piece) {
			this.position = position;
			this.piece = piece;
		}

		public static bool operator ==(Cell a, Cell b) {
			return a.Equals(b);
		}
		public static bool operator !=(Cell a, Cell b) {
			return !a.Equals(b);
		}

		public override bool Equals(object obj) {
			return this.position == ((Cell)obj).position;
		}
	}
}
