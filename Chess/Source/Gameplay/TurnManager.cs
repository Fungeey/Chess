using Chess.Source.Pieces;
using Chess.Source.PlayField;
using Nez;
using System.Collections.Generic;

namespace Chess.Source.Gameplay {
    class TurnManager : SceneComponent, IUpdatable {

        public static PieceColor CurrentColor { get; private set; }
        public static Cell? selectedCell;

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

            turns.Add(turn);
            board.ExecuteTurn(turn);

            CurrentPlayer = players[++playerNum % players.Count];
        }
    }
}
