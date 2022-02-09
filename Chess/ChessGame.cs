using Chess.Source;
using Chess.Source.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Chess {
	public class ChessGame : Nez.Core {

		public ChessGame() : base(Constants.WindowWidth, Constants.WindowHeight) {
		}

		protected override void Initialize() {
			base.Initialize();

			Scene = new MainScene();
		}

		protected override void Update(GameTime gameTime) {
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
				Exit();
			}

			base.Update(gameTime);
		}
	}
}
