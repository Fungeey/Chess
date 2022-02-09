using Chess.Source.Pieces;
using System;

namespace Chess.Source.Gameplay {
	abstract class Player {
		public Turn turn;
		public TurnManager turnManager;
		public readonly PieceColor color;
		public bool hasStarted;

		public Action<Turn> OnTurnCompleted;

		public virtual void StartTurn() {
			turn = new Turn();
			hasStarted = true;
			OnTurnCompleted += (t) => hasStarted = false;
		}

		public abstract void DoTurn();

		public Player(PieceColor color) {
			this.color = color;
		}
	}
}
