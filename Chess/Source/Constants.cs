using Microsoft.Xna.Framework;

namespace Chess.Source {
	static class Constants {
		public const int WindowWidth = 720;
		public const int WindowHeight = 720;

		public const int CellSize = 256;
		public static readonly Point CellArea = new Point(CellSize, CellSize);

		public const int BoardRenderLayer = 2;
		public const int SelectionRenderLayer = 1;
		public const int PieceRenderLayer = 0;

		public static readonly Color DarkTile = new Color(209, 139, 71);
		public static readonly Color LightTile = new Color(255, 206, 158);
	}
}
