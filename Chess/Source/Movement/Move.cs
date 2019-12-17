using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess.Source.Movement {
    class Move {
        public Point targetPosition;
        public List<Cell> extraCaptures;

        public Move(Point targetPosition) {
            this.targetPosition = targetPosition;
        }

        public Move(Point targetPosition, List<Cell> otherCaptures) {
            this.targetPosition = targetPosition;
            this.extraCaptures = otherCaptures;
        }

        public Move(Point targetPosition, params Cell[] otherCaptures) {
            this.targetPosition = targetPosition;
            this.extraCaptures = new List<Cell>(otherCaptures);
        }
    }
}
