using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace AnimationEngine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //rectangle();
            cells();
            //image();
        }

        private void image() {
            int pos = 1;

            PaintEngine paint = new PaintEngine(this, pictureBox1);
            PaintEngineImage image = new PaintEngineImage(Properties.Resources.pos1, 50, 250);
            paint.Objects.Add(image);

            

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);

                    if (pos == 1){
                        image.Image = Properties.Resources.pos1;
                    }else if (pos == 2){
                        image.Image = Properties.Resources.pos2;
                    }else if (pos == 3){
                        image.Image = Properties.Resources.pos3;

                        pos = 1;
                    }

                    pos++;
                    paint.Show();
                }
            }).Start();
        }

        private void rectangle() {
            int XAdd = 0;
            int YAdd = 0;

            int moveByEveryTick = 8;


            int recWidth = 150;
            int recHeight = 150;

            int minWidth = 100;
            int minHeight = 100;

            int maxWidth = 300;
            int maxHeight = 300;

            int WidthAdd = 0;
            int HeightAdd = 0;
            int increaseByEveryTick = 0;

            PaintEngine paint = new PaintEngine(this, pictureBox1);
            PaintEngineRectangle rec = new PaintEngineRectangle(Color.Navy, 1, 0, 0, recWidth, recHeight, true);
            paint.Objects.Add(rec);
            paint.Show();

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(5);

                    if (pictureBox1.Width <= rec.X1 + rec.Width)
                    {//X
                        XAdd = -moveByEveryTick;
                    }
                    else if (rec.X1 <= 0)
                    {
                        XAdd = moveByEveryTick;
                    }

                    if (pictureBox1.Height <= rec.Y1 + rec.Height)
                    {//Y
                        YAdd = -moveByEveryTick;
                    }
                    else if (rec.Y1 <= 0)
                    {
                        YAdd = moveByEveryTick;
                    }

                    rec.X1 += XAdd;
                    rec.Y1 += YAdd;

                    //rec size
                    if (rec.Width >= maxWidth)
                    {
                        WidthAdd = -increaseByEveryTick;
                    }
                    else if (rec.Width <= minWidth)
                    {
                        WidthAdd = increaseByEveryTick;
                    }

                    if (rec.Height >= maxHeight)
                    {
                        HeightAdd = -increaseByEveryTick;
                    }
                    else if (rec.Height <= minHeight)
                    {
                        HeightAdd = increaseByEveryTick;
                    }

                    rec.X1 -= WidthAdd / 2;
                    rec.Y1 -= HeightAdd / 2;
                    rec.Width += WidthAdd;
                    rec.Height += HeightAdd;

                    paint.Show();
                }
            }).Start();
        }

        DateTime time;
        int counter = 0;

        private void cells() {
            PaintEngineGrid grid = new PaintEngineGrid(12, 12, Color.Black, 1, 15, 15);

            Color[] colors = new Color[] {Color.Orange, Color.Red, Color.Purple, Color.Yellow, Color.Green,
            Color.Honeydew, Color.IndianRed, Color.Khaki, Color.Ivory, Color.LightSteelBlue, Color.MidnightBlue};

            PaintEngine paint = new PaintEngine(this, pictureBox1);
            int width = (grid.CellWidth * grid.NumberOfColumns) + (grid.Thickness * grid.NumberOfColumns) + grid.Thickness;
            int height = (grid.CellHeight * grid.NumberOfRows) + (grid.Thickness * grid.NumberOfRows) + grid.Thickness;
            PaintEngineSelection selection = new PaintEngineSelection(0, 0, width, height);
            selection.Objects.Add(grid);
            paint.Objects.Add(selection);

            //START

            int XAdd = 0;
            int YAdd = 0;

            int moveByEveryTick = 8;

            DateTime oldTime = new DateTime();

            new Thread(() =>
            {
                oldTime = DateTime.Now;
                time = DateTime.Now;

                while (true)
                {
                    Thread.Sleep(15);

                    if (pictureBox1.Width <= selection.X1 + selection.Width)
                    {//X
                        XAdd = -moveByEveryTick;
                    }
                    else if (selection.X1 <= 0)
                    {
                        XAdd = moveByEveryTick;
                    }

                    if (pictureBox1.Height <= selection.Y1 + selection.Height)
                    {//Y
                        YAdd = -moveByEveryTick;
                    }
                    else if (selection.Y1 <= 0)
                    {
                        YAdd = moveByEveryTick;
                    }

                    selection.X1 += XAdd;
                    selection.Y1 += YAdd;


                    //colors
                    if ((DateTime.Now - oldTime).TotalSeconds >= 0.2) {
                        Random rand = new Random();

                        for (int x = 0; x < grid.NumberOfColumns; x++)
                        {
                            for (int y = 0; y < grid.NumberOfRows; y++)
                            {
                                grid.Cells[x, y] = new PaintEngineCell(true, grid.Color, grid.Thickness);
                                grid.Cells[x, y].Fill = true;
                                grid.Cells[x, y].FillColor = colors[rand.Next(colors.Length)];
                            }
                        }

                        oldTime = DateTime.Now;
                    }

                    counter++;
                    setFPS();

                    paint.Show();
                }
            }).Start();
        }

        private void setFPS()
        {
            TimeSpan timeSpan = DateTime.Now - time;
            if (timeSpan.TotalSeconds >= 1)
            {
                double fps = (1.0 / timeSpan.TotalSeconds) * counter;
                Debug.WriteLine("Bitmap Test Animation - FPS: " + fps);
                time = DateTime.Now;
                counter = 0;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                Debug.WriteLine("Space");
            }else if (e.KeyCode == Keys.Shift)
            {

            }
        }
    }
}
