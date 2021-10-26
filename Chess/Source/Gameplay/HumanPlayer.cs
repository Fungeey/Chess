using Chess.Source.Movement;
using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using Nez;
using System.Linq;

namespace Chess.Source.Gameplay {
	class HumanPlayer : Player {
		public HumanPlayer(Pieces.PieceColor color) : base(color) { }

		public override void DoTurn() {
			if(Input.LeftMouseButtonPressed) {
				Vector2 mousePos = turnManager.Scene.Camera.ScreenToWorldPoint(Input.MousePosition);
				Point boardPos = GameBoard.WorldToBoard(mousePos);
				var cell = GameBoard.Instance.GetCell(boardPos);

				if(cell != null) {
					ProcessInput(cell);
				}
			}
		}

		// if start is null, assign it as the start.
		// If end is null
			// if you pick the same color, set that as the new start.
			// if you pick a blank, try to move there.
			// if you pick an opposite color, try to take there.

		private void ProcessInput(Cell cell) {
			if(turn.start == null) {

				// Only select the starting piece if the cell has a piece and it is of the proper color
				if(cell.piece != null && cell.piece.color == color)
					SelectStartingPiece(cell);

			} else if(turn.end == null) {
				if(cell.piece != null && cell.piece.color == color)
					SelectStartingPiece(cell);
				else
					AttemptMove(cell);
			}
		}

		private void SelectStartingPiece(Cell cell) {
			turn.start = cell;
			turnManager.selectedCell = cell;
		}

		private void AttemptMove(Cell cell) {
			// Generate a set of all valid moves for the starting piece
			var moves = MoveGenerator.GenerateMoves(turn.start);
			var validMove = moves.Where(m => m.targetPosition == cell.position).FirstOrDefault();

			// If the move we want to take is one of the valid moves, we can continue
			if(validMove != null) {
				// Moving to an empty space is fine. Capturing is only allowed if the piece is of opposite color to you.
				if(cell.piece == null || (cell.piece != null && cell.piece.color != color)) {
					turn.end = cell;
					OnTurnCompleted?.Invoke(turn);
					turnManager.selectedCell = null;
				}
			}
		}

		//public bool TurnCompleted(out Turn? turn) {
		//	turn = null;
		//	if(GameBoard.Instance.ClickedCell == null)
		//		return false;

		//	Cell c = GameBoard.Instance.ClickedCell;

		//	if(startTurn && c.piece != null) {
		//		if(c.piece.color != TurnManager.CurrentColor)
		//			return false;

		//		// Select cell to start
		//		startTurn = false;
		//		TurnManager.selectedCell = c;
		//		newTurn = new Turn() { start = c };
		//		this.moves = MoveGenerator.GenerateMoves(c);
		//	} else if(!startTurn) {
		//		if(c.piece != null && c.piece.color == color) {
		//			TurnManager.selectedCell = c;
		//			newTurn = new Turn() { start = c };
		//			this.moves = MoveGenerator.GenerateMoves(c);
		//			return false;
		//		}

		//		// End is same as start, cancel
		//		if(newTurn.start == c || !moves.Where(m => m.targetPosition == c.position).Any()) {
		//			startTurn = true;
		//			return false;
		//		}

		//		// Turn finished
		//		var move = moves.Find(m => m.targetPosition == c.position);

		//		newTurn.end = c;
		//		newTurn.extraCaptures = move.extraCaptures;
		//		newTurn.extraTurns = move.extraTurns;

		//		turn = newTurn;
		//		TurnManager.selectedCell = null;

		//		return startTurn = true;
		//	}

		//	return false;
		//}
	}
}
