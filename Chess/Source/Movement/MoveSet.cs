using System.Collections.Generic;
using System.Linq;

namespace Chess.Source.Movement {
    class MoveSet
    {
        public static readonly MoveSet Pawn = new MoveSet("o1^,oi2^,c1>");
        public static readonly MoveSet Knight = new MoveSet("~1/2");
        public static readonly MoveSet Bishop = new MoveSet("n<>");
        public static readonly MoveSet Rook = new MoveSet("n^|=");
        public static readonly MoveSet Queen = new MoveSet("n^|=<>");
        public static readonly MoveSet King = new MoveSet("1^|=<>");

        public List<MoveDefinition> moves;

        public MoveSet(string definition)
        {
            moves = ParseMoveSet(definition).ToList();
        }

        private IEnumerable<MoveDefinition> ParseMoveSet(string definition)
        {
            foreach(string s in definition.Split(',')) {
                if(s.Contains('~'))
                    yield return new LeaperMoveDefinition(s.Replace("~", ""));

                yield return new MoveDefinition(s);
            }
        }
    }
}
