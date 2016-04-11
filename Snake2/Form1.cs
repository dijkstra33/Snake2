using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Snake2
{
    public partial class Form1 : Form
    {
        Pen pen = new Pen(Color.Black);
        
        SolidBrush mybrush = new SolidBrush(Color.Green);
        SolidBrush foodBrush = new SolidBrush(Color.Red);
        SolidBrush headbrush = new SolidBrush(Color.GreenYellow);
        Random rnd = new Random();
        const int RAZMER = 10;
        
        public List<int> snakeX = new List<int>();
        public List<int> snakeY = new List<int>();
        public int NachaloZmX, NachaloZmY;      
        public int xOffset, yOffset;
        public int FoodX=3, FoodY=3, koordX, koordY, Score=0;
        public bool napravlenieVniz = false, napravlenieVverh = false, napravlenieVlevo = false, napravlenieVpravo = false;
        public CellType[,] board = new CellType[RAZMER, RAZMER];
        public Form1()
        {
            // Set default offset.
            xOffset = 1;
            yOffset = 0;
            napravlenieVniz = true;
            
            // Use after test phase.
            snakeX.Add(RAZMER/2);
            snakeY.Add(RAZMER / 2);


            InitializeComponent();
        }
        void timer1_Tick(object sender, EventArgs e)
        {

            moving();
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            Graphics drawLine = e.Graphics;
            Graphics g = pictureBox1.CreateGraphics();
            ClientSize = new Size(RAZMER * 36, RAZMER * 36);
            pictureBox1.Width = 30 * RAZMER;
            pictureBox1.Height = 30 * RAZMER;
            
            
            int PoleX = pictureBox1.Size.Width;
            int PoleY = pictureBox1.Size.Height;
            int shagx = PoleX / RAZMER;
            int shagy = PoleY / RAZMER;
            for (int i = 0; i < snakeX.Count; i++)
            {
                while ((snakeX[i]==FoodX) && (snakeY[i] == FoodY))
                {
                    FoodX = rnd.Next(0, RAZMER);
                    FoodY = rnd.Next(0, RAZMER);
                }
            }
            for (int i = 0; i < RAZMER; i++)
            {
                for (int j = 0; j < RAZMER; j++)
                {
                    board[i, j] = CellType.Empty;
                }
            }
            board[FoodX, FoodY] = CellType.Food;
            for (int i = 0; i < snakeX.Count; i++)
            {
                board[snakeX[i], snakeY[i]] = CellType.Snake;
                board[snakeX[0], snakeY[0]] = CellType.Head;
            }
            
            for (int i = 0; i < RAZMER; i++)
            {
                drawLine.DrawLine(pen, shagx * i, 0, shagx * i, PoleY);
                for (int j = 0; j < RAZMER; j++)
                {
                    drawLine.DrawLine(pen, 0, shagy * j, PoleX, shagy * j);
                    
                    if ((int)board[i, j] == 1)
                    {
                        e.Graphics.FillRectangle(mybrush, j * shagy+1, i * shagx+1, shagx-1, shagy-1);
                    }
                    if ((int)board[i, j] == 3)
                    {
                        e.Graphics.FillRectangle(headbrush, j * shagy + 1, i * shagx + 1, shagx - 1, shagy - 1);
                    }
                    if ((int)board[i, j] == 2)
                    {
                        e.Graphics.FillRectangle(foodBrush, j * shagy+1, i * shagx+1, shagx-1, shagy-1);
                    }
                } 
            }
            drawLine.DrawRectangle(pen, 0, 0, PoleX-1, PoleY-1);
        }

        private void moving()
        {
            if (xOffset == 0) { napravlenieVniz = false; napravlenieVverh = false; }
            if (yOffset == 0) { napravlenieVlevo = false; napravlenieVpravo = false; }
            int newX = snakeX[0] + xOffset;
            int newY = snakeY[0] + yOffset;
            koordX = snakeX[snakeX.Count - 1];
            koordY = snakeY[snakeY.Count - 1];

            for (int i = snakeX.Count - 1; i > 0; i--)
            {
                snakeX[i] = snakeX[i - 1];
                snakeY[i] = snakeY[i - 1];
            }
            if ((InsideBoard(newX, newY)) && (InsideSnake(newX, newY)))
            {
                snakeX[0] = snakeX[0] + xOffset;
                snakeY[0] = snakeY[0] + yOffset;

                if ((snakeX[0] == FoodX) && (snakeY[0] == FoodY))
                {
                    if (timer1.Interval > 120)
                        timer1.Interval -= 10;
                    if (snakeX.Count == 1)
                    {
                        snakeX.Add(snakeX[0] - xOffset);
                        snakeY.Add(snakeY[0] - yOffset);
                        board[snakeX[0], snakeY[0]] = CellType.Head;
                        board[snakeX[1], snakeY[1]] = CellType.Snake;
                    }
                    else
                    {
                        snakeX.Add(koordX);
                        snakeY.Add(koordY);
                        board[snakeX[0], snakeY[0]] = CellType.Head;
                        board[koordX, koordY] = CellType.Snake;
                    }
                    Score++;
                    toolStripStatusLabel1.Text = "Score:"+Score;
                }
                
            }
            
            else
            {
                timer1.Stop();
                MessageBox.Show("Score=" + Score);
            }
            
            
            


        }

        private bool InsideBoard(int x, int y)
        {
            return ((x > -1 && x < RAZMER) && (y > -1 && y < RAZMER));
        }

        private bool InsideSnake(int x, int y)
        {    
            for (int i = 0; i < snakeX.Count-1; i++)
            {
                if ((x == snakeX[i]) && (y == snakeY[i]))
                return false;
            }
            return true;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) timer1.Start();
            if ((e.KeyCode == Keys.W) && ((napravlenieVniz != true)||(snakeX.Count<2))) { xOffset = -1; napravlenieVverh = true; yOffset = 0; }
            if ((e.KeyCode == Keys.S) && ((napravlenieVverh != true)||(snakeX.Count<2))) { xOffset = 1; napravlenieVniz = true; yOffset = 0; }
            if ((e.KeyCode == Keys.D) && ((napravlenieVlevo != true)||(snakeY.Count<2))) { yOffset = 1; napravlenieVpravo = true; xOffset = 0; }
            if ((e.KeyCode == Keys.A) && ((napravlenieVpravo != true)||(snakeY.Count<2))) { yOffset = -1; napravlenieVlevo = true; xOffset = 0; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
