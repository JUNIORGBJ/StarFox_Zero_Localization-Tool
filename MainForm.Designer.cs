#nullable disable

namespace StarFoxZeroLocalizationTool
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip = new MenuStrip();
            arquivoToolStripMenuItem = new ToolStripMenuItem();
            abrirToolStripMenuItem = new ToolStripMenuItem();
            salvarToolStripMenuItem = new ToolStripMenuItem();
            exportarCsvToolStripMenuItem = new ToolStripMenuItem();
            importarCsvToolStripMenuItem = new ToolStripMenuItem();
            ferramentasToolStripMenuItem = new ToolStripMenuItem();
            gtxDdsToolStripMenuItem = new ToolStripMenuItem();
            gtxRawEditorToolStripMenuItem = new ToolStripMenuItem();
            headerPanel = new Panel();
            fileGroupBox = new GroupBox();
            loadButton = new Button();
            saveButton = new Button();
            loadedFileValueLabel = new Label();
            searchGroupBox = new GroupBox();
            searchFieldLabel = new Label();
            searchRequiredLabel = new Label();
            searchTextBox = new TextBox();
            caseSensitiveCheckBox = new CheckBox();
            replaceFieldLabel = new Label();
            replaceTextBox = new TextBox();
            searchHelperLabel = new Label();
            searchButton = new Button();
            nextMatchButton = new Button();
            replaceCurrentButton = new Button();
            replaceAllButton = new Button();
            validateCharsetButton = new Button();
            remapGroupBox = new GroupBox();
            remapTexturePreviewPictureBox = new PictureBox();
            remapSourceLabel = new Label();
            remapSourceComboBox = new ComboBox();
            remapTargetLabel = new Label();
            remapTargetTextBox = new TextBox();
            applyCharRemapButton = new Button();
            remapHelperLabel = new Label();
            remapLanguageCurrentLabel = new Label();
            remapLanguageCurrentValueLabel = new Label();
            remapLanguageTargetLabel = new Label();
            remapLanguageTargetTextBox = new TextBox();
            applyLanguageFlagsButton = new Button();
            remapVariantDetailsLabel = new Label();
            currentGlyphHeaderLabel = new Label();
            currentGlyphHintLabel = new Label();
            newCharacterHeaderLabel = new Label();
            newCharacterHintLabel = new Label();
            newCharacterLabel = new Label();
            newCharacterTextBox = new TextBox();
            newCharacterLanguageLabel = new Label();
            newCharacterLanguageTextBox = new TextBox();
            selectNewGlyphButton = new Button();
            createNewCharacterButton = new Button();
            updateSelectedGlyphButton = new Button();
            removeCharacterButton = new Button();
            remapSectionDividerPanel = new Panel();
            newCharacterBaseInfoLabel = new Label();
            newCharacterSelectionLabel = new Label();
            selectionAdjustLabel = new Label();
            selectionAdjustStepTextBox = new TextBox();
            selectionWidthDecreaseButton = new Button();
            selectionWidthIncreaseButton = new Button();
            selectionHeightDecreaseButton = new Button();
            selectionHeightIncreaseButton = new Button();
            selectionLeftDecreaseButton = new Button();
            selectionLeftIncreaseButton = new Button();
            selectionRightDecreaseButton = new Button();
            selectionRightIncreaseButton = new Button();
            selectionTopDecreaseButton = new Button();
            selectionTopIncreaseButton = new Button();
            selectionBottomDecreaseButton = new Button();
            selectionBottomIncreaseButton = new Button();
            resetSelectionToGlyphButton = new Button();
            remapGlyphZoomPictureBox = new PictureBox();
            remapTexturePreviewLabel = new Label();
            navigationGroupBox = new GroupBox();
            navigationSummaryLabel = new Label();
            eventTreeView = new TreeView();
            editorGroupBox = new GroupBox();
            editorFieldLabel = new Label();
            editorRequiredLabel = new Label();
            editorHelperLabel = new Label();
            selectedEntryLabel = new Label();
            editorPreviewInfoLabel = new Label();
            editorPreviewPictureBox = new PictureBox();
            textTextBox = new TextBox();
            statusStrip = new StatusStrip();
            statusToolStripStatusLabel = new ToolStripStatusLabel();
            validationErrorProvider = new ErrorProvider(components);
            menuStrip.SuspendLayout();
            fileGroupBox.SuspendLayout();
            searchGroupBox.SuspendLayout();
            remapGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)remapTexturePreviewPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)remapGlyphZoomPictureBox).BeginInit();
            navigationGroupBox.SuspendLayout();
            editorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)editorPreviewPictureBox).BeginInit();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)validationErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.White;
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { arquivoToolStripMenuItem, ferramentasToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(7, 2, 0, 2);
            menuStrip.Size = new Size(1214, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // arquivoToolStripMenuItem
            // 
            arquivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { abrirToolStripMenuItem, salvarToolStripMenuItem, exportarCsvToolStripMenuItem, importarCsvToolStripMenuItem });
            arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            arquivoToolStripMenuItem.Size = new Size(61, 20);
            arquivoToolStripMenuItem.Text = "&Arquivo";
            // 
            // abrirToolStripMenuItem
            // 
            abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            abrirToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            abrirToolStripMenuItem.Size = new Size(222, 22);
            abrirToolStripMenuItem.Text = "&Abrir MCD...";
            abrirToolStripMenuItem.Click += LoadButton_Click;
            // 
            // salvarToolStripMenuItem
            // 
            salvarToolStripMenuItem.Name = "salvarToolStripMenuItem";
            salvarToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            salvarToolStripMenuItem.Size = new Size(222, 22);
            salvarToolStripMenuItem.Text = "&Salvar MCD...";
            salvarToolStripMenuItem.Click += SaveButton_Click;
            // 
            // exportarCsvToolStripMenuItem
            // 
            exportarCsvToolStripMenuItem.Name = "exportarCsvToolStripMenuItem";
            exportarCsvToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.E;
            exportarCsvToolStripMenuItem.Size = new Size(222, 22);
            exportarCsvToolStripMenuItem.Text = "E&xportar CSV...";
            exportarCsvToolStripMenuItem.Click += ExportCsvButton_Click;
            // 
            // importarCsvToolStripMenuItem
            // 
            importarCsvToolStripMenuItem.Name = "importarCsvToolStripMenuItem";
            importarCsvToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.I;
            importarCsvToolStripMenuItem.Size = new Size(222, 22);
            importarCsvToolStripMenuItem.Text = "&Importar CSV...";
            importarCsvToolStripMenuItem.Click += ImportCsvButton_Click;
            // 
            // ferramentasToolStripMenuItem
            // 
            ferramentasToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { gtxDdsToolStripMenuItem, gtxRawEditorToolStripMenuItem });
            ferramentasToolStripMenuItem.Name = "ferramentasToolStripMenuItem";
            ferramentasToolStripMenuItem.Size = new Size(84, 20);
            ferramentasToolStripMenuItem.Text = "&Ferramentas";
            // 
            // gtxDdsToolStripMenuItem
            // 
            gtxDdsToolStripMenuItem.Name = "gtxDdsToolStripMenuItem";
            gtxDdsToolStripMenuItem.Size = new Size(195, 22);
            gtxDdsToolStripMenuItem.Text = "Conversor &GTX / DDS...";
            gtxDdsToolStripMenuItem.Click += OpenGtxDdsToolMenuItem_Click;
            // 
            // gtxRawEditorToolStripMenuItem
            // 
            gtxRawEditorToolStripMenuItem.Name = "gtxRawEditorToolStripMenuItem";
            gtxRawEditorToolStripMenuItem.Size = new Size(195, 22);
            gtxRawEditorToolStripMenuItem.Text = "Editor &Bruto de GTX...";
            gtxRawEditorToolStripMenuItem.Click += OpenGtxRawEditorMenuItem_Click;
            // 
            // headerPanel
            // 
            headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            headerPanel.BackColor = Color.White;
            headerPanel.BackgroundImage = (Image)resources.GetObject("headerPanel.BackgroundImage");
            headerPanel.BackgroundImageLayout = ImageLayout.Zoom;
            headerPanel.Location = new Point(300, 36);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(900, 76);
            headerPanel.TabIndex = 1;
            // 
            // fileGroupBox
            // 
            fileGroupBox.Controls.Add(loadButton);
            fileGroupBox.Controls.Add(saveButton);
            fileGroupBox.Controls.Add(loadedFileValueLabel);
            fileGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            fileGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            fileGroupBox.Location = new Point(14, 25);
            fileGroupBox.Name = "fileGroupBox";
            fileGroupBox.Size = new Size(280, 87);
            fileGroupBox.TabIndex = 2;
            fileGroupBox.TabStop = false;
            fileGroupBox.Text = "Arquivo";
            // 
            // loadButton
            // 
            loadButton.BackColor = Color.FromArgb(37, 99, 235);
            loadButton.Cursor = Cursors.Hand;
            loadButton.FlatAppearance.BorderSize = 0;
            loadButton.FlatStyle = FlatStyle.Flat;
            loadButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            loadButton.ForeColor = Color.White;
            loadButton.Location = new Point(12, 50);
            loadButton.Name = "loadButton";
            loadButton.Size = new Size(123, 27);
            loadButton.TabIndex = 2;
            loadButton.Text = "Abrir arquivo MCD";
            loadButton.UseVisualStyleBackColor = false;
            loadButton.Click += LoadButton_Click;
            // 
            // saveButton
            // 
            saveButton.BackColor = Color.FromArgb(22, 163, 74);
            saveButton.Cursor = Cursors.Hand;
            saveButton.Enabled = false;
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            saveButton.ForeColor = Color.White;
            saveButton.Location = new Point(141, 50);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(123, 27);
            saveButton.TabIndex = 3;
            saveButton.Text = "Salvar alteracoes";
            saveButton.UseVisualStyleBackColor = false;
            saveButton.Click += SaveButton_Click;
            // 
            // loadedFileValueLabel
            // 
            loadedFileValueLabel.AutoEllipsis = true;
            loadedFileValueLabel.Font = new Font("Segoe UI", 10F);
            loadedFileValueLabel.ForeColor = Color.FromArgb(15, 23, 42);
            loadedFileValueLabel.Location = new Point(12, 21);
            loadedFileValueLabel.Name = "loadedFileValueLabel";
            loadedFileValueLabel.Size = new Size(188, 26);
            loadedFileValueLabel.TabIndex = 1;
            loadedFileValueLabel.Text = "Nenhum arquivo carregado";
            loadedFileValueLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // searchGroupBox
            // 
            searchGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            searchGroupBox.Controls.Add(searchFieldLabel);
            searchGroupBox.Controls.Add(searchRequiredLabel);
            searchGroupBox.Controls.Add(searchTextBox);
            searchGroupBox.Controls.Add(caseSensitiveCheckBox);
            searchGroupBox.Controls.Add(replaceFieldLabel);
            searchGroupBox.Controls.Add(replaceTextBox);
            searchGroupBox.Controls.Add(searchHelperLabel);
            searchGroupBox.Controls.Add(searchButton);
            searchGroupBox.Controls.Add(nextMatchButton);
            searchGroupBox.Controls.Add(replaceCurrentButton);
            searchGroupBox.Controls.Add(replaceAllButton);
            searchGroupBox.Controls.Add(validateCharsetButton);
            searchGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            searchGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            searchGroupBox.Location = new Point(425, 428);
            searchGroupBox.Name = "searchGroupBox";
            searchGroupBox.Size = new Size(759, 137);
            searchGroupBox.TabIndex = 3;
            searchGroupBox.TabStop = false;
            searchGroupBox.Text = "Pesquisa";
            // 
            // searchFieldLabel
            // 
            searchFieldLabel.AutoSize = true;
            searchFieldLabel.Font = new Font("Segoe UI", 9.5F);
            searchFieldLabel.ForeColor = Color.FromArgb(15, 23, 42);
            searchFieldLabel.Location = new Point(12, 27);
            searchFieldLabel.Name = "searchFieldLabel";
            searchFieldLabel.Size = new Size(114, 17);
            searchFieldLabel.TabIndex = 0;
            searchFieldLabel.Text = "Texto da pesquisa";
            // 
            // searchRequiredLabel
            // 
            searchRequiredLabel.AutoSize = true;
            searchRequiredLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            searchRequiredLabel.ForeColor = Color.FromArgb(185, 28, 28);
            searchRequiredLabel.Location = new Point(126, 26);
            searchRequiredLabel.Name = "searchRequiredLabel";
            searchRequiredLabel.Size = new Size(15, 19);
            searchRequiredLabel.TabIndex = 1;
            searchRequiredLabel.Text = "*";
            // 
            // searchTextBox
            // 
            searchTextBox.BackColor = Color.White;
            searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            searchTextBox.Font = new Font("Segoe UI", 10F);
            searchTextBox.Location = new Point(12, 48);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.PlaceholderText = "Ex.: LOADING, conexao, menu...";
            searchTextBox.Size = new Size(330, 25);
            searchTextBox.TabIndex = 2;
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            searchTextBox.KeyDown += SearchTextBox_KeyDown;
            // 
            // caseSensitiveCheckBox
            // 
            caseSensitiveCheckBox.AutoSize = true;
            caseSensitiveCheckBox.Font = new Font("Segoe UI", 9F);
            caseSensitiveCheckBox.ForeColor = Color.FromArgb(15, 23, 42);
            caseSensitiveCheckBox.Location = new Point(162, 26);
            caseSensitiveCheckBox.Name = "caseSensitiveCheckBox";
            caseSensitiveCheckBox.Size = new Size(162, 19);
            caseSensitiveCheckBox.TabIndex = 3;
            caseSensitiveCheckBox.Text = "Diferenciar maius./minus.";
            caseSensitiveCheckBox.UseVisualStyleBackColor = true;
            caseSensitiveCheckBox.CheckedChanged += CaseSensitiveCheckBox_CheckedChanged;
            // 
            // replaceFieldLabel
            // 
            replaceFieldLabel.AutoSize = true;
            replaceFieldLabel.Font = new Font("Segoe UI", 9.5F);
            replaceFieldLabel.ForeColor = Color.FromArgb(15, 23, 42);
            replaceFieldLabel.Location = new Point(366, 28);
            replaceFieldLabel.Name = "replaceFieldLabel";
            replaceFieldLabel.Size = new Size(87, 17);
            replaceFieldLabel.TabIndex = 4;
            replaceFieldLabel.Text = "Substituir por";
            // 
            // replaceTextBox
            // 
            replaceTextBox.BackColor = Color.White;
            replaceTextBox.BorderStyle = BorderStyle.FixedSingle;
            replaceTextBox.Font = new Font("Segoe UI", 10F);
            replaceTextBox.Location = new Point(366, 48);
            replaceTextBox.Name = "replaceTextBox";
            replaceTextBox.PlaceholderText = "Texto de substituicao";
            replaceTextBox.Size = new Size(330, 25);
            replaceTextBox.TabIndex = 5;
            replaceTextBox.TextChanged += ReplaceTextBox_TextChanged;
            replaceTextBox.KeyDown += ReplaceTextBox_KeyDown;
            // 
            // searchHelperLabel
            // 
            searchHelperLabel.AutoSize = true;
            searchHelperLabel.Font = new Font("Segoe UI", 9F);
            searchHelperLabel.ForeColor = Color.FromArgb(71, 85, 105);
            searchHelperLabel.Location = new Point(12, 80);
            searchHelperLabel.Name = "searchHelperLabel";
            searchHelperLabel.Size = new Size(189, 15);
            searchHelperLabel.TabIndex = 6;
            searchHelperLabel.Text = "Digite um termo e pressione Enter.";
            // 
            // searchButton
            // 
            searchButton.BackColor = SystemColors.GrayText;
            searchButton.Cursor = Cursors.Hand;
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.FlatStyle = FlatStyle.Flat;
            searchButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            searchButton.ForeColor = Color.White;
            searchButton.Location = new Point(12, 101);
            searchButton.Name = "searchButton";
            searchButton.Size = new Size(105, 27);
            searchButton.TabIndex = 7;
            searchButton.Text = "Pesquisar";
            searchButton.UseVisualStyleBackColor = false;
            searchButton.Click += SearchButton_Click;
            // 
            // nextMatchButton
            // 
            nextMatchButton.BackColor = Color.White;
            nextMatchButton.Cursor = Cursors.Hand;
            nextMatchButton.Enabled = false;
            nextMatchButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            nextMatchButton.FlatStyle = FlatStyle.Flat;
            nextMatchButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            nextMatchButton.ForeColor = Color.FromArgb(15, 23, 42);
            nextMatchButton.Location = new Point(123, 101);
            nextMatchButton.Name = "nextMatchButton";
            nextMatchButton.Size = new Size(105, 27);
            nextMatchButton.TabIndex = 8;
            nextMatchButton.Text = "Proximo";
            nextMatchButton.UseVisualStyleBackColor = false;
            nextMatchButton.Click += NextMatchButton_Click;
            // 
            // replaceCurrentButton
            // 
            replaceCurrentButton.BackColor = Color.White;
            replaceCurrentButton.Cursor = Cursors.Hand;
            replaceCurrentButton.Enabled = false;
            replaceCurrentButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            replaceCurrentButton.FlatStyle = FlatStyle.Flat;
            replaceCurrentButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            replaceCurrentButton.ForeColor = Color.FromArgb(15, 23, 42);
            replaceCurrentButton.Location = new Point(239, 101);
            replaceCurrentButton.Name = "replaceCurrentButton";
            replaceCurrentButton.Size = new Size(105, 27);
            replaceCurrentButton.TabIndex = 9;
            replaceCurrentButton.Text = "Substituir";
            replaceCurrentButton.UseVisualStyleBackColor = false;
            replaceCurrentButton.Click += ReplaceCurrentButton_Click;
            // 
            // replaceAllButton
            // 
            replaceAllButton.BackColor = Color.FromArgb(22, 163, 74);
            replaceAllButton.Cursor = Cursors.Hand;
            replaceAllButton.Enabled = false;
            replaceAllButton.FlatAppearance.BorderSize = 0;
            replaceAllButton.FlatStyle = FlatStyle.Flat;
            replaceAllButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            replaceAllButton.ForeColor = Color.White;
            replaceAllButton.Location = new Point(366, 101);
            replaceAllButton.Name = "replaceAllButton";
            replaceAllButton.Size = new Size(105, 27);
            replaceAllButton.TabIndex = 10;
            replaceAllButton.Text = "Substituir tudo";
            replaceAllButton.UseVisualStyleBackColor = false;
            replaceAllButton.Click += ReplaceAllButton_Click;
            // 
            // validateCharsetButton
            // 
            validateCharsetButton.BackColor = Color.FromArgb(37, 99, 235);
            validateCharsetButton.Cursor = Cursors.Hand;
            validateCharsetButton.FlatAppearance.BorderSize = 0;
            validateCharsetButton.FlatStyle = FlatStyle.Flat;
            validateCharsetButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            validateCharsetButton.ForeColor = Color.White;
            validateCharsetButton.Location = new Point(482, 101);
            validateCharsetButton.Name = "validateCharsetButton";
            validateCharsetButton.Size = new Size(152, 27);
            validateCharsetButton.TabIndex = 11;
            validateCharsetButton.Text = "Verificar charset/LF";
            validateCharsetButton.UseVisualStyleBackColor = false;
            validateCharsetButton.Click += ValidateCharsetButton_Click;
            // 
            // remapGroupBox
            // 
            remapGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            remapGroupBox.Controls.Add(remapTexturePreviewPictureBox);
            remapGroupBox.Controls.Add(remapSourceLabel);
            remapGroupBox.Controls.Add(remapSourceComboBox);
            remapGroupBox.Controls.Add(searchGroupBox);
            remapGroupBox.Controls.Add(remapTargetLabel);
            remapGroupBox.Controls.Add(remapTargetTextBox);
            remapGroupBox.Controls.Add(applyCharRemapButton);
            remapGroupBox.Controls.Add(remapHelperLabel);
            remapGroupBox.Controls.Add(remapLanguageCurrentLabel);
            remapGroupBox.Controls.Add(remapLanguageCurrentValueLabel);
            remapGroupBox.Controls.Add(remapLanguageTargetLabel);
            remapGroupBox.Controls.Add(remapLanguageTargetTextBox);
            remapGroupBox.Controls.Add(applyLanguageFlagsButton);
            remapGroupBox.Controls.Add(remapVariantDetailsLabel);
            remapGroupBox.Controls.Add(currentGlyphHeaderLabel);
            remapGroupBox.Controls.Add(currentGlyphHintLabel);
            remapGroupBox.Controls.Add(newCharacterHeaderLabel);
            remapGroupBox.Controls.Add(newCharacterHintLabel);
            remapGroupBox.Controls.Add(newCharacterLabel);
            remapGroupBox.Controls.Add(newCharacterTextBox);
            remapGroupBox.Controls.Add(newCharacterLanguageLabel);
            remapGroupBox.Controls.Add(newCharacterLanguageTextBox);
            remapGroupBox.Controls.Add(selectNewGlyphButton);
            remapGroupBox.Controls.Add(createNewCharacterButton);
            remapGroupBox.Controls.Add(updateSelectedGlyphButton);
            remapGroupBox.Controls.Add(removeCharacterButton);
            remapGroupBox.Controls.Add(remapSectionDividerPanel);
            remapGroupBox.Controls.Add(newCharacterBaseInfoLabel);
            remapGroupBox.Controls.Add(newCharacterSelectionLabel);
            remapGroupBox.Controls.Add(selectionAdjustLabel);
            remapGroupBox.Controls.Add(selectionAdjustStepTextBox);
            remapGroupBox.Controls.Add(selectionWidthDecreaseButton);
            remapGroupBox.Controls.Add(selectionWidthIncreaseButton);
            remapGroupBox.Controls.Add(selectionHeightDecreaseButton);
            remapGroupBox.Controls.Add(selectionHeightIncreaseButton);
            remapGroupBox.Controls.Add(selectionLeftDecreaseButton);
            remapGroupBox.Controls.Add(selectionLeftIncreaseButton);
            remapGroupBox.Controls.Add(selectionRightDecreaseButton);
            remapGroupBox.Controls.Add(selectionRightIncreaseButton);
            remapGroupBox.Controls.Add(selectionTopDecreaseButton);
            remapGroupBox.Controls.Add(selectionTopIncreaseButton);
            remapGroupBox.Controls.Add(selectionBottomDecreaseButton);
            remapGroupBox.Controls.Add(selectionBottomIncreaseButton);
            remapGroupBox.Controls.Add(resetSelectionToGlyphButton);
            remapGroupBox.Controls.Add(remapGlyphZoomPictureBox);
            remapGroupBox.Controls.Add(remapTexturePreviewLabel);
            remapGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            remapGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            remapGroupBox.Location = new Point(14, 114);
            remapGroupBox.Name = "remapGroupBox";
            remapGroupBox.Size = new Size(1184, 565);
            remapGroupBox.TabIndex = 4;
            remapGroupBox.TabStop = false;
            remapGroupBox.Text = "Remapeamento de caractere";
            // 
            // remapTexturePreviewPictureBox
            // 
            remapTexturePreviewPictureBox.BackColor = Color.FromArgb(248, 250, 252);
            remapTexturePreviewPictureBox.BorderStyle = BorderStyle.FixedSingle;
            remapTexturePreviewPictureBox.Location = new Point(15, 124);
            remapTexturePreviewPictureBox.Name = "remapTexturePreviewPictureBox";
            remapTexturePreviewPictureBox.Size = new Size(348, 399);
            remapTexturePreviewPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            remapTexturePreviewPictureBox.TabIndex = 36;
            remapTexturePreviewPictureBox.TabStop = false;
            // 
            // remapSourceLabel
            // 
            remapSourceLabel.AutoSize = true;
            remapSourceLabel.Font = new Font("Segoe UI", 9.5F);
            remapSourceLabel.ForeColor = Color.FromArgb(15, 23, 42);
            remapSourceLabel.Location = new Point(645, 27);
            remapSourceLabel.Name = "remapSourceLabel";
            remapSourceLabel.Size = new Size(64, 17);
            remapSourceLabel.TabIndex = 0;
            remapSourceLabel.Text = "Caractere";
            // 
            // remapSourceComboBox
            // 
            remapSourceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            remapSourceComboBox.Font = new Font("Segoe UI", 10F);
            remapSourceComboBox.FormattingEnabled = true;
            remapSourceComboBox.Location = new Point(713, 24);
            remapSourceComboBox.Name = "remapSourceComboBox";
            remapSourceComboBox.Size = new Size(450, 25);
            remapSourceComboBox.TabIndex = 1;
            remapSourceComboBox.SelectedIndexChanged += RemapSourceComboBox_SelectedIndexChanged;
            // 
            // remapTargetLabel
            // 
            remapTargetLabel.AutoSize = true;
            remapTargetLabel.Font = new Font("Segoe UI", 9.5F);
            remapTargetLabel.ForeColor = Color.FromArgb(15, 23, 42);
            remapTargetLabel.Location = new Point(645, 73);
            remapTargetLabel.Name = "remapTargetLabel";
            remapTargetLabel.Size = new Size(98, 17);
            remapTargetLabel.TabIndex = 2;
            remapTargetLabel.Text = "Novo caractere";
            // 
            // remapTargetTextBox
            // 
            remapTargetTextBox.BorderStyle = BorderStyle.FixedSingle;
            remapTargetTextBox.Font = new Font("Segoe UI", 10F);
            remapTargetTextBox.Location = new Point(748, 69);
            remapTargetTextBox.MaxLength = 2;
            remapTargetTextBox.Name = "remapTargetTextBox";
            remapTargetTextBox.PlaceholderText = "Ex.: ã";
            remapTargetTextBox.Size = new Size(59, 25);
            remapTargetTextBox.TabIndex = 3;
            remapTargetTextBox.TextChanged += RemapTargetTextBox_TextChanged;
            // 
            // applyCharRemapButton
            // 
            applyCharRemapButton.BackColor = SystemColors.GrayText;
            applyCharRemapButton.Cursor = Cursors.Hand;
            applyCharRemapButton.Enabled = false;
            applyCharRemapButton.FlatAppearance.BorderSize = 0;
            applyCharRemapButton.FlatStyle = FlatStyle.Flat;
            applyCharRemapButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            applyCharRemapButton.ForeColor = Color.White;
            applyCharRemapButton.Location = new Point(821, 69);
            applyCharRemapButton.Name = "applyCharRemapButton";
            applyCharRemapButton.Size = new Size(59, 27);
            applyCharRemapButton.TabIndex = 4;
            applyCharRemapButton.Text = "Aplicar";
            applyCharRemapButton.UseVisualStyleBackColor = false;
            applyCharRemapButton.Click += ApplyCharRemapButton_Click;
            // 
            // remapHelperLabel
            // 
            remapHelperLabel.AutoSize = true;
            remapHelperLabel.Font = new Font("Segoe UI", 9F);
            remapHelperLabel.ForeColor = Color.FromArgb(71, 85, 105);
            remapHelperLabel.Location = new Point(15, 535);
            remapHelperLabel.Name = "remapHelperLabel";
            remapHelperLabel.Size = new Size(417, 15);
            remapHelperLabel.TabIndex = 39;
            remapHelperLabel.Text = "Troque um caractere existente ou cadastre um novo glifo usando a atlas atual.";
            // 
            // remapLanguageCurrentLabel
            // 
            remapLanguageCurrentLabel.AutoSize = true;
            remapLanguageCurrentLabel.Font = new Font("Segoe UI", 9.5F);
            remapLanguageCurrentLabel.ForeColor = Color.FromArgb(15, 23, 42);
            remapLanguageCurrentLabel.Location = new Point(893, 106);
            remapLanguageCurrentLabel.Name = "remapLanguageCurrentLabel";
            remapLanguageCurrentLabel.Size = new Size(127, 17);
            remapLanguageCurrentLabel.TabIndex = 5;
            remapLanguageCurrentLabel.Text = "LanguageFlags atual";
            // 
            // remapLanguageCurrentValueLabel
            // 
            remapLanguageCurrentValueLabel.AutoSize = true;
            remapLanguageCurrentValueLabel.Font = new Font("Segoe UI", 9F);
            remapLanguageCurrentValueLabel.ForeColor = Color.FromArgb(37, 99, 235);
            remapLanguageCurrentValueLabel.Location = new Point(1024, 107);
            remapLanguageCurrentValueLabel.Name = "remapLanguageCurrentValueLabel";
            remapLanguageCurrentValueLabel.Size = new Size(86, 15);
            remapLanguageCurrentValueLabel.TabIndex = 6;
            remapLanguageCurrentValueLabel.Text = "Nenhum valor.";
            // 
            // remapLanguageTargetLabel
            // 
            remapLanguageTargetLabel.AutoSize = true;
            remapLanguageTargetLabel.Font = new Font("Segoe UI", 9.5F);
            remapLanguageTargetLabel.ForeColor = Color.FromArgb(15, 23, 42);
            remapLanguageTargetLabel.Location = new Point(893, 73);
            remapLanguageTargetLabel.Name = "remapLanguageTargetLabel";
            remapLanguageTargetLabel.Size = new Size(131, 17);
            remapLanguageTargetLabel.TabIndex = 7;
            remapLanguageTargetLabel.Text = "Novo LanguageFlags";
            // 
            // remapLanguageTargetTextBox
            // 
            remapLanguageTargetTextBox.BorderStyle = BorderStyle.FixedSingle;
            remapLanguageTargetTextBox.Font = new Font("Segoe UI", 10F);
            remapLanguageTargetTextBox.Location = new Point(1030, 71);
            remapLanguageTargetTextBox.Name = "remapLanguageTargetTextBox";
            remapLanguageTargetTextBox.PlaceholderText = "Ex.: 12 ou 0x000C";
            remapLanguageTargetTextBox.Size = new Size(59, 25);
            remapLanguageTargetTextBox.TabIndex = 8;
            remapLanguageTargetTextBox.TextChanged += RemapLanguageTargetTextBox_TextChanged;
            // 
            // applyLanguageFlagsButton
            // 
            applyLanguageFlagsButton.BackColor = SystemColors.GrayText;
            applyLanguageFlagsButton.Cursor = Cursors.Hand;
            applyLanguageFlagsButton.Enabled = false;
            applyLanguageFlagsButton.FlatAppearance.BorderSize = 0;
            applyLanguageFlagsButton.FlatStyle = FlatStyle.Flat;
            applyLanguageFlagsButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            applyLanguageFlagsButton.ForeColor = Color.White;
            applyLanguageFlagsButton.Location = new Point(1099, 69);
            applyLanguageFlagsButton.Name = "applyLanguageFlagsButton";
            applyLanguageFlagsButton.Size = new Size(59, 27);
            applyLanguageFlagsButton.TabIndex = 9;
            applyLanguageFlagsButton.Text = "Aplicar";
            applyLanguageFlagsButton.UseVisualStyleBackColor = false;
            applyLanguageFlagsButton.Click += ApplyLanguageFlagsButton_Click;
            // 
            // remapVariantDetailsLabel
            // 
            remapVariantDetailsLabel.AutoSize = true;
            remapVariantDetailsLabel.Font = new Font("Segoe UI", 9F);
            remapVariantDetailsLabel.ForeColor = Color.FromArgb(37, 99, 235);
            remapVariantDetailsLabel.Location = new Point(675, 124);
            remapVariantDetailsLabel.Name = "remapVariantDetailsLabel";
            remapVariantDetailsLabel.Size = new Size(268, 15);
            remapVariantDetailsLabel.TabIndex = 10;
            remapVariantDetailsLabel.Text = "Detalhes da variante selecionada aparecerao aqui.";
            // 
            // currentGlyphHeaderLabel
            // 
            currentGlyphHeaderLabel.AutoSize = true;
            currentGlyphHeaderLabel.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            currentGlyphHeaderLabel.ForeColor = Color.FromArgb(15, 23, 42);
            currentGlyphHeaderLabel.Location = new Point(399, 180);
            currentGlyphHeaderLabel.Name = "currentGlyphHeaderLabel";
            currentGlyphHeaderLabel.Size = new Size(225, 17);
            currentGlyphHeaderLabel.TabIndex = 11;
            currentGlyphHeaderLabel.Text = "Alterar glifo da variante selecionada";
            // 
            // currentGlyphHintLabel
            // 
            currentGlyphHintLabel.AutoSize = true;
            currentGlyphHintLabel.Font = new Font("Segoe UI", 8.5F);
            currentGlyphHintLabel.ForeColor = Color.FromArgb(71, 85, 105);
            currentGlyphHintLabel.Location = new Point(399, 200);
            currentGlyphHintLabel.Name = "currentGlyphHintLabel";
            currentGlyphHintLabel.Size = new Size(311, 15);
            currentGlyphHintLabel.TabIndex = 12;
            currentGlyphHintLabel.Text = "Use a area marcada na atlas para atualizar a variante atual.";
            // 
            // newCharacterHeaderLabel
            // 
            newCharacterHeaderLabel.AutoSize = true;
            newCharacterHeaderLabel.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            newCharacterHeaderLabel.ForeColor = Color.FromArgb(15, 23, 42);
            newCharacterHeaderLabel.Location = new Point(12, 32);
            newCharacterHeaderLabel.Name = "newCharacterHeaderLabel";
            newCharacterHeaderLabel.Size = new Size(377, 17);
            newCharacterHeaderLabel.TabIndex = 13;
            newCharacterHeaderLabel.Text = "Cadastrar novo caractere usando a variante atual como base";
            // 
            // newCharacterHintLabel
            // 
            newCharacterHintLabel.AutoSize = true;
            newCharacterHintLabel.Font = new Font("Segoe UI", 8.5F);
            newCharacterHintLabel.ForeColor = Color.FromArgb(71, 85, 105);
            newCharacterHintLabel.Location = new Point(12, 52);
            newCharacterHintLabel.Name = "newCharacterHintLabel";
            newCharacterHintLabel.Size = new Size(354, 15);
            newCharacterHintLabel.TabIndex = 14;
            newCharacterHintLabel.Text = "Preencha o caractere novo e use a area marcada como novo glifo.";
            // 
            // newCharacterLabel
            // 
            newCharacterLabel.AutoSize = true;
            newCharacterLabel.Font = new Font("Segoe UI", 9.5F);
            newCharacterLabel.ForeColor = Color.FromArgb(15, 23, 42);
            newCharacterLabel.Location = new Point(12, 76);
            newCharacterLabel.Name = "newCharacterLabel";
            newCharacterLabel.Size = new Size(98, 17);
            newCharacterLabel.TabIndex = 15;
            newCharacterLabel.Text = "Novo caractere";
            // 
            // newCharacterTextBox
            // 
            newCharacterTextBox.BorderStyle = BorderStyle.FixedSingle;
            newCharacterTextBox.Font = new Font("Segoe UI", 10F);
            newCharacterTextBox.Location = new Point(116, 73);
            newCharacterTextBox.MaxLength = 2;
            newCharacterTextBox.Name = "newCharacterTextBox";
            newCharacterTextBox.PlaceholderText = "Ex.: ã";
            newCharacterTextBox.Size = new Size(52, 25);
            newCharacterTextBox.TabIndex = 16;
            newCharacterTextBox.TextChanged += NewCharacterTextBox_TextChanged;
            // 
            // newCharacterLanguageLabel
            // 
            newCharacterLanguageLabel.AutoSize = true;
            newCharacterLanguageLabel.Font = new Font("Segoe UI", 9.5F);
            newCharacterLanguageLabel.ForeColor = Color.FromArgb(15, 23, 42);
            newCharacterLanguageLabel.Location = new Point(180, 76);
            newCharacterLanguageLabel.Name = "newCharacterLanguageLabel";
            newCharacterLanguageLabel.Size = new Size(95, 17);
            newCharacterLanguageLabel.TabIndex = 17;
            newCharacterLanguageLabel.Text = "LanguageFlags";
            // 
            // newCharacterLanguageTextBox
            // 
            newCharacterLanguageTextBox.BorderStyle = BorderStyle.FixedSingle;
            newCharacterLanguageTextBox.Font = new Font("Segoe UI", 10F);
            newCharacterLanguageTextBox.Location = new Point(281, 73);
            newCharacterLanguageTextBox.Name = "newCharacterLanguageTextBox";
            newCharacterLanguageTextBox.PlaceholderText = "Ex.: 12";
            newCharacterLanguageTextBox.Size = new Size(57, 25);
            newCharacterLanguageTextBox.TabIndex = 18;
            newCharacterLanguageTextBox.TextChanged += NewCharacterLanguageTextBox_TextChanged;
            // 
            // selectNewGlyphButton
            // 
            selectNewGlyphButton.BackColor = Color.White;
            selectNewGlyphButton.Cursor = Cursors.Hand;
            selectNewGlyphButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectNewGlyphButton.FlatStyle = FlatStyle.Flat;
            selectNewGlyphButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            selectNewGlyphButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectNewGlyphButton.Location = new Point(399, 73);
            selectNewGlyphButton.Name = "selectNewGlyphButton";
            selectNewGlyphButton.Size = new Size(119, 27);
            selectNewGlyphButton.TabIndex = 19;
            selectNewGlyphButton.Text = "Selecionar glifo";
            selectNewGlyphButton.UseVisualStyleBackColor = false;
            selectNewGlyphButton.Click += SelectNewGlyphButton_Click;
            // 
            // createNewCharacterButton
            // 
            createNewCharacterButton.BackColor = Color.FromArgb(22, 163, 74);
            createNewCharacterButton.Cursor = Cursors.Hand;
            createNewCharacterButton.Enabled = false;
            createNewCharacterButton.FlatAppearance.BorderSize = 0;
            createNewCharacterButton.FlatStyle = FlatStyle.Flat;
            createNewCharacterButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            createNewCharacterButton.ForeColor = Color.White;
            createNewCharacterButton.Location = new Point(399, 39);
            createNewCharacterButton.Name = "createNewCharacterButton";
            createNewCharacterButton.Size = new Size(119, 27);
            createNewCharacterButton.TabIndex = 20;
            createNewCharacterButton.Text = "Cadastrar caractere";
            createNewCharacterButton.UseVisualStyleBackColor = false;
            createNewCharacterButton.Click += CreateNewCharacterButton_Click;
            // 
            // updateSelectedGlyphButton
            // 
            updateSelectedGlyphButton.BackColor = Color.FromArgb(37, 99, 235);
            updateSelectedGlyphButton.Cursor = Cursors.Hand;
            updateSelectedGlyphButton.Enabled = false;
            updateSelectedGlyphButton.FlatAppearance.BorderSize = 0;
            updateSelectedGlyphButton.FlatStyle = FlatStyle.Flat;
            updateSelectedGlyphButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            updateSelectedGlyphButton.ForeColor = Color.White;
            updateSelectedGlyphButton.Location = new Point(399, 220);
            updateSelectedGlyphButton.Name = "updateSelectedGlyphButton";
            updateSelectedGlyphButton.Size = new Size(121, 27);
            updateSelectedGlyphButton.TabIndex = 21;
            updateSelectedGlyphButton.Text = "Atualizar glifo atual";
            updateSelectedGlyphButton.UseVisualStyleBackColor = false;
            updateSelectedGlyphButton.Click += UpdateSelectedGlyphButton_Click;
            // 
            // removeCharacterButton
            // 
            removeCharacterButton.BackColor = Color.FromArgb(185, 28, 28);
            removeCharacterButton.Cursor = Cursors.Hand;
            removeCharacterButton.FlatAppearance.BorderSize = 0;
            removeCharacterButton.FlatStyle = FlatStyle.Flat;
            removeCharacterButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            removeCharacterButton.ForeColor = Color.White;
            removeCharacterButton.Location = new Point(526, 219);
            removeCharacterButton.Name = "removeCharacterButton";
            removeCharacterButton.Size = new Size(118, 27);
            removeCharacterButton.TabIndex = 22;
            removeCharacterButton.Text = "Remover caractere";
            removeCharacterButton.UseVisualStyleBackColor = false;
            removeCharacterButton.Click += RemoveCharacterButton_Click;
            // 
            // remapSectionDividerPanel
            // 
            remapSectionDividerPanel.BackColor = Color.FromArgb(226, 232, 240);
            remapSectionDividerPanel.Location = new Point(638, 32);
            remapSectionDividerPanel.Name = "remapSectionDividerPanel";
            remapSectionDividerPanel.Size = new Size(1, 74);
            remapSectionDividerPanel.TabIndex = 23;
            // 
            // newCharacterBaseInfoLabel
            // 
            newCharacterBaseInfoLabel.AutoSize = true;
            newCharacterBaseInfoLabel.Font = new Font("Segoe UI", 9F);
            newCharacterBaseInfoLabel.ForeColor = Color.FromArgb(37, 99, 235);
            newCharacterBaseInfoLabel.Location = new Point(12, 107);
            newCharacterBaseInfoLabel.Name = "newCharacterBaseInfoLabel";
            newCharacterBaseInfoLabel.Size = new Size(362, 15);
            newCharacterBaseInfoLabel.TabIndex = 24;
            newCharacterBaseInfoLabel.Text = "Base atual: selecione uma variante para herdar TextureID e metricas.";
            // 
            // newCharacterSelectionLabel
            // 
            newCharacterSelectionLabel.AutoSize = true;
            newCharacterSelectionLabel.Font = new Font("Segoe UI", 9F);
            newCharacterSelectionLabel.ForeColor = Color.FromArgb(71, 85, 105);
            newCharacterSelectionLabel.Location = new Point(12, 124);
            newCharacterSelectionLabel.Name = "newCharacterSelectionLabel";
            newCharacterSelectionLabel.Size = new Size(388, 15);
            newCharacterSelectionLabel.TabIndex = 25;
            newCharacterSelectionLabel.Text = "Selecao do glifo: Escolha uma variante para atualizar ou usar como base.";
            // 
            // selectionAdjustLabel
            // 
            selectionAdjustLabel.AutoSize = true;
            selectionAdjustLabel.Font = new Font("Segoe UI", 9F);
            selectionAdjustLabel.ForeColor = Color.FromArgb(15, 23, 42);
            selectionAdjustLabel.Location = new Point(748, 220);
            selectionAdjustLabel.Name = "selectionAdjustLabel";
            selectionAdjustLabel.Size = new Size(191, 15);
            selectionAdjustLabel.TabIndex = 26;
            selectionAdjustLabel.Text = "Ajuste fino (px) | setas / Shift+setas";
            // 
            // selectionAdjustStepTextBox
            // 
            selectionAdjustStepTextBox.BorderStyle = BorderStyle.FixedSingle;
            selectionAdjustStepTextBox.Font = new Font("Segoe UI", 9F);
            selectionAdjustStepTextBox.Location = new Point(913, 395);
            selectionAdjustStepTextBox.Name = "selectionAdjustStepTextBox";
            selectionAdjustStepTextBox.Size = new Size(30, 23);
            selectionAdjustStepTextBox.TabIndex = 27;
            selectionAdjustStepTextBox.Text = "1";
            selectionAdjustStepTextBox.TextChanged += SelectionAdjustStepTextBox_TextChanged;
            // 
            // selectionWidthDecreaseButton
            // 
            selectionWidthDecreaseButton.BackColor = Color.White;
            selectionWidthDecreaseButton.Cursor = Cursors.Hand;
            selectionWidthDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionWidthDecreaseButton.FlatStyle = FlatStyle.Flat;
            selectionWidthDecreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionWidthDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionWidthDecreaseButton.Location = new Point(713, 272);
            selectionWidthDecreaseButton.Name = "selectionWidthDecreaseButton";
            selectionWidthDecreaseButton.Size = new Size(58, 24);
            selectionWidthDecreaseButton.TabIndex = 28;
            selectionWidthDecreaseButton.Tag = "width-";
            selectionWidthDecreaseButton.Text = "Larg -";
            selectionWidthDecreaseButton.UseVisualStyleBackColor = false;
            selectionWidthDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionWidthIncreaseButton
            // 
            selectionWidthIncreaseButton.BackColor = Color.White;
            selectionWidthIncreaseButton.Cursor = Cursors.Hand;
            selectionWidthIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionWidthIncreaseButton.FlatStyle = FlatStyle.Flat;
            selectionWidthIncreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionWidthIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionWidthIncreaseButton.Location = new Point(713, 250);
            selectionWidthIncreaseButton.Name = "selectionWidthIncreaseButton";
            selectionWidthIncreaseButton.Size = new Size(58, 24);
            selectionWidthIncreaseButton.TabIndex = 29;
            selectionWidthIncreaseButton.Tag = "width+";
            selectionWidthIncreaseButton.Text = "Larg +";
            selectionWidthIncreaseButton.UseVisualStyleBackColor = false;
            selectionWidthIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionHeightDecreaseButton
            // 
            selectionHeightDecreaseButton.BackColor = Color.White;
            selectionHeightDecreaseButton.Cursor = Cursors.Hand;
            selectionHeightDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionHeightDecreaseButton.FlatStyle = FlatStyle.Flat;
            selectionHeightDecreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionHeightDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionHeightDecreaseButton.Location = new Point(926, 272);
            selectionHeightDecreaseButton.Name = "selectionHeightDecreaseButton";
            selectionHeightDecreaseButton.Size = new Size(58, 24);
            selectionHeightDecreaseButton.TabIndex = 30;
            selectionHeightDecreaseButton.Tag = "height-";
            selectionHeightDecreaseButton.Text = "Alt -";
            selectionHeightDecreaseButton.UseVisualStyleBackColor = false;
            selectionHeightDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionHeightIncreaseButton
            // 
            selectionHeightIncreaseButton.BackColor = Color.White;
            selectionHeightIncreaseButton.Cursor = Cursors.Hand;
            selectionHeightIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionHeightIncreaseButton.FlatStyle = FlatStyle.Flat;
            selectionHeightIncreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionHeightIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionHeightIncreaseButton.Location = new Point(926, 250);
            selectionHeightIncreaseButton.Name = "selectionHeightIncreaseButton";
            selectionHeightIncreaseButton.Size = new Size(58, 24);
            selectionHeightIncreaseButton.TabIndex = 31;
            selectionHeightIncreaseButton.Tag = "height+";
            selectionHeightIncreaseButton.Text = "Alt +";
            selectionHeightIncreaseButton.UseVisualStyleBackColor = false;
            selectionHeightIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionLeftDecreaseButton
            // 
            selectionLeftDecreaseButton.BackColor = Color.White;
            selectionLeftDecreaseButton.Cursor = Cursors.Hand;
            selectionLeftDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionLeftDecreaseButton.FlatStyle = FlatStyle.Flat;
            selectionLeftDecreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionLeftDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionLeftDecreaseButton.Location = new Point(713, 347);
            selectionLeftDecreaseButton.Name = "selectionLeftDecreaseButton";
            selectionLeftDecreaseButton.Size = new Size(58, 24);
            selectionLeftDecreaseButton.TabIndex = 32;
            selectionLeftDecreaseButton.Tag = "left-";
            selectionLeftDecreaseButton.Text = "Esq -";
            selectionLeftDecreaseButton.UseVisualStyleBackColor = false;
            selectionLeftDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionLeftIncreaseButton
            // 
            selectionLeftIncreaseButton.BackColor = Color.White;
            selectionLeftIncreaseButton.Cursor = Cursors.Hand;
            selectionLeftIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionLeftIncreaseButton.FlatStyle = FlatStyle.Flat;
            selectionLeftIncreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionLeftIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionLeftIncreaseButton.Location = new Point(712, 319);
            selectionLeftIncreaseButton.Name = "selectionLeftIncreaseButton";
            selectionLeftIncreaseButton.Size = new Size(58, 24);
            selectionLeftIncreaseButton.TabIndex = 33;
            selectionLeftIncreaseButton.Tag = "left+";
            selectionLeftIncreaseButton.Text = "Esq +";
            selectionLeftIncreaseButton.UseVisualStyleBackColor = false;
            selectionLeftIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionRightDecreaseButton
            // 
            selectionRightDecreaseButton.BackColor = Color.White;
            selectionRightDecreaseButton.Cursor = Cursors.Hand;
            selectionRightDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionRightDecreaseButton.FlatStyle = FlatStyle.Flat;
            selectionRightDecreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionRightDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionRightDecreaseButton.Location = new Point(931, 347);
            selectionRightDecreaseButton.Name = "selectionRightDecreaseButton";
            selectionRightDecreaseButton.Size = new Size(58, 24);
            selectionRightDecreaseButton.TabIndex = 34;
            selectionRightDecreaseButton.Tag = "right-";
            selectionRightDecreaseButton.Text = "Dir -";
            selectionRightDecreaseButton.UseVisualStyleBackColor = false;
            selectionRightDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionRightIncreaseButton
            // 
            selectionRightIncreaseButton.BackColor = Color.White;
            selectionRightIncreaseButton.Cursor = Cursors.Hand;
            selectionRightIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionRightIncreaseButton.FlatStyle = FlatStyle.Flat;
            selectionRightIncreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionRightIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionRightIncreaseButton.Location = new Point(931, 319);
            selectionRightIncreaseButton.Name = "selectionRightIncreaseButton";
            selectionRightIncreaseButton.Size = new Size(58, 24);
            selectionRightIncreaseButton.TabIndex = 35;
            selectionRightIncreaseButton.Tag = "right+";
            selectionRightIncreaseButton.Text = "Dir +";
            selectionRightIncreaseButton.UseVisualStyleBackColor = false;
            selectionRightIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionTopDecreaseButton
            // 
            selectionTopDecreaseButton.BackColor = Color.White;
            selectionTopDecreaseButton.Cursor = Cursors.Hand;
            selectionTopDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionTopDecreaseButton.FlatStyle = FlatStyle.Flat;
            selectionTopDecreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionTopDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionTopDecreaseButton.Location = new Point(791, 238);
            selectionTopDecreaseButton.Name = "selectionTopDecreaseButton";
            selectionTopDecreaseButton.Size = new Size(56, 24);
            selectionTopDecreaseButton.TabIndex = 36;
            selectionTopDecreaseButton.Tag = "top-";
            selectionTopDecreaseButton.Text = "Topo -";
            selectionTopDecreaseButton.UseVisualStyleBackColor = false;
            selectionTopDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionTopIncreaseButton
            // 
            selectionTopIncreaseButton.BackColor = Color.White;
            selectionTopIncreaseButton.Cursor = Cursors.Hand;
            selectionTopIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionTopIncreaseButton.FlatStyle = FlatStyle.Flat;
            selectionTopIncreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionTopIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionTopIncreaseButton.Location = new Point(849, 238);
            selectionTopIncreaseButton.Name = "selectionTopIncreaseButton";
            selectionTopIncreaseButton.Size = new Size(56, 24);
            selectionTopIncreaseButton.TabIndex = 37;
            selectionTopIncreaseButton.Tag = "top+";
            selectionTopIncreaseButton.Text = "Topo +";
            selectionTopIncreaseButton.UseVisualStyleBackColor = false;
            selectionTopIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionBottomDecreaseButton
            // 
            selectionBottomDecreaseButton.BackColor = Color.White;
            selectionBottomDecreaseButton.Cursor = Cursors.Hand;
            selectionBottomDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionBottomDecreaseButton.FlatStyle = FlatStyle.Flat;
            selectionBottomDecreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionBottomDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionBottomDecreaseButton.Location = new Point(789, 395);
            selectionBottomDecreaseButton.Name = "selectionBottomDecreaseButton";
            selectionBottomDecreaseButton.Size = new Size(58, 24);
            selectionBottomDecreaseButton.TabIndex = 38;
            selectionBottomDecreaseButton.Tag = "bottom-";
            selectionBottomDecreaseButton.Text = "Base -";
            selectionBottomDecreaseButton.UseVisualStyleBackColor = false;
            selectionBottomDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionBottomIncreaseButton
            // 
            selectionBottomIncreaseButton.BackColor = Color.White;
            selectionBottomIncreaseButton.Cursor = Cursors.Hand;
            selectionBottomIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            selectionBottomIncreaseButton.FlatStyle = FlatStyle.Flat;
            selectionBottomIncreaseButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            selectionBottomIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionBottomIncreaseButton.Location = new Point(849, 395);
            selectionBottomIncreaseButton.Name = "selectionBottomIncreaseButton";
            selectionBottomIncreaseButton.Size = new Size(58, 24);
            selectionBottomIncreaseButton.TabIndex = 39;
            selectionBottomIncreaseButton.Tag = "bottom+";
            selectionBottomIncreaseButton.Text = "Base +";
            selectionBottomIncreaseButton.UseVisualStyleBackColor = false;
            selectionBottomIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // resetSelectionToGlyphButton
            // 
            resetSelectionToGlyphButton.BackColor = Color.White;
            resetSelectionToGlyphButton.Cursor = Cursors.Hand;
            resetSelectionToGlyphButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resetSelectionToGlyphButton.FlatStyle = FlatStyle.Flat;
            resetSelectionToGlyphButton.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            resetSelectionToGlyphButton.ForeColor = Color.FromArgb(15, 23, 42);
            resetSelectionToGlyphButton.Location = new Point(949, 395);
            resetSelectionToGlyphButton.Name = "resetSelectionToGlyphButton";
            resetSelectionToGlyphButton.Size = new Size(92, 24);
            resetSelectionToGlyphButton.TabIndex = 40;
            resetSelectionToGlyphButton.Text = "Resetar glifo";
            resetSelectionToGlyphButton.UseVisualStyleBackColor = false;
            resetSelectionToGlyphButton.Click += ResetSelectionToGlyphButton_Click;
            // 
            // remapGlyphZoomPictureBox
            // 
            remapGlyphZoomPictureBox.BackColor = Color.FromArgb(248, 250, 252);
            remapGlyphZoomPictureBox.BorderStyle = BorderStyle.FixedSingle;
            remapGlyphZoomPictureBox.Location = new Point(776, 265);
            remapGlyphZoomPictureBox.Name = "remapGlyphZoomPictureBox";
            remapGlyphZoomPictureBox.Size = new Size(144, 124);
            remapGlyphZoomPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            remapGlyphZoomPictureBox.TabIndex = 37;
            remapGlyphZoomPictureBox.TabStop = false;
            // 
            // remapTexturePreviewLabel
            // 
            remapTexturePreviewLabel.Font = new Font("Segoe UI", 9F);
            remapTexturePreviewLabel.ForeColor = Color.FromArgb(71, 85, 105);
            remapTexturePreviewLabel.Location = new Point(369, 272);
            remapTexturePreviewLabel.Name = "remapTexturePreviewLabel";
            remapTexturePreviewLabel.Size = new Size(310, 119);
            remapTexturePreviewLabel.TabIndex = 41;
            remapTexturePreviewLabel.Text = "A posicao da letra na textura aparecera aqui.";
            // 
            // navigationGroupBox
            // 
            navigationGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            navigationGroupBox.Controls.Add(navigationSummaryLabel);
            navigationGroupBox.Controls.Add(eventTreeView);
            navigationGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            navigationGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            navigationGroupBox.Location = new Point(14, 685);
            navigationGroupBox.Name = "navigationGroupBox";
            navigationGroupBox.Size = new Size(419, 302);
            navigationGroupBox.TabIndex = 5;
            navigationGroupBox.TabStop = false;
            navigationGroupBox.Text = "Navegacao";
            // 
            // navigationSummaryLabel
            // 
            navigationSummaryLabel.AutoSize = true;
            navigationSummaryLabel.Font = new Font("Segoe UI", 9F);
            navigationSummaryLabel.ForeColor = Color.FromArgb(71, 85, 105);
            navigationSummaryLabel.Location = new Point(12, 27);
            navigationSummaryLabel.Name = "navigationSummaryLabel";
            navigationSummaryLabel.Size = new Size(288, 15);
            navigationSummaryLabel.TabIndex = 0;
            navigationSummaryLabel.Text = "Carregue um arquivo MCD para visualizar os eventos.";
            // 
            // eventTreeView
            // 
            eventTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            eventTreeView.BackColor = Color.White;
            eventTreeView.BorderStyle = BorderStyle.FixedSingle;
            eventTreeView.Font = new Font("Segoe UI", 9.5F);
            eventTreeView.HideSelection = false;
            eventTreeView.Location = new Point(12, 50);
            eventTreeView.Name = "eventTreeView";
            eventTreeView.Size = new Size(395, 246);
            eventTreeView.TabIndex = 1;
            eventTreeView.AfterSelect += EventTreeView_AfterSelect;
            // 
            // editorGroupBox
            // 
            editorGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            editorGroupBox.Controls.Add(editorFieldLabel);
            editorGroupBox.Controls.Add(editorRequiredLabel);
            editorGroupBox.Controls.Add(editorHelperLabel);
            editorGroupBox.Controls.Add(selectedEntryLabel);
            editorGroupBox.Controls.Add(editorPreviewInfoLabel);
            editorGroupBox.Controls.Add(editorPreviewPictureBox);
            editorGroupBox.Controls.Add(textTextBox);
            editorGroupBox.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            editorGroupBox.ForeColor = Color.FromArgb(15, 23, 42);
            editorGroupBox.Location = new Point(439, 685);
            editorGroupBox.Name = "editorGroupBox";
            editorGroupBox.Size = new Size(759, 302);
            editorGroupBox.TabIndex = 6;
            editorGroupBox.TabStop = false;
            editorGroupBox.Text = "Edicao";
            // 
            // editorFieldLabel
            // 
            editorFieldLabel.AutoSize = true;
            editorFieldLabel.Font = new Font("Segoe UI", 9.5F);
            editorFieldLabel.ForeColor = Color.FromArgb(15, 23, 42);
            editorFieldLabel.Location = new Point(12, 27);
            editorFieldLabel.Name = "editorFieldLabel";
            editorFieldLabel.Size = new Size(95, 17);
            editorFieldLabel.TabIndex = 0;
            editorFieldLabel.Text = "Texto da string";
            // 
            // editorRequiredLabel
            // 
            editorRequiredLabel.AutoSize = true;
            editorRequiredLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            editorRequiredLabel.ForeColor = Color.FromArgb(185, 28, 28);
            editorRequiredLabel.Location = new Point(107, 26);
            editorRequiredLabel.Name = "editorRequiredLabel";
            editorRequiredLabel.Size = new Size(15, 19);
            editorRequiredLabel.TabIndex = 1;
            editorRequiredLabel.Text = "*";
            // 
            // editorHelperLabel
            // 
            editorHelperLabel.AutoSize = true;
            editorHelperLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            editorHelperLabel.ForeColor = Color.FromArgb(71, 85, 105);
            editorHelperLabel.Location = new Point(129, 30);
            editorHelperLabel.Name = "editorHelperLabel";
            editorHelperLabel.Size = new Size(293, 15);
            editorHelperLabel.TabIndex = 2;
            editorHelperLabel.Text = "Selecione uma string para comecar a editar o texto.";
            // 
            // selectedEntryLabel
            // 
            selectedEntryLabel.AutoSize = true;
            selectedEntryLabel.Font = new Font("Segoe UI", 9F);
            selectedEntryLabel.ForeColor = Color.FromArgb(71, 85, 105);
            selectedEntryLabel.Location = new Point(12, 50);
            selectedEntryLabel.Name = "selectedEntryLabel";
            selectedEntryLabel.Size = new Size(158, 15);
            selectedEntryLabel.TabIndex = 3;
            selectedEntryLabel.Text = "Nenhuma string selecionada";
            // 
            // editorPreviewInfoLabel
            // 
            editorPreviewInfoLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            editorPreviewInfoLabel.AutoEllipsis = true;
            editorPreviewInfoLabel.Font = new Font("Segoe UI", 8.75F);
            editorPreviewInfoLabel.ForeColor = Color.FromArgb(37, 99, 235);
            editorPreviewInfoLabel.Location = new Point(12, 117);
            editorPreviewInfoLabel.Name = "editorPreviewInfoLabel";
            editorPreviewInfoLabel.Size = new Size(735, 18);
            editorPreviewInfoLabel.TabIndex = 5;
            editorPreviewInfoLabel.Text = "Linha azul = baseline. A pre-visualizacao aparecera aqui abaixo.";
            // 
            // editorPreviewPictureBox
            // 
            editorPreviewPictureBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            editorPreviewPictureBox.BackColor = Color.FromArgb(248, 250, 252);
            editorPreviewPictureBox.BorderStyle = BorderStyle.FixedSingle;
            editorPreviewPictureBox.Location = new Point(12, 138);
            editorPreviewPictureBox.Name = "editorPreviewPictureBox";
            editorPreviewPictureBox.Size = new Size(735, 158);
            editorPreviewPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            editorPreviewPictureBox.TabIndex = 6;
            editorPreviewPictureBox.TabStop = false;
            // 
            // textTextBox
            // 
            textTextBox.AcceptsReturn = true;
            textTextBox.AcceptsTab = true;
            textTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textTextBox.BackColor = Color.White;
            textTextBox.BorderStyle = BorderStyle.FixedSingle;
            textTextBox.Enabled = false;
            textTextBox.Font = new Font("Consolas", 11F);
            textTextBox.Location = new Point(12, 73);
            textTextBox.Multiline = true;
            textTextBox.Name = "textTextBox";
            textTextBox.ScrollBars = ScrollBars.Vertical;
            textTextBox.Size = new Size(735, 41);
            textTextBox.TabIndex = 4;
            textTextBox.TextChanged += TextTextBox_TextChanged;
            // 
            // statusStrip
            // 
            statusStrip.BackColor = Color.White;
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusToolStripStatusLabel });
            statusStrip.Location = new Point(0, 990);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(10, 0, 10, 0);
            statusStrip.Size = new Size(1214, 22);
            statusStrip.TabIndex = 7;
            statusStrip.Text = "statusStrip";
            // 
            // statusToolStripStatusLabel
            // 
            statusToolStripStatusLabel.Name = "statusToolStripStatusLabel";
            statusToolStripStatusLabel.Size = new Size(43, 17);
            statusToolStripStatusLabel.Text = "Pronto";
            // 
            // validationErrorProvider
            // 
            validationErrorProvider.ContainerControl = this;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1214, 1012);
            Controls.Add(editorGroupBox);
            Controls.Add(navigationGroupBox);
            Controls.Add(remapGroupBox);
            Controls.Add(fileGroupBox);
            Controls.Add(headerPanel);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 2, 3, 2);
            MinimumSize = new Size(964, 550);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "StarFox Zero Localization Tool v1.0 - Powered by: Junior GBJ";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            fileGroupBox.ResumeLayout(false);
            searchGroupBox.ResumeLayout(false);
            searchGroupBox.PerformLayout();
            remapGroupBox.ResumeLayout(false);
            remapGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)remapTexturePreviewPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)remapGlyphZoomPictureBox).EndInit();
            navigationGroupBox.ResumeLayout(false);
            navigationGroupBox.PerformLayout();
            editorGroupBox.ResumeLayout(false);
            editorGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)editorPreviewPictureBox).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)validationErrorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip = null!;
        private ToolStripMenuItem arquivoToolStripMenuItem = null!;
        private ToolStripMenuItem abrirToolStripMenuItem = null!;
        private ToolStripMenuItem salvarToolStripMenuItem = null!;
        private ToolStripMenuItem exportarCsvToolStripMenuItem = null!;
        private ToolStripMenuItem importarCsvToolStripMenuItem = null!;
        private ToolStripMenuItem ferramentasToolStripMenuItem = null!;
        private ToolStripMenuItem gtxDdsToolStripMenuItem = null!;
        private ToolStripMenuItem gtxRawEditorToolStripMenuItem = null!;
        private Panel headerPanel = null!;
        private GroupBox fileGroupBox = null!;
        private Button loadButton = null!;
        private Button saveButton = null!;
        private Label loadedFileValueLabel = null!;
        private GroupBox searchGroupBox = null!;
        private Label searchFieldLabel = null!;
        private Label searchRequiredLabel = null!;
        private TextBox searchTextBox = null!;
        private CheckBox caseSensitiveCheckBox = null!;
        private Label replaceFieldLabel = null!;
        private TextBox replaceTextBox = null!;
        private Label searchHelperLabel = null!;
        private Button searchButton = null!;
        private Button nextMatchButton = null!;
        private Button replaceCurrentButton = null!;
        private Button replaceAllButton = null!;
        private Button validateCharsetButton = null!;
        private GroupBox remapGroupBox = null!;
        private Label remapSourceLabel = null!;
        private ComboBox remapSourceComboBox = null!;
        private Label remapTargetLabel = null!;
        private TextBox remapTargetTextBox = null!;
        private Button applyCharRemapButton = null!;
        private Label remapLanguageTargetLabel = null!;
        private TextBox remapLanguageTargetTextBox = null!;
        private Button applyLanguageFlagsButton = null!;
        private Label remapVariantDetailsLabel = null!;
        private Label currentGlyphHeaderLabel = null!;
        private Label currentGlyphHintLabel = null!;
        private Label newCharacterHeaderLabel = null!;
        private Label newCharacterHintLabel = null!;
        private Label newCharacterLabel = null!;
        private TextBox newCharacterTextBox = null!;
        private Label newCharacterLanguageLabel = null!;
        private TextBox newCharacterLanguageTextBox = null!;
        private Button selectNewGlyphButton = null!;
        private Button createNewCharacterButton = null!;
        private Button updateSelectedGlyphButton = null!;
        private Button removeCharacterButton = null!;
        private Panel remapSectionDividerPanel = null!;
        private Label newCharacterSelectionLabel = null!;
        private Label newCharacterBaseInfoLabel = null!;
        private Label selectionAdjustLabel = null!;
        private TextBox selectionAdjustStepTextBox = null!;
        private Button selectionWidthDecreaseButton = null!;
        private Button selectionWidthIncreaseButton = null!;
        private Button selectionHeightDecreaseButton = null!;
        private Button selectionHeightIncreaseButton = null!;
        private Button selectionLeftDecreaseButton = null!;
        private Button selectionLeftIncreaseButton = null!;
        private Button selectionRightDecreaseButton = null!;
        private Button selectionRightIncreaseButton = null!;
        private Button selectionTopDecreaseButton = null!;
        private Button selectionTopIncreaseButton = null!;
        private Button selectionBottomDecreaseButton = null!;
        private Button selectionBottomIncreaseButton = null!;
        private Button resetSelectionToGlyphButton = null!;
        private PictureBox remapTexturePreviewPictureBox = null!;
        private PictureBox remapGlyphZoomPictureBox = null!;
        private Label remapTexturePreviewLabel = null!;
        private Label remapHelperLabel = null!;
        private GroupBox navigationGroupBox = null!;
        private Label navigationSummaryLabel = null!;
        private TreeView eventTreeView = null!;
        private GroupBox editorGroupBox = null!;
        private Label editorFieldLabel = null!;
        private Label editorRequiredLabel = null!;
        private Label selectedEntryLabel = null!;
        private TextBox textTextBox = null!;
        private Label editorHelperLabel = null!;
        private Label editorPreviewInfoLabel = null!;
        private PictureBox editorPreviewPictureBox = null!;
        private StatusStrip statusStrip = null!;
        private ToolStripStatusLabel statusToolStripStatusLabel = null!;
        private ErrorProvider validationErrorProvider = null!;
        private Label remapLanguageCurrentLabel;
        private Label remapLanguageCurrentValueLabel;
    }
}

#nullable restore
