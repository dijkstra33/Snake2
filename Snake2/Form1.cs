using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake2
{
    public partial class Form1 : Form
    {
        private readonly Pen pen = new Pen(Color.Black);
        private readonly SolidBrush snakeBrush = new SolidBrush(Color.Green);
        private readonly SolidBrush foodBrush = new SolidBrush(Color.Red);
        private readonly SolidBrush headBrush = new SolidBrush(Color.GreenYellow);
        private readonly Random rnd = new Random();
        private const int NumberOfCells = 10;
        private readonly List<int> snakeX = new List<int>();
        private readonly List<int> snakeY = new List<int>();
        private int xOffset;
        private int yOffset;
        public int FoodX = 3;
        public int FoodY = 3;
        private int coordX;
        private int coordY;
        public int Score;
        private bool isDirectionDown;
        public bool IsDirectionUp;
        public bool IsDirectionLeft;
        public bool IsDirectionRight;
        private readonly CellType[,] board = new CellType[NumberOfCells, NumberOfCells];
        private readonly int[] foodPlace = new int[NumberOfCells*NumberOfCells];

        public Form1()
        {
            // Set default offset.
            xOffset = 0;
            yOffset = 1;
            IsDirectionRight = true;

            // Use after test phase.
            snakeX.Add(NumberOfCells/2);
            snakeY.Add(NumberOfCells/2);

            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Moving();
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics drawLine = e.Graphics;
            pictureBox1.CreateGraphics();
            ClientSize = new Size(NumberOfCells*36, NumberOfCells*36 + 10);
            pictureBox1.Width = 30*NumberOfCells;
            pictureBox1.Height = 30*NumberOfCells;

            int pictureBoxWidth = pictureBox1.Size.Width;
            int pictureBoxHeight = pictureBox1.Size.Height;
            int oneCellx = pictureBoxWidth/NumberOfCells;
            int oneCelly = pictureBoxHeight/NumberOfCells;

            for (int i = 0; i < NumberOfCells; i++)
            {
                for (int j = 0; j < NumberOfCells; j++)
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

            for (int i = 0; i < NumberOfCells; i++)
            {
                drawLine.DrawLine(pen, oneCellx*i, 0, oneCellx*i, pictureBoxHeight);
                for (int j = 0; j < NumberOfCells; j++)
                {
                    drawLine.DrawLine(pen, 0, oneCelly*j, pictureBoxWidth, oneCelly*j);

                    switch (board[i, j])
                    {
                        case CellType.Snake:
                            e.Graphics.FillRectangle(snakeBrush, j*oneCelly + 1, i*oneCellx + 1, oneCellx - 1, oneCelly - 1);
                            break;
                        case CellType.Head:
                            e.Graphics.FillRectangle(headBrush, j*oneCelly + 1, i*oneCellx + 1, oneCellx - 1, oneCelly - 1);
                            break;
                        case CellType.Food:
                            e.Graphics.FillRectangle(foodBrush, j*oneCelly + 1, i*oneCellx + 1, oneCellx - 1, oneCelly - 1);
                            break;
                        case CellType.Empty:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            drawLine.DrawRectangle(pen, 0, 0, pictureBoxWidth - 1, pictureBoxHeight - 1);
        }

        public int FoodRespawn()
        {
            int count = 0;
            for (int i = 0; i < NumberOfCells; i++)
            {
                for (int j = 0; j < NumberOfCells; j++)
                {
                    if (board[i, j] == CellType.Empty)
                    {
                        foodPlace[count] = i*10 + j;
                        count++;
                    }
                }
            }
            int indexOfRespawn = rnd.Next(0, count - 1);
            return indexOfRespawn;
        }

        private void Moving()
        {
            if (xOffset == 0)
            {
                isDirectionDown = false;
                IsDirectionUp = false;
            }
            if (yOffset == 0)
            {
                IsDirectionLeft = false;
                IsDirectionRight = false;
            }
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
                    if (Score == NumberOfCells*NumberOfCells - 1)
                    {
                        timer1.Stop();
                        MessageBox.Show("You Win!!!\nScore=" + Score);
                    }
                    else
                    {
                        int indexOfRespawn = FoodRespawn();
                        if (foodPlace[indexOfRespawn] < 10)
                        {
                            FoodX = 0;
                            FoodY = foodPlace[indexOfRespawn];
                        }
                        else
                        {
                            FoodX = foodPlace[indexOfRespawn]/10;
                            FoodY = foodPlace[indexOfRespawn]%10;
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
            return ((x > -1 && x < NumberOfCells) && (y > -1 && y < NumberOfCells));
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
            if (((e.KeyCode == Keys.W) || (e.KeyCode == Keys.Up)) && ((isDirectionDown != true) || (snakeX.Count < 2)))
            {
                xOffset = -1;
                IsDirectionUp = true;
                yOffset = 0;
            }
            if (((e.KeyCode == Keys.S) || (e.KeyCode == Keys.Down)) && ((IsDirectionUp != true) || (snakeX.Count < 2)))
            {
                xOffset = 1;
                isDirectionDown = true;
                yOffset = 0;
            }
            if (((e.KeyCode == Keys.D) || (e.KeyCode == Keys.Right)) && ((IsDirectionLeft != true) || (snakeY.Count < 2)))
            {
                yOffset = 1;
                IsDirectionRight = true;
                xOffset = 0;
            }
            if (((e.KeyCode == Keys.A) || (e.KeyCode == Keys.Left)) && ((IsDirectionRight != true) || (snakeY.Count < 2)))
            {
                yOffset = -1;
                IsDirectionLeft = true;
                xOffset = 0;
            }
        }
    }
}