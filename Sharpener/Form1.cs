using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sharpener
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadSampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK ){
                pictureBox1.Load(openFileDialog1.FileName);
            }
        }

        private double f_power = 1.0;
        private double t_power = 1.0;
        private double output_max = 1.0;
        private double output_min = 0.0;

        private byte filter(byte input)
        {
            double f = input / 255.0 + 0.5 / 255.0;
            double t = 1.0 - f;
            f = Math.Pow(f, f_power);
            t = Math.Pow(t, t_power);
            double float_output = (1 / t) / (1 / f + 1 / t);
            if (float_output > output_max)
            {
                float_output = 1.0;
            }
            if (float_output < output_min)
            {
                float_output = 0.0;
            }
            return (byte)(float_output * 255);
        }

        private Bitmap sharpen(Bitmap bmp) {
            Bitmap t = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color c = bmp.GetPixel(j, i);
                    byte r = c.R;
                    byte g = c.G;
                    byte b = c.B;
                    r = filter(r);
                    g = filter(g);
                    b = filter(b);
                    t.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            return t;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f_power = (double)(numericUpDown2.Value);
            t_power = (double)(numericUpDown1.Value);
            output_max = (double)(numericUpDown3.Value) / 255.0;
            output_min = (double)(numericUpDown4.Value) / 255.0;
            
            pictureBox2.Image = sharpen(new Bitmap(pictureBox1.Image));
            MessageBox.Show("Done!");
        }

        private void sharpenALotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (String bmp_name in openFileDialog2.FileNames) {
                    Bitmap t = sharpen(new Bitmap(bmp_name));
                    String bmp_new_name = bmp_name.Insert(bmp_name.Length - 4, ".sharp");
                    t.Save(bmp_new_name);
                }
                MessageBox.Show("Done!");
            }
        }
    }
}
