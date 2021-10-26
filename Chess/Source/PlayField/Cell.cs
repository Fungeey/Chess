using Chess.Source.Pieces;
using Microsoft.Xna.Framework;

namespace Chess.Source.PlayField {
	class Cell {
		public readonly Point position;
		public Piece piece;

		public Cell(Point position) {
			this.position = position;
		}

		public Cell(Point position, Piece piece) {
			this.position = position;
			this.piece = piece;
		}
	}
}
