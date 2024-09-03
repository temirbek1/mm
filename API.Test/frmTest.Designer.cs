namespace API.Test
{
    partial class frmTest
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnBalanceByNumber = new System.Windows.Forms.Button();
            this.btnCancelOperation = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.button6);
            this.splitContainer1.Panel1.Controls.Add(this.button4);
            this.splitContainer1.Panel1.Controls.Add(this.btnBalanceByNumber);
            this.splitContainer1.Panel1.Controls.Add(this.btnCancelOperation);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(266, 28);
            this.button1.TabIndex = 7;
            this.button1.Text = "Изменение заказа";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button6
            // 
            this.button6.Dock = System.Windows.Forms.DockStyle.Top;
            this.button6.Location = new System.Drawing.Point(0, 84);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(266, 28);
            this.button6.TabIndex = 5;
            this.button6.Text = "Удаление заказа";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Top;
            this.button4.Location = new System.Drawing.Point(0, 56);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(266, 28);
            this.button4.TabIndex = 3;
            this.button4.Text = "Создание заказа";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnBalanceByNumber
            // 
            this.btnBalanceByNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBalanceByNumber.Location = new System.Drawing.Point(0, 28);
            this.btnBalanceByNumber.Name = "btnBalanceByNumber";
            this.btnBalanceByNumber.Size = new System.Drawing.Size(266, 28);
            this.btnBalanceByNumber.TabIndex = 0;
            this.btnBalanceByNumber.Text = "Получение информации по клиенту ID.";
            this.btnBalanceByNumber.UseVisualStyleBackColor = true;
            this.btnBalanceByNumber.Click += new System.EventHandler(this.btnBalanceByNumber_Click);
            // 
            // btnCancelOperation
            // 
            this.btnCancelOperation.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCancelOperation.Location = new System.Drawing.Point(0, 0);
            this.btnCancelOperation.Name = "btnCancelOperation";
            this.btnCancelOperation.Size = new System.Drawing.Size(266, 28);
            this.btnCancelOperation.TabIndex = 6;
            this.btnCancelOperation.Text = "Авторизация";
            this.btnCancelOperation.UseVisualStyleBackColor = true;
            this.btnCancelOperation.Click += new System.EventHandler(this.btnCancelOperation_Click);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(530, 450);
            this.textBox1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Location = new System.Drawing.Point(0, 140);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(266, 28);
            this.button2.TabIndex = 9;
            this.button2.Text = "Получение информации по клиенту PHONE";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmTest";
            this.Text = "frmTest";
            this.Load += new System.EventHandler(this.frmTest_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancelOperation;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnBalanceByNumber;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}