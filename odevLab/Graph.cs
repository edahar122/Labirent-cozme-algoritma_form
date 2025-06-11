using odevLab;

public class Graph {
    public Node[,] Nodes { get; set; }
    private int _mazeSize;

    public Graph(int[,] maze, int mazeSize) {
        _mazeSize = mazeSize;
        Nodes = new Node[mazeSize, mazeSize];

        for (int y = 0; y < mazeSize; y++) {
            for (int x = 0; x < mazeSize; x++) {
                bool isWall = maze[y, x] == 1;
            }
        }
        AddNeighbors();
    }

    public void AddNeighbors() {
        int[,] directions = new int[,]
        {
            { -1, 0 },
            { 1, 0 },
            { 0, -1 },
            { 0, 1 }
        };

        for (int y = 0; y < _mazeSize; y++) {
            for (int x = 0; x < _mazeSize; x++) {
                Node currentNode = Nodes[y, x];
            }
        }
    }
}