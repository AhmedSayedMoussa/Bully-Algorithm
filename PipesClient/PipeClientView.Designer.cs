namespace PipeClient
{
    partial class PipeClientView
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
            this.Lbl_processNumber = new System.Windows.Forms.Label();
            this.Lbl_proccessIdKey = new System.Windows.Forms.Label();
            this.Btn_close = new System.Windows.Forms.Button();
            this.lstBox_messagesLogs = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Lbl_processNumber
            // 
            this.Lbl_processNumber.AutoSize = true;
            this.Lbl_processNumber.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Lbl_processNumber.Location = new System.Drawing.Point(146, 14);
            this.Lbl_processNumber.Name = "Lbl_processNumber";
            this.Lbl_processNumber.Size = new System.Drawing.Size(0, 15);
            this.Lbl_processNumber.TabIndex = 2;
            // 
            // Lbl_proccessIdKey
            // 
            this.Lbl_proccessIdKey.AutoSize = true;
            this.Lbl_proccessIdKey.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Lbl_proccessIdKey.Location = new System.Drawing.Point(38, 14);
            this.Lbl_proccessIdKey.Name = "Lbl_proccessIdKey";
            this.Lbl_proccessIdKey.Size = new System.Drawing.Size(101, 15);
            this.Lbl_proccessIdKey.TabIndex = 3;
            this.Lbl_proccessIdKey.Text = "Process Number:";
            // 
            // Btn_close
            // 
            this.Btn_close.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Btn_close.ForeColor = System.Drawing.Color.Red;
            this.Btn_close.Location = new System.Drawing.Point(38, 491);
            this.Btn_close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Btn_close.Name = "Btn_close";
            this.Btn_close.Size = new System.Drawing.Size(127, 30);
            this.Btn_close.TabIndex = 4;
            this.Btn_close.Text = "Close";
            this.Btn_close.UseVisualStyleBackColor = true;
            this.Btn_close.Click += new System.EventHandler(this.Btn_close_Click);
            // 
            // lstBox_messagesLogs
            // 
            this.lstBox_messagesLogs.BackColor = System.Drawing.Color.Black;
            this.lstBox_messagesLogs.ForeColor = System.Drawing.Color.White;
            this.lstBox_messagesLogs.FormattingEnabled = true;
            this.lstBox_messagesLogs.ItemHeight = 15;
            this.lstBox_messagesLogs.Location = new System.Drawing.Point(38, 53);
            this.lstBox_messagesLogs.Name = "lstBox_messagesLogs";
            this.lstBox_messagesLogs.ScrollAlwaysVisible = true;
            this.lstBox_messagesLogs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstBox_messagesLogs.Size = new System.Drawing.Size(932, 424);
            this.lstBox_messagesLogs.TabIndex = 6;
            // 
            // PipeClientView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 532);
            this.Controls.Add(this.lstBox_messagesLogs);
            this.Controls.Add(this.Btn_close);
            this.Controls.Add(this.Lbl_proccessIdKey);
            this.Controls.Add(this.Lbl_processNumber);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PipeClientView";
            this.Text = "PipeClient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PipeClientView_FormClosing);
            this.Load += new System.EventHandler(this.PipeClientView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label Lbl_processNumber;
        private Label Lbl_proccessIdKey;
        private Button Btn_close;
        private ListBox lstBox_messagesLogs;
    }
}