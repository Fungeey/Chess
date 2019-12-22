using Chess.Source.Pieces;
using Chess.Source.PlayField;
using Nez;
using System;
using System.Collections.Generic;

namespace Chess.Source.Gameplay {
    class TurnManager : SceneComponent, IUpdatable {

        public static PieceColor CurrentColor { get; private set; }
        public static Cell selectedCell;

        private readonly List<Turn> turns;
        private readonly List<IPlayer> players;
        public IPlayer CurrentPlayer { get; private set; }
        private int playerNum;

        private GameBoard board;

        public TurnManager(params IPlayer[] players) {
            this.players = new List<IPlayer>();

            foreach(IPlayer p in players)
                AddPlayer(p);

            turns = new List<Turn>();
            CurrentColor = PieceColor.White;
        }

        public override void OnEnabled() {
            base.OnEnabled();

            board = Scene.GetSceneComponent<GameBoard>();
        }

        public override void Update() {
            base.Update();

            if(CurrentPlayer == null)
                return;

            if(CurrentPlayer.TurnCompleted(out var turn)) {
                ExecuteTurn(turn.Value);
            }
        }

        public void AddPlayer(IPlayer player) {
            if(players.Contains(player))
                return;

            players.Add(player);
            if(CurrentPlayer == null)
                CurrentPlayer = players[0];
        }

        public void ExecuteTurn(Turn turn) {
            if(turn.end.piece != null && turn.start.piece.color == turn.end.piece.color)
                return;

            CurrentColor = CurrentColor == PieceColor.Black ? PieceColor.White : PieceColor.Black;
            turn.start.piece.HasMoved = true;

            turn.start.piece.DidPawnJump =
                GameBoard.Instance.Layout == BoardLayout.DefaultLayout &&
                turn.start.piece.type == PieceType.Pawn &&
                Math.Abs(turn.end.position.Y - turn.start.position.Y) == 2;

            turns.Add(turn);
            board.ExecuteTurn(turn);

            CurrentPlayer = players[++playerNum % players.Count];
        }
    }
}
