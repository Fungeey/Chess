using Chess.Source.Gameplay;
using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Chess.Source.Rendering {
	class DebugGridRenderer : DefaultRenderer {
		static NezSpriteFont font;

		public override void OnAddedToScene(Scene scene) {
			base.OnAddedToScene(scene);

			var f = scene.Content.Load<SpriteFont>("Fonts/Font");
			font = new NezSpriteFont(f);
		}

		protected override void DebugRender(Scene scene, Camera cam) {
			//base.DebugRender(scene, cam);

			var batcher = Graphics.Instance.Batcher;

			if (GameBoard.Instance.Layout == null)
				return;

			for (int i = 0; i < GameBoard.Instance.Layout.width; i++)
				for(int j = 0; j < GameBoard.Instance.Layout.height; j++)
					batcher.DrawString(font, i + "," + j, new Vector2(i * Constants.CellSize, j * Constants.CellSize), new Color(Color.Black, 0.5f));

			batcher.DrawCircle(cam.Position, Constants.CellSize / 16, new Color(Color.Red, 0.5f), thickness: 20);
			batcher.DrawCircle(cam.ScreenToWorldPoint(Input.CurrentMouseState.Position.ToVector2()), Constants.CellSize / 16, new Color(Color.Blue, 0.5f), thickness: 20);

			batcher.DrawString(font, GameBoard.WorldToBoard(cam.ScreenToWorldPoint(Input.MousePosition.RoundToPoint())).ToString(), new Vector2(0, -100), Color.Black);

			batcher.DrawString(font, TurnManager.CurrentColor.ToString(), new Vector2(0, -200), Color.Black);
		}
	}
}
