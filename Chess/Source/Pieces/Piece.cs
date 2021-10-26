using Chess.Source.PlayField;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Chess.Source.Pieces {
	class Piece {
		public PieceType type;
		public PieceColor color;
		public Cell cell;

		public bool HasMoved = false;
		public bool DidPawnJump = false;

		public Texture2D texture;

		public Piece(PieceType type, PieceColor color, Cell cell) {
			this.type = type;
			this.color = color;
			this.cell = cell;
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
