using Chess.Source.Gameplay;
using Chess.Source.Pieces;
using Microsoft.Xna.Framework;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Source.PlayField {
    sealed class GameBoard : SceneComponent, IUpdatable {

        private static readonly Lazy<GameBoard> lazy = new Lazy<GameBoard>(() => new GameBoard());
        public static GameBoard Instance { get => lazy.Value; }

        private List<Cell> cells;

        private GameBoard() { }

        public BoardLayout Layout { get; private set; }

        private PieceRenderer pieceRenderer;
        private BoardRenderer boardRenderer;
        private SelectionRenderer selectionRenderer;

        public override void OnEnabled() {
            base.OnEnabled();

            Scene.AddEntity(pieceRenderer = new PieceRenderer());
            Scene.AddEntity(boardRenderer = new BoardRenderer());
            Scene.AddEntity(selectionRenderer = new SelectionRenderer());
        }

        public void Load(BoardLayout layout) {
            Layout = layout;
			cells = Layout.CreateBoardLayout();

            foreach(Cell c in cells) {
				if(c.piece != null)
					c.piece.LoadTexture(Scene);
            }
        }

        public override void Update() {
            base.Update();
        }

		// If we move a pawn normally, set the DidPawnJump flag to be false so it can't jump again.
        //public void UpdatePawns(Piece piece) {
        //    foreach(Cell cell in cells) {
        //        if(cell.piece == piece || cell.piece == null)
        //            continue;
        //        if(cell.piece.type == PieceType.Pawn)
        //            cell.piece.DidPawnJump = false;
        //    }
        //}

        #region Transformations
        public static Point BoardToWorld(Point boardPos) {
            return new Point(boardPos.X * Constants.CellSize, boardPos.Y * Constants.CellSize);
        }

        public static Point WorldToBoard(Vector2 worldPos) {
            return new Point((int)(worldPos.X / Constants.CellSize), (int)(worldPos.Y / Constants.CellSize));
        }
		#endregion

		#region Board Api
		public bool CellOccupied(Point position) => GetCell(position).piece != null;
		public void RemovePieceAtCell(Cell cell) => GetCell(cell.position).piece = null;
		public Cell GetCell(Point position) => cells.Where(c => c.position == position).FirstOrDefault();
        public bool TryGetPiece(Point position, out Piece piece) { // is there a cleaner way
            if(!InBounds(position)) {
                piece = null;
                return false;
            }
            var cell = GetCell(position);
            piece = cell.piece;
            return piece != null;
        }
        public List<Cell> GetCells() => new List<Cell>(cells);
        public static bool InBounds(Point p) => (p.X >= 0 && p.Y >= 0 && p.X < Instance.Layout.width && p.Y < Instance.Layout.width);
        public static bool InBounds(Vector2 v) => v.WithinBounds(GetBoardRect());
        public static Rectangle GetBoardRect() => new Rectangle(Point.Zero, new Point(Instance.Layout.width * Constants.CellSize, Instance.Layout.height * Constants.CellSize));
        #endregion
    }
}
