using Chess.Source.Movement;

namespace Chess.Source.Pieces {
	class PieceType {
		public static readonly PieceType Pawn = new PieceType("Pawn", 1, MoveSet.Pawn);
		public static readonly PieceType Knight = new PieceType("Knight", 3, MoveSet.Knight);
		public static readonly PieceType Bishop = new PieceType("Bishop", 3, MoveSet.Bishop);
		public static readonly PieceType Rook = new PieceType("Rook", 5, MoveSet.Rook);
		public static readonly PieceType Queen = new PieceType("Queen", 9, MoveSet.Queen);
		public static readonly PieceType King = new PieceType("King", int.MaxValue, MoveSet.King);

		public string name;
		public int value;
		public MoveSet moveSet;

		public PieceType(string name, int value, MoveSet moveSet) {
			this.name = name;
			this.value = value;
			this.moveSet = moveSet;
		}
	}
}
