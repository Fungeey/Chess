using Microsoft.Xna.Framework;

namespace Chess.Source {
	static class Utility {
		public static bool WithinBounds(this Vector2 v, Rectangle rect) {
			return v.X > rect.X && v.X < (rect.X + rect.Width) && v.Y > rect.Y && v.Y < (rect.Y + rect.Height);
		}
		public static bool WithinBounds(this Point p, Rectangle rect) {
			return p.X > rect.X && p.X < (rect.X + rect.Width) && p.Y > rect.Y && p.Y < (rect.Y + rect.Height);
		}
	}
}
