using Chess.Source.Pieces;
using System;

namespace Chess.Source.Gameplay {
	abstract class Player {
		public Turn turn;
		public TurnManager turnManager;
		public readonly PieceColor color;

		public Action<Turn> OnTurnCompleted;

		public virtual void StartTurn() {
			turn = new Turn();
		}

		public abstract void DoTurn();

		public Player(PieceColor color) {
			this.color = color;
		}
	}
}
