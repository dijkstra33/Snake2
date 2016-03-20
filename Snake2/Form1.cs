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
        Random rnd = new Random();
        const int RAZMER = 8;
        public int[][][] pos = new int[RAZMER * RAZMER][][];
        public int[][] board = new int[RAZMER][];
        public int DlinaZM = 2, NachaloZmX, NachaloZmY;
        bool pohodil;
        bool peredvinulHvost;
        public string dir = "Left";

        public Form1()
        {

            for (int i = 0; i < RAZMER * RAZMER; i++)
            {
                pos[i] = new int[RAZMER][];
            }
            for (int i = 0; i < RAZMER * RAZMER; i++)
            {
                for (int j = 0; j < RAZMER; j++)
                {
                    pos[i][j] = new int[RAZMER];
                }

            }
            for (int i = 0; i < RAZMER; i++)
            {
                board[i] = new int[RAZMER];
            }
            //NachaloZmX = (int)rnd.Next(0, RAZMER);
            //NachaloZmY = (int)rnd.Next(0, RAZMER);
            NachaloZmX = 6;
            NachaloZmY = 4;
            pos[0][NachaloZmX][NachaloZmY] = 1;
            pos[1][6][5] = 1;
            pos[2][6][6] = 1;
            InitializeComponent();
        }
        void timer1_Tick(object sender, EventArgs e)
        {

           
            moving(dir);
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            for (int i = 0; i <= DlinaZM; i++)
            {
                for (int j = 0; j < RAZMER; j++)
                {
                    for (int k = 0; k < RAZMER; k++)
                    {
                        if (pos[i][j][k] == 1) board[j][k] = 1;

                    }
                }
            }
            for (int j = 0; j < RAZMER-1; j++)
            {
                for (int k = 0; k < RAZMER-1; k++)
                {
                    Console.Write(board[j][k]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void moving(string direction)
        {
            
            pohodil = false;
            for (int SnakeSegment = DlinaZM; SnakeSegment >= 0; SnakeSegment--)
            {
                //Console.Wri1teLine("tuk-1");
                peredvinulHvost = false;

                for (int indexY = 0; indexY < RAZMER; indexY++)
                {
                    for (int indexX = 0; indexX < RAZMER; indexX++)
                    {
                       
                        if ((((indexX + 1 > RAZMER-1) && (direction == "Right")) || ((indexX - 1 < 0) && (direction == "Left")) || ((indexY + 1 > RAZMER-1) && (direction == "Down")) || ((indexY - 1 < 0) && (direction == "Up"))) && (pos[SnakeSegment][indexY][indexX] == 1))
                        {

                            timer1.Stop(); MessageBox.Show("XAXA "+indexY+" "+indexX+" "+RAZMER);
                        }
                        //if (((indexX + 1 >= RAZMER) && (direction == "Right") || (indexX - 1 < 0) && (direction == "Left") || (indexY-1 >= RAZMER) && (direction == "Down") || (indexY - 1 < 0) && (direction == "Up")) && (pos[SnakeSegment][indexY][indexX] == 1)) { timer1.Stop(); MessageBox.Show("XAXA"); }
                        //if((indexX>=0)&&(indexY>=0)&&(indexX<=6)&&(indexY<=6))
                        if ((pos[SnakeSegment][indexY][indexX] == 1) && (SnakeSegment != 0) && (peredvinulHvost == false))
                        {
                            peredvinulHvost = true;
                            //Console.WriteLine("tuk0");
                            if (indexY > 0)
                                if (pos[SnakeSegment - 1][indexY - 1][indexX] == 1) //Если продолжение змейки сверху
                                {
                                    //Console.WriteLine("tuk");
                                    pos[SnakeSegment][indexY][indexX] = 0;
                                    board[indexY][indexX] = 0;
                                    pos[SnakeSegment][indexY - 1][indexX] = 1;
                                }
                            if (indexY < RAZMER-1)
                                if ((pos[SnakeSegment - 1][indexY + 1][indexX] == 1)) //Если продолжение змейки снизу
                                {
                                    //Console.WriteLine("tuk1");
                                    pos[SnakeSegment][indexY][indexX] = 0;
                                    board[indexY][indexX] = 0;
                                    pos[SnakeSegment][indexY + 1][indexX] = 1;
                                }
                            if (indexX > 0)
                                if ((pos[SnakeSegment - 1][indexY][indexX - 1] == 1)) //Если продолжение змейки слева
                                {
                                    //Console.WriteLine("tuk2");
                                    pos[SnakeSegment][indexY][indexX] = 0;
                                    board[indexY][indexX] = 0;
                                    pos[SnakeSegment][indexY][indexX - 1] = 1;
                                }
                            if (indexX < RAZMER-1)
                                if ((pos[SnakeSegment - 1][indexY][indexX + 1] == 1)) //Если продолжение змейки справа
                                {
                                    //Console.WriteLine("tuk3");
                                    pos[SnakeSegment][indexY][indexX] = 0;
                                    board[indexY][indexX] = 0;
                                    pos[SnakeSegment][indexY][indexX + 1] = 1;
                                }
                        }
                        
                        if (direction == "Up")
                        {
                            
                            if ((indexY >= 1) && (pohodil == false))
                                if ((SnakeSegment == 0) && (pos[SnakeSegment][indexY][indexX] == 1))
                                {
                                    pos[SnakeSegment][indexY - 1][indexX] = pos[SnakeSegment][indexY][indexX];//Вконце двигаем голову наверх
                                    pos[SnakeSegment][indexY][indexX] = 0;
                                    //Console.WriteLine("tuk4");
                                    pohodil = true;
                                }
                        }
                        if (direction == "Down")
                        {
                            
                            if ((indexY < RAZMER-1) && (pohodil == false))
                                if ((SnakeSegment == 0) && (pos[SnakeSegment][indexY][indexX] == 1))
                                {
                                    pos[SnakeSegment][indexY + 1][indexX] = pos[SnakeSegment][indexY][indexX];//Вконце двигаем голову вниз
                                    pos[SnakeSegment][indexY][indexX] = 0;
                                    //Console.WriteLine("tuk4");
                                    pohodil = true;
                                }
                        }
                        if (direction == "Left")
                        {
                            
                            if ((indexX >= 1) && (pohodil == false))
                                if ((SnakeSegment == 0)  && (pos[SnakeSegment][indexY][indexX] == 1))
                                {
                                    pos[SnakeSegment][indexY][indexX-1] = pos[SnakeSegment][indexY][indexX];//Вконце двигаем голову влево
                                    pos[SnakeSegment][indexY][indexX] = 0;
                                    //Console.WriteLine("tuk4");
                                    pohodil = true;
                                }
                        }
                        if (direction == "Right")
                        {
                           
                            if ((indexX <RAZMER-1) && (pohodil == false))
                                if ((SnakeSegment == 0)  && (pos[SnakeSegment][indexY][indexX] == 1))
                                {
                                    pos[SnakeSegment][indexY][indexX + 1] = pos[SnakeSegment][indexY][indexX];//Вконце двигаем голову вправо
                                    pos[SnakeSegment][indexY][indexX] = 0;
                                    //Console.WriteLine("tuk4");
                                    pohodil = true;
                                }
                        }
                        
                    }
                }
            }
            
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) dir="Up" ;
            if (e.KeyCode == Keys.S) dir = "Down";
            if (e.KeyCode == Keys.D) dir = "Right";
            if (e.KeyCode == Keys.A) dir = "Left";
        }
    }

        
        

    
}
