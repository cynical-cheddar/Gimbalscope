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

        }



        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {

        }

        
        // left gimbal
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            
        }

        private void SetLeftGimbalPosition()
        {
            txt_gimbal_left_rotation.Text = vScrollBar1.Value.ToString();

            byte motorType = 0;
            byte motorID = 1;
            byte functionID = 1;
            float targetAngle = vScrollBar1.Value;
            float moveSpeed = 90f;
            float precision = 1f;
            string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + targetAngle.ToString() + "," + moveSpeed.ToString() + "," + precision + ",";


            System.Diagnostics.Debug.WriteLine("--vScrollBar1_Scroll--");
            //System.Diagnostics.Debug.WriteLine(commandString);
            byte[] utf8String = Encoding.UTF8.GetBytes(commandString);


            serialPort1.Write(utf8String, 0, utf8String.Length);
        }

        private void SetRightGimbalPosition()
        {
            txt_gimbal_left_rotation.Text = vScrollBar1.Value.ToString();

            byte motorType = 0;
            byte motorID = 2;
            byte functionID = 1;
            float targetAngle = vScrollBar2.Value;
            float moveSpeed = 90f;
            float precision = 1f;
            string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + targetAngle.ToString() + "," + moveSpeed.ToString() + "," + precision + ",";


            System.Diagnostics.Debug.WriteLine("--vScrollBar2_Scroll--");
            //System.Diagnostics.Debug.WriteLine(commandString);
            byte[] utf8String = Encoding.UTF8.GetBytes(commandString);


            serialPort1.Write(utf8String, 0, utf8String.Length);
        }

        private void SetBothGimbalPositions()
        {

            byte motorType = 0;
            byte motorID = 0;
            byte functionID = 1;
            float targetAngle = vScrollBar3.Value;
            float moveSpeed = 90f;
            float precision = 1f;
            string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + targetAngle.ToString() + "," + moveSpeed.ToString() + "," + precision + ",";


            System.Diagnostics.Debug.WriteLine("--vScrollBar3_Scroll--");
            //System.Diagnostics.Debug.WriteLine(commandString);
            byte[] utf8String = Encoding.UTF8.GetBytes(commandString);


            serialPort1.Write(utf8String, 0, utf8String.Length);
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        { 
        }

        private void vScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void vScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            
        }



        private void vScrollBar5_Scroll(object sender, ScrollEventArgs e)
        {
        }

        private void btnTargetRotationCommand_Click(object sender, EventArgs e)
        {
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

        private void SetBrushlessPWM(byte motorID, float targetPWM)
        {
            byte motorType = 2;
            byte functionID = 0;
            string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + targetPWM.ToString() + ",";
            System.Diagnostics.Debug.WriteLine("--brushless change --" + motorID.ToString());
            //System.Diagnostics.Debug.WriteLine(commandString);
            byte[] utf8String = Encoding.UTF8.GetBytes(commandString);

            serialPort1.Write(utf8String, 0, utf8String.Length);
        }

        int vScrollBar1_LastValue = 0;
        int vScrollBar2_LastValue = 0;
        int vScrollBar3_LastValue = 0;

        int hScrollBar_left_brushless_LastValue = 0;
        int hScrollBar_right_brushless_LastValue = 0;
        int hScrollBar_both_brushless_LastValue = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {

            // gimbal control
            if (vScrollBar3_LastValue != vScrollBar3.Value)
            {
                SetBothGimbalPositions();
                vScrollBar3_LastValue = vScrollBar3.Value;

                vScrollBar1.Value = vScrollBar3.Value;
                vScrollBar2.Value = vScrollBar3.Value;

                txt_gimbal_both_rotation.Text = vScrollBar3.Value.ToString();
                txt_gimbal_right_rotation.Text = vScrollBar3.Value.ToString();
                txt_gimbal_left_rotation.Text = vScrollBar3.Value.ToString();
            }

            else if (vScrollBar1_LastValue != vScrollBar1.Value)
            {
                SetLeftGimbalPosition();
                vScrollBar1_LastValue = vScrollBar1.Value;
                txt_gimbal_left_rotation.Text = vScrollBar1.Value.ToString();
            }
            else if (vScrollBar2_LastValue != vScrollBar2.Value)
            {
                SetRightGimbalPosition();
                vScrollBar2_LastValue = vScrollBar2.Value;
                txt_gimbal_right_rotation.Text = vScrollBar2.Value.ToString();
            }
            

            // brushless control
            if (hScrollBar_both_brushless_LastValue != hScrollBar_both_brushless.Value)
            {
                byte both_id = 0;
                SetBrushlessPWM(both_id, (float) hScrollBar_both_brushless.Value);

                txt_brushless_pwm_both.Text = hScrollBar_both_brushless.Value.ToString();
                txt_brushless_pwm_left.Text = hScrollBar_both_brushless.Value.ToString();
                txt_brushless_pwm_right.Text = hScrollBar_both_brushless.Value.ToString();

                hScrollBar_left_brushless.Value = hScrollBar_both_brushless.Value;
                hScrollBar_right_brushless.Value = hScrollBar_both_brushless.Value;

                hScrollBar_both_brushless_LastValue = hScrollBar_both_brushless.Value;
                hScrollBar_left_brushless_LastValue = hScrollBar_both_brushless.Value;
                hScrollBar_right_brushless_LastValue = hScrollBar_both_brushless.Value;
            }

            else if (hScrollBar_left_brushless_LastValue != hScrollBar_left_brushless.Value)
            {
                byte left_id = 1;
                SetBrushlessPWM(left_id, (float)hScrollBar_left_brushless.Value);
                txt_brushless_pwm_left.Text = hScrollBar_left_brushless_LastValue.ToString();
                hScrollBar_left_brushless_LastValue = hScrollBar_left_brushless.Value;
            }
            else if (hScrollBar_right_brushless_LastValue != hScrollBar_right_brushless.Value)
            {
                byte right_id = 2;
                SetBrushlessPWM(right_id, (float)hScrollBar_right_brushless.Value);
                txt_brushless_pwm_right.Text = hScrollBar_right_brushless_LastValue.ToString();
                hScrollBar_right_brushless_LastValue = hScrollBar_right_brushless.Value;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // timer1.Start();
        }
    }
}
