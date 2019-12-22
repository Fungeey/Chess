using Chess.Source.PlayField;
using System.Collections.Generic;

namespace Chess.Source.Gameplay {
	struct Turn {
		public Cell start, end;
		public List<Cell> extraCaptures;
		public List<Turn> extraTurns;

		public Turn(Cell start, Cell end, List<Cell> extraCaptures) {
			this.start = start;
			this.end = end;
			this.extraCaptures = extraCaptures;
			this.extraTurns = null;
		}

		public Turn(Cell start, Cell end, params Cell[] extraCaptures) {
			this.start = start;
			this.end = end;
			this.extraCaptures = new List<Cell>(extraCaptures);
			this.extraTurns = null;
		}
	}
}
