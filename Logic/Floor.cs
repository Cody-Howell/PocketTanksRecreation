// using HowlDev.Simulation.Physics.Primitive2D;

namespace Logic;

public class Floor {
    private int maxWidth;
    private IEnumerable<int> pointHeights;
    // public IEnumerable<Point2D> Points {
    //     get {
    //         return [];
    //     }
    // }

    // private static const int minHeight = 20;

    /// <summary>
    /// Initializes the internal array with an array of points spaced evenly
    /// between 0 and the max size.
    /// </summary>
    public Floor(IEnumerable<int> pointHeights, int maxWidth) {
        this.pointHeights = pointHeights;
        this.maxWidth = maxWidth;
    }
}
