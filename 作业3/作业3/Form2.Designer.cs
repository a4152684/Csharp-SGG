namespace 作业3
{
    partial class Form2
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.longtextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.denominatorTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.xx = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.yy = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.BB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.LL = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.middleLongitude = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.BB2 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.LL2 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.radioButtonD = new System.Windows.Forms.RadioButton();
            this.radioButtonDMS = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.ReadFileZS = new System.Windows.Forms.Button();
            this.ReadFileFS = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "椭球长半轴(m)";
            // 
            // longtextBox
            // 
            this.longtextBox.Location = new System.Drawing.Point(73, 82);
            this.longtextBox.Name = "longtextBox";
            this.longtextBox.Size = new System.Drawing.Size(100, 21);
            this.longtextBox.TabIndex = 2;
            this.longtextBox.Text = "6378245";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(467, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "椭球扁率分母";
            // 
            // denominatorTextBox
            // 
            this.denominatorTextBox.Location = new System.Drawing.Point(469, 82);
            this.denominatorTextBox.Name = "denominatorTextBox";
            this.denominatorTextBox.Size = new System.Drawing.Size(100, 21);
            this.denominatorTextBox.TabIndex = 4;
            this.denominatorTextBox.Text = "298.3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(467, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "x(m)";
            // 
            // xx
            // 
            this.xx.Location = new System.Drawing.Point(469, 218);
            this.xx.Name = "xx";
            this.xx.Size = new System.Drawing.Size(100, 21);
            this.xx.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(588, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "y(m)";
            // 
            // yy
            // 
            this.yy.Location = new System.Drawing.Point(590, 218);
            this.yy.Name = "yy";
            this.yy.Size = new System.Drawing.Size(100, 21);
            this.yy.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(465, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(189, 19);
            this.label5.TabIndex = 10;
            this.label5.Text = "高斯平面坐标(x,y):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(69, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 19);
            this.label6.TabIndex = 15;
            this.label6.Text = "大地坐标(L,B):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(189, 191);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "B(°)";
            // 
            // BB
            // 
            this.BB.Location = new System.Drawing.Point(190, 218);
            this.BB.Name = "BB";
            this.BB.Size = new System.Drawing.Size(100, 21);
            this.BB.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(71, 191);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "L(°)";
            // 
            // LL
            // 
            this.LL.Location = new System.Drawing.Point(73, 218);
            this.LL.Name = "LL";
            this.LL.Size = new System.Drawing.Size(100, 21);
            this.LL.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(69, 328);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(129, 19);
            this.label9.TabIndex = 16;
            this.label9.Text = "(L,B)->(x,y)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(465, 328);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(129, 19);
            this.label10.TabIndex = 17;
            this.label10.Text = "(x,y)->(L,B)";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(73, 362);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "正算";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(469, 362);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "反算";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(71, 430);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(245, 12);
            this.label11.TabIndex = 20;
            this.label11.Text = "注：默认采用克拉索夫斯基椭球，六度带计算";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(328, 191);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 12);
            this.label12.TabIndex = 22;
            this.label12.Text = "中央子午线经度(°)";
            // 
            // middleLongitude
            // 
            this.middleLongitude.Location = new System.Drawing.Point(329, 217);
            this.middleLongitude.Name = "middleLongitude";
            this.middleLongitude.Size = new System.Drawing.Size(100, 21);
            this.middleLongitude.TabIndex = 21;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(94, 472);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(389, 12);
            this.label13.TabIndex = 23;
            this.label13.Text = "正算时若不输入中央子午线经度，则自动按照六度带计算中央子午线经度";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(73, 10);
            this.button3.Name = "button3";
            this.button3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 24;
            this.button3.Text = "初始化";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(190, 10);
            this.button4.Name = "button4";
            this.button4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button4.Size = new System.Drawing.Size(126, 23);
            this.button4.TabIndex = 25;
            this.button4.Text = "克拉索夫斯基椭球";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(354, 10);
            this.button5.Name = "button5";
            this.button5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button5.Size = new System.Drawing.Size(106, 23);
            this.button5.TabIndex = 26;
            this.button5.Text = "1975国际椭球";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(497, 10);
            this.button6.Name = "button6";
            this.button6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button6.Size = new System.Drawing.Size(106, 23);
            this.button6.TabIndex = 27;
            this.button6.Text = "WGS84椭球体";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(639, 10);
            this.button7.Name = "button7";
            this.button7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button7.Size = new System.Drawing.Size(125, 23);
            this.button7.TabIndex = 28;
            this.button7.Text = "2000中国大地坐标系";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(189, 249);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(71, 12);
            this.label14.TabIndex = 32;
            this.label14.Text = "B(° ′ ″)";
            // 
            // BB2
            // 
            this.BB2.Location = new System.Drawing.Point(190, 276);
            this.BB2.Name = "BB2";
            this.BB2.Size = new System.Drawing.Size(100, 21);
            this.BB2.TabIndex = 31;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(71, 249);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 12);
            this.label15.TabIndex = 30;
            this.label15.Text = "L(° ′ ″)";
            // 
            // LL2
            // 
            this.LL2.Location = new System.Drawing.Point(73, 276);
            this.LL2.Name = "LL2";
            this.LL2.Size = new System.Drawing.Size(100, 21);
            this.LL2.TabIndex = 29;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(71, 307);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(287, 12);
            this.label16.TabIndex = 33;
            this.label16.Text = "度分秒输入时请逗号隔开，114°20′请输入114,20,0";
            // 
            // radioButtonD
            // 
            this.radioButtonD.AutoSize = true;
            this.radioButtonD.Checked = true;
            this.radioButtonD.Location = new System.Drawing.Point(73, 159);
            this.radioButtonD.Name = "radioButtonD";
            this.radioButtonD.Size = new System.Drawing.Size(95, 16);
            this.radioButtonD.TabIndex = 34;
            this.radioButtonD.TabStop = true;
            this.radioButtonD.Text = "以°形式输入";
            this.radioButtonD.UseVisualStyleBackColor = true;
            // 
            // radioButtonDMS
            // 
            this.radioButtonDMS.AutoSize = true;
            this.radioButtonDMS.Location = new System.Drawing.Point(191, 158);
            this.radioButtonDMS.Name = "radioButtonDMS";
            this.radioButtonDMS.Size = new System.Drawing.Size(143, 16);
            this.radioButtonDMS.TabIndex = 35;
            this.radioButtonDMS.Text = "以(° ′ ″)形式输入";
            this.radioButtonDMS.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(94, 450);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(239, 12);
            this.label17.TabIndex = 36;
            this.label17.Text = "正算时输入B,L或B,L,L0；反算时输入x,y,L0";
            // 
            // ReadFileZS
            // 
            this.ReadFileZS.Location = new System.Drawing.Point(73, 397);
            this.ReadFileZS.Name = "ReadFileZS";
            this.ReadFileZS.Size = new System.Drawing.Size(95, 23);
            this.ReadFileZS.TabIndex = 37;
            this.ReadFileZS.Text = "读取文件正算";
            this.ReadFileZS.UseVisualStyleBackColor = true;
            this.ReadFileZS.Click += new System.EventHandler(this.ReadFileZS_Click);
            // 
            // ReadFileFS
            // 
            this.ReadFileFS.Location = new System.Drawing.Point(469, 397);
            this.ReadFileFS.Name = "ReadFileFS";
            this.ReadFileFS.Size = new System.Drawing.Size(95, 23);
            this.ReadFileFS.TabIndex = 38;
            this.ReadFileFS.Text = "读取文件反算";
            this.ReadFileFS.UseVisualStyleBackColor = true;
            this.ReadFileFS.Click += new System.EventHandler(this.ReadFileFS_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 493);
            this.Controls.Add(this.ReadFileFS);
            this.Controls.Add(this.ReadFileZS);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.radioButtonDMS);
            this.Controls.Add(this.radioButtonD);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.BB2);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.LL2);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.middleLongitude);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LL);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.yy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.xx);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.denominatorTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.longtextBox);
            this.Name = "Form1";
            this.Text = "高斯投影正反算软件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox longtextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox denominatorTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox xx;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox yy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox BB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox LL;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox middleLongitude;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox BB2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox LL2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.RadioButton radioButtonD;
        private System.Windows.Forms.RadioButton radioButtonDMS;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button ReadFileZS;
        private System.Windows.Forms.Button ReadFileFS;
    }
}

