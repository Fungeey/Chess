using Chess.Source.Pieces;
using Microsoft.Xna.Framework;

namespace Chess.Source.PlayField {
	struct Cell {
		public readonly Point position;
		public readonly Pieces.Piece piece;

		public Cell(Point position, Pieces.Piece piece) {
			this.position = position;
			this.piece = piece;
		}
	}
}
