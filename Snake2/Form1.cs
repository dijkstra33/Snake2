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
        
        SolidBrush snakeBrush = new SolidBrush(Color.Green);
        SolidBrush foodBrush = new SolidBrush(Color.Red);
        SolidBrush headBrush = new SolidBrush(Color.GreenYellow);
        Random rnd = new Random();
        const int SIZE = 10;
        public List<int> snakeX = new List<int>();
        public List<int> snakeY = new List<int>();
        public int xOffset, yOffset;
        public int FoodX=3, FoodY=3, coordX, coordY, Score=0;
        public bool isDirectionDown = false, isDirectionUp = false, isDirectionLeft = false, isDirectionRight = false;
        public CellType[,] board = new CellType[SIZE, SIZE];
        int[] foodPlace = new int[SIZE * SIZE];
        public Form1()
        {
            // Set default offset.
            xOffset = 0;
            yOffset = 1;
            isDirectionRight = true;
            
            // Use after test phase.
            snakeX.Add(SIZE / 2);
            snakeY.Add(SIZE / 2);

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
            ClientSize = new Size(SIZE * 36, SIZE * 36+10);
            pictureBox1.Width = 30 * SIZE;
            pictureBox1.Height = 30 * SIZE;
           
            int pictureBoxWidth = pictureBox1.Size.Width;
            int pictureBoxHeight = pictureBox1.Size.Height;
            int OneCellx = pictureBoxWidth / SIZE;
            int OneCelly = pictureBoxHeight / SIZE;

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
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
            
            for (int i = 0; i < SIZE; i++)
            {
                drawLine.DrawLine(pen, OneCellx * i, 0, OneCellx * i, pictureBoxHeight);
                for (int j = 0; j < SIZE; j++)
                {
                    drawLine.DrawLine(pen, 0, OneCelly * j, pictureBoxWidth, OneCelly * j);
                    
                    if (board[i, j] == CellType.Snake)
                    {
                        e.Graphics.FillRectangle(snakeBrush, j * OneCelly+1, i * OneCellx+1, OneCellx-1, OneCelly-1);
                    }
                    if (board[i, j] == CellType.Head)
                    {
                        e.Graphics.FillRectangle(headBrush, j * OneCelly + 1, i * OneCellx + 1, OneCellx - 1, OneCelly - 1);
                    }
                    if (board[i, j] == CellType.Food)
                    {
                        e.Graphics.FillRectangle(foodBrush, j * OneCelly+1, i * OneCellx+1, OneCellx-1, OneCelly-1);
                    }
                } 
            }
            drawLine.DrawRectangle(pen, 0, 0, pictureBoxWidth-1, pictureBoxHeight-1);
        }

        public int FoodRespawn()
        {      
            int Count = 0;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (board[i, j] == CellType.Empty)
                    {
                        foodPlace[Count] = i * 10 + j;
                        Count++;
                    }
                }
            } 
            int IndexOfRespawn = rnd.Next(0, Count-1);
            return IndexOfRespawn; 
        }

        private void moving()
        {
            if (xOffset == 0) { isDirectionDown = false; isDirectionUp = false; }
            if (yOffset == 0) { isDirectionLeft = false; isDirectionRight = false; }
            int newX = snakeX[0] + xOffset;
            int newY = snakeY[0] + yOffset;
            coordX = snakeX[snakeX.Count - 1];
            coordY = snakeY[snakeY.Count - 1];

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
                     if (timer1.Interval > 150)
                     timer1.Interval -= 5;
                    if (snakeX.Count == 1)
                    {
                        snakeX.Add(snakeX[0] - xOffset);
                        snakeY.Add(snakeY[0] - yOffset);
                        board[snakeX[0], snakeY[0]] = CellType.Head;
                        board[snakeX[1], snakeY[1]] = CellType.Snake;
                    }
                    else
                    {
                        snakeX.Add(coordX);
                        snakeY.Add(coordY);
                        board[snakeX[0], snakeY[0]] = CellType.Head;
                        board[coordX, coordY] = CellType.Snake;
                    }
                    Score++;
                    toolStripStatusLabel1.Text = "Score:" + Score;
                    if (Score == SIZE * SIZE - 1)
                    {
                        timer1.Stop();
                        MessageBox.Show("You Win!!!\nScore=" + Score);
                    }
                    else
                    {
                        int IndexOfRespawn;
                        IndexOfRespawn = FoodRespawn();
                        if (foodPlace[IndexOfRespawn] < 10)
                        {
                            FoodX = 0;
                            FoodY = foodPlace[IndexOfRespawn];
                        }
                        else
                        {
                            FoodX = foodPlace[IndexOfRespawn] / 10;
                            FoodY = foodPlace[IndexOfRespawn] % 10;
                        }
                    }
                }            
            }
            else
            {
                timer1.Stop();
                MessageBox.Show("Game Over!!!\nScore=" + Score);
            }
        }

        private bool InsideBoard(int x, int y)
        {
            return ((x > -1 && x < SIZE) && (y > -1 && y < SIZE));
        }

        private bool InsideSnake(int x, int y)
        {    
            for (int i = 0; i < snakeX.Count; i++)
            {
                if ((x == snakeX[i]) && (y == snakeY[i]))
                return false;
            }
            return true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) timer1.Start();
            if (((e.KeyCode == Keys.W) || (e.KeyCode==Keys.Up)) && ((isDirectionDown != true)||(snakeX.Count<2))) { xOffset = -1; isDirectionUp = true; yOffset = 0; }
            if (((e.KeyCode == Keys.S) || (e.KeyCode == Keys.Down)) && ((isDirectionUp != true)||(snakeX.Count<2))) { xOffset = 1; isDirectionDown = true; yOffset = 0; }
            if (((e.KeyCode == Keys.D) || (e.KeyCode == Keys.Right)) && ((isDirectionLeft != true)||(snakeY.Count<2))) { yOffset = 1; isDirectionRight = true; xOffset = 0; }
            if (((e.KeyCode == Keys.A) || (e.KeyCode == Keys.Left)) && ((isDirectionRight != true)||(snakeY.Count<2))) { yOffset = -1; isDirectionLeft = true; xOffset = 0; }
        }
    }
}
