using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Source.Movement {
    static class MoveGenerator {

        public static List<Point> GenerateMoves(Cell cell) {
            Insist.IsNotNull(cell);
            Insist.IsNotNull(cell.piece);

            List<Point> moves = new List<Point>();

            foreach(MoveDefinition moveDef in cell.piece.type.moveSet.moves) {

                if(moveDef is LeaperMoveDefinition) {
                    moves.AddRange(AddLeaperMoves(moveDef, cell, (moveDef as LeaperMoveDefinition).leapDistance));
                    continue;
                }

                moves.AddRange(AddBasicMoves(cell, moveDef));
            }

            return moves;
        }

        private static IEnumerable<Point> AddBasicMoves(Cell cell, MoveDefinition moveDef) {
            var moves = new List<Point>();
            bool isTopColor = cell.piece.color == GameBoard.Instance.Layout.topColor;

            foreach(MoveDirection dir in Enum.GetValues(typeof(MoveDirection))) {
                if(dir == MoveDirection.None || !moveDef.direction.HasFlag(dir))
                    continue;

                foreach(Point p in GetDirectionOffset(dir)) {
                    var offset = isTopColor ? new Point(p.X, -p.Y) : p;

                    moves.AddRange(
                        FilterMoves(moveDef,
                        GetCellsInDirection(cell, moveDef.distance, offset).ToList(),
                        cell)
                    );
                }
            }

            return moves;
        }

        private static List<Point> AddLeaperMoves(MoveDefinition moveDef, Cell start, Point leapOffset) {
            var points = new List<Point>() {
                start.position + new Point(leapOffset.X, leapOffset.Y),
                start.position + new Point(-leapOffset.X, leapOffset.Y),
                start.position + new Point(leapOffset.X, -leapOffset.Y),
                start.position + new Point(-leapOffset.X, -leapOffset.Y),
                start.position + new Point(leapOffset.Y, leapOffset.X),
                start.position + new Point(-leapOffset.Y, leapOffset.X),
                start.position + new Point(leapOffset.Y, -leapOffset.X),
                start.position + new Point(-leapOffset.Y, -leapOffset.X)
            };

            points.RemoveAll(p => !GameBoard.InBounds(p));
            points.RemoveAll(p => GameBoard.Instance.TryGetPiece(p, out var piece) && piece.color == start.piece.color);

            return FilterMoves(moveDef, points, start);
        }

        private static List<Point> FilterMoves(MoveDefinition moveDef, List<Point> cells, Cell cell) {
            if(moveDef.condition.HasFlag(MoveCondition.Initial) && cell.piece.HasMoved)
                return new List<Point>();

            if(moveDef.condition.HasFlag(MoveCondition.CaptureOnly))
                cells.RemoveAll(point => !GameBoard.Instance.CellOccupied(point));
            
            if(moveDef.condition.HasFlag(MoveCondition.NotCaptureOnly))
                cells.RemoveAll(point => GameBoard.Instance.CellOccupied(point));

            return cells;
        }

        private static IEnumerable<Point> GetCellsInDirection(Cell start, int num, Point offset) {
            Point p = start.position + offset;
            for(int i = 0; i < num; i++) {
                if(!GameBoard.InBounds(p))
                    break;

                if(GameBoard.Instance.TryGetPiece(p, out var piece)) {
                    if(piece.color != start.piece.color)
                        yield return p;

                    break;
                }

                yield return p;
                p += offset;
            }   
        }

        private static IEnumerable<Point> GetDirectionOffset(MoveDirection direction) {
            switch(direction) {
                case MoveDirection.CardinalForwards: yield return new Point(0,-1); break;
                case MoveDirection.CardinalBackwards: yield return new Point(0, 1); break;
                case MoveDirection.CardinalSideways: { yield return new Point(-1, 0); yield return new Point(1, 0); } break;
                case MoveDirection.DiagonalForwards: { yield return new Point(-1, -1); yield return new Point(1, -1); } break;
                case MoveDirection.DiagonalBackwards: { yield return new Point(-1, 1); yield return new Point(1, 1); } break;
            }
        }
    }
}
