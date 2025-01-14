using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Space_Race
{


    public partial class Form1 : Form
    {
        Rectangle player1 = new Rectangle(180, 590, 30, 30);
        Rectangle player2 = new Rectangle(400, 590, 30, 30);

        bool wPressed = false;
        bool sPressed = false;
        bool upPressed = false;
        bool downPressed = false;

        Random randGen = new Random();
        SoundPlayer player = new SoundPlayer(Properties.Resources.explosion);

        List<Rectangle> asteroids = new List<Rectangle>();
        List<int> asteroidSpeeds = new List<int> ();

        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush grayBrush = new SolidBrush(Color.Gray);

        int playerSpeed = 7;
        int player1Score = 0;
        int player2Score = 0;
        int asteroidSize = 0;
        int randValue = 0;
   
        public Form1()
        {
            InitializeComponent();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //Move player 1
            if (wPressed == true)
            {
                player1.Y -= playerSpeed;
            }

            if (sPressed == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
            }

            //player 1 scoring
            if (player1.Y < 0)
            {
                player1.Y = 590;
                player1Score++;
            }

            //Move player 2
            if (upPressed == true)
            {
                player2.Y -= playerSpeed;
            }

            if (downPressed == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;
            }

            //player 2 scoring
            if (player2.Y < 0)
            {
                player2.Y = 590;
                player2Score++;
            }

            //update label
            player1ScoreLabel.Text = player1Score.ToString();
            player2ScoreLabel.Text = player2Score.ToString();

            //create asteroids
            randValue = randGen.Next(0, 101);

            if (randValue < 10)
            {
                asteroidSize = randGen.Next(6, 41);
                int y = randGen.Next(0, this.Height - 80);
                Rectangle newAsteroid = new Rectangle(0, y, asteroidSize, asteroidSize);
                asteroids.Add(newAsteroid);
                asteroidSpeeds.Add(randGen.Next(3, 11));
            }
            else if (randValue < 20)
            {
                asteroidSize = randGen.Next(6, 41);
                int y = randGen.Next(0, this.Height - 80);
                Rectangle newAsteroid = new Rectangle(this.Width, y, asteroidSize, asteroidSize);
                asteroids.Add(newAsteroid);
                asteroidSpeeds.Add(-randGen.Next(3, 11));
            }
            
            //move asteroids
            for (int i = 0; i < asteroids.Count; i++)
            {
                int x = asteroids[i].X + asteroidSpeeds[i];
                asteroids[i] = new Rectangle(x, asteroids[i].Y, asteroids[i].Width, asteroids[i].Height);
            }

            //check for intersection with players
            for (int i = 0; i < asteroids.Count; i++)
            {
                if (player1.IntersectsWith(asteroids[i]))
                {
                    asteroids.RemoveAt(i);
                    asteroidSpeeds.RemoveAt(i);
                    player.Play();
                    player1.Y = 590;
                }
            }

            for (int i = 0; i < asteroids.Count; i++)
            {
                if (player2.IntersectsWith(asteroids[i]))
                {
                    asteroids.RemoveAt(i);
                    asteroidSpeeds.RemoveAt(i);
                    player.Play();
                    player2.Y = 590;
                }
            }

            //scoring points
            if (player1Score == 3)
            {
                winLabel.Text = "Player 1 Wins!";
                gameTimer.Stop();
            }

            if (player2Score == 3)
            {
                winLabel.Text = "Player 2 Wins!";
                gameTimer.Stop();
            }

            Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //Player 1 movement
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;


                //Player 2 movement
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;

            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //Player 1 movement
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;


                //Player 2 movement
                case Keys.Up:
                    upPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;

            }
        }

        //draw everything
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(blueBrush, player1);
            e.Graphics.FillEllipse(redBrush, player2);

            for (int i = 0; i < asteroids.Count; i++)
            {
                e.Graphics.FillEllipse(grayBrush, asteroids[i]);
            }

        }
    }
}
