using Chess.Source.Gameplay;
using Chess.Source.Pieces;
using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Source.Movement {
    static class MoveGenerator {

		private static List<Cell> opponentPieces;
		private static Cell startCell;
		private static Cell kingCell;

		// TODO: Add pawn promotion

		/// <summary>
		/// Generate all of the valid moves for the piece in this cell
		/// </summary>
		/// <param name="cell">The cell to move from</param>
		/// <returns>A list of all moves</returns>
		public static List<Move> GenerateMoves(Cell cell) {
			kingCell = null;
			opponentPieces = null;
			startCell = cell;

			var moves = GenerateMovesWithoutCheck(cell);

			// If king is under check, the only valid moves are moves that bring the king out of check.
			// If none exist, it is checkmate.
			kingCell = FindKingCell();

			// remove all moves that 
				// a) don't get the king out of check.
				// b) put the king into check
			var invalidMoves = new List<Move>();
			foreach(Move move in moves) {
				if(!KingSafeAfterMove(move))
					invalidMoves.Add(move);
			}
			invalidMoves.ForEach(m => moves.Remove(m));

			if(moves.Count == 0) ; // Checkmate, this player has lost

			return moves;
		}

		private static Cell FindKingCell() {
			var cells = GameBoard.Instance.GetCells();
			cells.RemoveAll(c => c.piece == null);
			return cells.Where(c => c.piece.type == PieceType.King && c.piece.color == TurnManager.Instance.CurrentPlayer.color).First();
		}

		private static List<Move> GenerateMovesWithoutCheck(Cell cell) {
			Insist.IsNotNull(cell);
			Insist.IsNotNull(cell.piece);
			var moves = new List<Move>();		// Rename

			// For each type of movement defined for the piece in this cell
			foreach(MoveDefinition moveDef in cell.piece.type.moveSet.moves) {

				if(GameBoard.Instance.Layout == BoardLayout.DefaultLayout || GameBoard.Instance.Layout == BoardLayout.FlippedDefaultLayout) {

					moves.AddRange(GetCastlingMoves(cell));

					if(cell.piece.type != PieceType.Pawn)
						moves.AddRange(GetEnPassentMoves(cell));
				}

				if(moveDef is LeaperMoveDefinition) {
					moves.AddRange(AddLeaperMoves(moveDef, cell, (moveDef as LeaperMoveDefinition).leapDistance, moves));
					continue;
				}

				moves.AddRange(AddBasicMoves(cell, moveDef));
			}

			return moves;
		}

		private static List<Cell> GetOpponentPieces() {
			if(opponentPieces != null)
				return opponentPieces;

			var cells = GameBoard.Instance.GetCells();
			cells.RemoveAll(c => c.piece == null);
			// Only look for opponent's color, remove our own
			cells.RemoveAll(c => c.piece.color == TurnManager.Instance.CurrentPlayer.color);

			return opponentPieces = cells;
		}

		private static bool KingIsUnderCheck() {
			// Find our king
			var cells = GameBoard.Instance.GetCells();
			cells.RemoveAll(c => c.piece == null);
			Console.Out.WriteLine(cells.Count);
			kingCell = cells.Where(c => c.piece.type == PieceType.King && c.piece.color == TurnManager.Instance.CurrentPlayer.color).First();

			return CellIsUnderCheck(kingCell);
		}

		// Sees if the cell would be under check. (usually the king's cell)
		// 1. Generate moves for all opponent pieces
		// 2. If any opponent's valid moves contain the king's position, king is under check.
		private static bool CellIsUnderCheck(Cell cell) {
			var opponentPieces = GetOpponentPieces();

			foreach(Cell c in opponentPieces) {
				var moves = GenerateMovesWithoutCheck(c);			// Theres some tricky recursion here. Don't generate 
				foreach(Move move in moves) {
					if(move.targetPosition == cell.position) {
						// this cell can be captured by an opponent piece
						return true;
					}
				}
			}

			return false;
		}
		
		// Check if Move resolves check on cell.
		// 1. perform move temporarily
		// 2. if CellIsUnderCheck(), the move was invalid (would have put king under check).
		// 3. undo move and return outputs. 
		private static bool KingSafeAfterMove(Move move) {
			// Perform Move

			//startCell
			var endCell = GameBoard.Instance.GetCell(move.targetPosition);

			var storeEndCellPiece = endCell.piece;
			endCell.piece = startCell.piece;
			startCell.piece = null;

			// if the king is no longer under check after the move, it has resolved the check.
			bool kingNotChecked = !CellIsUnderCheck(FindKingCell());

			// Undo Move
			startCell.piece = endCell.piece;
			endCell.piece = storeEndCellPiece;

			return kingNotChecked;
		}


		private static IEnumerable<Move> GetCastlingMoves(Cell cell) {
			var moves = new List<Move>();

			if(cell.piece.type != PieceType.King || cell.piece.HasMoved)
                return moves;

            void AddMoves(int direction) {
                if(CanCastle(cell, direction)) {
                    int rookStart = (direction == 1 ? 7 : 0);
                    int rookEnd = (direction == 1 ? 5 : 3);

                    if(cell.position.X == 3)
                        rookEnd -= 1;

                    var rookMove = new Turn(
                        GameBoard.Instance.GetCell(new Point(rookStart, cell.position.Y)),
                        GameBoard.Instance.GetCell(new Point(rookEnd, cell.position.Y))
                    );

                    moves.Add(new Move(cell.position + new Point(2 * Math.Sign(direction), 0), rookMove));
                }
            }

            AddMoves(-1);
            AddMoves(1);

			return moves;
        }

        private static bool CanCastle(Cell cell, int direction) {
            int rookX = (direction == 1 ? 7 : 0);
            {
                var checkRook = new Point(rookX, cell.position.Y);

                if(GameBoard.Instance.TryGetPiece(checkRook, out var piece)
                        && piece.type == PieceType.Rook && piece.HasMoved
                    || !GameBoard.Instance.CellOccupied(checkRook))
                    return false;
            }

            for(int i = cell.position.X + Math.Sign(direction); i != rookX; i += Math.Sign(direction)) {
                if(GameBoard.Instance.CellOccupied(new Point(i, cell.position.Y)))
                    return false;
            }

            return true;
        }

        private static IEnumerable<Move> GetEnPassentMoves(Cell cell) {
			var moves = new List<Move>(); 

			var leftOffset = cell.position + new Point(-1, 0);
            var rightOffset = cell.position + new Point(1, 0);
            var topColor = GameBoard.Instance.Layout.topColor;

            if(cell.piece.color == topColor && cell.position.Y == 4 || cell.piece.color != topColor && cell.position.Y == 3) {
                var checkDirection = cell.position.Y == 3 ? -1 : 1;

                if(GameBoard.Instance.TryGetPiece(leftOffset, out var left) && left.DidPawnJump)
                    moves.Add(new Move(cell.position + new Point(-1, checkDirection), new Cell(leftOffset, left)));

                if(GameBoard.Instance.TryGetPiece(rightOffset, out var right) && right.DidPawnJump)
                    moves.Add(new Move(cell.position + new Point(1, checkDirection), new Cell(rightOffset, right)));
            }

			return moves;
		}

		/// <summary>
		/// generates the regular cardinal movements
		/// </summary>
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
                        GetCellsInDirection(cell, moveDef.distance, offset).ToList(), cell)
                    ); 
                }
            }

            return moves;
        }

        private static List<Move> AddLeaperMoves(MoveDefinition moveDef, Cell start, Point leapOffset, List<Move> moves) {

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

			moves.RemoveAll(m => !GameBoard.InBounds(m.targetPosition));

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
