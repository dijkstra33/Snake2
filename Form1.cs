using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Snake
{
    public partial class Snake : Form
    {

        public Snake()
        {
            InitializeComponent();
            
        }
        private void Snake_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) Console.WriteLine("W");
            if (e.KeyCode == Keys.A) Console.WriteLine("A");
            if (e.KeyCode == Keys.S) Console.WriteLine("S");
            if (e.KeyCode == Keys.D) Console.WriteLine("D");

        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            
            const int razmer = 5;
            Random rnd = new Random();

            this.ClientSize = new System.Drawing.Size(razmer*30, razmer*30);
            //Refresh();
            button1.Hide();
            Graphics formGraphics = this.CreateGraphics();
            Pen myPen = new Pen(Color.Blue);
            SolidBrush mySnake = new SolidBrush(Color.Red);
            SolidBrush myFood = new SolidBrush(Color.Green);
            
            int Foodx = rnd.Next(0, razmer), Foody = rnd.Next(0, razmer);
            int na4alox = rnd.Next(0, razmer);
            int na4aloy = rnd.Next(0, razmer);

            while((Foodx==na4alox)&&(Foody==na4aloy)) 
            {
                Foodx = rnd.Next(0, razmer);
                Foody = rnd.Next(0, razmer);
            }

            int[][] board=new int[razmer][];
            for (int i = 0; i < razmer; i++)
			{
			 board[i]=new int[razmer];
			}
            for (int i = 0; i < razmer; i++)
            {
                for (int j = 0; j < razmer; j++)
                {
                    board[i][j] = 0;
                    if ((i == na4alox) && (j == na4aloy)) board[i][j] = 1;
                    if ((i == Foodx) && (j == Foody)) board[i][j] = 2;
                    Console.Write(board[i][j]);
                }
                Console.WriteLine();
            }
               
            int x2 = ClientSize.Width;
            int y2= 0;
            int shagx = ClientSize.Width / razmer;
            int shagy = ClientSize.Height / razmer;
            int x1 = ClientSize.Width;
            int y1 = 0;

            formGraphics.FillRectangle(mySnake, na4aloy * shagx + 1, na4alox * shagy + 1, shagx, shagy);
            formGraphics.FillRectangle(myFood, Foody * shagx + 1, Foodx * shagy + 1, shagx, shagy);
            for (int i = 0; i < razmer; i++)
            {
                for (int j = 0; j < razmer; j++)
                {
                    formGraphics.DrawLine(myPen, x1, 0, x2, ClientSize.Height);
                    x1 -= shagx;
                    x2 -= shagx;
                    
                }
                formGraphics.DrawLine(myPen, 0, y1, ClientSize.Width, y2);
                y1 += shagy;
                y2 += shagy;
            }
            formGraphics.DrawRectangle(myPen, 0, 0, ClientSize.Width-1, ClientSize.Height-1);

            myPen.Dispose();
            mySnake.Dispose();
            myFood.Dispose();
            formGraphics.Dispose();
           
        }

        
    }
}
