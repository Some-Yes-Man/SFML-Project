using System.Numerics;

namespace SfmlProject.Geometry.Utils {
    public class GeometryUtils {
        public static float CrossProduct(Vector2 vectorA, Vector2 vectorB) {
            return vectorA.X * vectorB.Y - vectorA.Y * vectorB.X;
        }
    }
}
