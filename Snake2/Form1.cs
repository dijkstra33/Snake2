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
        
        Random rnd = new Random();
        const int RAZMER = 8;
        
        public List<int> snakeX = new List<int>();
        public List<int> snakeY = new List<int>();
        public int NachaloZmX, NachaloZmY;      
        public int xOffset, yOffset;
        public int FoodX=1, FoodY=1;
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
            snakeY.Add(RAZMER/2);

            // Temporary solution.
            //snakeX.AddRange(new[] { 5 });
            //snakeY.AddRange(new[] { 5 });
            
            
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
            Graphics g = this.CreateGraphics();
            SolidBrush mybrush = new SolidBrush(Color.Green);
            ClientSize = new Size(RAZMER * 35, RAZMER * 35);
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

            for (int i = 0; i < snakeX.Count; i++)
            {
                board[FoodX, FoodY] = CellType.Food;
                board[snakeX[i], snakeY[i]] = CellType.Snake;
            }

            for (int i = 0; i < RAZMER; i++)
            {
                for (int j = 0; j < RAZMER; j++)
                {
                    drawLine.DrawLine(pen, 0, shagy * j, PoleX, shagy * j);
                    if ((int)board[i, j] == 1)
                    {
                        //g.FillRectangle(mybrush, (i+1) * shagx, (j+1) * shagy, shagy, shagx);
                    }
                    Console.Write((int)board[i, j]);
                }
                drawLine.DrawLine(pen, shagx*i, 0, shagx*i, PoleY);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void moving()
        {
            if (xOffset == 0) { napravlenieVniz = false; napravlenieVverh = false; }
            if (yOffset == 0) { napravlenieVlevo = false; napravlenieVpravo = false; }
            for (int i = 0; i < snakeX.Count-1; i++)
            {
                snakeX[i] = snakeX[i + 1];
                snakeY[i] = snakeY[i + 1];
            }

            int newX = snakeX[snakeX.Count - 1] + xOffset;
            int newY = snakeY[snakeY.Count - 1] + yOffset;


            if ((InsideBoard(newX, newY))&&(InsideSnake(newX,newY)))
            {
                snakeX[snakeX.Count - 1] = snakeX[snakeX.Count - 1] + xOffset;
                snakeY[snakeY.Count - 1] = snakeY[snakeY.Count - 1] + yOffset;
                if ((snakeX[snakeX.Count - 1] == FoodX) && (snakeY[snakeY.Count - 1] == FoodY))
                {

                    snakeX.Add(FoodX);
                    snakeY.Add(FoodY);
                    board[FoodX, FoodY] = CellType.Snake;
                }
            }
            else
            {
                timer1.Stop();
                MessageBox.Show("XAXA");
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
