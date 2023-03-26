using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace FireSpread
{
    public partial class Form1 : Form
    {
        Forest f;
        bool paused = false;
        bool debugColor = true;
        bool randomFire = false;

        public Form1()
        {
            InitializeComponent();

            Debug.WriteLine("W: " + fireBox.Width + " H: " + fireBox.Height);

            ResetForest();
        }

        private void ResetForest()
        {
            f = new Forest(fireBox.Width, fireBox.Height, (int)burstChanceBox.Value, (int)treeChanceBox.Value, randomFire, debugColor);
            fireBox.Image = f.ToImage();
        }

        public void StartSimulation()
        {
            paused = false;

            SetButtonState(startButton, false);
            SetButtonState(resetButton, false);
            SetButtonState(pauseButton, true);

            f.StartFire();

            while (f.burning)
            {
                if (paused)
                    continue;

                f.Burn();
                UpdateImage(f.ToImage());
                SetBurningText(f.burningCells.Count);
            }

            SetButtonState(startButton, true);
            SetButtonState(resetButton, true);
            SetButtonState(pauseButton, false);
        }

        private void SetBurningText(int amount)
        {
            if (InvokeRequired)
            {
                try { this.Invoke(new Action<int>(SetBurningText), new object[] { amount }); }
                catch { }
                return;
            }

            burningLabel.Text = amount.ToString();
            burningLabel.Update();
        }

        private void UpdateImage(Bitmap img)
        {
            if (InvokeRequired)
            {
                try { this.Invoke(new Action<Bitmap>(UpdateImage), new object[] { img }); }
                catch { }
                return;
            }

            fireBox.Image = img;
            fireBox.Update();
        }

        private void SetButtonState(Button b, bool enabled)
        {
            if (InvokeRequired)
            {
                try { this.Invoke(new Action<Button, bool>(SetButtonState), new object[] { b, enabled }); }
                catch { }
                return;
            }

            b.Enabled = enabled;
            b.Update();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                StartSimulation();
            }).Start();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            paused = !paused;
            pauseButton.Text = paused ? "Unpause" : "Pause";
            resetButton.Enabled = paused;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            f.burning = false;
            ResetForest();
            pauseButton.Text = "Pause";
            SetBurningText(0);
        }
    }

    public class Forest
    {
        public Cell[,] cells;
        public List<Cell> burningCells = new List<Cell>();

        public bool burning = false;

        private int width;
        private int height;
        private int burstChance;
        private bool debugColor;
        private bool randomFire;

        public Forest(int width, int height, int burstChance, int treeChance, bool randomFire, bool debugColor)
        {
            this.width = width;
            this.height = height;
            this.burstChance = burstChance;
            this.randomFire = randomFire;
            this.debugColor = debugColor;

            cells = new Cell[width, height];

            Random r = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = new Cell(new Point(x, y), r.Next(1, 100) < treeChance ? STATE.TREE : STATE.EMPTY, debugColor);
                }
            }
        }

        public void StartFire()
        {
            if (randomFire)
            {
                Random r = new Random();

                Cell c = cells[r.Next(0, width), r.Next(0, height)];
                Debug.WriteLine(c.p.ToString());
                c.state = STATE.BURNING;
                burningCells.Add(c);
            }
            else
            {
                Debug.WriteLine("Setting left side on fire");
                for (int y = 0; y < height; y++)
                {
                    Cell c = cells[0, y];
                    c.state = STATE.BURNING;
                    burningCells.Add(c);
                }

                Debug.WriteLine(burningCells.Count);
            }

            burning = true;
        }

        public void Burn()
        {
            bool foundCell = false;

            foreach (Cell c in burningCells.ToArray())
            {
                c.state = STATE.SMOLDERED;
                burningCells.Remove(c);

                Cell[] adjecent = AdjecentCells(c.p);
                if (adjecent.Length > 1)
                    Debug.WriteLine(adjecent.Length);

                if (adjecent.Length > 0)
                    foundCell = true;

                foreach (Cell c1 in adjecent)
                {
                    c1.state = STATE.BURNING;
                    burningCells.Add(c1);
                }
            }

            burning = foundCell;
        }

        public Cell[] AdjecentCells(Point p)
        {
            Cell[] cs = new Cell[]
            {
                FindNextInDirection(p, new Point(1, 0)),
                FindNextInDirection(p, new Point(-1, 0)),
                FindNextInDirection(p, new Point(0, 1)),
                FindNextInDirection(p, new Point(0, -1))
            };

            List<Cell> cells = new List<Cell>();
            foreach (Cell c in cs)
            {
                if (c != null) cells.Add(c);
            }

            return cells.ToArray();
        }

        private Cell FindNextInDirection(Point origin, Point dir)
        {
            Random r = new Random();

            for (int i = 1; i < 100; i++)
            {
                Point point = new Point(origin.X + dir.X * i, origin.Y + dir.Y * i);

                if (!InBounds(point))
                    return null;

                if (cells[point.X, point.Y].state == STATE.TREE && (i == 1 || r.Next(1, 101) < burstChance && r.Next(1, 100) > i))
                    return cells[point.X, point.Y];
                    

                if (cells[point.X, point.Y].state != STATE.EMPTY)
                    return null;
            }

            return null;
        }

        private bool InBounds(Point point)
        {
            if (point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height)
                return true;

            return false;
        }

        public Bitmap ToImage()
        {
            Bitmap b = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    b.SetPixel(x, y, cells[x, y].StateToColor());
                }
            }

            return b;
        }
    }

    public enum STATE { EMPTY, TREE, BURNING, SMOLDERED };
    public class Cell
    {
        public Point p = new Point();
        public STATE state = STATE.EMPTY;
        private bool debugColor;

        public Cell (Point p, STATE state, bool debugColor)
        {
            this.p = p;
            this.state = state;
            this.debugColor = debugColor;
        }

        public Color StateToColor()
        {
            switch (state)
            {
                case STATE.EMPTY:
                    return Color.Black;
                case STATE.TREE:
                    return Color.Green;
                case STATE.BURNING:
                    return Color.Red;
                case STATE.SMOLDERED:
                    return debugColor ? Color.Pink : Color.FromArgb(102, 51, 0);
                default:
                    return Color.Purple;
            }
        }
    }
}