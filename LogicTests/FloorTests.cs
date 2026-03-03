using HowlDev.Simulation.Physics.Primitive2D;
using Logic;

namespace LogicTests;

public class FloorTests {
    [Test]
    public async Task FloorCoordinates() {
        Floor f = new Floor([70, 65, 80, 70, 60, 50], 100);
        IEnumerable<Point2D> points = f.GetPoints();
        await Assert.That(points).IsEquivalentTo([
            new Point2D(0, 70),
            new Point2D(20, 65),
            new Point2D(40, 80),
            new Point2D(60, 70),
            new Point2D(80, 60),
            new Point2D(100, 50),
            ]);
    }

    [Test]
    [Arguments(20, 20, true)]
    [Arguments(20, 70, false)]
    [Arguments(80, 70, false)]
    [Arguments(80, 59, true)]
    [Arguments(50, 76, false)]
    [Arguments(50, 74, true)]
    [Arguments(30, 70, true)]
    [Arguments(30, 75, false)]
    public async Task PointIsUnder(double x, double y, bool exp) {
        Floor f = new Floor([70, 65, 80, 70, 60, 50], 100);
        await Assert.That(f.IsUnder(new Point2D(x, y))).IsEqualTo(exp);
    }
}
