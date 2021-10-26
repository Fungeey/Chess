using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using Nez;

namespace Chess.Source.Pieces {
	class PieceRenderer : Entity {
		public PieceRenderer() {
			this.Name = "PieceRenderer";
		}

		public override void OnAddedToScene() {
			base.OnAddedToScene();

			AddComponent(new PieceRendererComponent(this));
		}

		private class PieceRendererComponent : RenderableComponent {
			public override RectangleF Bounds => new RectangleF(Vector2.Zero, new Vector2(8, 8) * Constants.CellSize);

			public PieceRenderer pieceRenderer;

			public PieceRendererComponent(PieceRenderer pieceRenderer) {
				this.pieceRenderer = pieceRenderer;
				this.RenderLayer = Constants.PieceRenderLayer;
			}

			public override void Render(Batcher batcher, Camera camera) {
				foreach(Cell cell in GameBoard.Instance.GetCells()) {
					if(cell.piece == null)
						continue;
					Rectangle r = new Rectangle(GameBoard.BoardToWorld(cell.position), new Point(Constants.CellSize, Constants.CellSize));
					batcher.Draw(cell.piece.texture, r);
				}
			}
		}
	}
}
