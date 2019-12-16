using Chess.Source.Movement;
using Chess.Source.PlayField;
using Nez;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Chess.Source.Gameplay {
    class ComputerPlayer : IPlayer {
        public string Name { get; set; } = "beepboop";

        public ComputerPlayer(string name) {
            this.Name = name;
        }

        public bool TurnCompleted(out Turn? turn) {
            turn = null;
            var pieces = GameBoard.Instance.GetPieces();

            while(true) {
                var kvp = RandomValues(pieces).First();

                if(kvp.Value.color != TurnManager.CurrentColor)
                    continue;

                var moves = MoveGenerator.GenerateMoves(new Cell(kvp.Key, kvp.Value));

                if(moves.Count != 0) {
                    var move = moves[Random.Range(0, moves.Count)];
                    GameBoard.Instance.TryGetPiece(move, out var piece);
                    turn = new Turn(new Cell(kvp.Key, kvp.Value), new Cell(move, piece));
                    Thread.Sleep(500);
                    return true;
                }
            }
            return false;
        }

        private IEnumerable<KeyValuePair<TKey, TValue>> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict) {
            List<TValue> values = Enumerable.ToList(dict.Values);
            List<TKey> keys = Enumerable.ToList(dict.Keys);
            int size = dict.Count;
            while(true) {
                var i = Random.NextInt(size);
                yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
            }
        }
    }
}
