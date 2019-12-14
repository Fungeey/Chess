using Chess.Source.Gameplay;
using Chess.Source.Pieces;
using Microsoft.Xna.Framework;
using Nez;
using System;

namespace Chess.Source.PlayField {
    sealed class GameBoard : SceneComponent, IUpdatable {

        private static readonly Lazy<GameBoard> lazy = new Lazy<GameBoard>(() => new GameBoard());
        public static GameBoard Instance { get => lazy.Value; }

        private GameBoard() { }

        public BoardLayout Layout { get; private set; }
        public Cell? ClickedCell { get; private set; }

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

            foreach(PieceDefinition p in layout.pieces) {
                Piece e = p.ToEntity();
                e.LoadTexture(Scene);
                pieceRenderer.AddPiece(e);
            }

            boardRenderer.SetSize(layout.width, layout.height);
        }

        public override void Update() {
            base.Update();

            ClickedCell = null;
            if(Input.LeftMouseButtonPressed) {
                Vector2 mousePos = Scene.Camera.ScreenToWorldPoint(Input.MousePosition);

                if(mousePos.WithinBounds(GetBoardRect())) {
                    Point boardPos = WorldToBoard(mousePos);
                    TryGetPiece(boardPos, out var piece);

                    ClickedCell = new Cell(boardPos, piece);
                }
            }
        }

        public void ExecuteTurn(Turn turn) {
            RemovePiece(turn.start.piece);

            if(TryGetPiece(turn.end.position, out Piece toCapture))
                RemovePiece(toCapture);

            turn.start.piece.boardPosition = turn.end.position;
            AddPiece(turn.start.piece);
        }

        #region Transformations
        public static Vector2 BoardToWorld(Point boardPos) {
            return (boardPos.ToVector2()) * Constants.CellSize;
        }

        public static Point WorldToBoard(Vector2 worldPos) {
            return new Point((int)(worldPos.X / Constants.CellSize), (int)(worldPos.Y / Constants.CellSize));
        }
        #endregion

        #region Board Api
        public bool PieceExists(Point position) => pieceRenderer.pieces.ContainsKey(position);
        public void RemovePiece(Piece piece) => pieceRenderer.RemovePiece(piece);
        public void AddPiece(Piece piece) => pieceRenderer.AddPiece(piece);
        public bool TryGetPiece(Point position, out Pieces.Piece piece) => pieceRenderer.pieces.TryGetValue(position, out piece);
        public static bool InBounds(Point p) => (p.X >= 0 && p.Y >= 0 && p.X < Instance.Layout.width && p.Y < Instance.Layout.width);
        public static bool InBounds(Vector2 v) => v.WithinBounds(GetBoardRect());
        public static Rectangle GetBoardRect() => new Rectangle(Point.Zero, new Point(Instance.Layout.width * Constants.CellSize, Instance.Layout.height * Constants.CellSize));
        #endregion
    }
}
