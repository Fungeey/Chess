using Chess.Source.Pieces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess.Source.PlayField {
	class BoardLayout {
		public static readonly BoardLayout DefaultLayout = New(8, 8, PieceColor.Black)
			.AddRowSequence(8, 0, PieceColor.Black, PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook)
			.AddRow(8, 1, PieceType.Pawn, PieceColor.Black)
			.AddRow(8, 6, PieceType.Pawn, PieceColor.White)
			.AddRowSequence(8, 7, PieceColor.White, PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook);

		public static readonly BoardLayout FlippedDefaultLayout = New(8, 8, PieceColor.White)
			.AddRowSequence(8, 0, PieceColor.White, PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.King, PieceType.Queen, PieceType.Bishop, PieceType.Knight, PieceType.Rook)
			.AddRow(8, 1, PieceType.Pawn, PieceColor.White)
			.AddRow(8, 6, PieceType.Pawn, PieceColor.Black)
			.AddRowSequence(8, 7, PieceColor.Black, PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.King, PieceType.Queen, PieceType.Bishop, PieceType.Knight, PieceType.Rook);

		public static readonly BoardLayout MicroChess = New(4, 5, PieceColor.Black)
			.AddRowSequence(4, 0, PieceColor.Black, PieceType.King, PieceType.Knight, PieceType.Bishop, PieceType.Rook)
			.Add(new Point(0, 1), PieceType.Pawn, PieceColor.Black)
			.Add(new Point(3, 3), PieceType.Pawn, PieceColor.White)
			.AddRowSequence(4, 4, PieceColor.White, PieceType.Rook, PieceType.Bishop, PieceType.Knight, PieceType.King);

		public HashSet<PieceDefinition> pieces;
		public readonly int width;
		public readonly int height;
		public readonly PieceColor topColor;

		public static BoardLayout New(int width, int height, PieceColor topColor) {
			return new BoardLayout(width, height, topColor);
		}

		private BoardLayout(int width, int height, PieceColor topColor) {
			pieces = new HashSet<PieceDefinition>();
			this.width = width;
			this.height = height;
			this.topColor = topColor;
		}

		private BoardLayout Add(Point position, PieceType type, PieceColor color) {
			if(position.X >= 0 && position.X < width && position.Y >= 0 && position.Y < height && type != PieceType.Empty)
				pieces.Add(new PieceDefinition(type, color, position));

			return this;
		}

		private BoardLayout AddRow(int length, int y, PieceType type, PieceColor color) {
			for(int i = 0; i < length; i++)
				Add(new Point(i, y), type, color);

			return this;
		}

		private BoardLayout AddRowSequence(int length, int y, PieceColor color, params PieceType[] types) {
			for (int i = 0; i < length; i++)
				Add(new Point(i, y), types[i], color);

			return this;
		}
	}
}
