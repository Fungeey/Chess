using System;
using System.Linq;

namespace Chess.Source.Movement
{
    class MoveDefinition
    {
        public MoveDirection direction;
        public int distance;
        public MoveCondition condition;

        public MoveDefinition() {

        }

        public MoveDefinition(MoveDirection direction, int distance, MoveCondition condition)
        {
            this.direction = direction;
            this.distance = distance;
            this.condition = condition;
        }

        public MoveDefinition(string definition) {
            this.direction = MoveDirection.None;
            this.condition = MoveCondition.Default;

            foreach(char c in definition) {
                if("^|=><".Contains(c))
                    this.direction |= (MoveDirection)Math.Pow(2, "^|=><".IndexOf(c) + 1);

                if("12n".Contains(c))
                    this.distance = c == 'n' ? int.MaxValue : (int)char.GetNumericValue(c);

                if("ico".Contains(c))
                    this.condition = (MoveCondition)c;
            }

            direction &= ~MoveDirection.None;
        }
    }

    [Flags]
    public enum MoveDirection
    {
        None = 1,
        CardinalForwards = 2,
        CardinalBackwards = 4,
        CardinalSideways = 8,
        DiagonalForwards = 16,
        DiagonalBackwards = 32
    }

    public enum MoveCondition
    {
        Default,
        Initial = 'i',
        Capture = 'c',
        NotCapture = 'o'
    }
}
