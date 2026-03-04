namespace Logic;

public class Player {
    public int X_Coord {get;set;} = 500;
    private double rotation = 90;
    public double Rotation => rotation;
    public int Count {get => field; set {
            if (value < 0) field = 0;
            else field = value;
        }
    }
    public bool CanFire => Count == 0;

    public void AdjustRotation(double amount) {
        rotation = Math.Min(Math.Max(rotation + amount, 10), 170);
    }
}
