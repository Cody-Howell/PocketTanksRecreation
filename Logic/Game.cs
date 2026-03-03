namespace Logic;

public class Game {
    private Floor floor;
    public Floor Floor => floor;

    public Game() {
        floor = new Floor(30, 1000, 1000);
    }
}
