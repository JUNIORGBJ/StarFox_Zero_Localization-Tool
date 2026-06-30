#nullable disable

namespace StarFoxZeroLocalizationTool
{
    partial class GtxDdsToolForm
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
            toolGroupBox = new GroupBox();
            toolLayout = new TableLayoutPanel();
            toolPathLabel = new Label();
            toolPathTextBox = new TextBox();
            browseToolButton = new Button();
            detectToolButton = new Button();
            conversionGroupBox = new GroupBox();
            conversionLayout = new TableLayoutPanel();
            modeLabel = new Label();
            modeComboBox = new ComboBox();
            profileLabel = new Label();
            profileComboBox = new ComboBox();
            originalGtxLabel = new Label();
            originalGtxTextBox = new TextBox();
            browseOriginalGtxButton = new Button();
            analyzeOriginalButton = new Button();
            inputFileLabel = new Label();
            inputFileTextBox = new TextBox();
            browseInputFileButton = new Button();
            outputFileLabel = new Label();
            outputFileTextBox = new TextBox();
            browseOutputFileButton = new Button();
            fillDefaultOutputButton = new Button();
            conversionHintLabel = new Label();
            actionsPanel = new FlowLayoutPanel();
            convertButton = new Button();
            analysisGroupBox = new GroupBox();
            analysisLayout = new TableLayoutPanel();
            detectedFormatTitleLabel = new Label();
            detectedFormatValueLabel = new Label();
            detectedTileModeTitleLabel = new Label();
            detectedTileModeValueLabel = new Label();
            detectedSwizzleTitleLabel = new Label();
            detectedSwizzleValueLabel = new Label();
            detectedComponentTitleLabel = new Label();
            detectedComponentValueLabel = new Label();
            advisoryLabel = new Label();
            logGroupBox = new GroupBox();
            executionLogTextBox = new TextBox();
            rootLayout.SuspendLayout();
            headerPanel.SuspendLayout();
            toolGroupBox.SuspendLayout();
            toolLayout.SuspendLayout();
            conversionGroupBox.SuspendLayout();
            conversionLayout.SuspendLayout();
            actionsPanel.SuspendLayout();
            analysisGroupBox.SuspendLayout();
            analysisLayout.SuspendLayout();
            logGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.BackColor = Color.FromArgb(245, 247, 250);
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(headerPanel, 0, 0);
            rootLayout.Controls.Add(toolGroupBox, 0, 1);
            rootLayout.Controls.Add(conversionGroupBox, 0, 2);
            rootLayout.Controls.Add(analysisGroupBox, 0, 3);
            rootLayout.Controls.Add(logGroupBox, 0, 4);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(14, 12, 14, 12);
            rootLayout.RowCount = 5;
            rootLayout.RowStyles.Add(new RowStyle());
            rootLayout.RowStyles.Add(new RowStyle());
            rootLayout.RowStyles.Add(new RowStyle());
            rootLayout.RowStyles.Add(new RowStyle());
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Size = new Size(984, 761);
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
            headerPanel.Size = new Size(950, 82);
            headerPanel.TabIndex = 0;
            // 
            // subtitleLabel
            // 
            subtitleLabel.AutoSize = true;
            subtitleLabel.Font = new Font("Segoe UI", 10F);
            subtitleLabel.ForeColor = Color.FromArgb(71, 85, 105);
            subtitleLabel.Location = new Point(21, 46);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new Size(732, 19);
            subtitleLabel.TabIndex = 1;
            subtitleLabel.Text = "Analise o GTX original, detecte SRGB/UNORM e execute o gtx_extract com mais seguranca para texturas de fonte e UI.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.FromArgb(37, 99, 235);
            titleLabel.Location = new Point(19, 12);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(249, 32);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Ferramenta GTX / DDS";
            // 
            // toolGroupBox
            // 
            toolGroupBox.Controls.Add(toolLayout);
            toolGroupBox.Dock = DockStyle.Fill;
            toolGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            toolGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            toolGroupBox.Location = new Point(17, 110);
            toolGroupBox.Name = "toolGroupBox";
            toolGroupBox.Padding = new Padding(12, 10, 12, 10);
            toolGroupBox.Size = new Size(950, 81);
            toolGroupBox.TabIndex = 1;
            toolGroupBox.TabStop = false;
            toolGroupBox.Text = "Executavel";
            // 
            // toolLayout
            // 
            toolLayout.ColumnCount = 4;
            toolLayout.ColumnStyles.Add(new ColumnStyle());
            toolLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            toolLayout.ColumnStyles.Add(new ColumnStyle());
            toolLayout.ColumnStyles.Add(new ColumnStyle());
            toolLayout.Controls.Add(toolPathLabel, 0, 0);
            toolLayout.Controls.Add(toolPathTextBox, 1, 0);
            toolLayout.Controls.Add(browseToolButton, 2, 0);
            toolLayout.Controls.Add(detectToolButton, 3, 0);
            toolLayout.Dock = DockStyle.Fill;
            toolLayout.Location = new Point(12, 28);
            toolLayout.Name = "toolLayout";
            toolLayout.RowCount = 1;
            toolLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            toolLayout.Size = new Size(926, 43);
            toolLayout.TabIndex = 0;
            // 
            // toolPathLabel
            // 
            toolPathLabel.Anchor = AnchorStyles.Left;
            toolPathLabel.AutoSize = true;
            toolPathLabel.Font = new Font("Segoe UI", 9.5F);
            toolPathLabel.ForeColor = Color.FromArgb(15, 23, 42);
            toolPathLabel.Location = new Point(0, 13);
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
            toolPathTextBox.Location = new Point(134, 9);
            toolPathTextBox.Margin = new Padding(0, 3, 10, 3);
            toolPathTextBox.Name = "toolPathTextBox";
            toolPathTextBox.PlaceholderText = "Ferramenta externa opcional para formatos nao suportados nativamente";
            toolPathTextBox.Size = new Size(553, 25);
            toolPathTextBox.TabIndex = 1;
            // 
            // browseToolButton
            // 
            browseToolButton.BackColor = Color.White;
            browseToolButton.Cursor = Cursors.Hand;
            browseToolButton.FlatStyle = FlatStyle.Flat;
            browseToolButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            browseToolButton.ForeColor = Color.FromArgb(15, 23, 42);
            browseToolButton.Location = new Point(697, 7);
            browseToolButton.Margin = new Padding(0, 1, 8, 1);
            browseToolButton.Name = "browseToolButton";
            browseToolButton.Size = new Size(92, 29);
            browseToolButton.TabIndex = 2;
            browseToolButton.Text = "Procurar...";
            browseToolButton.UseVisualStyleBackColor = false;
            browseToolButton.Click += BrowseToolButton_Click;
            // 
            // detectToolButton
            // 
            detectToolButton.BackColor = Color.FromArgb(15, 23, 42);
            detectToolButton.Cursor = Cursors.Hand;
            detectToolButton.FlatAppearance.BorderSize = 0;
            detectToolButton.FlatStyle = FlatStyle.Flat;
            detectToolButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            detectToolButton.ForeColor = Color.White;
            detectToolButton.Location = new Point(797, 7);
            detectToolButton.Margin = new Padding(0, 1, 0, 1);
            detectToolButton.Name = "detectToolButton";
            detectToolButton.Size = new Size(129, 29);
            detectToolButton.TabIndex = 3;
            detectToolButton.Text = "Detectar automatico";
            detectToolButton.UseVisualStyleBackColor = false;
            detectToolButton.Click += DetectToolButton_Click;
            // 
            // conversionGroupBox
            // 
            conversionGroupBox.Controls.Add(conversionLayout);
            conversionGroupBox.Dock = DockStyle.Fill;
            conversionGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            conversionGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            conversionGroupBox.Location = new Point(17, 204);
            conversionGroupBox.Name = "conversionGroupBox";
            conversionGroupBox.Padding = new Padding(12, 10, 12, 10);
            conversionGroupBox.Size = new Size(950, 219);
            conversionGroupBox.TabIndex = 2;
            conversionGroupBox.TabStop = false;
            conversionGroupBox.Text = "Conversao";
            // 
            // conversionLayout
            // 
            conversionLayout.ColumnCount = 4;
            conversionLayout.ColumnStyles.Add(new ColumnStyle());
            conversionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            conversionLayout.ColumnStyles.Add(new ColumnStyle());
            conversionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 124F));
            conversionLayout.Controls.Add(modeLabel, 0, 0);
            conversionLayout.Controls.Add(modeComboBox, 1, 0);
            conversionLayout.Controls.Add(profileLabel, 2, 0);
            conversionLayout.Controls.Add(profileComboBox, 3, 0);
            conversionLayout.Controls.Add(originalGtxLabel, 0, 1);
            conversionLayout.Controls.Add(originalGtxTextBox, 1, 1);
            conversionLayout.Controls.Add(browseOriginalGtxButton, 2, 1);
            conversionLayout.Controls.Add(analyzeOriginalButton, 3, 1);
            conversionLayout.Controls.Add(inputFileLabel, 0, 2);
            conversionLayout.Controls.Add(inputFileTextBox, 1, 2);
            conversionLayout.Controls.Add(browseInputFileButton, 2, 2);
            conversionLayout.Controls.Add(outputFileLabel, 0, 3);
            conversionLayout.Controls.Add(outputFileTextBox, 1, 3);
            conversionLayout.Controls.Add(browseOutputFileButton, 2, 3);
            conversionLayout.Controls.Add(fillDefaultOutputButton, 3, 3);
            conversionLayout.Controls.Add(conversionHintLabel, 0, 4);
            conversionLayout.Controls.Add(actionsPanel, 0, 5);
            conversionLayout.Dock = DockStyle.Fill;
            conversionLayout.Location = new Point(12, 28);
            conversionLayout.Name = "conversionLayout";
            conversionLayout.RowCount = 6;
            conversionLayout.RowStyles.Add(new RowStyle());
            conversionLayout.RowStyles.Add(new RowStyle());
            conversionLayout.RowStyles.Add(new RowStyle());
            conversionLayout.RowStyles.Add(new RowStyle());
            conversionLayout.RowStyles.Add(new RowStyle());
            conversionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            conversionLayout.Size = new Size(926, 181);
            conversionLayout.TabIndex = 0;
            // 
            // modeLabel
            // 
            modeLabel.Anchor = AnchorStyles.Left;
            modeLabel.AutoSize = true;
            modeLabel.Font = new Font("Segoe UI", 9.5F);
            modeLabel.ForeColor = Color.FromArgb(15, 23, 42);
            modeLabel.Location = new Point(0, 7);
            modeLabel.Margin = new Padding(0, 0, 10, 0);
            modeLabel.Name = "modeLabel";
            modeLabel.Size = new Size(39, 17);
            modeLabel.TabIndex = 0;
            modeLabel.Text = "Modo";
            // 
            // modeComboBox
            // 
            modeComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            modeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            modeComboBox.Font = new Font("Segoe UI", 10F);
            modeComboBox.FormattingEnabled = true;
            modeComboBox.Location = new Point(49, 3);
            modeComboBox.Margin = new Padding(0, 3, 12, 3);
            modeComboBox.Name = "modeComboBox";
            modeComboBox.Size = new Size(622, 25);
            modeComboBox.TabIndex = 1;
            modeComboBox.SelectedIndexChanged += ModeComboBox_SelectedIndexChanged;
            // 
            // profileLabel
            // 
            profileLabel.Anchor = AnchorStyles.Left;
            profileLabel.AutoSize = true;
            profileLabel.Font = new Font("Segoe UI", 9.5F);
            profileLabel.ForeColor = Color.FromArgb(15, 23, 42);
            profileLabel.Location = new Point(683, 7);
            profileLabel.Margin = new Padding(0, 0, 10, 0);
            profileLabel.Name = "profileLabel";
            profileLabel.Size = new Size(39, 17);
            profileLabel.TabIndex = 2;
            profileLabel.Text = "Perfil";
            // 
            // profileComboBox
            // 
            profileComboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            profileComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            profileComboBox.Font = new Font("Segoe UI", 10F);
            profileComboBox.FormattingEnabled = true;
            profileComboBox.Location = new Point(732, 3);
            profileComboBox.Margin = new Padding(0, 3, 0, 3);
            profileComboBox.Name = "profileComboBox";
            profileComboBox.Size = new Size(194, 25);
            profileComboBox.TabIndex = 3;
            // 
            // originalGtxLabel
            // 
            originalGtxLabel.Anchor = AnchorStyles.Left;
            originalGtxLabel.AutoSize = true;
            originalGtxLabel.Font = new Font("Segoe UI", 9.5F);
            originalGtxLabel.ForeColor = Color.FromArgb(15, 23, 42);
            originalGtxLabel.Location = new Point(0, 40);
            originalGtxLabel.Margin = new Padding(0, 0, 10, 0);
            originalGtxLabel.Name = "originalGtxLabel";
            originalGtxLabel.Size = new Size(39, 17);
            originalGtxLabel.TabIndex = 4;
            originalGtxLabel.Text = "GTX base";
            // 
            // originalGtxTextBox
            // 
            originalGtxTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            originalGtxTextBox.BorderStyle = BorderStyle.FixedSingle;
            originalGtxTextBox.Font = new Font("Segoe UI", 10F);
            originalGtxTextBox.Location = new Point(49, 36);
            originalGtxTextBox.Margin = new Padding(0, 3, 12, 3);
            originalGtxTextBox.Name = "originalGtxTextBox";
            originalGtxTextBox.PlaceholderText = "Original para detectar o formato e servir de referencia";
            originalGtxTextBox.Size = new Size(622, 25);
            originalGtxTextBox.TabIndex = 5;
            // 
            // browseOriginalGtxButton
            // 
            browseOriginalGtxButton.BackColor = Color.White;
            browseOriginalGtxButton.Cursor = Cursors.Hand;
            browseOriginalGtxButton.FlatStyle = FlatStyle.Flat;
            browseOriginalGtxButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            browseOriginalGtxButton.ForeColor = Color.FromArgb(15, 23, 42);
            browseOriginalGtxButton.Location = new Point(683, 34);
            browseOriginalGtxButton.Margin = new Padding(0, 1, 8, 1);
            browseOriginalGtxButton.Name = "browseOriginalGtxButton";
            browseOriginalGtxButton.Size = new Size(96, 29);
            browseOriginalGtxButton.TabIndex = 6;
            browseOriginalGtxButton.Text = "Procurar...";
            browseOriginalGtxButton.UseVisualStyleBackColor = false;
            browseOriginalGtxButton.Click += BrowseOriginalGtxButton_Click;
            // 
            // analyzeOriginalButton
            // 
            analyzeOriginalButton.BackColor = Color.FromArgb(37, 99, 235);
            analyzeOriginalButton.Cursor = Cursors.Hand;
            analyzeOriginalButton.FlatAppearance.BorderSize = 0;
            analyzeOriginalButton.FlatStyle = FlatStyle.Flat;
            analyzeOriginalButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            analyzeOriginalButton.ForeColor = Color.White;
            analyzeOriginalButton.Location = new Point(732, 34);
            analyzeOriginalButton.Margin = new Padding(0, 1, 0, 1);
            analyzeOriginalButton.Name = "analyzeOriginalButton";
            analyzeOriginalButton.Size = new Size(126, 29);
            analyzeOriginalButton.TabIndex = 7;
            analyzeOriginalButton.Text = "Analisar GTX base";
            analyzeOriginalButton.UseVisualStyleBackColor = false;
            analyzeOriginalButton.Click += AnalyzeOriginalButton_Click;
            // 
            // inputFileLabel
            // 
            inputFileLabel.Anchor = AnchorStyles.Left;
            inputFileLabel.AutoSize = true;
            inputFileLabel.Font = new Font("Segoe UI", 9.5F);
            inputFileLabel.ForeColor = Color.FromArgb(15, 23, 42);
            inputFileLabel.Location = new Point(0, 73);
            inputFileLabel.Margin = new Padding(0, 0, 10, 0);
            inputFileLabel.Name = "inputFileLabel";
            inputFileLabel.Size = new Size(44, 17);
            inputFileLabel.TabIndex = 8;
            inputFileLabel.Text = "Entrada";
            // 
            // inputFileTextBox
            // 
            inputFileTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            inputFileTextBox.BorderStyle = BorderStyle.FixedSingle;
            inputFileTextBox.Font = new Font("Segoe UI", 10F);
            inputFileTextBox.Location = new Point(49, 69);
            inputFileTextBox.Margin = new Padding(0, 3, 12, 3);
            inputFileTextBox.Name = "inputFileTextBox";
            inputFileTextBox.PlaceholderText = "Selecione o arquivo de entrada (.gtx ou .dds)";
            inputFileTextBox.Size = new Size(622, 25);
            inputFileTextBox.TabIndex = 9;
            inputFileTextBox.TextChanged += InputFileTextBox_TextChanged;
            // 
            // browseInputFileButton
            // 
            browseInputFileButton.BackColor = Color.White;
            browseInputFileButton.Cursor = Cursors.Hand;
            browseInputFileButton.FlatStyle = FlatStyle.Flat;
            browseInputFileButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            browseInputFileButton.ForeColor = Color.FromArgb(15, 23, 42);
            browseInputFileButton.Location = new Point(683, 67);
            browseInputFileButton.Margin = new Padding(0, 1, 8, 1);
            browseInputFileButton.Name = "browseInputFileButton";
            browseInputFileButton.Size = new Size(96, 29);
            browseInputFileButton.TabIndex = 10;
            browseInputFileButton.Text = "Procurar...";
            browseInputFileButton.UseVisualStyleBackColor = false;
            browseInputFileButton.Click += BrowseInputFileButton_Click;
            // 
            // outputFileLabel
            // 
            outputFileLabel.Anchor = AnchorStyles.Left;
            outputFileLabel.AutoSize = true;
            outputFileLabel.Font = new Font("Segoe UI", 9.5F);
            outputFileLabel.ForeColor = Color.FromArgb(15, 23, 42);
            outputFileLabel.Location = new Point(0, 106);
            outputFileLabel.Margin = new Padding(0, 0, 10, 0);
            outputFileLabel.Name = "outputFileLabel";
            outputFileLabel.Size = new Size(33, 17);
            outputFileLabel.TabIndex = 11;
            outputFileLabel.Text = "Saida";
            // 
            // outputFileTextBox
            // 
            outputFileTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            outputFileTextBox.BorderStyle = BorderStyle.FixedSingle;
            outputFileTextBox.Font = new Font("Segoe UI", 10F);
            outputFileTextBox.Location = new Point(49, 102);
            outputFileTextBox.Margin = new Padding(0, 3, 12, 3);
            outputFileTextBox.Name = "outputFileTextBox";
            outputFileTextBox.PlaceholderText = "Informe o arquivo de saida";
            outputFileTextBox.Size = new Size(622, 25);
            outputFileTextBox.TabIndex = 12;
            // 
            // browseOutputFileButton
            // 
            browseOutputFileButton.BackColor = Color.White;
            browseOutputFileButton.Cursor = Cursors.Hand;
            browseOutputFileButton.FlatStyle = FlatStyle.Flat;
            browseOutputFileButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            browseOutputFileButton.ForeColor = Color.FromArgb(15, 23, 42);
            browseOutputFileButton.Location = new Point(683, 100);
            browseOutputFileButton.Margin = new Padding(0, 1, 8, 1);
            browseOutputFileButton.Name = "browseOutputFileButton";
            browseOutputFileButton.Size = new Size(96, 29);
            browseOutputFileButton.TabIndex = 13;
            browseOutputFileButton.Text = "Procurar...";
            browseOutputFileButton.UseVisualStyleBackColor = false;
            browseOutputFileButton.Click += BrowseOutputFileButton_Click;
            // 
            // fillDefaultOutputButton
            // 
            fillDefaultOutputButton.BackColor = Color.White;
            fillDefaultOutputButton.Cursor = Cursors.Hand;
            fillDefaultOutputButton.FlatStyle = FlatStyle.Flat;
            fillDefaultOutputButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            fillDefaultOutputButton.ForeColor = Color.FromArgb(15, 23, 42);
            fillDefaultOutputButton.Location = new Point(732, 100);
            fillDefaultOutputButton.Margin = new Padding(0, 1, 0, 1);
            fillDefaultOutputButton.Name = "fillDefaultOutputButton";
            fillDefaultOutputButton.Size = new Size(126, 29);
            fillDefaultOutputButton.TabIndex = 14;
            fillDefaultOutputButton.Text = "Usar nome padrao";
            fillDefaultOutputButton.UseVisualStyleBackColor = false;
            fillDefaultOutputButton.Click += FillDefaultOutputButton_Click;
            // 
            // conversionHintLabel
            // 
            conversionHintLabel.AutoSize = true;
            conversionLayout.SetColumnSpan(conversionHintLabel, 4);
            conversionHintLabel.Font = new Font("Segoe UI", 9F);
            conversionHintLabel.ForeColor = Color.FromArgb(71, 85, 105);
            conversionHintLabel.Location = new Point(0, 132);
            conversionHintLabel.Margin = new Padding(0, 0, 0, 8);
            conversionHintLabel.Name = "conversionHintLabel";
            conversionHintLabel.Size = new Size(780, 15);
            conversionHintLabel.TabIndex = 15;
            conversionHintLabel.Text = "Use 'Auto (usar GTX base)' ao recriar um GTX a partir de DDS. Para texturas R8_G8_UNORM de fonte/UI, o roundtrip pode nao ser identico.";
            // 
            // actionsPanel
            // 
            actionsPanel.AutoSize = true;
            conversionLayout.SetColumnSpan(actionsPanel, 4);
            actionsPanel.Controls.Add(convertButton);
            actionsPanel.Dock = DockStyle.Fill;
            actionsPanel.Location = new Point(0, 155);
            actionsPanel.Margin = new Padding(0);
            actionsPanel.Name = "actionsPanel";
            actionsPanel.Size = new Size(926, 26);
            actionsPanel.TabIndex = 16;
            // 
            // convertButton
            // 
            convertButton.BackColor = Color.FromArgb(22, 163, 74);
            convertButton.Cursor = Cursors.Hand;
            convertButton.FlatAppearance.BorderSize = 0;
            convertButton.FlatStyle = FlatStyle.Flat;
            convertButton.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            convertButton.ForeColor = Color.White;
            convertButton.Location = new Point(0, 0);
            convertButton.Margin = new Padding(0);
            convertButton.Name = "convertButton";
            convertButton.Size = new Size(180, 30);
            convertButton.TabIndex = 0;
            convertButton.Text = "Executar conversao";
            convertButton.UseVisualStyleBackColor = false;
            convertButton.Click += ConvertButton_Click;
            // 
            // analysisGroupBox
            // 
            analysisGroupBox.Controls.Add(analysisLayout);
            analysisGroupBox.Dock = DockStyle.Fill;
            analysisGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            analysisGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            analysisGroupBox.Location = new Point(17, 436);
            analysisGroupBox.Name = "analysisGroupBox";
            analysisGroupBox.Padding = new Padding(12, 10, 12, 10);
            analysisGroupBox.Size = new Size(950, 116);
            analysisGroupBox.TabIndex = 3;
            analysisGroupBox.TabStop = false;
            analysisGroupBox.Text = "Analise do GTX base";
            // 
            // analysisLayout
            // 
            analysisLayout.ColumnCount = 4;
            analysisLayout.ColumnStyles.Add(new ColumnStyle());
            analysisLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            analysisLayout.ColumnStyles.Add(new ColumnStyle());
            analysisLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            analysisLayout.Controls.Add(detectedFormatTitleLabel, 0, 0);
            analysisLayout.Controls.Add(detectedFormatValueLabel, 1, 0);
            analysisLayout.Controls.Add(detectedTileModeTitleLabel, 2, 0);
            analysisLayout.Controls.Add(detectedTileModeValueLabel, 3, 0);
            analysisLayout.Controls.Add(detectedSwizzleTitleLabel, 0, 1);
            analysisLayout.Controls.Add(detectedSwizzleValueLabel, 1, 1);
            analysisLayout.Controls.Add(detectedComponentTitleLabel, 2, 1);
            analysisLayout.Controls.Add(detectedComponentValueLabel, 3, 1);
            analysisLayout.Controls.Add(advisoryLabel, 0, 2);
            analysisLayout.Dock = DockStyle.Fill;
            analysisLayout.Location = new Point(12, 28);
            analysisLayout.Name = "analysisLayout";
            analysisLayout.RowCount = 3;
            analysisLayout.RowStyles.Add(new RowStyle());
            analysisLayout.RowStyles.Add(new RowStyle());
            analysisLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            analysisLayout.Size = new Size(926, 78);
            analysisLayout.TabIndex = 0;
            // 
            // detectedFormatTitleLabel
            // 
            detectedFormatTitleLabel.Anchor = AnchorStyles.Left;
            detectedFormatTitleLabel.AutoSize = true;
            detectedFormatTitleLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            detectedFormatTitleLabel.ForeColor = Color.FromArgb(15, 23, 42);
            detectedFormatTitleLabel.Location = new Point(0, 4);
            detectedFormatTitleLabel.Margin = new Padding(0, 0, 8, 0);
            detectedFormatTitleLabel.Name = "detectedFormatTitleLabel";
            detectedFormatTitleLabel.Size = new Size(52, 15);
            detectedFormatTitleLabel.TabIndex = 0;
            detectedFormatTitleLabel.Text = "Formato";
            // 
            // detectedFormatValueLabel
            // 
            detectedFormatValueLabel.AutoSize = true;
            detectedFormatValueLabel.Font = new Font("Segoe UI", 9F);
            detectedFormatValueLabel.ForeColor = Color.FromArgb(71, 85, 105);
            detectedFormatValueLabel.Location = new Point(60, 4);
            detectedFormatValueLabel.Margin = new Padding(0, 0, 12, 0);
            detectedFormatValueLabel.Name = "detectedFormatValueLabel";
            detectedFormatValueLabel.Size = new Size(10, 15);
            detectedFormatValueLabel.TabIndex = 1;
            detectedFormatValueLabel.Text = "-";
            // 
            // detectedTileModeTitleLabel
            // 
            detectedTileModeTitleLabel.Anchor = AnchorStyles.Left;
            detectedTileModeTitleLabel.AutoSize = true;
            detectedTileModeTitleLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            detectedTileModeTitleLabel.ForeColor = Color.FromArgb(15, 23, 42);
            detectedTileModeTitleLabel.Location = new Point(469, 4);
            detectedTileModeTitleLabel.Margin = new Padding(0, 0, 8, 0);
            detectedTileModeTitleLabel.Name = "detectedTileModeTitleLabel";
            detectedTileModeTitleLabel.Size = new Size(57, 15);
            detectedTileModeTitleLabel.TabIndex = 2;
            detectedTileModeTitleLabel.Text = "TileMode";
            // 
            // detectedTileModeValueLabel
            // 
            detectedTileModeValueLabel.AutoSize = true;
            detectedTileModeValueLabel.Font = new Font("Segoe UI", 9F);
            detectedTileModeValueLabel.ForeColor = Color.FromArgb(71, 85, 105);
            detectedTileModeValueLabel.Location = new Point(534, 4);
            detectedTileModeValueLabel.Margin = new Padding(0);
            detectedTileModeValueLabel.Name = "detectedTileModeValueLabel";
            detectedTileModeValueLabel.Size = new Size(10, 15);
            detectedTileModeValueLabel.TabIndex = 3;
            detectedTileModeValueLabel.Text = "-";
            // 
            // detectedSwizzleTitleLabel
            // 
            detectedSwizzleTitleLabel.Anchor = AnchorStyles.Left;
            detectedSwizzleTitleLabel.AutoSize = true;
            detectedSwizzleTitleLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            detectedSwizzleTitleLabel.ForeColor = Color.FromArgb(15, 23, 42);
            detectedSwizzleTitleLabel.Location = new Point(0, 27);
            detectedSwizzleTitleLabel.Margin = new Padding(0, 0, 8, 0);
            detectedSwizzleTitleLabel.Name = "detectedSwizzleTitleLabel";
            detectedSwizzleTitleLabel.Size = new Size(47, 15);
            detectedSwizzleTitleLabel.TabIndex = 4;
            detectedSwizzleTitleLabel.Text = "Swizzle";
            // 
            // detectedSwizzleValueLabel
            // 
            detectedSwizzleValueLabel.AutoSize = true;
            detectedSwizzleValueLabel.Font = new Font("Segoe UI", 9F);
            detectedSwizzleValueLabel.ForeColor = Color.FromArgb(71, 85, 105);
            detectedSwizzleValueLabel.Location = new Point(60, 27);
            detectedSwizzleValueLabel.Margin = new Padding(0, 0, 12, 0);
            detectedSwizzleValueLabel.Name = "detectedSwizzleValueLabel";
            detectedSwizzleValueLabel.Size = new Size(10, 15);
            detectedSwizzleValueLabel.TabIndex = 5;
            detectedSwizzleValueLabel.Text = "-";
            // 
            // detectedComponentTitleLabel
            // 
            detectedComponentTitleLabel.Anchor = AnchorStyles.Left;
            detectedComponentTitleLabel.AutoSize = true;
            detectedComponentTitleLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            detectedComponentTitleLabel.ForeColor = Color.FromArgb(15, 23, 42);
            detectedComponentTitleLabel.Location = new Point(469, 27);
            detectedComponentTitleLabel.Margin = new Padding(0, 0, 8, 0);
            detectedComponentTitleLabel.Name = "detectedComponentTitleLabel";
            detectedComponentTitleLabel.Size = new Size(111, 15);
            detectedComponentTitleLabel.TabIndex = 6;
            detectedComponentTitleLabel.Text = "Component selector";
            // 
            // detectedComponentValueLabel
            // 
            detectedComponentValueLabel.AutoSize = true;
            detectedComponentValueLabel.Font = new Font("Segoe UI", 9F);
            detectedComponentValueLabel.ForeColor = Color.FromArgb(71, 85, 105);
            detectedComponentValueLabel.Location = new Point(588, 27);
            detectedComponentValueLabel.Margin = new Padding(0);
            detectedComponentValueLabel.Name = "detectedComponentValueLabel";
            detectedComponentValueLabel.Size = new Size(10, 15);
            detectedComponentValueLabel.TabIndex = 7;
            detectedComponentValueLabel.Text = "-";
            // 
            // advisoryLabel
            // 
            advisoryLabel.AutoSize = true;
            analysisLayout.SetColumnSpan(advisoryLabel, 4);
            advisoryLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            advisoryLabel.ForeColor = Color.FromArgb(71, 85, 105);
            advisoryLabel.Location = new Point(0, 50);
            advisoryLabel.Margin = new Padding(0, 8, 0, 0);
            advisoryLabel.Name = "advisoryLabel";
            advisoryLabel.Size = new Size(275, 15);
            advisoryLabel.TabIndex = 8;
            advisoryLabel.Text = "Selecione um GTX base e clique em 'Analisar GTX base'.";
            // 
            // logGroupBox
            // 
            logGroupBox.Controls.Add(executionLogTextBox);
            logGroupBox.Dock = DockStyle.Fill;
            logGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            logGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            logGroupBox.Location = new Point(17, 565);
            logGroupBox.Name = "logGroupBox";
            logGroupBox.Padding = new Padding(12, 10, 12, 10);
            logGroupBox.Size = new Size(950, 181);
            logGroupBox.TabIndex = 4;
            logGroupBox.TabStop = false;
            logGroupBox.Text = "Saida do gtx_extract";
            // 
            // executionLogTextBox
            // 
            executionLogTextBox.BackColor = Color.White;
            executionLogTextBox.BorderStyle = BorderStyle.FixedSingle;
            executionLogTextBox.Dock = DockStyle.Fill;
            executionLogTextBox.Font = new Font("Consolas", 10F);
            executionLogTextBox.Location = new Point(12, 28);
            executionLogTextBox.Multiline = true;
            executionLogTextBox.Name = "executionLogTextBox";
            executionLogTextBox.ReadOnly = true;
            executionLogTextBox.ScrollBars = ScrollBars.Both;
            executionLogTextBox.Size = new Size(926, 143);
            executionLogTextBox.TabIndex = 0;
            executionLogTextBox.Text = "A analise e a conversao exibem aqui a saida completa do gtx_extract.";
            executionLogTextBox.WordWrap = false;
            // 
            // GtxDdsToolForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(984, 761);
            Controls.Add(rootLayout);
            MinimumSize = new Size(940, 760);
            Name = "GtxDdsToolForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ferramenta GTX / DDS";
            rootLayout.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            toolGroupBox.ResumeLayout(false);
            toolLayout.ResumeLayout(false);
            toolLayout.PerformLayout();
            conversionGroupBox.ResumeLayout(false);
            conversionLayout.ResumeLayout(false);
            conversionLayout.PerformLayout();
            actionsPanel.ResumeLayout(false);
            analysisGroupBox.ResumeLayout(false);
            analysisLayout.ResumeLayout(false);
            analysisLayout.PerformLayout();
            logGroupBox.ResumeLayout(false);
            logGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private Panel headerPanel;
        private Label titleLabel;
        private Label subtitleLabel;
        private GroupBox toolGroupBox;
        private TableLayoutPanel toolLayout;
        private Label toolPathLabel;
        private TextBox toolPathTextBox;
        private Button browseToolButton;
        private Button detectToolButton;
        private GroupBox conversionGroupBox;
        private TableLayoutPanel conversionLayout;
        private Label modeLabel;
        private ComboBox modeComboBox;
        private Label profileLabel;
        private ComboBox profileComboBox;
        private Label originalGtxLabel;
        private TextBox originalGtxTextBox;
        private Button browseOriginalGtxButton;
        private Button analyzeOriginalButton;
        private Label inputFileLabel;
        private TextBox inputFileTextBox;
        private Button browseInputFileButton;
        private Label outputFileLabel;
        private TextBox outputFileTextBox;
        private Button browseOutputFileButton;
        private Button fillDefaultOutputButton;
        private Label conversionHintLabel;
        private FlowLayoutPanel actionsPanel;
        private Button convertButton;
        private GroupBox analysisGroupBox;
        private TableLayoutPanel analysisLayout;
        private Label detectedFormatTitleLabel;
        private Label detectedFormatValueLabel;
        private Label detectedTileModeTitleLabel;
        private Label detectedTileModeValueLabel;
        private Label detectedSwizzleTitleLabel;
        private Label detectedSwizzleValueLabel;
        private Label detectedComponentTitleLabel;
        private Label detectedComponentValueLabel;
        private Label advisoryLabel;
        private GroupBox logGroupBox;
        private TextBox executionLogTextBox;
    }
}

#nullable restore
