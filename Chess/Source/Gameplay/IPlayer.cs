namespace Chess.Source.Gameplay {
	interface IPlayer {
		bool TurnCompleted(out Turn? turn);
	}
}
