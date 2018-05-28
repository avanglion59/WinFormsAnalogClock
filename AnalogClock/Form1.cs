using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace AnalogClock
{
    public partial class Form1 : Form
    {
        Timer mTimer = new Timer();
        DateTime stopwatchStart = DateTime.Now;
        DateTime stopwatchStop = DateTime.Now;
        bool stopwatchActive = false;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            mTimer.Interval = 1000;
            mTimer.Tick += new EventHandler(onTimer);
            mTimer.Start();
            Text = "Analog clock";
            SetStyle(ControlStyles.ResizeRedraw, true);
            (Controls["button1"] as Button).Enabled = true;
            (Controls["button2"] as Button).Enabled = false;
            (Controls["button3"] as Button).Enabled = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            SolidBrush red = new SolidBrush(Color.Red);
            SolidBrush green = new SolidBrush(Color.Green);
            SolidBrush blue = new SolidBrush(Color.Blue);
            SolidBrush white = new SolidBrush(Color.White);
            SolidBrush black = new SolidBrush(Color.Black);
            InitializeTransform(g, 150, 150);

            DrawClock(g, black, white, black);

            InitializeTransform(g, 450, 150);

            DrawClock(g, black, white, black);

            //get current time
            DateTime nowDateTime = DateTime.Now;
            int secondInt = nowDateTime.Second;
            int minuteInt = nowDateTime.Minute;
            int hourInt24 = nowDateTime.Hour;
            int hourInt = hourInt24 % 12;

            //digitalLabel.Text = String.Format("{0,2}:{1,2}:{2,2}", hourInt24, minuteInt, secondInt);
            String digitalClockString =
                String.Format("{0,2:00}:{1,2:00}:{2,2:00}", hourInt24, minuteInt, secondInt);

            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            g.ResetTransform();

            g.DrawString(digitalClockString, drawFont, drawBrush, 120, 20);

            //process stopwatch
            if (stopwatchActive)
            {
                stopwatchStop = DateTime.Now;
            }

            TimeSpan delta = stopwatchStop.Subtract(stopwatchStart);
            String stopwatchString =
                String.Format("{0,2:00}:{1,2:00}:{2,2:00}", delta.Hours, delta.Minutes, delta.Seconds);

            g.DrawString(stopwatchString, drawFont, drawBrush, 420, 20);

            InitializeTransform(g, 150, 150);
            //hour hand draw
            g.RotateTransform((hourInt * 30) + (minuteInt / 2));
            DrawHand(g, blue, 70, false);
            InitializeTransform(g, 150, 150);
            //minute hand draw
            g.RotateTransform(minuteInt * 6);
            DrawHand(g, red, 100, false);
            InitializeTransform(g, 150, 150);
            //second hand draw
            g.RotateTransform(secondInt * 6);
            DrawHand(g, green, 100, true);

            InitializeTransform(g, 450, 150);
            //hour hand draw
            g.RotateTransform((delta.Hours * 30) + (delta.Minutes / 2));
            DrawHand(g, blue, 70, false);
            InitializeTransform(g, 450, 150);
            //minute hand draw
            g.RotateTransform(delta.Minutes * 6);
            DrawHand(g, red, 100, false);
            InitializeTransform(g, 450, 150);
            //second hand draw
            g.RotateTransform(delta.Seconds * 6);
            DrawHand(g, green, 100, true);

            red.Dispose();
            green.Dispose();
            blue.Dispose();
            white.Dispose();
            black.Dispose();
        }

        private void DrawHand(Graphics g, SolidBrush solidBrush, int length, bool thin)
        {
            Point[] points = new Point[4];
            points[0].X = 0;
            points[0].Y = -length;
            points[1].X = (thin) ? -2 : -10;
            points[1].Y = 0;
            points[2].X = 0;
            points[2].Y = (thin) ? 2 : 10;
            points[3].X = (thin) ? 2 : 10;
            points[3].Y = 0;
            g.FillPolygon(solidBrush, points);
        }

        private void DrawClock(Graphics g, SolidBrush borderBrush, SolidBrush hourBrush, SolidBrush minuteBrush)
        {
            //draw border
            for (int i = 0; i < 120; i++)
            {
                g.RotateTransform(5.0f);
                g.FillRectangle(borderBrush, 90, -5, 10, 10);
            }
            //draw hour mark
            for (int i = 0; i < 12; i++)
            {
                g.RotateTransform(30.0f);
                g.FillRectangle(hourBrush, 85, -5, 10, 10);
            }
            //draw minute mark
            for (int i = 0; i < 60; i++)
            {
                g.RotateTransform(6.0f);
                g.FillRectangle(minuteBrush, 85, -10, 5, 1);
            }
        }

        private void InitializeTransform(Graphics g, float translateX, float translateY)
        {
            g.ResetTransform();
            g.TranslateTransform(translateX, translateY);
        }

        private void onTimer(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stopwatchStart = DateTime.Now;
            stopwatchActive = true;
            (Controls["button1"] as Button).Enabled = false;
            (Controls["button2"] as Button).Enabled = true;
            (Controls["button3"] as Button).Enabled = false;
            Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stopwatchActive = false;
            (Controls["button1"] as Button).Enabled = false;
            (Controls["button2"] as Button).Enabled = false;
            (Controls["button3"] as Button).Enabled = true;
            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stopwatchStart = stopwatchStop;
            (Controls["button1"] as Button).Enabled = true;
            (Controls["button2"] as Button).Enabled = false;
            (Controls["button3"] as Button).Enabled = false;
            Invalidate();
        }
    }
}
