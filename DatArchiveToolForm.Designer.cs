#nullable disable

namespace StarFoxZeroLocalizationTool
{
    partial class DatArchiveToolForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatArchiveToolForm));
            mainPanel = new Panel();
            lblExtractTitle = new Label();
            lblExtractSub = new Label();
            btnExtract = new Button();
            lblRepackTitle = new Label();
            lblRepackSub = new Label();
            btnRepack = new Button();
            lblLogTitle = new Label();
            txtLog = new RichTextBox();
            mainPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.FromArgb(37, 37, 38);
            mainPanel.Controls.Add(lblExtractTitle);
            mainPanel.Controls.Add(lblExtractSub);
            mainPanel.Controls.Add(btnExtract);
            mainPanel.Controls.Add(lblRepackTitle);
            mainPanel.Controls.Add(lblRepackSub);
            mainPanel.Controls.Add(btnRepack);
            mainPanel.Location = new Point(15, 15);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(605, 310);
            mainPanel.TabIndex = 0;
            // 
            // lblExtractTitle
            // 
            lblExtractTitle.AutoSize = true;
            lblExtractTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblExtractTitle.ForeColor = Color.FromArgb(0, 122, 204);
            lblExtractTitle.Location = new Point(15, 15);
            lblExtractTitle.Name = "lblExtractTitle";
            lblExtractTitle.Size = new Size(196, 21);
            lblExtractTitle.TabIndex = 0;
            lblExtractTitle.Text = "Extract DAT/DTT Archive";
            // 
            // lblExtractSub
            // 
            lblExtractSub.Font = new Font("Segoe UI", 8.5F);
            lblExtractSub.ForeColor = Color.FromArgb(133, 133, 133);
            lblExtractSub.Location = new Point(15, 42);
            lblExtractSub.Name = "lblExtractSub";
            lblExtractSub.Size = new Size(575, 20);
            lblExtractSub.TabIndex = 1;
            lblExtractSub.Text = "Select a .dat, .dtt, .eff, or .evn file to extract its contents.";
            // 
            // btnExtract
            // 
            btnExtract.BackColor = Color.FromArgb(0, 122, 204);
            btnExtract.Cursor = Cursors.Hand;
            btnExtract.FlatAppearance.BorderSize = 0;
            btnExtract.FlatStyle = FlatStyle.Flat;
            btnExtract.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnExtract.ForeColor = Color.White;
            btnExtract.Location = new Point(15, 68);
            btnExtract.Name = "btnExtract";
            btnExtract.Size = new Size(575, 38);
            btnExtract.TabIndex = 2;
            btnExtract.Text = "Select & Extract DAT";
            btnExtract.UseVisualStyleBackColor = false;
            btnExtract.Click += BtnExtract_Click;
            // 
            // lblRepackTitle
            // 
            lblRepackTitle.AutoSize = true;
            lblRepackTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblRepackTitle.ForeColor = Color.FromArgb(0, 122, 204);
            lblRepackTitle.Location = new Point(15, 130);
            lblRepackTitle.Name = "lblRepackTitle";
            lblRepackTitle.Size = new Size(226, 21);
            lblRepackTitle.TabIndex = 3;
            lblRepackTitle.Text = "Repackage DAT/DTT Archive";
            // 
            // lblRepackSub
            // 
            lblRepackSub.Font = new Font("Segoe UI", 8.5F);
            lblRepackSub.ForeColor = Color.FromArgb(133, 133, 133);
            lblRepackSub.Location = new Point(15, 157);
            lblRepackSub.Name = "lblRepackSub";
            lblRepackSub.Size = new Size(575, 20);
            lblRepackSub.TabIndex = 4;
            lblRepackSub.Text = "Select an extracted directory (e.g. *_dat) to repackage it back.";
            // 
            // btnRepack
            // 
            btnRepack.BackColor = Color.FromArgb(45, 45, 48);
            btnRepack.Cursor = Cursors.Hand;
            btnRepack.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
            btnRepack.FlatStyle = FlatStyle.Flat;
            btnRepack.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRepack.ForeColor = Color.FromArgb(212, 212, 212);
            btnRepack.Location = new Point(15, 183);
            btnRepack.Name = "btnRepack";
            btnRepack.Size = new Size(575, 38);
            btnRepack.TabIndex = 5;
            btnRepack.Text = "Select & Repack Folder";
            btnRepack.UseVisualStyleBackColor = false;
            btnRepack.Click += BtnRepack_Click;
            // 
            // lblLogTitle
            // 
            lblLogTitle.AutoSize = true;
            lblLogTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblLogTitle.ForeColor = Color.FromArgb(212, 212, 212);
            lblLogTitle.Location = new Point(15, 340);
            lblLogTitle.Name = "lblLogTitle";
            lblLogTitle.Size = new Size(132, 15);
            lblLogTitle.TabIndex = 1;
            lblLogTitle.Text = "Operation Log Output:";
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(30, 30, 30);
            txtLog.BorderStyle = BorderStyle.FixedSingle;
            txtLog.Font = new Font("Consolas", 8.5F);
            txtLog.ForeColor = Color.FromArgb(0, 255, 0);
            txtLog.Location = new Point(15, 362);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new Size(605, 160);
            txtLog.TabIndex = 2;
            txtLog.Text = "";
            // 
            // DatArchiveToolForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(634, 541);
            Controls.Add(txtLog);
            Controls.Add(lblLogTitle);
            Controls.Add(mainPanel);
            Font = new Font("Segoe UI", 9F);
            ForeColor = Color.FromArgb(212, 212, 212);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimumSize = new Size(650, 580);
            Name = "DatArchiveToolForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Extrair e Reempacotar DAT/DTT";
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label lblExtractTitle;
        private System.Windows.Forms.Label lblExtractSub;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Label lblRepackTitle;
        private System.Windows.Forms.Label lblRepackSub;
        private System.Windows.Forms.Button btnRepack;
        private System.Windows.Forms.Label lblLogTitle;
        private System.Windows.Forms.RichTextBox txtLog;
    }
}
