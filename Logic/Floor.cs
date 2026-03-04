using System.Diagnostics;
using HowlDev.Simulation.Physics.Primitive2D;

namespace Logic;

public class Floor {
    private int maxWidth;
    private Point2D[] points;
    public Point2D[] GetPoints() {
        return points;
    }

    private static int MinHeight => 50;
    private static int HeightDifference => 30;

    public Floor(int totalCount, int maxWidth, int maxHeight) {
        int currentHeight = maxHeight / 2;
        List<int> heights = [];
        Random r = new Random();
        for (int i = 0; i < totalCount; i++) {
            int semiNormal = (r.Next(-HeightDifference, HeightDifference) + r.Next(-HeightDifference, HeightDifference)) / 2;
            int result = currentHeight + semiNormal;
            heights.Add(result);
            currentHeight = result;
        }

        this.maxWidth = maxWidth;
        points = [.. InitializePoints([.. heights])];
    }

    /// <summary>
    /// Initializes the internal array with an array of points spaced evenly
    /// between 0 and the max size.
    /// </summary>
    public Floor(int[] pointHeights, int maxWidth) {
        this.maxWidth = maxWidth;
        points = [.. InitializePoints(pointHeights)];
    }

    public double GetHeightAt(double xCoordinate) {
        for (int i = 1; i < points.Length; i++) {
            if (xCoordinate <= points[i].X) {
                Equation2D e = new Equation2D(points[i - 1], points[i]);
                double result = e.Slope * xCoordinate + e.Intercept;
                return result;
            }
        }

        throw new Exception("Coordinate was not within expected bounds.");
    }

    /// <summary>
    /// Returns true if the provided point is beneath the floor. Defaults to False if x-coordinate 
    /// is outside of the range.
    /// </summary>
    public bool IsUnder(Point2D point) {
        double coordinate = GetHeightAt(point.X);
        if (point.Y <= coordinate) return true;
        return false;
    }

    /// <summary>
    /// Given a point and radius, decreases the heights of nearby points to simulate destructive environments.
    /// </summary>
    /// <param name="point">Point of the explosion</param>
    /// <param name="radius">Radius of the floor to remove</param>
    public void CalculateExplosion(Point2D point, int radius) {
        IEnumerable<(Point2D point, int index)> affectedPoints = points.Select((a, i) => (a, i)).Where(a => Math.Abs(a.a.X - point.X) <= radius);
        foreach ((Point2D p, int i) in affectedPoints) {
            double xDistance = Math.Abs(point.X - p.X);
            double yDistance = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(xDistance, 2));
            if (p.Y <= point.Y) {
                if (p.GetDistance(point) > radius) continue;

                yDistance = Math.Min(yDistance, radius - Math.Abs(p.Y - point.Y));
                points[i] = new Point2D(p.X, Math.Max(p.Y - yDistance, MinHeight));
            } else {
                double removalHeight = yDistance + Math.Min(p.Y - point.Y, yDistance);
                points[i] = new Point2D(p.X, Math.Max(p.Y - removalHeight, MinHeight));
            }
        }
    }

    private Point2D[] InitializePoints(int[] pointHeights) {
        int length = pointHeights.Length;
        int[] x_coords = [.. MathHelpers.GetSpacedOutValues(length, maxWidth)];
        Debug.Assert(x_coords.Length == length);
        List<Point2D> calcPoints = [];
        for (int i = 0; i < length; i++) {
            calcPoints.Add(new Point2D(x_coords[i], Math.Max(pointHeights[i], MinHeight)));
        }

        return [.. calcPoints];
    }
}
