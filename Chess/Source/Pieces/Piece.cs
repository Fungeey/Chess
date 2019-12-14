using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Chess.Source.Pieces {
	class Piece {
		public PieceType type;
		public PieceColor color;
		public Point boardPosition;
		public bool HasMoved = false;

		public Texture2D texture;

		public Piece(PieceType type, PieceColor color, Point boardPosition) {
			this.type = type;
			this.color = color;
			this.boardPosition = boardPosition;
		}

		public void LoadTexture(Scene scene) {
			string texturePath = type.name + color.ToString();
			texture = scene.Content.Load<Texture2D>("Pieces/" + texturePath);
		}
	}

	enum PieceColor {
		White,
		Black
	}
}
