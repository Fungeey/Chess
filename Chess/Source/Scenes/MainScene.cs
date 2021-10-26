using Chess.Source.Gameplay;
using Chess.Source.InputHandling;
using Chess.Source.PlayField;
using Chess.Source.Rendering;
using Microsoft.Xna.Framework;
using Nez;

namespace Chess.Source.Scenes {
	class MainScene : Scene {

		public override void Initialize() {
			base.Initialize();

			AddSceneComponent(GameBoard.Instance);
			AddSceneComponent(new TurnManager(new ComputerPlayer(Pieces.PieceColor.White), new ComputerPlayer(Pieces.PieceColor.Black)));
			AddEntityProcessor(new InputEventManager());

			AddRenderer(new DebugGridRenderer());
		}

		public override void OnStart() {
			base.OnStart();

			BoardLayout layout = BoardLayout.DefaultLayout;
			GameBoard.Instance.Load(layout);

			Camera.MinimumZoom = 0.2f;
			Camera.MaximumZoom = 30;
			Camera.ZoomOut(30);
			Camera.Position = new Vector2((float)layout.width/2, (float)layout.height/2) * Constants.CellSize;
		}
	}
}
