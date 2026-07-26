#nullable disable

namespace StarFoxZeroLocalizationTool
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            mainPanel = new Panel();
            titleLabel = new Label();
            subtitleLabel = new Label();
            versionValueLabel = new Label();
            copyrightLabel = new Label();
            detailsLabel = new Label();
            githubTitleLabel = new Label();
            projectLinkLabel = new LinkLabel();
            okButton = new Button();
            mainPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.FromArgb(9, 11, 32);
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(subtitleLabel);
            mainPanel.Controls.Add(versionValueLabel);
            mainPanel.Controls.Add(copyrightLabel);
            mainPanel.Controls.Add(detailsLabel);
            mainPanel.Controls.Add(githubTitleLabel);
            mainPanel.Controls.Add(projectLinkLabel);
            mainPanel.Controls.Add(okButton);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(24);
            mainPanel.Size = new Size(560, 270);
            mainPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Location = new Point(24, 20);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(358, 32);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "StarFox Zero Localization Tool";
            // 
            // subtitleLabel
            // 
            subtitleLabel.ForeColor = Color.Gainsboro;
            subtitleLabel.Location = new Point(26, 63);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(500, 40);
            subtitleLabel.TabIndex = 1;
            subtitleLabel.Text = "Ferramenta versátil para editar arquivos MCD, extrair e empacotar DAT, revisar strings e gerenciar todo o fluxo de localização do StarFox Zero.";
            // 
            // versionValueLabel
            // 
            versionValueLabel.AutoSize = true;
            versionValueLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            versionValueLabel.ForeColor = Color.FromArgb(34, 197, 94);
            versionValueLabel.Location = new Point(26, 119);
            versionValueLabel.Name = "versionValueLabel";
            versionValueLabel.Size = new Size(82, 19);
            versionValueLabel.TabIndex = 2;
            versionValueLabel.Text = "Versao: 1.0";
            // 
            // copyrightLabel
            // 
            copyrightLabel.AutoSize = true;
            copyrightLabel.ForeColor = Color.WhiteSmoke;
            copyrightLabel.Location = new Point(26, 151);
            copyrightLabel.Name = "copyrightLabel";
            copyrightLabel.Size = new Size(263, 15);
            copyrightLabel.TabIndex = 3;
            copyrightLabel.Text = "2026 - Powered by JuniorGBJ - All rights reserved";
            // 
            // detailsLabel
            // 
            detailsLabel.ForeColor = Color.Gainsboro;
            detailsLabel.Location = new Point(26, 181);
            detailsLabel.Name = "detailsLabel";
            detailsLabel.Size = new Size(376, 36);
            detailsLabel.TabIndex = 4;
            detailsLabel.Text = "Desenvolvido para facilitar a edicao de fontes, textos e assets relacionados a localizacao do projeto.";
            // 
            // githubTitleLabel
            // 
            githubTitleLabel.AutoSize = true;
            githubTitleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            githubTitleLabel.ForeColor = Color.White;
            githubTitleLabel.Location = new Point(26, 221);
            githubTitleLabel.Name = "githubTitleLabel";
            githubTitleLabel.Size = new Size(50, 15);
            githubTitleLabel.TabIndex = 5;
            githubTitleLabel.Text = "GitHub:";
            // 
            // projectLinkLabel
            // 
            projectLinkLabel.ActiveLinkColor = Color.FromArgb(56, 189, 248);
            projectLinkLabel.AutoEllipsis = true;
            projectLinkLabel.LinkBehavior = LinkBehavior.HoverUnderline;
            projectLinkLabel.LinkColor = Color.FromArgb(0, 122, 204);
            projectLinkLabel.Location = new Point(82, 221);
            projectLinkLabel.Name = "projectLinkLabel";
            projectLinkLabel.Size = new Size(345, 15);
            projectLinkLabel.TabIndex = 6;
            projectLinkLabel.TabStop = true;
            projectLinkLabel.Text = "https://github.com/JUNIORGBJ/StarFox_Zero_Localization-Tool";
            projectLinkLabel.VisitedLinkColor = Color.FromArgb(0, 122, 204);
            projectLinkLabel.LinkClicked += ProjectLinkLabel_LinkClicked;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.BackColor = Color.FromArgb(0, 122, 204);
            okButton.Cursor = Cursors.Hand;
            okButton.FlatAppearance.BorderColor = Color.FromArgb(56, 189, 248);
            okButton.FlatStyle = FlatStyle.Flat;
            okButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            okButton.ForeColor = Color.White;
            okButton.Location = new Point(441, 218);
            okButton.Name = "okButton";
            okButton.Size = new Size(95, 34);
            okButton.TabIndex = 7;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = false;
            okButton.Click += OkButton_Click;
            // 
            // AboutForm
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(560, 270);
            Controls.Add(mainPanel);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sobre";
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private Label versionValueLabel;
        private Label copyrightLabel;
        private Label detailsLabel;
        private Label githubTitleLabel;
        private LinkLabel projectLinkLabel;
        private Button okButton;
    }
}
