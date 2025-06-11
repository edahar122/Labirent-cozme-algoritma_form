namespace odevLab {
    public partial class Form1 : Form {
        private Button btnCreateMaze;
        private Button btnSolveMaze;
        private Button btnSolveMazeDFS;
        private Button btnClearMaze;
        private PictureBox mazeBox;
        private Label lblStepCount;
        private Label lblPathLength;

        private int[,] maze;
        private Point start = new Point(0, 0);
        private Point end = new Point(mazeSize - 1, mazeSize - 1);
        private const int cellSize = 25;
        private const int mazeSize = 25;
        private int bfsStepCount;
        private int dfsStepCount;
        int PathLength = 0;


        public Form1() {
            InitializeComponent();
            this.Load += Form1_Load;
            GenerateRandomMaze();
            label2.Text = "";
            label2.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e) {
            btnCreateMaze = new Button {
                Text = "Labirent Oluþtur",
                Location = new Point(10, 10),
                Size = new Size(120, 30)
            };
            btnCreateMaze.Click += BtnCreateMaze_Click;
            this.Controls.Add(btnCreateMaze);

            btnSolveMaze = new Button {
                Text = "Labirenti Çöz (BFS)",
                Location = new Point(140, 10),
                Size = new Size(150, 30)
            };

            btnSolveMaze.Click += BtnSolveMaze_Click;
            this.Controls.Add(btnSolveMaze);

            btnSolveMazeDFS = new Button {
                Text = "Labirenti Çöz (DFS)",
                Location = new Point(300, 10),
                Size = new Size(150, 30)
            };
            btnSolveMazeDFS.Click += BtnSolveMazeDFS_Click;
            this.Controls.Add(btnSolveMazeDFS);

            btnClearMaze = new Button {
                Text = "Temizle",
                Location = new Point(460, 10),
                Size = new Size(100, 30)
            };
            btnClearMaze.Click += BtnClearMaze_Click;
            this.Controls.Add(btnClearMaze);

            lblStepCount = new Label {
                Text = "Adýmlar: 0",
                Location = new Point(680, 10),
                Size = new Size(100, 50)
            };
            this.Controls.Add(lblStepCount);

            lblPathLength = new Label {
                Text = "Yol Uzunlugu: 0",
                Location = new Point(680, 60),
                Size = new Size(100, 150)
            };
            this.Controls.Add(lblPathLength);

            mazeBox = new PictureBox {
                Location = new Point(10, 50),
                Size = new Size(mazeSize * cellSize, mazeSize * cellSize),
                BorderStyle = BorderStyle.FixedSingle
            };
            mazeBox.MouseClick += MazeBox_MouseClick;
            mazeBox.Paint += MazeBox_Paint;
            this.Controls.Add(mazeBox);
        }

        private void BtnCreateMaze_Click(object sender, EventArgs e) {
            GenerateRandomMaze();
            mazeBox.Invalidate();
            bfsStepCount = 0;
            dfsStepCount = 0;
            PathLength = 0;
            start = new Point(0, 0);
            end = new Point(mazeSize - 1, mazeSize - 1);
            lblPathLength.Text = "Yol Uzunlugu: " + PathLength;
            lblStepCount.Text = "Adýmlar: " + dfsStepCount;
        }

        private void BtnSolveMaze_Click(object sender, EventArgs e) {
            ClearMaze();
            SolveMazeBFS();
        }

        private void BtnSolveMazeDFS_Click(object sender, EventArgs e) {
            ClearMaze();
            SolveMazeDFS();
        }

        private void MazeBox_MouseClick(object sender, MouseEventArgs e) {
            int x = e.X / cellSize;
            int y = e.Y / cellSize;

            if (x >= 0 && x < mazeSize && y >= 0 && y < mazeSize && maze[y, x] != 1) {
                if (e.Button == MouseButtons.Left) {
                    start = new Point(x, y);
                }
                else if (e.Button == MouseButtons.Right) {
                    end = new Point(x, y);
                }
                mazeBox.Invalidate();
            }
        }

        private void BtnClearMaze_Click(object sender, EventArgs e) {
            ClearMaze();
            start = new Point(0, 0);
            end = new Point(mazeSize - 1, mazeSize - 1);
        }

        private void ClearMaze() {
            for (int y = 0; y < mazeSize; y++) {
                for (int x = 0; x < mazeSize; x++) {
                    if (maze[y, x] == 2 || maze[y, x] == 3) {
                        maze[y, x] = 0;
                    }
                }
            }

            bfsStepCount = 0;
            dfsStepCount = 0;
            PathLength = 0;

            lblStepCount.Text = "Adýmlar: " + dfsStepCount;

            mazeBox.Invalidate();
        }

        private void MazeBox_Paint(object sender, PaintEventArgs e) {
            if (maze == null) return;

            for (int y = 0; y < mazeSize; y++) {
                for (int x = 0; x < mazeSize; x++) {
                    Brush brush;

                    if (x == start.X && y == start.Y)
                        brush = Brushes.Yellow;
                    else if (x == end.X && y == end.Y)
                        brush = Brushes.Orange;
                    else if (maze[y, x] == 1)
                        brush = Brushes.Black;
                    else if (maze[y, x] == 2) {
                        brush = Brushes.Green;
                        PathLength++;
                    }
                    else if (maze[y, x] == 3) {
                        brush = Brushes.Blue;
                        PathLength++;
                    }
                    else
                        brush = Brushes.White;

                    e.Graphics.FillRectangle(brush, x * cellSize, y * cellSize, cellSize, cellSize);
                    e.Graphics.DrawRectangle(Pens.Gray, x * cellSize, y * cellSize, cellSize, cellSize);
                }
            }
            lblPathLength.Text = "Yol Uzunlugu: " + PathLength;
        }

        private void GenerateRandomMaze() {
            Random rand = new Random();
            maze = new int[mazeSize, mazeSize];

            for (int y = 0; y < mazeSize; y++) {
                for (int x = 0; x < mazeSize; x++) {
                    maze[y, x] = rand.Next(100) < 30 ? 1 : 0;
                }
            }

            maze[0, 0] = 0;
            maze[mazeSize - 1, mazeSize - 1] = 0;
        }

        private void SolveMazeBFS() {
            int[,] directions = new int[,]
            {
        { 0, -1 },
        { -1, 0 },
        { 0, 1 },
        { 1, 0 }
            };

            bool[,] visited = new bool[mazeSize, mazeSize];
            Node[,] parents = new Node[mazeSize, mazeSize];

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(new Node(start.X, start.Y));
            visited[start.Y, start.X] = true;

            bool pathFound = false;
            bfsStepCount = 0;

            while (queue.Count > 0) {
                Node current = queue.Dequeue();
                bfsStepCount++;

                lblStepCount.Text = "Adýmlar: " + bfsStepCount;

                if (current.X == end.X && current.Y == end.Y) {
                    pathFound = true;
                    break;
                }

                for (int i = 0; i < 4; i++) {
                    int newX = current.X + directions[i, 0];
                    int newY = current.Y + directions[i, 1];

                    if (newX >= 0 && newX < mazeSize && newY >= 0 && newY < mazeSize &&
                        !visited[newY, newX] && maze[newY, newX] == 0) {
                        visited[newY, newX] = true;
                        parents[newY, newX] = current;
                        queue.Enqueue(new Node(newX, newY));
                    }
                }
            }

            if (pathFound) {
                Node? current = new Node(end.X, end.Y);
                while (current != null && !(current.X == start.X && current.Y == start.Y)) {
                    if (!(current.X == end.X && current.Y == end.Y))
                        maze[current.Y, current.X] = 2;

                    current = parents[current.Y, current.X];
                }

                mazeBox.Invalidate();
            }
            else {
                MessageBox.Show("Çözüm yolu bulunamadý.");
            }
        }


        private void SolveMazeDFS() {
            bool[,] visited = new bool[mazeSize, mazeSize];
            Node[,] parents = new Node[mazeSize, mazeSize];

            bool found = DFS(new Node(start.X, start.Y), visited, parents);

            if (found) {
                Node current = new Node(end.X, end.Y);

                while (!(current.X == start.X && current.Y == start.Y)) {
                    if (!(current.X == end.X && current.Y == end.Y))
                        maze[current.Y, current.X] = 3;

                    current = parents[current.Y, current.X];
                }

                mazeBox.Invalidate();
            }
            else {
                MessageBox.Show("DFS ile çözüm bulunamadý.");
            }
        }

        private bool DFS(Node current, bool[,] visited, Node[,] parents) {
            int x = current.X;
            int y = current.Y;

            if (x < 0 || x >= mazeSize || y < 0 || y >= mazeSize)
                return false;

            if (maze[y, x] != 0 || visited[y, x])
                return false;

            visited[y, x] = true;

            dfsStepCount++;

            lblStepCount.Text = "Adýmlar: " + dfsStepCount;

            if (x == end.X && y == end.Y)
                return true;

            int[,] directions = new int[,]
            {
        { 1, 0 },
        { 0, 1 },
        { -1, 0 },
        { 0, -1 }
            };

            for (int i = 0; i < 4; i++) {
                int newX = x + directions[i, 0];
                int newY = y + directions[i, 1];

                if (DFS(new Node(newX, newY), visited, parents)) {
                    parents[newY, newX] = current;
                    return true;
                }
            }

            return false;
        }
    }
}