using Chess.Source.Pieces;
using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Source.Movement {
    static class MoveGenerator {

        public static List<Move> GenerateMoves(Cell cell) {
            Insist.IsNotNull(cell);
            Insist.IsNotNull(cell.piece);

            List<Move> moves = new List<Move>();

            foreach(MoveDefinition moveDef in cell.piece.type.moveSet.moves) {

                if(GameBoard.Instance.Layout == BoardLayout.DefaultLayout) {
                    GetEnPassentMoves(cell, moves);
                }

                if(moveDef is LeaperMoveDefinition) {
                    moves.AddRange(AddLeaperMoves(moveDef, cell, (moveDef as LeaperMoveDefinition).leapDistance));
                    continue;
                }

                moves.AddRange(AddBasicMoves(cell, moveDef));
            }

            return moves;
        }

        private static void GetEnPassentMoves(Cell cell, List<Move> moves) {
            if(cell.piece.type != PieceType.Pawn)
                return;

            var leftOffset = cell.position + new Point(-1, 0);
            var rightOffset = cell.position + new Point(1, 0);
            var topColor = GameBoard.Instance.Layout.topColor;

            if(cell.piece.color == topColor && cell.position.Y == 4 || cell.piece.color != topColor && cell.position.Y == 3) {
                var checkDirection = cell.position.Y == 3 ? -1 : 1;

                if(GameBoard.Instance.TryGetPiece(leftOffset, out var left) && left.DidPawnJump)
                    moves.Add(new Move(cell.position + new Point(-1, checkDirection), new Cell(rightOffset, left)));

                if(GameBoard.Instance.TryGetPiece(rightOffset, out var right) && right.DidPawnJump)
                    moves.Add(new Move(cell.position + new Point(1, checkDirection), new Cell(rightOffset, right)));
            }
        }

        private static IEnumerable<Move> AddBasicMoves(Cell cell, MoveDefinition moveDef) {
            var moves = new List<Move>();
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

        private static List<Move> AddLeaperMoves(MoveDefinition moveDef, Cell start, Point leapOffset) {

            var points = new List<Point>();
            for(int i = -1; i <= 1; i += 2) {
                for(int j = -1; j <= 1; j += 2) {
                    points.Add(start.position + new Point(leapOffset.X * i, leapOffset.Y * j));
                    points.Add(start.position + new Point(leapOffset.Y * i, leapOffset.X * j));
                }
            }

            points.RemoveAll(p => !GameBoard.InBounds(p));
            points.RemoveAll(p => GameBoard.Instance.TryGetPiece(p, out var piece) && piece.color == start.piece.color);

            return FilterMoves(moveDef, points.Select(p => new Move(p)).ToList(), start);
        }

        private static List<Move> FilterMoves(MoveDefinition moveDef, List<Move> moves, Cell cell) {
            if(moveDef.condition.HasFlag(MoveCondition.Initial) && cell.piece.HasMoved)
                return new List<Move>();

            if(moveDef.condition.HasFlag(MoveCondition.CaptureOnly))
                moves.RemoveAll(move => !GameBoard.Instance.CellOccupied(move.targetPosition));

            if(moveDef.condition.HasFlag(MoveCondition.NotCaptureOnly))
                moves.RemoveAll(move => GameBoard.Instance.CellOccupied(move.targetPosition));

            return moves;
        }

        private static IEnumerable<Move> GetCellsInDirection(Cell start, int num, Point offset) {
            Point p = start.position + offset;
            for(int i = 0; i < num; i++) {
                if(!GameBoard.InBounds(p))
                    break;

                if(GameBoard.Instance.TryGetPiece(p, out var piece)) {
                    if(piece.color != start.piece.color)
                        yield return new Move(p);

                    break;
                }

                yield return new Move(p);
                p += offset;
            }
        }

        private static IEnumerable<Point> GetDirectionOffset(MoveDirection direction) {
            switch(direction) {
                case MoveDirection.CardinalForwards: yield return new Point(0, -1); break;
                case MoveDirection.CardinalBackwards: yield return new Point(0, 1); break;
                case MoveDirection.CardinalSideways: { yield return new Point(-1, 0); yield return new Point(1, 0); } break;
                case MoveDirection.DiagonalForwards: { yield return new Point(-1, -1); yield return new Point(1, -1); } break;
                case MoveDirection.DiagonalBackwards: { yield return new Point(-1, 1); yield return new Point(1, 1); } break;
            }
        }
    }
}
