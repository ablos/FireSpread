using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.IO;
using OxyPlot;
using OxyPlot.Series;

namespace FireSpread
{
    public partial class Form : System.Windows.Forms.Form
    {
        Forest f;
        bool paused = false;
        bool debugColor = false;
        bool randomFire = false;
        PlotModel model;
        string waterbodyPath;
        string currentWaterbody;
        Bitmap waterbody = new Bitmap(500, 250);

        public Form()
        {
            InitializeComponent();

            AdjustRunsToDensityRange();

            fireBox.Width = 500;
            fireBox.Height = 250;

            Debug.WriteLine("W: " + fireBox.Width + " H: " + fireBox.Height);

            waterBodyDropdown.Items.Add("None");
            waterBodyDropdown.SelectedItem = "None";

            LoadWaterBodies();

            model = new PlotModel();
            model.Series.Add(new LineSeries());

            plotView.Model = model;
        }

        private void LoadWaterBodies()
        {
            waterbodyPath = Directory.GetCurrentDirectory() + "\\waterbodies";
            string[] items = Directory.GetFiles(waterbodyPath);
            foreach (string item in items )
            {
                string filename = Path.GetFileName(item);
                if (filename.EndsWith(".png"))
                    filename = filename.Substring(0, filename.LastIndexOf(".png"));

                waterBodyDropdown.Items.Add(filename);
            }
        }

        private void ResetForest()
        {
            f = new Forest(fireBox.Width, fireBox.Height, (int)burstChanceBox.Value, (int)treeChanceBox.Value, waterbody, randomFire, debugColor);
            fireBox.Image = f.ToImage();
        }

        public void StartSimulation()
        {
            paused = false;

            SetButtonState(startButton, false);
            SetButtonState(resetButton, false);
            SetButtonState(pauseButton, true);

            if (!runMultipleBox.Checked)
            {
                RunSimulation();
                AddDatapoint(0);

                if (outputFileBox.Checked)
                    WriteDataToFile(currentWaterbody, GetDataFromGraph());
            }
            else
            {
                int totalRuns = (int)runsAmountBox.Value;
                int runsPerDensity = int.Parse(runsPerDensityLabel.Text);
                
                int currentDensity = (int)treeRangeMinBox.Value;

                int[] average = new int[0];

                for (int i = 0; i < totalRuns / runsPerDensity; i++)
                {
                    Debug.WriteLine("Running " + currentDensity + "% density " + runsPerDensity + " times...");

                    for (int x = 0; x < runsPerDensity; x++)
                    {
                        //Debug.WriteLine("Run " + x + " for density: " + currentDensity);

                        f = new Forest(fireBox.Width, fireBox.Height, (int)burstChanceBox.Value, currentDensity, waterbody, randomFire, debugColor);

                        tickIndex = 0;
                        (model.Series[0] as LineSeries).Points.Clear();

                        RunSimulation();

                        int[] output = GetDataFromGraph();

                        if (output.Length > average.Length)
                        {
                            //Debug.WriteLine("Output bigger, adjusting...");

                            int[] temp = average;
                            average = new int[output.Length];

                            for (int y = 0; y < temp.Length; y++)
                            {
                                average[y] = temp[y];
                            }
                        }

                        for (int y = 0; y < output.Length; y++)
                        {
                            average[y] += output[y];
                        }
                    }

                    currentDensity++;

                    Debug.WriteLine("Done. (" + (((float)i + 1f) / ((float)totalRuns / (float)runsPerDensity) * 100f) + "% completed)");
                }

                (model.Series[0] as LineSeries).Points.Clear();
                for (int i = 0; i < average.Length; i++)
                {
                    average[i] /= totalRuns;
                    AddDatapoint(average[i], i);
                }

                if (outputFileBox.Checked)
                    WriteDataToFile(currentWaterbody + "_average_" + totalRuns + "x_" + (int)treeRangeMinBox.Value + "-" + (int)treeRangeMaxBox.Value, average);
            }

            UpdateImage(f.ToImage());
            SetLabelText(burningLabel, f.burningCells.Count);
            SetLabelText(tickLabel, tickIndex);
            UpdatePlot();

            SetButtonState(resetButton, true);
            SetButtonState(pauseButton, false);
        }

        private void RunSimulation()
        {
            f.StartFire();

            while (f.burning)
            {
                if (paused)
                    continue;

                f.Burn();

                AddDatapoint(f.burningCells.Count);

                if (showGraphBox.Checked)
                    UpdatePlot();

                if (showVisualBox.Checked)
                    UpdateImage(f.ToImage());

                if (showCountBox.Checked)
                {
                    SetLabelText(burningLabel, f.burningCells.Count);
                    SetLabelText(tickLabel, tickIndex);
                }
            }
        }

        private void WriteDataToFile(string filename, int[] data)
        {
            if (data == null || data.Length <= 0) return;

            filename += ".txt";

            Debug.WriteLine("Writing results to: " + filename);

            string path = Directory.GetCurrentDirectory() + "\\results";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += "\\" + filename;

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);

                if (lines.Length <= 0) return;

                List<string[]> series = new List<string[]>();

                foreach (string line in lines)
                {
                    series.Add(line.Split(','));
                }

                if (data.Length > series[0].Length)
                {
                    for (int x = 0; x < series.Count; x++)
                    {
                        for (int i = 0; i < data.Length - series[0].Length; i++)
                        {
                            lines[x] += ",0";
                        }
                    }

                    File.WriteAllLines(path, lines);
                }
                else if (series[0].Length > data.Length)
                {
                    List<int> _data = data.ToList();

                    for (int i = 0; i < series[0].Length - data.Length; i++)
                    {
                        _data.Add(0);
                    }

                    data = _data.ToArray();
                }
            }

            string text = data[0].ToString();

            for (int d = 1; d < data.Length; d++)
            {
                text += "," + data[d].ToString();
            }

            File.AppendAllText(path, text + Environment.NewLine);
        }

        int tickIndex = 0;

        private void SetLabelText(Label label, int amount)
        {
            if (InvokeRequired)
            {
                try { this.Invoke(new Action<Label, int>(SetLabelText), new object[] { label, amount }); }
                catch { }
                return;
            }

            label.Text = amount.ToString();
            label.Update();
        }

        private void AddDatapoint(int amount, int index = -1)
        {
            if (index == -1) index = tickIndex;

            if (InvokeRequired)
            {
                try { this.Invoke(new Action<int, int>(AddDatapoint), new object[] { amount, index }); }
                catch { }
                return;
            }

            (model.Series[0] as LineSeries).Points.Add(new DataPoint(index, amount));
            tickIndex++;
        }

        private int[] GetDataFromGraph()
        {
            LineSeries ls = model.Series[0] as LineSeries;
            int[] output = new int[ls.Points.Count];

            foreach (DataPoint dp in ls.Points)
            {
                output[(int)dp.X] = (int)dp.Y;
            }

            return output;
        }

        private void UpdatePlot()
        {
            if (InvokeRequired)
            {
                try { this.Invoke(new Action(UpdatePlot), new object[] { }); }
                catch { }
                return;
            }

            model.InvalidatePlot(true);
            plotView.Update();
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
            tickIndex = 0;
            (model.Series[0] as LineSeries).Points.Clear();

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
            SetLabelText(burningLabel, 0);
            SetLabelText(tickLabel, 0);
            SetButtonState(startButton, true);
        }

        private void waterBodyDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (waterBodyDropdown.SelectedItem == "None")
            {
                for (int x = 0; x < waterbody.Width; x++)
                {
                    for (int y = 0; y < waterbody.Height; y++)
                    {
                        waterbody.SetPixel(x, y, Color.Black);
                    }
                }
            }else
            {
                waterbody = new Bitmap(waterbodyPath + "\\" + waterBodyDropdown.SelectedItem + ".png");
            }

            ResetForest();

            currentWaterbody = waterBodyDropdown.SelectedItem.ToString();
        }

        private void treeRangeMaxBox_ValueChanged(object sender, EventArgs e)
        {
            if (treeRangeMaxBox.Value < treeRangeMinBox.Value)
                treeRangeMinBox.Value = treeRangeMaxBox.Value;

            AdjustRunsToDensityRange();
        }

        private void treeRangeMinBox_ValueChanged(object sender, EventArgs e)
        {
            if (treeRangeMinBox.Value > treeRangeMaxBox.Value)
                treeRangeMaxBox.Value = treeRangeMinBox.Value;

            AdjustRunsToDensityRange();
        }

        private void AdjustRunsToDensityRange()
        {
            int diff = (int)treeRangeMaxBox.Value - (int)treeRangeMinBox.Value;
            if (runsAmountBox.Value < diff)
                runsAmountBox.Value = diff;

            if (runsAmountBox.Value % diff != 0)
                runsAmountBox.Value -= runsAmountBox.Value % diff;

            runsAmountBox.Increment = diff;

            runsPerDensityLabel.Text = ((int)runsAmountBox.Value / diff).ToString();
        }

        private void runsAmountBox_ValueChanged(object sender, EventArgs e)
        {
            AdjustRunsToDensityRange();
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
        private bool randomFire;

        public Forest(int width, int height, int burstChance, int treeChance, Bitmap waterbody, bool randomFire, bool debugColor)
        {
            this.width = width;
            this.height = height;
            this.burstChance = burstChance;
            this.randomFire = randomFire;

            cells = new Cell[width, height];

            Random r = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (waterbody.GetPixel(x, y) == Color.FromArgb(255, 255, 255, 255))
                    {
                        cells[x, y] = new Cell(new Point(x, y), STATE.WATER, debugColor);
                        continue;
                    }

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
                //Debug.WriteLine("Setting left side on fire");
                for (int y = 0; y < height; y++)
                {
                    Cell c = cells[0, y];

                    if (c.state != STATE.TREE)
                        continue;

                    c.state = STATE.BURNING;
                    burningCells.Add(c);
                }
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
                    

                if (cells[point.X, point.Y].state != STATE.EMPTY && cells[point.X, point.Y].state != STATE.WATER)
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

    public enum STATE { EMPTY, TREE, BURNING, SMOLDERED, WATER };
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
                case STATE.WATER:
                    return Color.Blue;
                default:
                    return Color.Purple;
            }
        }
    }
}