using System.Diagnostics;
using HowlDev.Simulation.Physics.Primitive2D;

namespace Logic;

public class Floor {
    private int maxWidth;
    private Point2D[] points;
    public IEnumerable<Point2D> GetPoints() {
        return points;
    }

    private static int MinHeight => 20;

    /// <summary>
    /// Initializes the internal array with an array of points spaced evenly
    /// between 0 and the max size.
    /// </summary>
    public Floor(int[] pointHeights, int maxWidth) {
        int length = pointHeights.Length;
        int[] x_coords = [.. MathHelpers.GetSpacedOutValues(length, maxWidth)];
        Debug.Assert(x_coords.Length == length);
        List<Point2D> calcPoints = [];
        for (int i = 0; i < length; i++) {
            calcPoints.Add(new Point2D(x_coords[i], Math.Max(pointHeights[i], MinHeight)));
        }

        points = [.. calcPoints];
        this.maxWidth = maxWidth;
    }

    /// <summary>
    /// Returns true if the provided point is beneath the floor. Defaults to False if x-coordinate 
    /// is outside of the range.
    /// </summary>
    public bool IsUnder(Point2D point) {
        for (int i = 1; i < points.Length; i++) {
            if (point.X <= points[i].X) {
                Equation2D e = new Equation2D(points[i-1], points[i]);
                double result = e.Slope * point.X + e.Intercept;
                if (point.Y > result) return false;
                return true;
            }
        }

        return false;
    }

    // /// <summary>
    // /// Given a point and radius, decreases the heights of nearby points to simulate destructive environments.
    // /// </summary>
    // /// <param name="point">Point of the explosion</param>
    // /// <param name="radius">Radius of the floor to remove</param>
    // public void CalculateExplosion(Point2D point, int radius) {

    // }
}
