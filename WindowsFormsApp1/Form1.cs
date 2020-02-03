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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SoundPlayer pongsound;
        private SoundPlayer scoresound;
        public Form1()
        {
            InitializeComponent();
            pongsound = new SoundPlayer(@"C:\Users\Owner\source\repos\WindowsFormsApp1\pongsound.wav");
            scoresound = new SoundPlayer(@"C:\Users\Owner\source\repos\WindowsFormsApp1\ding.wav");
        }

        //Stops the flickering
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        Pen white = new Pen(Color.White);
        Rectangle paddle1 = new Rectangle(10, 150, 10, 50);
        Rectangle paddle2 = new Rectangle(565, 150, 10, 50);
        Rectangle ball = new Rectangle(0, 0, 10, 10);
        System.Drawing.SolidBrush fillWhite = new System.Drawing.SolidBrush(Color.White);
        string vertical = "Up";
        string horizontal = "Right";
        bool paddle1_ball = true;
        bool paddle2_ball = false;
        int paddle1_score = 0;
        int paddle2_score = 0;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(white, paddle1);
            g.FillRectangle(fillWhite, paddle1);
            g.DrawRectangle(white, paddle2);
            g.FillRectangle(fillWhite, paddle2);
            g.DrawEllipse(white, ball);
            g.FillEllipse(fillWhite, ball);
            g.DrawString(Convert.ToString(paddle1_score), new Font("Arial", 16), new SolidBrush(Color.White), 15, 330, new StringFormat());
            g.DrawString(Convert.ToString(paddle2_score), new Font("Arial", 16), new SolidBrush(Color.White), 550, 330, new StringFormat());

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Z) {

                paddle1.Y += 10;

            }
            if (e.KeyData == Keys.A)
            {

                paddle1.Y -= 10;
            }
            if (e.KeyData == Keys.Down)
            {

                paddle2.Y += 10;

            }
            if (e.KeyData == Keys.Up)
            {

                paddle2.Y -= 10;
            }
            if (e.KeyData == Keys.Space)
            {

                if (paddle1_ball == true)
                {
                    paddle1_ball = false;
                }
                if (paddle2_ball == true)
                {
                    paddle2_ball = false;
                }
            }
        }

        static int Stopper(Rectangle shape) {

            if (shape.Y <= 0) {
                shape.Y = 0;
            }
            if (shape.Y >= 305) {
                shape.Y = 305;
            }
            return shape.Y;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            paddle1.Y = Stopper(paddle1);
            paddle2.Y = Stopper(paddle2);
            Invalidate();
        }

        private string get_vertical() {
            if (ball.Y <= 0)
            {
                vertical = "Down";
            }
            if (ball.Y >= 350)
            {
                vertical = "Up";
            }
            return vertical;
        }

        private string get_horizontal()
        {
            if (horizontal == "Right" && (ball.X >= paddle2.X-paddle2.Width && ball.X <= paddle2.X+paddle2.Width))
            {
               
                if (ball.Y >= paddle2.Y && ball.Y <= paddle2.Y + paddle2.Height)
                {
                    horizontal = "Left";
                    pongsound.Play();
                }

            }

            if (horizontal == "Left" && (ball.X <= paddle1.X+paddle1.Width && ball.X >= paddle1.X-paddle1.Width))
            {
                if (ball.Y >= paddle1.Y && ball.Y <= paddle1.Y + paddle1.Height)
                {

                    horizontal = "Right";
                    pongsound.Play();
                }

            }

            return horizontal;
        }

        private string initial_direction()
        {
            if (paddle1_ball == true || paddle2_ball == true)
            {
                if (ball.Y >= 175)
                {
                    vertical = "Down";
                }
                else
                {
                    vertical = "Up";
                }
            }
            return vertical;
        }

        private void check_if_score(){
            if (ball.X <= 0)
            {
                paddle1_ball = true;
                horizontal = "Right";
                paddle2_score += 1;
                scoresound.Play();
            }
            if (ball.X >= 575)
            {
                paddle2_ball = true;
                horizontal = "Left";
                paddle1_score += 1;
                scoresound.Play();
            }

        }

        private void check_game_over()
        {
            if (paddle1_score == 5) {
                reset_game("Player 1 Wins!");
            }
            if (paddle2_score == 5)
            {
                reset_game("Player 2 Wins!");
            }
        }

        private void reset_game(string winner)
        {
            
            paddle1_score = 0;
            paddle2_score = 0;
            paddle1.Y = 150;
            paddle2.Y = 150;
            paddle1_ball = true;
            paddle2_ball = false;
            horizontal = "Right";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            vertical = initial_direction();

            vertical = get_vertical();

            if (vertical == "Up" && (paddle1_ball == false && paddle2_ball == false)) {

                ball.Y -= 10;
            }
            else if (vertical == "Down" && (paddle1_ball == false && paddle2_ball == false)){
                ball.Y += 10;
            }
            else {
                 
                if (paddle1_ball == true)
                    {
                        ball.Y = paddle1.Y + 20;
                    }
                else
                    {
                        ball.Y = paddle2.Y + 20;
                    }
                }
            

            horizontal = get_horizontal();

            if (horizontal == "Right" && (paddle1_ball == false && paddle2_ball == false)) { 
                    ball.X += 10;    
            }
            else if (horizontal == "Left" && (paddle1_ball == false && paddle2_ball == false)){
                    ball.X -= 10; 
            }
            else
            {
                if (paddle1_ball == true) {
                    ball.X = 20;
                }
                else
                {
                    ball.X = 552;
                }
            }
           
            check_if_score();
            check_game_over();
      


        }
    }
}
