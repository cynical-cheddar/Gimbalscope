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
            this.hScrollBar_left_brushless = new System.Windows.Forms.HScrollBar();
            this.hScrollBar_right_brushless = new System.Windows.Forms.HScrollBar();
            this.lblBrushless2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_gimbal_left_rotation = new System.Windows.Forms.TextBox();
            this.txt_gimbal_right_rotation = new System.Windows.Forms.TextBox();
            this.vScrollBar3 = new System.Windows.Forms.VScrollBar();
            this.txt_gimbal_both_rotation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
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
            this.txt_brushless_pwm_left = new System.Windows.Forms.TextBox();
            this.txt_brushless_pwm_right = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.hScrollBar_both_brushless = new System.Windows.Forms.HScrollBar();
            this.txt_brushless_pwm_both = new System.Windows.Forms.TextBox();
            this.txt_interpolation_time_pwm = new System.Windows.Forms.TextBox();
            this.txt_reload_target_orientation = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM11";
            // 
            // hScrollBar_left_brushless
            // 
            this.hScrollBar_left_brushless.LargeChange = 4;
            this.hScrollBar_left_brushless.Location = new System.Drawing.Point(282, 121);
            this.hScrollBar_left_brushless.Maximum = 120;
            this.hScrollBar_left_brushless.Minimum = 50;
            this.hScrollBar_left_brushless.Name = "hScrollBar_left_brushless";
            this.hScrollBar_left_brushless.Size = new System.Drawing.Size(251, 30);
            this.hScrollBar_left_brushless.TabIndex = 2;
            this.hScrollBar_left_brushless.Value = 70;
            this.hScrollBar_left_brushless.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // hScrollBar_right_brushless
            // 
            this.hScrollBar_right_brushless.LargeChange = 4;
            this.hScrollBar_right_brushless.Location = new System.Drawing.Point(282, 213);
            this.hScrollBar_right_brushless.Maximum = 120;
            this.hScrollBar_right_brushless.Minimum = 50;
            this.hScrollBar_right_brushless.Name = "hScrollBar_right_brushless";
            this.hScrollBar_right_brushless.Size = new System.Drawing.Size(251, 30);
            this.hScrollBar_right_brushless.TabIndex = 7;
            this.hScrollBar_right_brushless.Value = 70;
            this.hScrollBar_right_brushless.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
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
            this.label1.Location = new System.Drawing.Point(362, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Brushless 1";
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(57, 114);
            this.vScrollBar1.Maximum = 180;
            this.vScrollBar1.Minimum = -180;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(39, 245);
            this.vScrollBar1.TabIndex = 14;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Location = new System.Drawing.Point(120, 114);
            this.vScrollBar2.Maximum = 180;
            this.vScrollBar2.Minimum = -180;
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
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Left Gimbal";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Right Gimbal";
            // 
            // txt_gimbal_left_rotation
            // 
            this.txt_gimbal_left_rotation.Location = new System.Drawing.Point(55, 372);
            this.txt_gimbal_left_rotation.Name = "txt_gimbal_left_rotation";
            this.txt_gimbal_left_rotation.ReadOnly = true;
            this.txt_gimbal_left_rotation.Size = new System.Drawing.Size(41, 20);
            this.txt_gimbal_left_rotation.TabIndex = 18;
            this.txt_gimbal_left_rotation.Text = "0";
            // 
            // txt_gimbal_right_rotation
            // 
            this.txt_gimbal_right_rotation.Location = new System.Drawing.Point(118, 372);
            this.txt_gimbal_right_rotation.Name = "txt_gimbal_right_rotation";
            this.txt_gimbal_right_rotation.ReadOnly = true;
            this.txt_gimbal_right_rotation.Size = new System.Drawing.Size(41, 20);
            this.txt_gimbal_right_rotation.TabIndex = 19;
            this.txt_gimbal_right_rotation.Text = "0";
            // 
            // vScrollBar3
            // 
            this.vScrollBar3.Location = new System.Drawing.Point(202, 114);
            this.vScrollBar3.Maximum = 180;
            this.vScrollBar3.Minimum = -180;
            this.vScrollBar3.Name = "vScrollBar3";
            this.vScrollBar3.Size = new System.Drawing.Size(39, 245);
            this.vScrollBar3.TabIndex = 20;
            this.vScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar3_Scroll);
            // 
            // txt_gimbal_both_rotation
            // 
            this.txt_gimbal_both_rotation.Location = new System.Drawing.Point(200, 372);
            this.txt_gimbal_both_rotation.Name = "txt_gimbal_both_rotation";
            this.txt_gimbal_both_rotation.ReadOnly = true;
            this.txt_gimbal_both_rotation.Size = new System.Drawing.Size(41, 20);
            this.txt_gimbal_both_rotation.TabIndex = 21;
            this.txt_gimbal_both_rotation.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Both Gimbals";
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
            this.txt_target_orientation.Location = new System.Drawing.Point(141, 478);
            this.txt_target_orientation.Name = "txt_target_orientation";
            this.txt_target_orientation.Size = new System.Drawing.Size(100, 20);
            this.txt_target_orientation.TabIndex = 35;
            this.txt_target_orientation.Text = "60";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(247, 478);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(139, 13);
            this.label10.TabIndex = 36;
            this.label10.Text = "Target Orientation (degrees)";
            this.label10.Click += new System.EventHandler(this.label10_Click_1);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(247, 509);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Fire Rotational Speed";
            // 
            // txt_fire_speed
            // 
            this.txt_fire_speed.Location = new System.Drawing.Point(141, 507);
            this.txt_fire_speed.Name = "txt_fire_speed";
            this.txt_fire_speed.Size = new System.Drawing.Size(100, 20);
            this.txt_fire_speed.TabIndex = 37;
            this.txt_fire_speed.Text = "720";
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
            this.timer1.Enabled = true;
            this.timer1.Interval = 800;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txt_brushless_pwm_left
            // 
            this.txt_brushless_pwm_left.Location = new System.Drawing.Point(560, 131);
            this.txt_brushless_pwm_left.Name = "txt_brushless_pwm_left";
            this.txt_brushless_pwm_left.ReadOnly = true;
            this.txt_brushless_pwm_left.Size = new System.Drawing.Size(41, 20);
            this.txt_brushless_pwm_left.TabIndex = 49;
            this.txt_brushless_pwm_left.Text = "0";
            // 
            // txt_brushless_pwm_right
            // 
            this.txt_brushless_pwm_right.Location = new System.Drawing.Point(560, 223);
            this.txt_brushless_pwm_right.Name = "txt_brushless_pwm_right";
            this.txt_brushless_pwm_right.ReadOnly = true;
            this.txt_brushless_pwm_right.Size = new System.Drawing.Size(41, 20);
            this.txt_brushless_pwm_right.TabIndex = 50;
            this.txt_brushless_pwm_right.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 52;
            this.label2.Text = "Brushless Both";
            // 
            // hScrollBar_both_brushless
            // 
            this.hScrollBar_both_brushless.LargeChange = 4;
            this.hScrollBar_both_brushless.Location = new System.Drawing.Point(282, 305);
            this.hScrollBar_both_brushless.Maximum = 120;
            this.hScrollBar_both_brushless.Minimum = 50;
            this.hScrollBar_both_brushless.Name = "hScrollBar_both_brushless";
            this.hScrollBar_both_brushless.Size = new System.Drawing.Size(251, 30);
            this.hScrollBar_both_brushless.TabIndex = 51;
            this.hScrollBar_both_brushless.Value = 70;
            // 
            // txt_brushless_pwm_both
            // 
            this.txt_brushless_pwm_both.Location = new System.Drawing.Point(560, 305);
            this.txt_brushless_pwm_both.Name = "txt_brushless_pwm_both";
            this.txt_brushless_pwm_both.ReadOnly = true;
            this.txt_brushless_pwm_both.Size = new System.Drawing.Size(41, 20);
            this.txt_brushless_pwm_both.TabIndex = 53;
            this.txt_brushless_pwm_both.Text = "0";
            // 
            // txt_interpolation_time_pwm
            // 
            this.txt_interpolation_time_pwm.Location = new System.Drawing.Point(560, 54);
            this.txt_interpolation_time_pwm.Name = "txt_interpolation_time_pwm";
            this.txt_interpolation_time_pwm.Size = new System.Drawing.Size(41, 20);
            this.txt_interpolation_time_pwm.TabIndex = 54;
            this.txt_interpolation_time_pwm.Text = "1";
            // 
            // txt_reload_target_orientation
            // 
            this.txt_reload_target_orientation.Location = new System.Drawing.Point(141, 533);
            this.txt_reload_target_orientation.Name = "txt_reload_target_orientation";
            this.txt_reload_target_orientation.Size = new System.Drawing.Size(100, 20);
            this.txt_reload_target_orientation.TabIndex = 55;
            this.txt_reload_target_orientation.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(247, 540);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 13);
            this.label6.TabIndex = 56;
            this.label6.Text = "Reload Target Orientation";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 701);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_reload_target_orientation);
            this.Controls.Add(this.txt_interpolation_time_pwm);
            this.Controls.Add(this.txt_brushless_pwm_both);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hScrollBar_both_brushless);
            this.Controls.Add(this.txt_brushless_pwm_right);
            this.Controls.Add(this.txt_brushless_pwm_left);
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
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_gimbal_both_rotation);
            this.Controls.Add(this.vScrollBar3);
            this.Controls.Add(this.txt_gimbal_right_rotation);
            this.Controls.Add(this.txt_gimbal_left_rotation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.vScrollBar2);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblBrushless2);
            this.Controls.Add(this.hScrollBar_right_brushless);
            this.Controls.Add(this.hScrollBar_left_brushless);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.HScrollBar hScrollBar_left_brushless;
        private System.Windows.Forms.HScrollBar hScrollBar_right_brushless;
        private System.Windows.Forms.Label lblBrushless2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_gimbal_left_rotation;
        private System.Windows.Forms.TextBox txt_gimbal_right_rotation;
        private System.Windows.Forms.VScrollBar vScrollBar3;
        private System.Windows.Forms.TextBox txt_gimbal_both_rotation;
        private System.Windows.Forms.Label label5;
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
        private System.Windows.Forms.TextBox txt_brushless_pwm_left;
        private System.Windows.Forms.TextBox txt_brushless_pwm_right;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.HScrollBar hScrollBar_both_brushless;
        private System.Windows.Forms.TextBox txt_brushless_pwm_both;
        private System.Windows.Forms.TextBox txt_interpolation_time_pwm;
        private System.Windows.Forms.TextBox txt_reload_target_orientation;
        private System.Windows.Forms.Label label6;
    }
}

