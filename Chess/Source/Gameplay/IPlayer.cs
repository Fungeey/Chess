namespace Chess.Source.Gameplay {
	interface IPlayer {
		string Name { get; set; }
		bool TurnCompleted(out Turn? turn);
	}
}
