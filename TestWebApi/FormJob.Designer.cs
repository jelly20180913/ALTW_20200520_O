namespace TestWebApi
{
    partial class FormJob
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
            this.btnStart = new System.Windows.Forms.Button();
            this.timerCreateOrder = new System.Windows.Forms.Timer(this.components);
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnGet = new System.Windows.Forms.Button();
            this.timerOrderComfirm = new System.Windows.Forms.Timer(this.components);
            this.lbTest = new System.Windows.Forms.Label();
            this.btnCreateOrder = new System.Windows.Forms.Button();
            this.btnOrderComfirm = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEdifact = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.txtJob = new System.Windows.Forms.TextBox();
            this.lbSec = new System.Windows.Forms.Label();
            this.lbEvery = new System.Windows.Forms.Label();
            this.btnSet = new System.Windows.Forms.Button();
            this.txtEDIinput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInformation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(42, 33);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(124, 27);
            this.btnStart.TabIndex = 19;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // timerCreateOrder
            // 
            this.timerCreateOrder.Enabled = true;
            this.timerCreateOrder.Interval = 600000;
            this.timerCreateOrder.Tick += new System.EventHandler(this.TimerCreateOrder_Tick);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(6, 28);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(124, 27);
            this.btnLogin.TabIndex = 20;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(9, 75);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(121, 33);
            this.btnGet.TabIndex = 21;
            this.btnGet.Text = "get webapi";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.BtnGet_Click);
            // 
            // timerOrderComfirm
            // 
            this.timerOrderComfirm.Enabled = true;
            this.timerOrderComfirm.Interval = 600000;
            this.timerOrderComfirm.Tick += new System.EventHandler(this.TimerOrderComfirm_Tick);
            // 
            // lbTest
            // 
            this.lbTest.AutoSize = true;
            this.lbTest.Location = new System.Drawing.Point(493, 33);
            this.lbTest.Name = "lbTest";
            this.lbTest.Size = new System.Drawing.Size(0, 18);
            this.lbTest.TabIndex = 22;
            // 
            // btnCreateOrder
            // 
            this.btnCreateOrder.Location = new System.Drawing.Point(9, 126);
            this.btnCreateOrder.Name = "btnCreateOrder";
            this.btnCreateOrder.Size = new System.Drawing.Size(124, 27);
            this.btnCreateOrder.TabIndex = 23;
            this.btnCreateOrder.Text = "TestOrder";
            this.btnCreateOrder.UseVisualStyleBackColor = true;
            this.btnCreateOrder.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // btnOrderComfirm
            // 
            this.btnOrderComfirm.Location = new System.Drawing.Point(6, 174);
            this.btnOrderComfirm.Name = "btnOrderComfirm";
            this.btnOrderComfirm.Size = new System.Drawing.Size(152, 27);
            this.btnOrderComfirm.TabIndex = 24;
            this.btnOrderComfirm.Text = "TestOrderComfirm";
            this.btnOrderComfirm.UseVisualStyleBackColor = true;
            this.btnOrderComfirm.Click += new System.EventHandler(this.BtnOrderComfirm_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEdifact);
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.btnOrderComfirm);
            this.groupBox1.Controls.Add(this.btnGet);
            this.groupBox1.Controls.Add(this.btnCreateOrder);
            this.groupBox1.Location = new System.Drawing.Point(33, 137);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 266);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "程序";
            // 
            // btnEdifact
            // 
            this.btnEdifact.Location = new System.Drawing.Point(9, 220);
            this.btnEdifact.Name = "btnEdifact";
            this.btnEdifact.Size = new System.Drawing.Size(124, 27);
            this.btnEdifact.TabIndex = 25;
            this.btnEdifact.Text = "TestEdifact";
            this.btnEdifact.UseVisualStyleBackColor = true;
            this.btnEdifact.Click += new System.EventHandler(this.BtnEdifact_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(233, 29);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1478, 700);
            this.txtLog.TabIndex = 26;
            // 
            // txtJob
            // 
            this.txtJob.Location = new System.Drawing.Point(68, 75);
            this.txtJob.Name = "txtJob";
            this.txtJob.Size = new System.Drawing.Size(70, 29);
            this.txtJob.TabIndex = 27;
            // 
            // lbSec
            // 
            this.lbSec.AutoSize = true;
            this.lbSec.Location = new System.Drawing.Point(152, 80);
            this.lbSec.Name = "lbSec";
            this.lbSec.Size = new System.Drawing.Size(26, 18);
            this.lbSec.TabIndex = 28;
            this.lbSec.Text = "分";
            // 
            // lbEvery
            // 
            this.lbEvery.AutoSize = true;
            this.lbEvery.Location = new System.Drawing.Point(36, 80);
            this.lbEvery.Name = "lbEvery";
            this.lbEvery.Size = new System.Drawing.Size(26, 18);
            this.lbEvery.TabIndex = 29;
            this.lbEvery.Text = "每";
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(184, 74);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(43, 27);
            this.btnSet.TabIndex = 30;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.BtnSet_Click);
            // 
            // txtEDIinput
            // 
            this.txtEDIinput.Location = new System.Drawing.Point(233, 938);
            this.txtEDIinput.Multiline = true;
            this.txtEDIinput.Name = "txtEDIinput";
            this.txtEDIinput.Size = new System.Drawing.Size(1478, 161);
            this.txtEDIinput.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(235, 917);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 18);
            this.label1.TabIndex = 32;
            this.label1.Text = "Test data";
            // 
            // txtInformation
            // 
            this.txtInformation.Location = new System.Drawing.Point(233, 753);
            this.txtInformation.Multiline = true;
            this.txtInformation.Name = "txtInformation";
            this.txtInformation.Size = new System.Drawing.Size(1478, 161);
            this.txtInformation.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(235, 732);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 18);
            this.label2.TabIndex = 34;
            this.label2.Text = "Connection";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(33, 432);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(145, 27);
            this.btnClear.TabIndex = 35;
            this.btnClear.Text = "Clear Log";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // FormJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1784, 1195);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtInformation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEDIinput);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.lbEvery);
            this.Controls.Add(this.lbSec);
            this.Controls.Add(this.txtJob);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbTest);
            this.Controls.Add(this.btnStart);
            this.Name = "FormJob";
            this.Text = "EDI訊息解析程式";
            this.Load += new System.EventHandler(this.FormJob_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timerCreateOrder;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Timer timerOrderComfirm;
        private System.Windows.Forms.Label lbTest;
        private System.Windows.Forms.Button btnCreateOrder;
        private System.Windows.Forms.Button btnOrderComfirm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.TextBox txtJob;
        private System.Windows.Forms.Label lbSec;
        private System.Windows.Forms.Label lbEvery;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnEdifact;
        private System.Windows.Forms.TextBox txtEDIinput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInformation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClear;
    }
}