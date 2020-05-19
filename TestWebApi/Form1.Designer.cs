namespace TestWebApi
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TestWebApi = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtDate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.lbSapPrice = new System.Windows.Forms.Label();
            this.lbCount = new System.Windows.Forms.Label();
            this.btnPostSapPrice = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.gpSapBapi = new System.Windows.Forms.GroupBox();
            this.btnOrder = new System.Windows.Forms.Button();
            this.gpHana = new System.Windows.Forms.GroupBox();
            this.btnCountry = new System.Windows.Forms.Button();
            this.btnArea = new System.Windows.Forms.Button();
            this.btnBpXY = new System.Windows.Forms.Button();
            this.btnPriceGroup = new System.Windows.Forms.Button();
            this.btnMOQ_MARC = new System.Windows.Forms.Button();
            this.btnMoqOrder = new System.Windows.Forms.Button();
            this.btnSales = new System.Windows.Forms.Button();
            this.btnAddressAll = new System.Windows.Forms.Button();
            this.btnBpAddress = new System.Windows.Forms.Button();
            this.btnGetBpEmail = new System.Windows.Forms.Button();
            this.btnWaterMark = new System.Windows.Forms.Button();
            this.btnEdiTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.btnRSSAPI = new System.Windows.Forms.Button();
            this.txtANSIX12 = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.gpSapBapi.SuspendLayout();
            this.gpHana.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TestWebApi
            // 
            this.TestWebApi.Location = new System.Drawing.Point(62, 45);
            this.TestWebApi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TestWebApi.Name = "TestWebApi";
            this.TestWebApi.Size = new System.Drawing.Size(112, 33);
            this.TestWebApi.TabIndex = 0;
            this.TestWebApi.Text = "Test(PLM)";
            this.TestWebApi.UseVisualStyleBackColor = true;
            this.TestWebApi.Click += new System.EventHandler(this.TestWebApi_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(194, 45);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 33);
            this.button1.TabIndex = 1;
            this.button1.Text = "Test Hana";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(62, 105);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 33);
            this.button2.TabIndex = 2;
            this.button2.Text = "Get Sap Price";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // txtDate
            // 
            this.txtDate.Location = new System.Drawing.Point(357, 105);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(124, 29);
            this.txtDate.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(262, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "取得日期";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(204, 111);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(52, 22);
            this.chkAll.TabIndex = 5;
            this.chkAll.Text = "all";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.Click += new System.EventHandler(this.ChkAll_Click);
            // 
            // lbSapPrice
            // 
            this.lbSapPrice.AutoSize = true;
            this.lbSapPrice.Location = new System.Drawing.Point(526, 105);
            this.lbSapPrice.Name = "lbSapPrice";
            this.lbSapPrice.Size = new System.Drawing.Size(0, 18);
            this.lbSapPrice.TabIndex = 6;
            // 
            // lbCount
            // 
            this.lbCount.AutoSize = true;
            this.lbCount.Location = new System.Drawing.Point(672, 106);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(26, 18);
            this.lbCount.TabIndex = 7;
            this.lbCount.Text = "筆";
            // 
            // btnPostSapPrice
            // 
            this.btnPostSapPrice.Location = new System.Drawing.Point(882, 99);
            this.btnPostSapPrice.Name = "btnPostSapPrice";
            this.btnPostSapPrice.Size = new System.Drawing.Size(123, 33);
            this.btnPostSapPrice.TabIndex = 8;
            this.btnPostSapPrice.Text = "Post Sap Price";
            this.btnPostSapPrice.UseVisualStyleBackColor = true;
            this.btnPostSapPrice.Click += new System.EventHandler(this.BtnPostSapPrice_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(724, 98);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(123, 33);
            this.btnExcel.TabIndex = 9;
            this.btnExcel.Text = "Export Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.BtnExcel_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(345, 45);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 33);
            this.button3.TabIndex = 11;
            this.button3.Text = "Test Insert";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // gpSapBapi
            // 
            this.gpSapBapi.Controls.Add(this.btnLogin);
            this.gpSapBapi.Controls.Add(this.btnOrder);
            this.gpSapBapi.Location = new System.Drawing.Point(675, 12);
            this.gpSapBapi.Name = "gpSapBapi";
            this.gpSapBapi.Size = new System.Drawing.Size(438, 70);
            this.gpSapBapi.TabIndex = 12;
            this.gpSapBapi.TabStop = false;
            this.gpSapBapi.Text = "Bapi test";
            // 
            // btnOrder
            // 
            this.btnOrder.Location = new System.Drawing.Point(141, 20);
            this.btnOrder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(112, 33);
            this.btnOrder.TabIndex = 1;
            this.btnOrder.Text = "order";
            this.btnOrder.UseVisualStyleBackColor = true;
            this.btnOrder.Click += new System.EventHandler(this.BtnOrder_Click);
            // 
            // gpHana
            // 
            this.gpHana.Controls.Add(this.btnCountry);
            this.gpHana.Controls.Add(this.btnArea);
            this.gpHana.Controls.Add(this.btnBpXY);
            this.gpHana.Controls.Add(this.btnPriceGroup);
            this.gpHana.Controls.Add(this.btnMOQ_MARC);
            this.gpHana.Controls.Add(this.btnMoqOrder);
            this.gpHana.Controls.Add(this.btnSales);
            this.gpHana.Controls.Add(this.btnAddressAll);
            this.gpHana.Controls.Add(this.btnBpAddress);
            this.gpHana.Controls.Add(this.btnGetBpEmail);
            this.gpHana.Location = new System.Drawing.Point(68, 158);
            this.gpHana.Name = "gpHana";
            this.gpHana.Size = new System.Drawing.Size(964, 207);
            this.gpHana.TabIndex = 13;
            this.gpHana.TabStop = false;
            this.gpHana.Text = "Hana";
            // 
            // btnCountry
            // 
            this.btnCountry.Location = new System.Drawing.Point(798, 126);
            this.btnCountry.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCountry.Name = "btnCountry";
            this.btnCountry.Size = new System.Drawing.Size(147, 33);
            this.btnCountry.TabIndex = 12;
            this.btnCountry.Text = "Get Country";
            this.btnCountry.UseVisualStyleBackColor = true;
            this.btnCountry.Click += new System.EventHandler(this.BtnCountry_Click);
            // 
            // btnArea
            // 
            this.btnArea.Location = new System.Drawing.Point(798, 42);
            this.btnArea.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnArea.Name = "btnArea";
            this.btnArea.Size = new System.Drawing.Size(147, 33);
            this.btnArea.TabIndex = 11;
            this.btnArea.Text = "Get Area";
            this.btnArea.UseVisualStyleBackColor = true;
            this.btnArea.Click += new System.EventHandler(this.BtnArea_Click);
            // 
            // btnBpXY
            // 
            this.btnBpXY.Location = new System.Drawing.Point(568, 126);
            this.btnBpXY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnBpXY.Name = "btnBpXY";
            this.btnBpXY.Size = new System.Drawing.Size(147, 33);
            this.btnBpXY.TabIndex = 10;
            this.btnBpXY.Text = "Get BP XY";
            this.btnBpXY.UseVisualStyleBackColor = true;
            this.btnBpXY.Click += new System.EventHandler(this.BtnBpXY_Click);
            // 
            // btnPriceGroup
            // 
            this.btnPriceGroup.Location = new System.Drawing.Point(352, 126);
            this.btnPriceGroup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPriceGroup.Name = "btnPriceGroup";
            this.btnPriceGroup.Size = new System.Drawing.Size(147, 33);
            this.btnPriceGroup.TabIndex = 9;
            this.btnPriceGroup.Text = "Get Price Group";
            this.btnPriceGroup.UseVisualStyleBackColor = true;
            this.btnPriceGroup.Click += new System.EventHandler(this.BtnPriceGroup_Click);
            // 
            // btnMOQ_MARC
            // 
            this.btnMOQ_MARC.Location = new System.Drawing.Point(164, 126);
            this.btnMOQ_MARC.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMOQ_MARC.Name = "btnMOQ_MARC";
            this.btnMOQ_MARC.Size = new System.Drawing.Size(152, 33);
            this.btnMOQ_MARC.TabIndex = 8;
            this.btnMOQ_MARC.Tag = "MARC";
            this.btnMOQ_MARC.Text = "Get Moq MARC";
            this.btnMOQ_MARC.UseVisualStyleBackColor = true;
            this.btnMOQ_MARC.Click += new System.EventHandler(this.BtnMoqOrder_Click);
            // 
            // btnMoqOrder
            // 
            this.btnMoqOrder.Location = new System.Drawing.Point(12, 126);
            this.btnMoqOrder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMoqOrder.Name = "btnMoqOrder";
            this.btnMoqOrder.Size = new System.Drawing.Size(141, 33);
            this.btnMoqOrder.TabIndex = 7;
            this.btnMoqOrder.Tag = "EINE";
            this.btnMoqOrder.Text = "Get Moq Order";
            this.btnMoqOrder.UseVisualStyleBackColor = true;
            this.btnMoqOrder.Click += new System.EventHandler(this.BtnMoqOrder_Click);
            // 
            // btnSales
            // 
            this.btnSales.Location = new System.Drawing.Point(568, 42);
            this.btnSales.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSales.Name = "btnSales";
            this.btnSales.Size = new System.Drawing.Size(192, 33);
            this.btnSales.TabIndex = 6;
            this.btnSales.Text = "Get BP Sales";
            this.btnSales.UseVisualStyleBackColor = true;
            this.btnSales.Click += new System.EventHandler(this.BtnSales_Click);
            // 
            // btnAddressAll
            // 
            this.btnAddressAll.Location = new System.Drawing.Point(350, 44);
            this.btnAddressAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAddressAll.Name = "btnAddressAll";
            this.btnAddressAll.Size = new System.Drawing.Size(192, 33);
            this.btnAddressAll.TabIndex = 5;
            this.btnAddressAll.Text = "Get BP Address All";
            this.btnAddressAll.UseVisualStyleBackColor = true;
            this.btnAddressAll.Click += new System.EventHandler(this.BtnAddressAll_Click);
            // 
            // btnBpAddress
            // 
            this.btnBpAddress.Location = new System.Drawing.Point(164, 42);
            this.btnBpAddress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnBpAddress.Name = "btnBpAddress";
            this.btnBpAddress.Size = new System.Drawing.Size(135, 33);
            this.btnBpAddress.TabIndex = 4;
            this.btnBpAddress.Text = "Get BP Address";
            this.btnBpAddress.UseVisualStyleBackColor = true;
            this.btnBpAddress.Click += new System.EventHandler(this.BtnBpAddress_Click);
            // 
            // btnGetBpEmail
            // 
            this.btnGetBpEmail.Location = new System.Drawing.Point(12, 44);
            this.btnGetBpEmail.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnGetBpEmail.Name = "btnGetBpEmail";
            this.btnGetBpEmail.Size = new System.Drawing.Size(112, 33);
            this.btnGetBpEmail.TabIndex = 3;
            this.btnGetBpEmail.Text = "Get BP Email";
            this.btnGetBpEmail.UseVisualStyleBackColor = true;
            this.btnGetBpEmail.Click += new System.EventHandler(this.BtnGetBpEmail_Click);
            // 
            // btnWaterMark
            // 
            this.btnWaterMark.Location = new System.Drawing.Point(491, 45);
            this.btnWaterMark.Name = "btnWaterMark";
            this.btnWaterMark.Size = new System.Drawing.Size(134, 33);
            this.btnWaterMark.TabIndex = 14;
            this.btnWaterMark.Text = "wartermark";
            this.btnWaterMark.UseVisualStyleBackColor = true;
            this.btnWaterMark.Click += new System.EventHandler(this.BtnWaterMark_Click);
            // 
            // btnEdiTest
            // 
            this.btnEdiTest.Location = new System.Drawing.Point(15, 45);
            this.btnEdiTest.Name = "btnEdiTest";
            this.btnEdiTest.Size = new System.Drawing.Size(84, 27);
            this.btnEdiTest.TabIndex = 15;
            this.btnEdiTest.Text = "EDI Test";
            this.btnEdiTest.UseVisualStyleBackColor = true;
            this.btnEdiTest.Click += new System.EventHandler(this.BtnEdiTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.btnRSSAPI);
            this.groupBox1.Controls.Add(this.txtANSIX12);
            this.groupBox1.Controls.Add(this.btnEdiTest);
            this.groupBox1.Location = new System.Drawing.Point(68, 400);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1599, 392);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EDI Test";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(262, 38);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(196, 34);
            this.button4.TabIndex = 21;
            this.button4.Text = "RSS API send file test";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // btnRSSAPI
            // 
            this.btnRSSAPI.Location = new System.Drawing.Point(126, 38);
            this.btnRSSAPI.Margin = new System.Windows.Forms.Padding(4);
            this.btnRSSAPI.Name = "btnRSSAPI";
            this.btnRSSAPI.Size = new System.Drawing.Size(112, 34);
            this.btnRSSAPI.TabIndex = 19;
            this.btnRSSAPI.Text = "RSS API test";
            this.btnRSSAPI.UseVisualStyleBackColor = true;
            this.btnRSSAPI.Click += new System.EventHandler(this.BtnRSSAPI_Click);
            // 
            // txtANSIX12
            // 
            this.txtANSIX12.Location = new System.Drawing.Point(15, 93);
            this.txtANSIX12.Multiline = true;
            this.txtANSIX12.Name = "txtANSIX12";
            this.txtANSIX12.Size = new System.Drawing.Size(834, 211);
            this.txtANSIX12.TabIndex = 17;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(6, 20);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(112, 33);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1918, 1247);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnWaterMark);
            this.Controls.Add(this.gpHana);
            this.Controls.Add(this.gpSapBapi);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnPostSapPrice);
            this.Controls.Add(this.lbCount);
            this.Controls.Add(this.lbSapPrice);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TestWebApi);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gpSapBapi.ResumeLayout(false);
            this.gpHana.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button TestWebApi;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Label lbSapPrice;
        private System.Windows.Forms.Label lbCount;
        private System.Windows.Forms.Button btnPostSapPrice;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox gpSapBapi;
        private System.Windows.Forms.Button btnOrder;
        private System.Windows.Forms.GroupBox gpHana;
        private System.Windows.Forms.Button btnGetBpEmail;
        private System.Windows.Forms.Button btnBpAddress;
        private System.Windows.Forms.Button btnAddressAll;
        private System.Windows.Forms.Button btnSales;
        private System.Windows.Forms.Button btnMoqOrder;
        private System.Windows.Forms.Button btnMOQ_MARC;
        private System.Windows.Forms.Button btnPriceGroup;
        private System.Windows.Forms.Button btnBpXY;
        private System.Windows.Forms.Button btnWaterMark;
        private System.Windows.Forms.Button btnEdiTest;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtANSIX12;
        private System.Windows.Forms.Button btnArea;
        private System.Windows.Forms.Button btnCountry;
        private System.Windows.Forms.Button btnRSSAPI;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnLogin;
    }
}

