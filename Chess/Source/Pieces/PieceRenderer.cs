using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using Nez;
using System.Collections.Generic;

namespace Chess.Source.Pieces {
	class PieceRenderer : Entity {
		public Dictionary<Point, Piece> pieces;

		public PieceRenderer() {
			pieces = new Dictionary<Point, Piece>();
			this.Name = "PieceRenderer";
		}

		public override void OnAddedToScene() {
			base.OnAddedToScene();

			AddComponent(new PieceRendererComponent(this));
		}

		public void AddPiece(Piece p) {
			pieces.Add(p.boardPosition, p);
		}

		public void RemovePiece(Piece p) {
			pieces.Remove(p.boardPosition);
		}

		private class PieceRendererComponent : RenderableComponent {
			public override RectangleF Bounds => new RectangleF(Vector2.Zero, new Vector2(8, 8) * Constants.CellSize);

			public PieceRenderer pieceRenderer;

			public PieceRendererComponent(PieceRenderer pieceRenderer) {
				this.pieceRenderer = pieceRenderer;
				this.RenderLayer = Constants.PieceRenderLayer;
			}

			public override void Render(Batcher batcher, Camera camera) {
				foreach(Piece piece in pieceRenderer.pieces.Values) {
					Rectangle r = new Rectangle(GameBoard.BoardToWorld(piece.boardPosition).RoundToPoint(), new Point(Constants.CellSize, Constants.CellSize));
					batcher.Draw(piece.texture, r);
				}
			}
		}
	}
}
