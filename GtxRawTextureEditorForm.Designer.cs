#nullable disable

namespace StarFoxZeroLocalizationTool
{
    partial class GtxRawTextureEditorForm
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
            rootLayout = new TableLayoutPanel();
            headerPanel = new Panel();
            subtitleLabel = new Label();
            titleLabel = new Label();
            sourceGroupBox = new GroupBox();
            sourceLayout = new TableLayoutPanel();
            toolPathLabel = new Label();
            toolPathTextBox = new TextBox();
            detectToolButton = new Button();
            browseToolButton = new Button();
            gtxPathLabel = new Label();
            gtxPathTextBox = new TextBox();
            browseGtxButton = new Button();
            openGtxButton = new Button();
            saveGroupBox = new GroupBox();
            saveLayout = new TableLayoutPanel();
            outputPathLabel = new Label();
            outputPathTextBox = new TextBox();
            browseOutputButton = new Button();
            saveGtxButton = new Button();
            infoLabel = new Label();
            previewGroupBox = new GroupBox();
            previewLayout = new TableLayoutPanel();
            compositeGroupBox = new GroupBox();
            compositePictureBox = new PictureBox();
            channelRGroupBox = new GroupBox();
            channelRLayout = new TableLayoutPanel();
            channelRPictureBox = new PictureBox();
            channelRButtonsPanel = new FlowLayoutPanel();
            exportChannelRButton = new Button();
            importChannelRButton = new Button();
            channelGGroupBox = new GroupBox();
            channelGLayout = new TableLayoutPanel();
            channelGPictureBox = new PictureBox();
            channelGButtonsPanel = new FlowLayoutPanel();
            exportChannelGButton = new Button();
            importChannelGButton = new Button();
            statusGroupBox = new GroupBox();
            statusTextBox = new TextBox();
            rootLayout.SuspendLayout();
            headerPanel.SuspendLayout();
            sourceGroupBox.SuspendLayout();
            sourceLayout.SuspendLayout();
            saveGroupBox.SuspendLayout();
            saveLayout.SuspendLayout();
            previewGroupBox.SuspendLayout();
            previewLayout.SuspendLayout();
            compositeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)compositePictureBox).BeginInit();
            channelRGroupBox.SuspendLayout();
            channelRLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)channelRPictureBox).BeginInit();
            channelRButtonsPanel.SuspendLayout();
            channelGGroupBox.SuspendLayout();
            channelGLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)channelGPictureBox).BeginInit();
            channelGButtonsPanel.SuspendLayout();
            statusGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.BackColor = Color.FromArgb(245, 247, 250);
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(headerPanel, 0, 0);
            rootLayout.Controls.Add(sourceGroupBox, 0, 1);
            rootLayout.Controls.Add(saveGroupBox, 0, 2);
            rootLayout.Controls.Add(previewGroupBox, 0, 3);
            rootLayout.Controls.Add(statusGroupBox, 0, 4);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(14, 12, 14, 12);
            rootLayout.RowCount = 5;
            rootLayout.RowStyles.Add(new RowStyle());
            rootLayout.RowStyles.Add(new RowStyle());
            rootLayout.RowStyles.Add(new RowStyle());
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
            rootLayout.Size = new Size(1184, 861);
            rootLayout.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.White;
            headerPanel.Controls.Add(subtitleLabel);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.Location = new Point(17, 15);
            headerPanel.Margin = new Padding(3, 3, 3, 10);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(18, 14, 18, 14);
            headerPanel.Size = new Size(1150, 82);
            headerPanel.TabIndex = 0;
            // 
            // subtitleLabel
            // 
            subtitleLabel.AutoSize = true;
            subtitleLabel.Font = new Font("Segoe UI", 10F);
            subtitleLabel.ForeColor = Color.FromArgb(71, 85, 105);
            subtitleLabel.Location = new Point(21, 46);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(746, 19);
            subtitleLabel.TabIndex = 1;
            subtitleLabel.Text = "Editor visual para texturas GTX brutas de fonte/UI. Trabalha com os dois canais da textura preservando o GTX base.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(37, 99, 235);
            titleLabel.Location = new Point(19, 12);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(231, 32);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Editor Bruto de GTX";
            // 
            // sourceGroupBox
            // 
            sourceGroupBox.Controls.Add(sourceLayout);
            sourceGroupBox.Dock = DockStyle.Fill;
            sourceGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            sourceGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            sourceGroupBox.Location = new Point(17, 110);
            sourceGroupBox.Name = "sourceGroupBox";
            sourceGroupBox.Padding = new Padding(12, 10, 12, 10);
            sourceGroupBox.Size = new Size(1150, 117);
            sourceGroupBox.TabIndex = 1;
            sourceGroupBox.TabStop = false;
            sourceGroupBox.Text = "Origem";
            // 
            // sourceLayout
            // 
            sourceLayout.ColumnCount = 4;
            sourceLayout.ColumnStyles.Add(new ColumnStyle());
            sourceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            sourceLayout.ColumnStyles.Add(new ColumnStyle());
            sourceLayout.ColumnStyles.Add(new ColumnStyle());
            sourceLayout.Controls.Add(toolPathLabel, 0, 0);
            sourceLayout.Controls.Add(toolPathTextBox, 1, 0);
            sourceLayout.Controls.Add(detectToolButton, 2, 0);
            sourceLayout.Controls.Add(browseToolButton, 3, 0);
            sourceLayout.Controls.Add(gtxPathLabel, 0, 1);
            sourceLayout.Controls.Add(gtxPathTextBox, 1, 1);
            sourceLayout.Controls.Add(browseGtxButton, 2, 1);
            sourceLayout.Controls.Add(openGtxButton, 3, 1);
            sourceLayout.Dock = DockStyle.Fill;
            sourceLayout.Location = new Point(12, 28);
            sourceLayout.Name = "sourceLayout";
            sourceLayout.RowCount = 2;
            sourceLayout.RowStyles.Add(new RowStyle());
            sourceLayout.RowStyles.Add(new RowStyle());
            sourceLayout.Size = new Size(1126, 79);
            sourceLayout.TabIndex = 0;
            // 
            // toolPathLabel
            // 
            toolPathLabel.Anchor = AnchorStyles.Left;
            toolPathLabel.AutoSize = true;
            toolPathLabel.Font = new Font("Segoe UI", 9.5F);
            toolPathLabel.Location = new Point(0, 7);
            toolPathLabel.Margin = new Padding(0, 0, 10, 0);
            toolPathLabel.Name = "toolPathLabel";
            toolPathLabel.Size = new Size(124, 17);
            toolPathLabel.TabIndex = 0;
            toolPathLabel.Text = "Caminho do extrator";
            // 
            // toolPathTextBox
            // 
            toolPathTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            toolPathTextBox.BorderStyle = BorderStyle.FixedSingle;
            toolPathTextBox.Font = new Font("Segoe UI", 10F);
            toolPathTextBox.Location = new Point(134, 3);
            toolPathTextBox.Margin = new Padding(0, 3, 12, 3);
            toolPathTextBox.Name = "toolPathTextBox";
            toolPathTextBox.Size = new Size(781, 25);
            toolPathTextBox.TabIndex = 1;
            // 
            // detectToolButton
            // 
            detectToolButton.BackColor = Color.FromArgb(15, 23, 42);
            detectToolButton.Cursor = Cursors.Hand;
            detectToolButton.FlatAppearance.BorderSize = 0;
            detectToolButton.FlatStyle = FlatStyle.Flat;
            detectToolButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            detectToolButton.ForeColor = Color.White;
            detectToolButton.Location = new Point(927, 1);
            detectToolButton.Margin = new Padding(0, 1, 8, 1);
            detectToolButton.Name = "detectToolButton";
            detectToolButton.Size = new Size(110, 29);
            detectToolButton.TabIndex = 2;
            detectToolButton.Text = "Detectar";
            detectToolButton.UseVisualStyleBackColor = false;
            detectToolButton.Click += DetectToolButton_Click;
            // 
            // browseToolButton
            // 
            browseToolButton.BackColor = Color.White;
            browseToolButton.Cursor = Cursors.Hand;
            browseToolButton.FlatStyle = FlatStyle.Flat;
            browseToolButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            browseToolButton.ForeColor = Color.FromArgb(15, 23, 42);
            browseToolButton.Location = new Point(1045, 1);
            browseToolButton.Margin = new Padding(0, 1, 0, 1);
            browseToolButton.Name = "browseToolButton";
            browseToolButton.Size = new Size(81, 29);
            browseToolButton.TabIndex = 3;
            browseToolButton.Text = "Procurar...";
            browseToolButton.UseVisualStyleBackColor = false;
            browseToolButton.Click += BrowseToolButton_Click;
            // 
            // gtxPathLabel
            // 
            gtxPathLabel.Anchor = AnchorStyles.Left;
            gtxPathLabel.AutoSize = true;
            gtxPathLabel.Font = new Font("Segoe UI", 9.5F);
            gtxPathLabel.Location = new Point(0, 46);
            gtxPathLabel.Margin = new Padding(0, 0, 10, 0);
            gtxPathLabel.Name = "gtxPathLabel";
            gtxPathLabel.Size = new Size(55, 17);
            gtxPathLabel.TabIndex = 4;
            gtxPathLabel.Text = "GTX base";
            // 
            // gtxPathTextBox
            // 
            gtxPathTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            gtxPathTextBox.BorderStyle = BorderStyle.FixedSingle;
            gtxPathTextBox.Font = new Font("Segoe UI", 10F);
            gtxPathTextBox.Location = new Point(134, 42);
            gtxPathTextBox.Margin = new Padding(0, 3, 12, 3);
            gtxPathTextBox.Name = "gtxPathTextBox";
            gtxPathTextBox.Size = new Size(781, 25);
            gtxPathTextBox.TabIndex = 5;
            // 
            // browseGtxButton
            // 
            browseGtxButton.BackColor = Color.White;
            browseGtxButton.Cursor = Cursors.Hand;
            browseGtxButton.FlatStyle = FlatStyle.Flat;
            browseGtxButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            browseGtxButton.ForeColor = Color.FromArgb(15, 23, 42);
            browseGtxButton.Location = new Point(927, 40);
            browseGtxButton.Margin = new Padding(0, 1, 8, 1);
            browseGtxButton.Name = "browseGtxButton";
            browseGtxButton.Size = new Size(110, 29);
            browseGtxButton.TabIndex = 6;
            browseGtxButton.Text = "Procurar...";
            browseGtxButton.UseVisualStyleBackColor = false;
            browseGtxButton.Click += BrowseGtxButton_Click;
            // 
            // openGtxButton
            // 
            openGtxButton.BackColor = Color.FromArgb(37, 99, 235);
            openGtxButton.Cursor = Cursors.Hand;
            openGtxButton.FlatAppearance.BorderSize = 0;
            openGtxButton.FlatStyle = FlatStyle.Flat;
            openGtxButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            openGtxButton.ForeColor = Color.White;
            openGtxButton.Location = new Point(1045, 40);
            openGtxButton.Margin = new Padding(0, 1, 0, 1);
            openGtxButton.Name = "openGtxButton";
            openGtxButton.Size = new Size(81, 29);
            openGtxButton.TabIndex = 7;
            openGtxButton.Text = "Abrir GTX";
            openGtxButton.UseVisualStyleBackColor = false;
            openGtxButton.Click += OpenGtxButton_Click;
            // 
            // saveGroupBox
            // 
            saveGroupBox.Controls.Add(saveLayout);
            saveGroupBox.Dock = DockStyle.Fill;
            saveGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            saveGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            saveGroupBox.Location = new Point(17, 240);
            saveGroupBox.Name = "saveGroupBox";
            saveGroupBox.Padding = new Padding(12, 10, 12, 10);
            saveGroupBox.Size = new Size(1150, 84);
            saveGroupBox.TabIndex = 2;
            saveGroupBox.TabStop = false;
            saveGroupBox.Text = "Salvar";
            // 
            // saveLayout
            // 
            saveLayout.ColumnCount = 4;
            saveLayout.ColumnStyles.Add(new ColumnStyle());
            saveLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            saveLayout.ColumnStyles.Add(new ColumnStyle());
            saveLayout.ColumnStyles.Add(new ColumnStyle());
            saveLayout.Controls.Add(outputPathLabel, 0, 0);
            saveLayout.Controls.Add(outputPathTextBox, 1, 0);
            saveLayout.Controls.Add(browseOutputButton, 2, 0);
            saveLayout.Controls.Add(saveGtxButton, 3, 0);
            saveLayout.Controls.Add(infoLabel, 0, 1);
            saveLayout.Dock = DockStyle.Fill;
            saveLayout.Location = new Point(12, 28);
            saveLayout.Name = "saveLayout";
            saveLayout.RowCount = 2;
            saveLayout.RowStyles.Add(new RowStyle());
            saveLayout.RowStyles.Add(new RowStyle());
            saveLayout.Size = new Size(1126, 46);
            saveLayout.TabIndex = 0;
            // 
            // outputPathLabel
            // 
            outputPathLabel.Anchor = AnchorStyles.Left;
            outputPathLabel.AutoSize = true;
            outputPathLabel.Font = new Font("Segoe UI", 9.5F);
            outputPathLabel.Location = new Point(0, 7);
            outputPathLabel.Margin = new Padding(0, 0, 10, 0);
            outputPathLabel.Name = "outputPathLabel";
            outputPathLabel.Size = new Size(69, 17);
            outputPathLabel.TabIndex = 0;
            outputPathLabel.Text = "GTX de saida";
            // 
            // outputPathTextBox
            // 
            outputPathTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            outputPathTextBox.BorderStyle = BorderStyle.FixedSingle;
            outputPathTextBox.Font = new Font("Segoe UI", 10F);
            outputPathTextBox.Location = new Point(79, 3);
            outputPathTextBox.Margin = new Padding(0, 3, 12, 3);
            outputPathTextBox.Name = "outputPathTextBox";
            outputPathTextBox.Size = new Size(836, 25);
            outputPathTextBox.TabIndex = 1;
            // 
            // browseOutputButton
            // 
            browseOutputButton.BackColor = Color.White;
            browseOutputButton.Cursor = Cursors.Hand;
            browseOutputButton.FlatStyle = FlatStyle.Flat;
            browseOutputButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            browseOutputButton.ForeColor = Color.FromArgb(15, 23, 42);
            browseOutputButton.Location = new Point(927, 1);
            browseOutputButton.Margin = new Padding(0, 1, 8, 1);
            browseOutputButton.Name = "browseOutputButton";
            browseOutputButton.Size = new Size(110, 29);
            browseOutputButton.TabIndex = 2;
            browseOutputButton.Text = "Procurar...";
            browseOutputButton.UseVisualStyleBackColor = false;
            browseOutputButton.Click += BrowseOutputButton_Click;
            // 
            // saveGtxButton
            // 
            saveGtxButton.BackColor = Color.FromArgb(22, 163, 74);
            saveGtxButton.Cursor = Cursors.Hand;
            saveGtxButton.Enabled = false;
            saveGtxButton.FlatAppearance.BorderSize = 0;
            saveGtxButton.FlatStyle = FlatStyle.Flat;
            saveGtxButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            saveGtxButton.ForeColor = Color.White;
            saveGtxButton.Location = new Point(1045, 1);
            saveGtxButton.Margin = new Padding(0, 1, 0, 1);
            saveGtxButton.Name = "saveGtxButton";
            saveGtxButton.Size = new Size(81, 29);
            saveGtxButton.TabIndex = 3;
            saveGtxButton.Text = "Salvar";
            saveGtxButton.UseVisualStyleBackColor = false;
            saveGtxButton.Click += SaveGtxButton_Click;
            // 
            // infoLabel
            // 
            infoLabel.AutoSize = true;
            saveLayout.SetColumnSpan(infoLabel, 4);
            infoLabel.Font = new Font("Segoe UI", 9F);
            infoLabel.ForeColor = Color.FromArgb(71, 85, 105);
            infoLabel.Location = new Point(0, 31);
            infoLabel.Margin = new Padding(0, 0, 0, 0);
            infoLabel.Name = "infoLabel";
            infoLabel.Size = new Size(650, 15);
            infoLabel.TabIndex = 4;
            infoLabel.Text = "Esta tela e focada em GTX de fonte/UI no formato R8_G8_UNORM. Importe e exporte os dois canais separadamente.";
            // 
            // previewGroupBox
            // 
            previewGroupBox.Controls.Add(previewLayout);
            previewGroupBox.Dock = DockStyle.Fill;
            previewGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            previewGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            previewGroupBox.Location = new Point(17, 337);
            previewGroupBox.Name = "previewGroupBox";
            previewGroupBox.Padding = new Padding(12, 10, 12, 10);
            previewGroupBox.Size = new Size(1150, 369);
            previewGroupBox.TabIndex = 3;
            previewGroupBox.TabStop = false;
            previewGroupBox.Text = "Preview e canais";
            // 
            // previewLayout
            // 
            previewLayout.ColumnCount = 3;
            previewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            previewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            previewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            previewLayout.Controls.Add(compositeGroupBox, 0, 0);
            previewLayout.Controls.Add(channelRGroupBox, 1, 0);
            previewLayout.Controls.Add(channelGGroupBox, 2, 0);
            previewLayout.Dock = DockStyle.Fill;
            previewLayout.Location = new Point(12, 28);
            previewLayout.Name = "previewLayout";
            previewLayout.RowCount = 1;
            previewLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            previewLayout.Size = new Size(1126, 331);
            previewLayout.TabIndex = 0;
            // 
            // compositeGroupBox
            // 
            compositeGroupBox.Controls.Add(compositePictureBox);
            compositeGroupBox.Dock = DockStyle.Fill;
            compositeGroupBox.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            compositeGroupBox.Location = new Point(0, 0);
            compositeGroupBox.Margin = new Padding(0, 0, 10, 0);
            compositeGroupBox.Name = "compositeGroupBox";
            compositeGroupBox.Padding = new Padding(10, 8, 10, 10);
            compositeGroupBox.Size = new Size(440, 331);
            compositeGroupBox.TabIndex = 0;
            compositeGroupBox.TabStop = false;
            compositeGroupBox.Text = "Visualizacao aproximada";
            // 
            // compositePictureBox
            // 
            compositePictureBox.BackColor = Color.Black;
            compositePictureBox.Dock = DockStyle.Fill;
            compositePictureBox.Location = new Point(10, 24);
            compositePictureBox.Name = "compositePictureBox";
            compositePictureBox.Size = new Size(420, 297);
            compositePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            compositePictureBox.TabIndex = 0;
            compositePictureBox.TabStop = false;
            // 
            // channelRGroupBox
            // 
            channelRGroupBox.Controls.Add(channelRLayout);
            channelRGroupBox.Dock = DockStyle.Fill;
            channelRGroupBox.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            channelRGroupBox.Location = new Point(450, 0);
            channelRGroupBox.Margin = new Padding(0, 0, 10, 0);
            channelRGroupBox.Name = "channelRGroupBox";
            channelRGroupBox.Padding = new Padding(10, 8, 10, 10);
            channelRGroupBox.Size = new Size(327, 331);
            channelRGroupBox.TabIndex = 1;
            channelRGroupBox.TabStop = false;
            channelRGroupBox.Text = "Canal 1";
            // 
            // channelRLayout
            // 
            channelRLayout.ColumnCount = 1;
            channelRLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            channelRLayout.Controls.Add(channelRPictureBox, 0, 0);
            channelRLayout.Controls.Add(channelRButtonsPanel, 0, 1);
            channelRLayout.Dock = DockStyle.Fill;
            channelRLayout.Location = new Point(10, 24);
            channelRLayout.Name = "channelRLayout";
            channelRLayout.RowCount = 2;
            channelRLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            channelRLayout.RowStyles.Add(new RowStyle());
            channelRLayout.Size = new Size(307, 297);
            channelRLayout.TabIndex = 0;
            // 
            // channelRPictureBox
            // 
            channelRPictureBox.BackColor = Color.Black;
            channelRPictureBox.Dock = DockStyle.Fill;
            channelRPictureBox.Location = new Point(0, 0);
            channelRPictureBox.Margin = new Padding(0, 0, 0, 8);
            channelRPictureBox.Name = "channelRPictureBox";
            channelRPictureBox.Size = new Size(307, 259);
            channelRPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            channelRPictureBox.TabIndex = 0;
            channelRPictureBox.TabStop = false;
            // 
            // channelRButtonsPanel
            // 
            channelRButtonsPanel.AutoSize = true;
            channelRButtonsPanel.Controls.Add(exportChannelRButton);
            channelRButtonsPanel.Controls.Add(importChannelRButton);
            channelRButtonsPanel.Dock = DockStyle.Fill;
            channelRButtonsPanel.Location = new Point(0, 267);
            channelRButtonsPanel.Margin = new Padding(0);
            channelRButtonsPanel.Name = "channelRButtonsPanel";
            channelRButtonsPanel.Size = new Size(307, 30);
            channelRButtonsPanel.TabIndex = 1;
            // 
            // exportChannelRButton
            // 
            exportChannelRButton.BackColor = Color.White;
            exportChannelRButton.Cursor = Cursors.Hand;
            exportChannelRButton.Enabled = false;
            exportChannelRButton.FlatStyle = FlatStyle.Flat;
            exportChannelRButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            exportChannelRButton.ForeColor = Color.FromArgb(15, 23, 42);
            exportChannelRButton.Location = new Point(0, 0);
            exportChannelRButton.Margin = new Padding(0, 0, 8, 0);
            exportChannelRButton.Name = "exportChannelRButton";
            exportChannelRButton.Size = new Size(140, 30);
            exportChannelRButton.TabIndex = 0;
            exportChannelRButton.Text = "Exportar Canal 1";
            exportChannelRButton.UseVisualStyleBackColor = false;
            exportChannelRButton.Click += ExportChannelRButton_Click;
            // 
            // importChannelRButton
            // 
            importChannelRButton.BackColor = Color.FromArgb(37, 99, 235);
            importChannelRButton.Cursor = Cursors.Hand;
            importChannelRButton.Enabled = false;
            importChannelRButton.FlatAppearance.BorderSize = 0;
            importChannelRButton.FlatStyle = FlatStyle.Flat;
            importChannelRButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            importChannelRButton.ForeColor = Color.White;
            importChannelRButton.Location = new Point(148, 0);
            importChannelRButton.Margin = new Padding(0);
            importChannelRButton.Name = "importChannelRButton";
            importChannelRButton.Size = new Size(140, 30);
            importChannelRButton.TabIndex = 1;
            importChannelRButton.Text = "Importar Canal 1";
            importChannelRButton.UseVisualStyleBackColor = false;
            importChannelRButton.Click += ImportChannelRButton_Click;
            // 
            // channelGGroupBox
            // 
            channelGGroupBox.Controls.Add(channelGLayout);
            channelGGroupBox.Dock = DockStyle.Fill;
            channelGGroupBox.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            channelGGroupBox.Location = new Point(787, 0);
            channelGGroupBox.Margin = new Padding(0);
            channelGGroupBox.Name = "channelGGroupBox";
            channelGGroupBox.Padding = new Padding(10, 8, 10, 10);
            channelGGroupBox.Size = new Size(339, 331);
            channelGGroupBox.TabIndex = 2;
            channelGGroupBox.TabStop = false;
            channelGGroupBox.Text = "Canal 2";
            // 
            // channelGLayout
            // 
            channelGLayout.ColumnCount = 1;
            channelGLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            channelGLayout.Controls.Add(channelGPictureBox, 0, 0);
            channelGLayout.Controls.Add(channelGButtonsPanel, 0, 1);
            channelGLayout.Dock = DockStyle.Fill;
            channelGLayout.Location = new Point(10, 24);
            channelGLayout.Name = "channelGLayout";
            channelGLayout.RowCount = 2;
            channelGLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            channelGLayout.RowStyles.Add(new RowStyle());
            channelGLayout.Size = new Size(319, 297);
            channelGLayout.TabIndex = 0;
            // 
            // channelGPictureBox
            // 
            channelGPictureBox.BackColor = Color.Black;
            channelGPictureBox.Dock = DockStyle.Fill;
            channelGPictureBox.Location = new Point(0, 0);
            channelGPictureBox.Margin = new Padding(0, 0, 0, 8);
            channelGPictureBox.Name = "channelGPictureBox";
            channelGPictureBox.Size = new Size(319, 259);
            channelGPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            channelGPictureBox.TabIndex = 0;
            channelGPictureBox.TabStop = false;
            // 
            // channelGButtonsPanel
            // 
            channelGButtonsPanel.AutoSize = true;
            channelGButtonsPanel.Controls.Add(exportChannelGButton);
            channelGButtonsPanel.Controls.Add(importChannelGButton);
            channelGButtonsPanel.Dock = DockStyle.Fill;
            channelGButtonsPanel.Location = new Point(0, 267);
            channelGButtonsPanel.Margin = new Padding(0);
            channelGButtonsPanel.Name = "channelGButtonsPanel";
            channelGButtonsPanel.Size = new Size(319, 30);
            channelGButtonsPanel.TabIndex = 1;
            // 
            // exportChannelGButton
            // 
            exportChannelGButton.BackColor = Color.White;
            exportChannelGButton.Cursor = Cursors.Hand;
            exportChannelGButton.Enabled = false;
            exportChannelGButton.FlatStyle = FlatStyle.Flat;
            exportChannelGButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            exportChannelGButton.ForeColor = Color.FromArgb(15, 23, 42);
            exportChannelGButton.Location = new Point(0, 0);
            exportChannelGButton.Margin = new Padding(0, 0, 8, 0);
            exportChannelGButton.Name = "exportChannelGButton";
            exportChannelGButton.Size = new Size(140, 30);
            exportChannelGButton.TabIndex = 0;
            exportChannelGButton.Text = "Exportar Canal 2";
            exportChannelGButton.UseVisualStyleBackColor = false;
            exportChannelGButton.Click += ExportChannelGButton_Click;
            // 
            // importChannelGButton
            // 
            importChannelGButton.BackColor = Color.FromArgb(37, 99, 235);
            importChannelGButton.Cursor = Cursors.Hand;
            importChannelGButton.Enabled = false;
            importChannelGButton.FlatAppearance.BorderSize = 0;
            importChannelGButton.FlatStyle = FlatStyle.Flat;
            importChannelGButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            importChannelGButton.ForeColor = Color.White;
            importChannelGButton.Location = new Point(148, 0);
            importChannelGButton.Margin = new Padding(0);
            importChannelGButton.Name = "importChannelGButton";
            importChannelGButton.Size = new Size(140, 30);
            importChannelGButton.TabIndex = 1;
            importChannelGButton.Text = "Importar Canal 2";
            importChannelGButton.UseVisualStyleBackColor = false;
            importChannelGButton.Click += ImportChannelGButton_Click;
            // 
            // statusGroupBox
            // 
            statusGroupBox.Controls.Add(statusTextBox);
            statusGroupBox.Dock = DockStyle.Fill;
            statusGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            statusGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            statusGroupBox.Location = new Point(17, 719);
            statusGroupBox.Name = "statusGroupBox";
            statusGroupBox.Padding = new Padding(12, 10, 12, 10);
            statusGroupBox.Size = new Size(1150, 127);
            statusGroupBox.TabIndex = 4;
            statusGroupBox.TabStop = false;
            statusGroupBox.Text = "Status";
            // 
            // statusTextBox
            // 
            statusTextBox.BackColor = Color.White;
            statusTextBox.BorderStyle = BorderStyle.FixedSingle;
            statusTextBox.Dock = DockStyle.Fill;
            statusTextBox.Font = new Font("Consolas", 10F);
            statusTextBox.Location = new Point(12, 28);
            statusTextBox.Multiline = true;
            statusTextBox.Name = "statusTextBox";
            statusTextBox.ReadOnly = true;
            statusTextBox.ScrollBars = ScrollBars.Vertical;
            statusTextBox.Size = new Size(1126, 89);
            statusTextBox.TabIndex = 0;
            statusTextBox.Text = "Abra um GTX base para iniciar.";
            // 
            // GtxRawTextureEditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1184, 861);
            Controls.Add(rootLayout);
            MinimumSize = new Size(1120, 820);
            Name = "GtxRawTextureEditorForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editor Bruto de GTX";
            rootLayout.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            sourceGroupBox.ResumeLayout(false);
            sourceLayout.ResumeLayout(false);
            sourceLayout.PerformLayout();
            saveGroupBox.ResumeLayout(false);
            saveLayout.ResumeLayout(false);
            saveLayout.PerformLayout();
            previewGroupBox.ResumeLayout(false);
            previewLayout.ResumeLayout(false);
            compositeGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)compositePictureBox).EndInit();
            channelRGroupBox.ResumeLayout(false);
            channelRLayout.ResumeLayout(false);
            channelRLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)channelRPictureBox).EndInit();
            channelRButtonsPanel.ResumeLayout(false);
            channelGGroupBox.ResumeLayout(false);
            channelGLayout.ResumeLayout(false);
            channelGLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)channelGPictureBox).EndInit();
            channelGButtonsPanel.ResumeLayout(false);
            statusGroupBox.ResumeLayout(false);
            statusGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private Panel headerPanel;
        private Label subtitleLabel;
        private Label titleLabel;
        private GroupBox sourceGroupBox;
        private TableLayoutPanel sourceLayout;
        private Label toolPathLabel;
        private TextBox toolPathTextBox;
        private Button detectToolButton;
        private Button browseToolButton;
        private Label gtxPathLabel;
        private TextBox gtxPathTextBox;
        private Button browseGtxButton;
        private Button openGtxButton;
        private GroupBox saveGroupBox;
        private TableLayoutPanel saveLayout;
        private Label outputPathLabel;
        private TextBox outputPathTextBox;
        private Button browseOutputButton;
        private Button saveGtxButton;
        private Label infoLabel;
        private GroupBox previewGroupBox;
        private TableLayoutPanel previewLayout;
        private GroupBox compositeGroupBox;
        private PictureBox compositePictureBox;
        private GroupBox channelRGroupBox;
        private TableLayoutPanel channelRLayout;
        private PictureBox channelRPictureBox;
        private FlowLayoutPanel channelRButtonsPanel;
        private Button exportChannelRButton;
        private Button importChannelRButton;
        private GroupBox channelGGroupBox;
        private TableLayoutPanel channelGLayout;
        private PictureBox channelGPictureBox;
        private FlowLayoutPanel channelGButtonsPanel;
        private Button exportChannelGButton;
        private Button importChannelGButton;
        private GroupBox statusGroupBox;
        private TextBox statusTextBox;
    }
}

#nullable restore
