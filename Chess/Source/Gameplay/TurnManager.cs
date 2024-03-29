﻿using Chess.Source.Pieces;
using Chess.Source.PlayField;
using Nez;
using System;
using System.Collections.Generic;

namespace Chess.Source.Gameplay {
    class TurnManager : SceneComponent, IUpdatable {

		public static TurnManager Instance { get => _instance; set => _instance = value; }
		private static TurnManager _instance;

		public Cell selectedCell;
		private Action OnMouseClick;

        private readonly List<Turn> turns;
		private Player player1, player2;
        public Player CurrentPlayer { get; private set; }

		public TurnManager(Player player1, Player player2) {
			if(_instance == null)
				_instance = this;

			this.player1 = CurrentPlayer = player1;
			this.player2 = player2;

			CurrentPlayer.OnTurnCompleted += ExecuteTurn;
			if(CurrentPlayer is ComputerPlayer)
				OnMouseClick += StartNextTurn;
			else
				CurrentPlayer.StartTurn();

			player1.turnManager = this;
			player2.turnManager = this;

			turns = new List<Turn>();
        }

        public override void Update() {
            base.Update();
			
			if(CurrentPlayer.hasStarted)
				CurrentPlayer.DoTurn();

			//if(Input.LeftMouseButtonPressed)				// Uncomment so computers will wait for click before playing their next turn
				OnMouseClick?.Invoke();
		}

        public void ExecuteTurn(Turn turn) {
			turn.start.piece.HasMoved = true;

            turn.start.piece.DidPawnJump =
                GameBoard.Instance.Layout == BoardLayout.DefaultLayout &&			// pawn jumping only on a default board
                turn.start.piece.type == PieceType.Pawn &&							// is a pawn
                Math.Abs(turn.end.position.Y - turn.start.position.Y) == 2;			// moved two squares in one turn

			MovePieces(turn);

			if(CurrentPlayer is HumanPlayer)
				StartNextTurn();
			else
				OnMouseClick += StartNextTurn;

		}

		private void StartNextTurn() {
			OnMouseClick = null;
			CurrentPlayer.OnTurnCompleted = null;                                   //unsubscribe from old player
			CurrentPlayer = CurrentPlayer == player1 ? player2 : player1;
			CurrentPlayer.OnTurnCompleted += ExecuteTurn;                           //subscribe to new player
			CurrentPlayer.StartTurn();
		}

		private void MovePieces(Turn turn) {
			turn.end.piece = turn.start.piece;
			turn.start.piece = null;
			turns.Add(turn);
		}
    }
}
