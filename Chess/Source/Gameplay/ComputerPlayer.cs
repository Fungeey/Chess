using Chess.Source.Movement;
using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using Nez;
using System.Collections.Generic;

namespace Chess.Source.Gameplay {
    class ComputerPlayer : IPlayer {

        public bool TurnCompleted(out Turn? turn) {
            turn = null;
            var cells = GameBoard.Instance.GetCells();
            cells.RemoveAll(c => c.piece.color != TurnManager.CurrentColor);

            Cell cell;
            List<Point> moves;
            while((moves = MoveGenerator.GenerateMoves(cell = cells[Random.NextInt(cells.Count)])).Count != 0) { 
                var move = moves[Random.NextInt(moves.Count)];
                GameBoard.Instance.TryGetPiece(move, out var piece);
                turn = new Turn(cell, new Cell(move, piece));
                return true;
            }
            return false;
        }
    }
}
