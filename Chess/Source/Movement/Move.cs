using Chess.Source.Gameplay;
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

        public Move(Point targetPosition, List<Cell> extraCaptures) {
            this.targetPosition = targetPosition;
            this.extraCaptures = extraCaptures;
        }

        public Move(Point targetPosition, List<Turn> extraTurns) {
            this.targetPosition = targetPosition;
        }

        public Move(Point targetPosition, params Cell[] extraCaptures) {
            this.targetPosition = targetPosition;
            this.extraCaptures = new List<Cell>(extraCaptures);
        }

        public Move(Point targetPosition, params Turn[] extraTurns) {
            this.targetPosition = targetPosition;
        }
    }
}
