namespace Logic;

public class Game {
    private Floor floor;
    public Floor Floor => floor;
    private (int min, int max) range = (10, 990);
    private Player p;
    public Player Player => p;
    private double playerSlopeDifference = 1.5;

    public Game() {
        floor = new Floor(50, 1000, 1000);
        p = new();
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
}
