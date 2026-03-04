namespace Logic;

public class Player {
    public int X_Coord {get;set;} = 500;
    private double rotation = 90;
    public double Rotation => rotation;

    public void AdjustRotation(double amount) {
        rotation = Math.Min(Math.Max(rotation + amount, 10), 170);
    }
}
