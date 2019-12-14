using Chess.Source.PlayField;

namespace Chess.Source.Gameplay {
	struct Turn {
		public Cell start, end;

		public Turn(Cell start, Cell end) {
			this.start = start;
			this.end = end;
		}
	}
}
