using HowlDev.Simulation.Physics.Primitive2D;

namespace Logic;

public class Game {
    private Floor floor;
    public Floor Floor => floor;
    private (int min, int max) range = (10, 990);
    private Player p = new();
    public Player Player => p;
    private double playerSlopeDifference = 1;
    public List<Bullet> bullets = [];
    private Vector2D Gravity = new(270, 1);

    public Game() {
        floor = new Floor(50, 1000, 1000);
        p = new();
    }

    public void Tick() {
        p.Count--;

        foreach (Bullet bullet in bullets) {
            bullet.GameTick(Gravity);
        }

        // Out of bounds
        bullets.RemoveAll(a => a.Point.X < 0 || a.Point.X > 1000);

        // Hit
        List<Bullet> kaboom = [..bullets.Where(a => floor.IsUnder(a.Point))];
        foreach (Bullet b in  kaboom) {
            floor.CalculateExplosion(b.Point, 20);
            bullets.Remove(b);
        }
    }

    public void MoveCharacter(bool right) {
        double slope = floor.GetSlopeAt(p.X_Coord);
        if (right) {
            if (slope > playerSlopeDifference) return;
            p.X_Coord = Math.Min(range.max, p.X_Coord + 1);
        } else {
            if (slope < -playerSlopeDifference) return;
            p.X_Coord = Math.Max(range.min, p.X_Coord - 1);
        }
    }

    public void AdjustPlayerAngle(double angle) {
        p.AdjustRotation(angle);
    }

    public void Fire() {
        if (p.CanFire) {
            p.Count = 30;
            Point2D player = new(p.X_Coord, floor.GetHeightAt(p.X_Coord) + 10);
            Rotation2D turretRotation = p.Rotation;
            Point2D turret = player + (turretRotation * 30);
            bullets.Add(new Bullet(turret, p.Rotation));
        }
    }
}
