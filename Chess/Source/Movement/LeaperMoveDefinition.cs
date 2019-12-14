using Microsoft.Xna.Framework;
using System.Linq;

namespace Chess.Source.Movement {
    class LeaperMoveDefinition : MoveDefinition {
        public Point leapDistance;
        public LeaperMoveDefinition(string definition) {
            foreach(char c in definition) {
                if("ico".Contains(c)) {
                    condition = (MoveCondition)c;
                    definition = definition.Replace(c+"", "");
                }
            }

            var d = definition.Split('/');

            if(d.Length != 2)
                throw new System.ArgumentException("Leaper Move Definition is invalid!");

            leapDistance = new Point(int.Parse(d[0]), int.Parse(d[1]));
        }
    }
}
