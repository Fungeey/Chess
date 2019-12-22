using Chess.Source.Gameplay;
using Chess.Source.PlayField;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess.Source.Movement {
    class Move {
        public Point targetPosition;
        public List<Cell> extraCaptures;
        public List<Turn> extraTurns;

        public Move(Point targetPosition) {
            this.targetPosition = targetPosition;
        }

        public Move(Point targetPosition, List<Cell> extraTurns) {
            this.targetPosition = targetPosition;
            this.extraCaptures = extraTurns;
        }

        public Move(Point targetPosition, List<Turn> extraTurns) {
            this.targetPosition = targetPosition;
            this.extraTurns = extraTurns;
        }

        public Move(Point targetPosition, params Cell[] extraTurns) {
            this.targetPosition = targetPosition;
            this.extraCaptures = new List<Cell>(extraTurns);
        }

        public Move(Point targetPosition, params Turn[] extraTurns) {
            this.targetPosition = targetPosition;
            this.extraTurns = new List<Turn>(extraTurns);
        }
    }
}
