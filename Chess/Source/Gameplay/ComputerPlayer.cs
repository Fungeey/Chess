using Chess.Source.Movement;
using Chess.Source.PlayField;
using Nez;
using System.Collections.Generic;

namespace Chess.Source.Gameplay {
    class ComputerPlayer : Player {

		public ComputerPlayer(Pieces.PieceColor color) : base(color) {

		}

		public override void DoTurn() {
			var cells = GameBoard.Instance.GetCells();
			cells.RemoveAll(c => c.piece == null);
			cells.RemoveAll(c => c.piece.color != color);

			while(cells.Count > 0) {
				var cell = cells.RandomItem();
				cells.Remove(cell);

				List<Move> moves = MoveGenerator.GenerateMoves(cell);
				while(moves.Count > 0) {
					var move = moves.RandomItem();
					moves.Remove(move);

					turn = new Turn(cell, GameBoard.Instance.GetCell(move.targetPosition)) {
						extraCaptures = move.extraCaptures
					};

					OnTurnCompleted?.Invoke(turn);
					return;
				}
			}
			
			// There are no possible moves for any of the pieces
		}

		//public bool TurnCompleted(out Turn? turn) {
		//          turn = null;
		//          var cells = GameBoard.Instance.GetCells();
		//          cells.RemoveAll(c => c.piece == null);
		//          cells.RemoveAll(c => c.piece.color != color);

		//          Cell cell;
		//          List<Move> moves;
		//          while((moves = MoveGenerator.GenerateMoves(cell = cells[Random.NextInt(cells.Count)])).Count != 0) { 
		//              var move = moves[Random.NextInt(moves.Count)];
		//              GameBoard.Instance.TryGetPiece(move.targetPosition, out var piece);

		//              turn = new Turn(cell, new Cell(move.targetPosition, piece)) {
		//                  extraCaptures = move.extraCaptures,
		//                  extraTurns = move.extraTurns
		//              };
		//              return true;
		//          }
		//          return false;
		//      }
	}
}
