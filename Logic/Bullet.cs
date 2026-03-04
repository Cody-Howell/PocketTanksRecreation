using HowlDev.Simulation.Physics.Primitive2D;

namespace Logic;

public class Bullet(Point2D point, double angle) {
    private Point2D point = point;
    private Vector2D direction = new Vector2D(angle, 40);
    public Point2D Point => point;

    public void GameTick(Vector2D gravity) {
        direction += gravity;
        point += direction;
    }
}
