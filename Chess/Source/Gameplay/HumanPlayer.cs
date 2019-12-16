using Chess.Source.Movement;
using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess.Source.Gameplay {
	class HumanPlayer : IPlayer {

		private Turn newTurn;
		private bool startTurn = true;
		private List<Point> moves;

		public bool TurnCompleted(out Turn? turn) {
			turn = null;
			if(!GameBoard.Instance.ClickedCell.HasValue)
				return false;

			Cell c = GameBoard.Instance.ClickedCell.Value;

			if(startTurn && c.piece != null) {
				if(c.piece.color != TurnManager.CurrentColor)
					return false;
				
				// Select cell to start
				startTurn = false;
				TurnManager.selectedCell = c;
				newTurn = new Turn() { start = c };
				this.moves = MoveGenerator.GenerateMoves(c);
			} else if(!startTurn) {
				if(c.piece != null && c.piece.color == TurnManager.CurrentColor) {
					TurnManager.selectedCell = c;
					newTurn = new Turn() { start = c };
					this.moves = MoveGenerator.GenerateMoves(c);
					return false;
				}

				// End is same as start, cancel
				if(newTurn.start.position == c.position || !moves.Contains(c.position)) {
					startTurn = true;
					return false;
				}

				// Turn finished
				newTurn.end = c;
				turn = newTurn;
				TurnManager.selectedCell = null;

				return startTurn = true;
			}

			return false;
		}
	}
}
