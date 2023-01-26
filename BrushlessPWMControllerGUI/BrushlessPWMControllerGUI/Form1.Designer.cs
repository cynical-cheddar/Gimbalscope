namespace BrushlessPWMControllerGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.btn_0 = new System.Windows.Forms.Button();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lblBrushless2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.vScrollBar3 = new System.Windows.Forms.VScrollBar();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.vScrollBar4 = new System.Windows.Forms.VScrollBar();
            this.label6 = new System.Windows.Forms.Label();
            this.vScrollBar5 = new System.Windows.Forms.VScrollBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.targetRotationTextBox1 = new System.Windows.Forms.TextBox();
            this.targetRotationCommandBox = new System.Windows.Forms.TextBox();
            this.btnTargetRotationCommand = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_fire_command = new System.Windows.Forms.Button();
            this.separator = new System.Windows.Forms.Label();
            this.txt_target_orientation = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_fire_speed = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txt_reload_speed = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_fire_precision = new System.Windows.Forms.TextBox();
            this.txt_reload_precision = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btn_read = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.btn_send_garbage_byte = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM11";
            // 
            // btn_0
            // 
            this.btn_0.Location = new System.Drawing.Point(0, 0);
            this.btn_0.Name = "btn_0";
            this.btn_0.Size = new System.Drawing.Size(75, 23);
            this.btn_0.TabIndex = 6;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(282, 329);
            this.hScrollBar1.Maximum = 255;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(251, 30);
            this.hScrollBar1.TabIndex = 2;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(310, 372);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(173, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "begin";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Location = new System.Drawing.Point(275, 210);
            this.hScrollBar2.Maximum = 255;
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(251, 30);
            this.hScrollBar2.TabIndex = 7;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(310, 256);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(173, 20);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "0";
            // 
            // lblBrushless2
            // 
            this.lblBrushless2.AutoSize = true;
            this.lblBrushless2.Location = new System.Drawing.Point(362, 185);
            this.lblBrushless2.Name = "lblBrushless2";
            this.lblBrushless2.Size = new System.Drawing.Size(61, 13);
            this.lblBrushless2.TabIndex = 9;
            this.lblBrushless2.Text = "Brushless 2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(362, 295);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Brushless 1";
            // 
            // hScrollBar3
            // 
            this.hScrollBar3.Location = new System.Drawing.Point(282, 114);
            this.hScrollBar3.Maximum = 255;
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(251, 30);
            this.hScrollBar3.TabIndex = 11;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Both";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(310, 147);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(173, 20);
            this.textBox3.TabIndex = 13;
            this.textBox3.Text = "0";
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(57, 114);
            this.vScrollBar1.Maximum = 255;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(39, 245);
            this.vScrollBar1.TabIndex = 14;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Location = new System.Drawing.Point(120, 114);
            this.vScrollBar2.Maximum = 255;
            this.vScrollBar2.Name = "vScrollBar2";
            this.vScrollBar2.Size = new System.Drawing.Size(39, 245);
            this.vScrollBar2.TabIndex = 15;
            this.vScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar2_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Servo 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Servo 2";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(55, 372);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(41, 20);
            this.textBox4.TabIndex = 18;
            this.textBox4.Text = "0";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(118, 372);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(41, 20);
            this.textBox5.TabIndex = 19;
            this.textBox5.Text = "0";
            // 
            // vScrollBar3
            // 
            this.vScrollBar3.Location = new System.Drawing.Point(172, 114);
            this.vScrollBar3.Maximum = 255;
            this.vScrollBar3.Name = "vScrollBar3";
            this.vScrollBar3.Size = new System.Drawing.Size(39, 245);
            this.vScrollBar3.TabIndex = 20;
            this.vScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar3_Scroll);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(170, 372);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(41, 20);
            this.textBox6.TabIndex = 21;
            this.textBox6.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "both";
            // 
            // vScrollBar4
            // 
            this.vScrollBar4.Location = new System.Drawing.Point(222, 114);
            this.vScrollBar4.Maximum = 255;
            this.vScrollBar4.Name = "vScrollBar4";
            this.vScrollBar4.Size = new System.Drawing.Size(39, 245);
            this.vScrollBar4.TabIndex = 23;
            this.vScrollBar4.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar4_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(219, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "both-";
            // 
            // vScrollBar5
            // 
            this.vScrollBar5.Location = new System.Drawing.Point(687, 74);
            this.vScrollBar5.Maximum = 10000;
            this.vScrollBar5.Minimum = -10000;
            this.vScrollBar5.Name = "vScrollBar5";
            this.vScrollBar5.Size = new System.Drawing.Size(39, 245);
            this.vScrollBar5.TabIndex = 25;
            this.vScrollBar5.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar5_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(684, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Target Rotation";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(640, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "-10000";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(640, 306);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "10000";
            // 
            // targetRotationTextBox1
            // 
            this.targetRotationTextBox1.Location = new System.Drawing.Point(687, 339);
            this.targetRotationTextBox1.Name = "targetRotationTextBox1";
            this.targetRotationTextBox1.Size = new System.Drawing.Size(41, 20);
            this.targetRotationTextBox1.TabIndex = 29;
            this.targetRotationTextBox1.Text = "0";
            this.targetRotationTextBox1.TextChanged += new System.EventHandler(this.targetRotationTextBox1_TextChanged);
            // 
            // targetRotationCommandBox
            // 
            this.targetRotationCommandBox.Location = new System.Drawing.Point(585, 339);
            this.targetRotationCommandBox.Name = "targetRotationCommandBox";
            this.targetRotationCommandBox.Size = new System.Drawing.Size(41, 20);
            this.targetRotationCommandBox.TabIndex = 30;
            this.targetRotationCommandBox.Text = "0";
            // 
            // btnTargetRotationCommand
            // 
            this.btnTargetRotationCommand.Location = new System.Drawing.Point(571, 365);
            this.btnTargetRotationCommand.Name = "btnTargetRotationCommand";
            this.btnTargetRotationCommand.Size = new System.Drawing.Size(75, 23);
            this.btnTargetRotationCommand.TabIndex = 31;
            this.btnTargetRotationCommand.Text = "send";
            this.btnTargetRotationCommand.UseVisualStyleBackColor = true;
            this.btnTargetRotationCommand.Click += new System.EventHandler(this.btnTargetRotationCommand_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(359, 339);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 32;
            this.button2.Text = "begin";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btn_fire_command
            // 
            this.btn_fire_command.Location = new System.Drawing.Point(36, 525);
            this.btn_fire_command.Name = "btn_fire_command";
            this.btn_fire_command.Size = new System.Drawing.Size(75, 23);
            this.btn_fire_command.TabIndex = 33;
            this.btn_fire_command.Text = "Fire!";
            this.btn_fire_command.UseVisualStyleBackColor = true;
            this.btn_fire_command.Click += new System.EventHandler(this.btn_fire_command_Click);
            // 
            // separator
            // 
            this.separator.AutoSize = true;
            this.separator.Location = new System.Drawing.Point(33, 426);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(715, 13);
            this.separator.TabIndex = 34;
            this.separator.Text = "=================================================================================" +
    "=====================================";
            this.separator.Click += new System.EventHandler(this.label10_Click);
            // 
            // txt_target_orientation
            // 
            this.txt_target_orientation.Location = new System.Drawing.Point(141, 499);
            this.txt_target_orientation.Name = "txt_target_orientation";
            this.txt_target_orientation.Size = new System.Drawing.Size(100, 20);
            this.txt_target_orientation.TabIndex = 35;
            this.txt_target_orientation.Text = "60";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(247, 499);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(139, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Target Orientation (degrees)";
            this.label10.Click += new System.EventHandler(this.label10_Click_1);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(247, 530);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Fire Rotational Speed";
            // 
            // txt_fire_speed
            // 
            this.txt_fire_speed.Location = new System.Drawing.Point(141, 528);
            this.txt_fire_speed.Name = "txt_fire_speed";
            this.txt_fire_speed.Size = new System.Drawing.Size(100, 20);
            this.txt_fire_speed.TabIndex = 37;
            this.txt_fire_speed.Text = "180";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(247, 570);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(126, 13);
            this.label12.TabIndex = 40;
            this.label12.Text = "Reload Rotational Speed";
            // 
            // txt_reload_speed
            // 
            this.txt_reload_speed.Location = new System.Drawing.Point(141, 563);
            this.txt_reload_speed.Name = "txt_reload_speed";
            this.txt_reload_speed.Size = new System.Drawing.Size(100, 20);
            this.txt_reload_speed.TabIndex = 39;
            this.txt_reload_speed.Text = "45";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(247, 608);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 13);
            this.label13.TabIndex = 42;
            this.label13.Text = "Fire Precision (degrees)";
            // 
            // txt_fire_precision
            // 
            this.txt_fire_precision.Location = new System.Drawing.Point(141, 601);
            this.txt_fire_precision.Name = "txt_fire_precision";
            this.txt_fire_precision.Size = new System.Drawing.Size(100, 20);
            this.txt_fire_precision.TabIndex = 41;
            this.txt_fire_precision.Text = "3";
            // 
            // txt_reload_precision
            // 
            this.txt_reload_precision.Location = new System.Drawing.Point(141, 642);
            this.txt_reload_precision.Name = "txt_reload_precision";
            this.txt_reload_precision.Size = new System.Drawing.Size(100, 20);
            this.txt_reload_precision.TabIndex = 43;
            this.txt_reload_precision.Text = "1";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(247, 645);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(134, 13);
            this.label14.TabIndex = 44;
            this.label14.Text = "Reload Precision (degrees)";
            // 
            // btn_read
            // 
            this.btn_read.Location = new System.Drawing.Point(495, 520);
            this.btn_read.Name = "btn_read";
            this.btn_read.Size = new System.Drawing.Size(75, 23);
            this.btn_read.TabIndex = 45;
            this.btn_read.Text = "Read!";
            this.btn_read.UseVisualStyleBackColor = true;
            this.btn_read.Click += new System.EventHandler(this.btn_read_Click);
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(606, 520);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(142, 20);
            this.textBox7.TabIndex = 46;
            this.textBox7.TextChanged += new System.EventHandler(this.textBox7_TextChanged);
            // 
            // btn_send_garbage_byte
            // 
            this.btn_send_garbage_byte.Location = new System.Drawing.Point(495, 459);
            this.btn_send_garbage_byte.Name = "btn_send_garbage_byte";
            this.btn_send_garbage_byte.Size = new System.Drawing.Size(75, 23);
            this.btn_send_garbage_byte.TabIndex = 47;
            this.btn_send_garbage_byte.Text = "Sent byte";
            this.btn_send_garbage_byte.UseVisualStyleBackColor = true;
            this.btn_send_garbage_byte.Click += new System.EventHandler(this.btn_send_garbage_byte_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 701);
            this.Controls.Add(this.btn_send_garbage_byte);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.btn_read);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txt_reload_precision);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txt_fire_precision);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txt_reload_speed);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txt_fire_speed);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txt_target_orientation);
            this.Controls.Add(this.separator);
            this.Controls.Add(this.btn_fire_command);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnTargetRotationCommand);
            this.Controls.Add(this.targetRotationCommandBox);
            this.Controls.Add(this.targetRotationTextBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.vScrollBar5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.vScrollBar4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.vScrollBar3);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.vScrollBar2);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hScrollBar3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblBrushless2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.hScrollBar2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.btn_0);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button btn_0;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.HScrollBar hScrollBar2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lblBrushless2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HScrollBar hScrollBar3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.VScrollBar vScrollBar3;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.VScrollBar vScrollBar4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.VScrollBar vScrollBar5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox targetRotationTextBox1;
        private System.Windows.Forms.TextBox targetRotationCommandBox;
        private System.Windows.Forms.Button btnTargetRotationCommand;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_fire_command;
        private System.Windows.Forms.Label separator;
        private System.Windows.Forms.TextBox txt_target_orientation;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_fire_speed;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txt_reload_speed;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_fire_precision;
        private System.Windows.Forms.TextBox txt_reload_precision;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_read;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Button btn_send_garbage_byte;
        private System.Windows.Forms.Timer timer1;
    }
}

