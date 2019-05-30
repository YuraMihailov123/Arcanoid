using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arkanoid
{
    public partial class Form1 : Form
    {
        private int dirX, dirY;
        private int width = 22,height=22;
        private int[,] map;
        PictureBox playerBall;
        PictureBox playerBar;
        PictureBox[] boards;
        bool isLost = false;
        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(keyboard);
            this.Width = width * 30+15;
            this.Height = height*30+40;
            this.Text = "Arkanoid";
            map = new int[width, height];
            boards = new PictureBox[width * height];

            playerBall = new PictureBox();
            playerBall.Size = new Size(30, 30);
            playerBall.Location = new Point(330, 570);
            playerBall.BackColor = Color.Red;
            map[570/30, 330/30] = 7;
            this.Controls.Add(playerBall);

            playerBar = new PictureBox();
            playerBar.Size = new Size(60, 30);
            playerBar.Location = new Point(330, 600);
            playerBar.BackColor = Color.Green;
            map[600 / 30, 330 / 30] = 8;
            map[600 / 30, 360 / 30] = 8;
            this.Controls.Add(playerBar);
            
            
            timer1.Interval = 100;
            timer1.Tick += new EventHandler(update);
            timer1.Start();
            
            dirX = 1;
            dirY = -1;

            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    if (i == 0 || j==0 || i==width-1 || j==height-1)
                    {
                        PictureBox bounds = new PictureBox();
                        bounds.Size = new Size(30, 30);
                        bounds.Location = new Point(30 * j, 30 * i);
                        bounds.BackColor = Color.Black;
                        map[i, j] = 9;
                        this.Controls.Add(bounds);
                    }
                }
            }
            generateBoards();
        }

        private void generateBoards()
        {
            for (int i = 0; i < 5; i++)
            {
                boards[i] = new PictureBox();
                boards[i].Size = new Size(60, 30);
                int _x = 0;
                if (i % 2 == 0) _x = 330 - 30 * i;
                else _x = 330 + 30 * i;
                boards[i].Location = new Point(_x, 60*(i+1));
                boards[i].BackColor = Color.Blue;
                map[60 * (i+1) / 30, _x/30] = 2;
                map[60 * (i+1) / 30, (_x+30)/30] = 2;
                this.Controls.Add(boards[i]);
            }
        }

        private void keyboard(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    if (playerBar.Location.X / 30 +1 < width - 2)
                    {
                        map[playerBar.Location.Y / 30, playerBar.Location.X / 30] = 0;
                        map[playerBar.Location.Y / 30, playerBar.Location.X / 30 + 1] = 0;
                        playerBar.Location = new Point(playerBar.Location.X + 30, playerBar.Location.Y);
                        map[playerBar.Location.Y / 30, playerBar.Location.X / 30] = 8;
                        map[playerBar.Location.Y / 30, playerBar.Location.X / 30 + 1] = 8;
                    }
                    break;
                case "Left":
                    if (playerBar.Location.X / 30 + 1 > 2)
                    {
                        map[playerBar.Location.Y / 30, playerBar.Location.X / 30] = 0;
                        map[playerBar.Location.Y / 30, playerBar.Location.X / 30 + 1] = 0;
                        playerBar.Location = new Point(playerBar.Location.X - 30, playerBar.Location.Y);
                        map[playerBar.Location.Y / 30, playerBar.Location.X / 30] = 8;
                        map[playerBar.Location.Y / 30, playerBar.Location.X / 30 + 1] = 8;
                    }
                    break;
            }
        }

        private void winOrLose(string text)
        {
            timer1.Stop();
            isLost = true;
            Label lostLabel = new Label();
            lostLabel.Text = text;
            lostLabel.Location = new Point(200, 270);
            lostLabel.Size = new Size(400, 100);
            lostLabel.Font = new Font(new FontFamily("Microsoft Sans Serif"), 50);
            this.Controls.Add(lostLabel);
        }

        private void collide()
        {
            if (playerBall.Location.Y / 30 == height-2)
            {
                winOrLose("You lost!");
                return;
            }            

            if (map[playerBall.Location.Y / 30, playerBall.Location.X / 30 + dirX]!=0)
            {
                if (map[playerBall.Location.Y / 30, playerBall.Location.X / 30 + dirX] < 7)
                {
                    map[playerBall.Location.Y / 30, playerBall.Location.X / 30 + dirX] -= 1;
                    if (map[playerBall.Location.Y / 30, playerBall.Location.X / 30 + dirX] == 0)
                    {
                        for (int i = 0; i < boards.Length; i++)
                        {
                            if (boards[i] != null)
                            {
                                if (boards[i].Location == new Point(playerBall.Location.X / 30 + dirX, playerBall.Location.Y / 30) || new Point(boards[i].Location.X + 30, boards[i].Location.Y) == new Point(playerBall.Location.X / 30 + dirX, playerBall.Location.Y / 30))
                                {
                                    this.Controls.Remove(boards[i]);
                                    map[boards[i].Location.Y / 30, boards[i].Location.X / 30] = 0;
                                    map[boards[i].Location.Y / 30, boards[i].Location.X / 30 +1] = 0;
                                    boards[i] = null;
                                    if (isEmptyBoards())
                                    {
                                        winOrLose("You win!");
                                    }
                                }
                            }
                        }
                    }
                }
                dirX = -1*dirX;
                
            }
            if (map[playerBall.Location.Y / 30 +dirY, playerBall.Location.X / 30 ] != 0)
            {
                if (map[playerBall.Location.Y / 30 + dirY, playerBall.Location.X / 30] <7)
                {
                    map[playerBall.Location.Y / 30+dirY, playerBall.Location.X / 30 ] -= 1;
                    if(map[playerBall.Location.Y / 30 + dirY, playerBall.Location.X / 30] == 0)
                    {
                        for(int i = 0; i < boards.Length; i++)
                        {
                            if (boards[i] != null)
                            {
                                if (boards[i].Location == new Point( playerBall.Location.X, playerBall.Location.Y + dirY * 30) || new Point(boards[i].Location.X + 30, boards[i].Location.Y) == new Point( playerBall.Location.X, playerBall.Location.Y + dirY * 30))
                                {
                                    this.Controls.Remove(boards[i]);
                                    map[boards[i].Location.Y / 30, boards[i].Location.X / 30] = 0;
                                    map[boards[i].Location.Y / 30, boards[i].Location.X / 30 + 1] = 0;
                                    boards[i] = null;
                                    if (isEmptyBoards())
                                    {
                                        winOrLose("You win!");
                                    }
                                }
                            }
                        }
                    }
                }
                dirY = -1*dirY;
            }

            if (map[playerBall.Location.Y / 30 + dirY, playerBall.Location.X / 30 + dirX] != 0)
            {
                dirY = -1 * dirY;
                dirX = -1 * dirX;
            }
        }

        private bool isEmptyBoards()
        {
            for(int i = 0; i < boards.Length; i++)
            {
                if (boards[i] != null)
                    return false;                
            }
            return true;
        }

        private void update(object sender, EventArgs e)
        {
            collide();

            if (!isLost)
            {
                map[playerBall.Location.Y / 30, playerBall.Location.X / 30] = 0;
                playerBall.Location = new Point(playerBall.Location.X + 30 * dirX, playerBall.Location.Y + 30 * dirY);
                //this.Text = playerBall.Location.X / 30 + "-"+ playerBall.Location.Y / 30 ;
                map[playerBall.Location.Y / 30, playerBall.Location.X / 30] = 7;
            }
        }
    }
}
