using Microsoft.Xna.Framework;

namespace Chess.Source.Pieces {
	struct PieceDefinition {
		public readonly PieceType type;
		public readonly PieceColor color;
		public readonly Point position;

		public PieceDefinition(PieceType type, PieceColor color, Point position) {
			this.type = type;
			this.color = color;
			this.position = position;
		}

		public Piece ToEntity() {
			return new Piece(type, color, position);
		}
	}
}
