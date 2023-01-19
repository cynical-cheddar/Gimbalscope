using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrushlessPWMControllerGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            serialPort1.Open();

        }


        // key
        // a = brushless 1
        // b = brushless 2
        // c = brushless both
        // d = servo 1
        // e = servo 2
        // f = servo both



        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            serialPort1.Write("a");
            serialPort1.Write(hScrollBar1.Value.ToString());
            serialPort1.Write("/");
            textBox1.Text = hScrollBar1.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Write("b");
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            serialPort1.Write("b");
            serialPort1.Write(hScrollBar2.Value.ToString());
            serialPort1.Write("/");
            textBox2.Text = hScrollBar2.Value.ToString();
        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            serialPort1.Write("c");
            serialPort1.Write(hScrollBar3.Value.ToString());
            serialPort1.Write("/");
            textBox3.Text = hScrollBar3.Value.ToString();
            textBox2.Text = hScrollBar3.Value.ToString();
            textBox1.Text = hScrollBar3.Value.ToString();

            hScrollBar2.Value = hScrollBar3.Value;
            hScrollBar1.Value = hScrollBar3.Value;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            serialPort1.Write("d");
            serialPort1.Write(vScrollBar1.Value.ToString());
            serialPort1.Write("/");
            textBox4.Text = vScrollBar1.Value.ToString();
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            serialPort1.Write("e");
            serialPort1.Write(vScrollBar2.Value.ToString());
            serialPort1.Write("/");
            textBox5.Text = vScrollBar2.Value.ToString();
        }

        private void vScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            serialPort1.Write("f");
            serialPort1.Write(vScrollBar3.Value.ToString());
            serialPort1.Write("/");
            textBox6.Text = vScrollBar3.Value.ToString();
            vScrollBar1.Value = vScrollBar3.Value;
            vScrollBar2.Value = vScrollBar3.Value;
        }

        private void vScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            serialPort1.Write("g");
            serialPort1.Write(vScrollBar4.Value.ToString());
            serialPort1.Write("/");
            textBox6.Text = vScrollBar4.Value.ToString();
            vScrollBar1.Value = vScrollBar4.Value;
            vScrollBar2.Value = 255 - vScrollBar4.Value;
        }



        private void vScrollBar5_Scroll(object sender, ScrollEventArgs e)
        {
            serialPort1.Write("h");
            serialPort1.Write(vScrollBar5.Value.ToString());
            serialPort1.Write("/");
            targetRotationTextBox1.Text = vScrollBar5.Value.ToString();
            
        }

        private void btnTargetRotationCommand_Click(object sender, EventArgs e)
        {
            serialPort1.Write("h");
            serialPort1.Write(targetRotationCommandBox.Text);
            serialPort1.Write("/");
            vScrollBar5.Value = Int32.Parse(targetRotationCommandBox.Text);
            targetRotationTextBox1.Text = targetRotationCommandBox.Text;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void targetRotationTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
