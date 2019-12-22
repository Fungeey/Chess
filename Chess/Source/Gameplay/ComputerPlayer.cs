using Chess.Source.Movement;
using Chess.Source.PlayField;
using Nez;
using System.Collections.Generic;

namespace Chess.Source.Gameplay {
    class ComputerPlayer : IPlayer {

        public bool TurnCompleted(out Turn? turn) {
            turn = null;
            var cells = GameBoard.Instance.GetCells();
            cells.RemoveAll(c => c.piece == null);
            cells.RemoveAll(c => c.piece.color != TurnManager.CurrentColor);

            Cell cell;
            List<Move> moves;
            while((moves = MoveGenerator.GenerateMoves(cell = cells[Random.NextInt(cells.Count)])).Count != 0) { 
                var move = moves[Random.NextInt(moves.Count)];
                GameBoard.Instance.TryGetPiece(move.targetPosition, out var piece);

                turn = new Turn(cell, new Cell(move.targetPosition, piece)) {
                    extraCaptures = move.extraCaptures
                };
                return true;
            }
            return false;
        }
    }
}
