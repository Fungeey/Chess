﻿using Chess.Source.Pieces;
using Microsoft.Xna.Framework;

namespace Chess.Source.PlayField {
	class Cell {
		public readonly Point position;
		public Piece piece;

		public Cell(Point position) {
			this.position = position;
		}

		public Cell(Point position, Piece piece) {
			this.position = position;
			this.piece = piece;
		}

		public static bool operator ==(Cell a, Cell b) {
			if(a is null)
				return b is null;

			if(b is null)
				return a is null;

			return a.Equals(b);
		}
		public static bool operator !=(Cell a, Cell b) {
			if(a is null)
				return !(b is null);

			if(b is null)
				return !(a is null);

			return !a.Equals(b);
		}

		public override bool Equals(object obj) {
			return this.position == ((Cell)obj).position;
		}
	}
}
