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

		public static readonly BoardLayout MegaChess = New(16, 16, PieceColor.Black)
			.AddRowSequence(16, 0, PieceColor.Black, PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook,
			PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook)
			.AddRow(16, 1, PieceType.Pawn, PieceColor.Black)
			.AddRow(16, 2, PieceType.Pawn, PieceColor.Black)
			.AddRow(16, 4, PieceType.Pawn, PieceColor.Black)
			.AddRow(16, 5, PieceType.Pawn, PieceColor.Black)
			.AddRow(16, 11, PieceType.Pawn, PieceColor.White)
			.AddRow(16, 12, PieceType.Pawn, PieceColor.White)
			.AddRow(16, 13, PieceType.Pawn, PieceColor.White)
			.AddRow(16, 14, PieceType.Pawn, PieceColor.White)
			.AddRowSequence(16, 15, PieceColor.White, PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook,
			PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook);


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

		/// <summary>
		/// Converts all pieceDefinitions to piece entities, and creates all of the cells.
		/// Assigns the pieces to the cells they are in.
		/// </summary>
		/// <returns>The list of cells</returns>
		public List<Cell> CreateBoardLayout() {
			var cells = new List<Cell>();
			for(int i = 0; i < width; i++) {
				for(int j = 0; j < height; j++) {
					var newCell = new Cell(new Point(i, j));
					foreach(PieceDefinition p in pieces)
						if(p.position == newCell.position)
							newCell.piece = p.ToEntity(newCell);
					cells.Add(newCell);
				}
			}

			return cells;
		}
	}
}
