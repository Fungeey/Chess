using Chess.Source.Movement;
using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Source.Gameplay {
	class HumanPlayer : IPlayer {

		private Turn newTurn;
		private bool startTurn = true;
		private List<Move> moves;

		public bool TurnCompleted(out Turn? turn) {
			turn = null;
			if(GameBoard.Instance.ClickedCell == null)
				return false;

			Cell c = GameBoard.Instance.ClickedCell;

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
				if(newTurn.start == c || !moves.Where(m => m.targetPosition == c.position).Any()) {
					startTurn = true;
					return false;
				}

				// Turn finished
				var move = moves.Find(m => m.targetPosition == c.position);

				newTurn.end = c;
				newTurn.extraCaptures = move.extraCaptures;
				newTurn.extraTurns = move.extraTurns;

				turn = newTurn;
				TurnManager.selectedCell = null;

				return startTurn = true;
			}

			return false;
		}
	}
}
