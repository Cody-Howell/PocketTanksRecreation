using HowlDev.Simulation.Physics.Primitive2D;
using Logic;

namespace LogicTests;

public class FloorTests {
    [Test]
    public async Task FloorCoordinates() {
        Floor f = new Floor([70, 65, 80, 70, 60, 50], 100);
        Point2D[] points = f.GetPoints();
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

    [Test]
    public async Task FloorCanBeDestructed1() {
        Floor f = new Floor([70, 65, 80, 70, 60, 50], 100);
        f.CalculateExplosion(new Point2D(40, 80), 10);
        await Assert.That(f.GetPoints()[2].Y).IsEqualTo(70);
    }

    [Test]
    public async Task FloorCanBeDestructed2() {
        Floor f = new Floor([70, 65, 80, 70, 60, 50], 100);
        f.CalculateExplosion(new Point2D(40, 70), 10);
        await Assert.That(f.GetPoints()[2].Y).IsEqualTo(60);
    }

    [Test]
    public async Task FloorCanBeDestructed3() {
        Floor f = new Floor([70, 65, 80, 70, 60, 50], 100);
        f.CalculateExplosion(new Point2D(40, 75), 10);
        await Assert.That(f.GetPoints()[2].Y).IsEqualTo(65);
    }

    [Test]
    public async Task FloorCanBeDestructed4() {
        Floor f = new Floor([70, 65, 80, 70, 60, 50], 100);
        f.CalculateExplosion(new Point2D(40, 85), 10);
        await Assert.That(f.GetPoints()[2].Y).IsEqualTo(75);
    }

    [Test]
    public async Task FloorCanBeDestructed5() {
        Floor f = new Floor([70, 65, 80, 70, 60, 50], 100);
        f.CalculateExplosion(new Point2D(40, 90), 10);
        await Assert.That(f.GetPoints()[2].Y).IsEqualTo(80);
    }

    [Test]
    public async Task FloorCanBeDestructed6() {
        Floor f = new Floor([70, 65, 80, 70, 60, 50], 10);
        f.CalculateExplosion(new Point2D(5, 70), 5);
        await Assert.That(f.GetPoints()[1].Y).IsEqualTo(65);
        await Assert.That(f.GetPoints()[2].Y).IsLessThan(80);
        await Assert.That(f.GetPoints()[3].Y).IsLessThan(70);
        await Assert.That(f.GetPoints()[4].Y).IsEqualTo(60);
        await Assert.That(f.GetPoints()[5].Y).IsEqualTo(50);
    }
}
