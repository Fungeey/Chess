using Microsoft.Xna.Framework;
using Nez;

namespace Chess.Source.PlayField {
	class BoardRenderer : Entity {

		BoardRendererComponent renderComponent;

		public BoardRenderer() {
			this.Name = "BoardRenderer";
		}

		public override void OnAddedToScene() {
			base.OnAddedToScene();

			AddComponent(renderComponent = new BoardRendererComponent());
		}

		private class BoardRendererComponent : RenderableComponent {
			public override RectangleF Bounds => new RectangleF(new Vector2(0, 0), new Vector2(GameBoard.Instance.Layout.width, GameBoard.Instance.Layout.height) * Constants.CellSize);

			public BoardRendererComponent() {
				this.RenderLayer = Constants.BoardRenderLayer;
			}

			public override void Render(Batcher batcher, Camera camera) {
				batcher.DrawRect(GameBoard.GetBoardRect(), Constants.LightTile);

				for(int i = 0; i < GameBoard.Instance.Layout.width; i++) {
					for(int j = 0; j < GameBoard.Instance.Layout.height; j++) {

						if(i % 2 == 0 && j % 2 == 0 || i % 2 != 0 && j % 2 != 0)
							continue;

						batcher.DrawRect(new Rectangle(
							GameBoard.BoardToWorld(new Point(i, j)),
							new Point(Constants.CellSize, Constants.CellSize)
						), Constants.DarkTile);
					}
				}
			}
		}
	}
}
