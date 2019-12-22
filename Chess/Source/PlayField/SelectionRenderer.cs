using Chess.Source.Gameplay;
using Chess.Source.Movement;
using Microsoft.Xna.Framework;
using Nez;
using System.Collections.Generic;

namespace Chess.Source.PlayField {
	class SelectionRenderer : Entity {

		private SelectionRendererComponent renderer;
		public SelectionRenderer() {
			this.Name = "SelectionRenderer";
		}

		public override void OnAddedToScene() {
			base.OnAddedToScene();

			AddComponent(renderer = new SelectionRendererComponent());
		}

		public override void Update() {
			base.Update();

			if(TurnManager.selectedCell != null)
				renderer.SetPiece(TurnManager.selectedCell);
		}

		private class SelectionRendererComponent : RenderableComponent {
			public Cell cell;
			public List<Move> moves;

			public override float Height => Constants.CellSize;
			public override float Width => Constants.CellSize;

			public SelectionRendererComponent() {
				this.RenderLayer = Constants.SelectionRenderLayer;
			}

			public override void Render(Batcher batcher, Camera camera) {
				if(GameBoard.Instance.Layout == null)
					return;

				DrawMouseOverlay(batcher, camera);
				DrawPossibleMoves(batcher);
			}

			private void DrawPossibleMoves(Batcher batcher) {
				if(moves == null || TurnManager.selectedCell == null)
					return;

				foreach(Move m in this.moves)
					batcher.DrawRect(GetCellRectangle(m.targetPosition), new Color(Color.DarkGreen, 0.5f));
			}

			private void DrawMouseOverlay(Batcher batcher, Camera camera) {
				Vector2 mousePos = camera.ScreenToWorldPoint(Input.CurrentMouseState.Position.ToVector2());

				if(GameBoard.InBounds(mousePos)) {
					Point boardMousePos = GameBoard.WorldToBoard(camera.ScreenToWorldPoint(Input.MousePosition));
					batcher.DrawHollowRect(GetCellRectangle(boardMousePos), Color.Red, 10f);
				}
			}

			private Rectangle GetCellRectangle(Point position) => new Rectangle(GameBoard.BoardToWorld(position).RoundToPoint(), Constants.CellArea);

			public void SetPiece(Cell cell) {
				if(this.cell == cell)
					return;

				this.cell = cell;
				this.moves = MoveGenerator.GenerateMoves(cell);
			}
		}
	}
}
