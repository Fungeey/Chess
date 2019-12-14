using Microsoft.Xna.Framework;
using Nez;

namespace Chess.Source.PlayField {
	class BoardRenderer : Entity {

		public int width, height;
		BoardRendererComponent renderComponent;

		public BoardRenderer() {
			this.Name = "BoardRenderer";
		}

		public override void OnAddedToScene() {
			base.OnAddedToScene();

			AddComponent(renderComponent = new BoardRendererComponent(width, height));
		}

		public void SetSize(int width, int height) {
			this.width = width;
			this.height = height;

			if(renderComponent != null)
				renderComponent = new BoardRendererComponent(width, height);
		}

		private class BoardRendererComponent : RenderableComponent {
			public override RectangleF Bounds => new RectangleF(new Vector2(0, 0), new Vector2(width, height) * Constants.CellSize);

			public readonly int width, height;

			public BoardRendererComponent(int width, int height) {
				this.width = width;
				this.height = height;

				this.RenderLayer = Constants.BoardRenderLayer;
			}

			public override void Render(Batcher batcher, Camera camera) {
				batcher.DrawRect(GameBoard.GetBoardRect(), Constants.LightTile);

				for(int i = 0; i < width; i++) {
					for(int j = 0; j < height; j++) {

						if(i % 2 == 0 && j % 2 == 0 || i % 2 != 0 && j % 2 != 0)
							continue;

						batcher.DrawRect(new Rectangle(
							GameBoard.BoardToWorld(new Point(i, j)).RoundToPoint(),
							new Point(Constants.CellSize, Constants.CellSize)
						), Constants.DarkTile);
					}
				}
			}
		}
	}
}
