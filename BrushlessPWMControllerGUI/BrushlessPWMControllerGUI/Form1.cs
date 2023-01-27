using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace BrushlessPWMControllerGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            serialPort1.Open();

        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            string indata = sp.ReadLine();

            if(indata.Length > 2)
            {
                System.Diagnostics.Debug.WriteLine(indata);
            }

            
            

            
            /*string indata = sp.ReadExisting();
            if (indata.Contains('\n'))
            {
                System.Diagnostics.Debug.Write(indata);
                System.Diagnostics.Debug.Write("\n");
            }
            else
            {
                System.Diagnostics.Debug.Write(indata);
            }*/
            


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

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click_1(object sender, EventArgs e)
        {

        }

        private void btn_fire_command_Click(object sender, EventArgs e)
        {
            byte motorType = 0;
            byte motorID = 0;
            byte functionID = 0;
            byte endChar = 47; // '/'

            double fireTargetAngle = Single.Parse(txt_target_orientation.Text);
            fireTargetAngle = Math.Round(fireTargetAngle, 4);
            //byte[] bytes_fireTargetAngle = BitConverter.GetBytes(fireTargetAngle);

            double fireDegreesPerSecond = Single.Parse(txt_fire_speed.Text);
            fireDegreesPerSecond = Math.Round(fireDegreesPerSecond, 4);
            //byte[] bytes_fireDegreesPerSecond = BitConverter.GetBytes(fireDegreesPerSecond);


            string firePrecisonString = txt_fire_precision.Text;

            double reloadTargetAngle = 0;
            reloadTargetAngle = Math.Round(reloadTargetAngle, 4);


            byte[] bytes_reloadTargetAngle = BitConverter.GetBytes(reloadTargetAngle);
            double reloadDegreesPerSecond = Single.Parse(txt_reload_speed.Text);
            reloadDegreesPerSecond = Math.Round(reloadDegreesPerSecond, 4);
            byte[] bytes_reloadDegreesPerSecond = BitConverter.GetBytes(reloadDegreesPerSecond);
            byte reloadPrecision = 1;

            string reloadPrecisionString = txt_reload_precision.Text;



            /*
            byte[] buf = new byte[] { motorType, motorID, functionID, bytes_fireTargetAngle[0], bytes_fireTargetAngle[1],
                bytes_fireTargetAngle[2], bytes_fireTargetAngle[3], bytes_fireDegreesPerSecond[0], bytes_fireDegreesPerSecond[1],
                bytes_fireDegreesPerSecond[2], bytes_fireDegreesPerSecond[3], firePrecision, bytes_reloadTargetAngle[0], bytes_reloadTargetAngle[1],
                bytes_reloadTargetAngle[2], bytes_reoadTargetAngle[3], bytes_reloadDegreesPerSecond[0], bytes_reloadDegreesPerSecond[1],
                bytes_reloadDegreesPerSecond[2], bytes_reloadDegreesPerSecond[3], reloadPrecision, endChar };
            */

            string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + fireTargetAngle.ToString() + "," + fireDegreesPerSecond.ToString() + "," + firePrecisonString +  "," + reloadTargetAngle.ToString() + "," + reloadDegreesPerSecond.ToString() + "," + reloadPrecisionString + ",";


            System.Diagnostics.Debug.WriteLine("--btn_fire_command_Click--");
            //System.Diagnostics.Debug.WriteLine(commandString);
            byte[] utf8String = Encoding.UTF8.GetBytes(commandString);


            serialPort1.Write(utf8String, 0, utf8String.Length);
            
            
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_read_Click(object sender, EventArgs e)
        {
            String portContents = (serialPort1.ReadLine());
            textBox7.Text = portContents;
            System.Diagnostics.Debug.WriteLine(portContents);
        }

        private void btn_send_garbage_byte_Click(object sender, EventArgs e)
        {
            byte[] garbageByte = new byte[] { 1 };
            serialPort1.Write(garbageByte, 0, 1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // POLLING METHDOD

            //String portContents = (serialPort1.ReadLine());
            //textBox7.Text = portContents;
            //System.Diagnostics.Debug.WriteLine(portContents);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // timer1.Start();
        }
    }
}
