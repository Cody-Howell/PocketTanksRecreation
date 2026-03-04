namespace Logic;

public class Game {
    private Floor floor;
    public Floor Floor => floor;

    public Game() {
        floor = new Floor(50, 1000, 1000);
    }
}
