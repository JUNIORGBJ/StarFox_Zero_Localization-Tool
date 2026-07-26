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
            fecharMcdToolStripMenuItem = new ToolStripMenuItem();
            exportarCsvToolStripMenuItem = new ToolStripMenuItem();
            importarCsvToolStripMenuItem = new ToolStripMenuItem();
            ferramentasToolStripMenuItem = new ToolStripMenuItem();
            datArchiveToolStripMenuItem = new ToolStripMenuItem();
            sobreToolStripMenuItem = new ToolStripMenuItem();
            languageToolStripComboBox = new ToolStripComboBox();
            languageToolStripLabel = new ToolStripLabel();
            headerPanel = new Panel();
            fileGroupBox = new GroupBox();
            loadButton = new Button();
            saveButton = new Button();
            closeButton = new Button();
            loadedFileValueLabel = new Label();
            searchGroupBox = new GroupBox();
            searchFieldLabel = new Label();
            searchTextBox = new TextBox();
            caseSensitiveCheckBox = new CheckBox();
            replaceFieldLabel = new Label();
            replaceTextBox = new TextBox();
            searchHelperLabel = new Label();
            searchButton = new Button();
            nextMatchButton = new Button();
            replaceCurrentButton = new Button();
            replaceAllButton = new Button();
            editorHelperLabel = new Label();
            validateCharsetButton = new Button();
            remapGroupBox = new GroupBox();
            remapTexturePreviewPictureBox = new PictureBox();
            remapSourceLabel = new Label();
            remapSourceComboBox = new ComboBox();
            remapTargetLabel = new Label();
            remapTargetTextBox = new TextBox();
            applyCharRemapButton = new Button();
            remapHelperLabel = new Label();
            remapLanguageTargetLabel = new Label();
            remapLanguageTargetTextBox = new TextBox();
            applyLanguageFlagsButton = new Button();
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
            selectedEntryLabel = new Label();
            editorPreviewPictureBox = new PictureBox();
            baselinePanel = new Panel();
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
            editorPreviewPictureBox.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)validationErrorProvider).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.White;
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { arquivoToolStripMenuItem, ferramentasToolStripMenuItem, sobreToolStripMenuItem, languageToolStripComboBox, languageToolStripLabel });
            resources.ApplyResources(menuStrip, "menuStrip");
            menuStrip.Name = "menuStrip";
            // 
            // arquivoToolStripMenuItem
            // 
            arquivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { abrirToolStripMenuItem, salvarToolStripMenuItem, fecharMcdToolStripMenuItem, exportarCsvToolStripMenuItem, importarCsvToolStripMenuItem });
            arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            resources.ApplyResources(arquivoToolStripMenuItem, "arquivoToolStripMenuItem");
            // 
            // abrirToolStripMenuItem
            // 
            abrirToolStripMenuItem.Image = Properties.Resources.OpenFile;
            abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            resources.ApplyResources(abrirToolStripMenuItem, "abrirToolStripMenuItem");
            abrirToolStripMenuItem.Click += LoadButton_Click;
            // 
            // salvarToolStripMenuItem
            // 
            salvarToolStripMenuItem.Image = Properties.Resources.Save;
            salvarToolStripMenuItem.Name = "salvarToolStripMenuItem";
            resources.ApplyResources(salvarToolStripMenuItem, "salvarToolStripMenuItem");
            salvarToolStripMenuItem.Click += SaveButton_Click;
            // 
            // fecharMcdToolStripMenuItem
            // 
            resources.ApplyResources(fecharMcdToolStripMenuItem, "fecharMcdToolStripMenuItem");
            fecharMcdToolStripMenuItem.Image = Properties.Resources.Close;
            fecharMcdToolStripMenuItem.Name = "fecharMcdToolStripMenuItem";
            fecharMcdToolStripMenuItem.Click += CloseMcdButton_Click;
            // 
            // exportarCsvToolStripMenuItem
            // 
            exportarCsvToolStripMenuItem.Image = Properties.Resources.Export;
            exportarCsvToolStripMenuItem.Name = "exportarCsvToolStripMenuItem";
            resources.ApplyResources(exportarCsvToolStripMenuItem, "exportarCsvToolStripMenuItem");
            exportarCsvToolStripMenuItem.Click += ExportCsvButton_Click;
            // 
            // importarCsvToolStripMenuItem
            // 
            importarCsvToolStripMenuItem.Image = Properties.Resources.Import;
            importarCsvToolStripMenuItem.Name = "importarCsvToolStripMenuItem";
            resources.ApplyResources(importarCsvToolStripMenuItem, "importarCsvToolStripMenuItem");
            importarCsvToolStripMenuItem.Click += ImportCsvButton_Click;
            // 
            // ferramentasToolStripMenuItem
            // 
            ferramentasToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { datArchiveToolStripMenuItem });
            ferramentasToolStripMenuItem.Name = "ferramentasToolStripMenuItem";
            resources.ApplyResources(ferramentasToolStripMenuItem, "ferramentasToolStripMenuItem");
            // 
            // datArchiveToolStripMenuItem
            // 
            datArchiveToolStripMenuItem.Image = Properties.Resources.Toolbox;
            datArchiveToolStripMenuItem.Name = "datArchiveToolStripMenuItem";
            resources.ApplyResources(datArchiveToolStripMenuItem, "datArchiveToolStripMenuItem");
            datArchiveToolStripMenuItem.Click += OpenDatArchiveToolMenuItem_Click;
            // 
            // sobreToolStripMenuItem
            // 
            sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            resources.ApplyResources(sobreToolStripMenuItem, "sobreToolStripMenuItem");
            sobreToolStripMenuItem.Click += OpenAboutToolStripMenuItem_Click;
            // 
            // languageToolStripComboBox
            // 
            languageToolStripComboBox.Alignment = ToolStripItemAlignment.Right;
            languageToolStripComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            languageToolStripComboBox.Name = "languageToolStripComboBox";
            resources.ApplyResources(languageToolStripComboBox, "languageToolStripComboBox");
            languageToolStripComboBox.SelectedIndexChanged += LanguageToolStripComboBox_SelectedIndexChanged;
            // 
            // languageToolStripLabel
            // 
            languageToolStripLabel.Alignment = ToolStripItemAlignment.Right;
            languageToolStripLabel.Name = "languageToolStripLabel";
            resources.ApplyResources(languageToolStripLabel, "languageToolStripLabel");
            // 
            // headerPanel
            // 
            resources.ApplyResources(headerPanel, "headerPanel");
            headerPanel.BackColor = Color.Transparent;
            headerPanel.Name = "headerPanel";
            // 
            // fileGroupBox
            // 
            fileGroupBox.BackColor = Color.FromArgb(37, 37, 38);
            fileGroupBox.Controls.Add(loadButton);
            fileGroupBox.Controls.Add(saveButton);
            fileGroupBox.Controls.Add(closeButton);
            fileGroupBox.Controls.Add(loadedFileValueLabel);
            resources.ApplyResources(fileGroupBox, "fileGroupBox");
            fileGroupBox.ForeColor = Color.FromArgb(0, 122, 204);
            fileGroupBox.Name = "fileGroupBox";
            fileGroupBox.TabStop = false;
            // 
            // loadButton
            // 
            loadButton.BackColor = Color.FromArgb(45, 45, 65);
            loadButton.Cursor = Cursors.Hand;
            loadButton.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
            resources.ApplyResources(loadButton, "loadButton");
            loadButton.ForeColor = Color.White;
            loadButton.Image = Properties.Resources.OpenFile;
            loadButton.Name = "loadButton";
            loadButton.UseVisualStyleBackColor = false;
            loadButton.Click += LoadButton_Click;
            // 
            // saveButton
            // 
            saveButton.BackColor = Color.FromArgb(22, 163, 74);
            saveButton.Cursor = Cursors.Hand;
            resources.ApplyResources(saveButton, "saveButton");
            saveButton.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
            saveButton.ForeColor = Color.White;
            saveButton.Image = Properties.Resources.Save;
            saveButton.Name = "saveButton";
            saveButton.UseVisualStyleBackColor = false;
            saveButton.Click += SaveButton_Click;
            // 
            // closeButton
            // 
            closeButton.BackColor = Color.FromArgb(185, 28, 28);
            closeButton.Cursor = Cursors.Hand;
            resources.ApplyResources(closeButton, "closeButton");
            closeButton.FlatAppearance.BorderColor = Color.FromArgb(0, 122, 204);
            closeButton.ForeColor = Color.White;
            validationErrorProvider.SetIconAlignment(closeButton, (ErrorIconAlignment)resources.GetObject("closeButton.IconAlignment"));
            closeButton.Image = Properties.Resources.Close;
            closeButton.Name = "closeButton";
            closeButton.UseVisualStyleBackColor = false;
            closeButton.Click += CloseMcdButton_Click;
            // 
            // loadedFileValueLabel
            // 
            loadedFileValueLabel.AutoEllipsis = true;
            resources.ApplyResources(loadedFileValueLabel, "loadedFileValueLabel");
            loadedFileValueLabel.ForeColor = Color.FromArgb(185, 28, 28);
            loadedFileValueLabel.Name = "loadedFileValueLabel";
            // 
            // searchGroupBox
            // 
            resources.ApplyResources(searchGroupBox, "searchGroupBox");
            searchGroupBox.BackColor = Color.FromArgb(37, 37, 38);
            searchGroupBox.Controls.Add(searchFieldLabel);
            searchGroupBox.Controls.Add(searchTextBox);
            searchGroupBox.Controls.Add(caseSensitiveCheckBox);
            searchGroupBox.Controls.Add(replaceFieldLabel);
            searchGroupBox.Controls.Add(replaceTextBox);
            searchGroupBox.Controls.Add(searchHelperLabel);
            searchGroupBox.Controls.Add(searchButton);
            searchGroupBox.Controls.Add(nextMatchButton);
            searchGroupBox.Controls.Add(replaceCurrentButton);
            searchGroupBox.Controls.Add(replaceAllButton);
            searchGroupBox.ForeColor = Color.FromArgb(0, 122, 204);
            searchGroupBox.Name = "searchGroupBox";
            searchGroupBox.TabStop = false;
            // 
            // searchFieldLabel
            // 
            resources.ApplyResources(searchFieldLabel, "searchFieldLabel");
            searchFieldLabel.ForeColor = Color.FromArgb(133, 133, 133);
            searchFieldLabel.Name = "searchFieldLabel";
            // 
            // searchTextBox
            // 
            searchTextBox.BackColor = Color.Gainsboro;
            searchTextBox.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(searchTextBox, "searchTextBox");
            searchTextBox.Name = "searchTextBox";
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            searchTextBox.KeyDown += SearchTextBox_KeyDown;
            // 
            // caseSensitiveCheckBox
            // 
            resources.ApplyResources(caseSensitiveCheckBox, "caseSensitiveCheckBox");
            caseSensitiveCheckBox.ForeColor = Color.FromArgb(133, 133, 133);
            caseSensitiveCheckBox.Name = "caseSensitiveCheckBox";
            caseSensitiveCheckBox.UseVisualStyleBackColor = true;
            caseSensitiveCheckBox.CheckedChanged += CaseSensitiveCheckBox_CheckedChanged;
            // 
            // replaceFieldLabel
            // 
            resources.ApplyResources(replaceFieldLabel, "replaceFieldLabel");
            replaceFieldLabel.ForeColor = Color.FromArgb(133, 133, 133);
            replaceFieldLabel.Name = "replaceFieldLabel";
            // 
            // replaceTextBox
            // 
            replaceTextBox.BackColor = Color.Gainsboro;
            replaceTextBox.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(replaceTextBox, "replaceTextBox");
            replaceTextBox.Name = "replaceTextBox";
            replaceTextBox.TextChanged += ReplaceTextBox_TextChanged;
            replaceTextBox.KeyDown += ReplaceTextBox_KeyDown;
            // 
            // searchHelperLabel
            // 
            resources.ApplyResources(searchHelperLabel, "searchHelperLabel");
            searchHelperLabel.ForeColor = Color.FromArgb(133, 133, 133);
            searchHelperLabel.Name = "searchHelperLabel";
            // 
            // searchButton
            // 
            searchButton.BackColor = SystemColors.GrayText;
            searchButton.Cursor = Cursors.Hand;
            searchButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(searchButton, "searchButton");
            searchButton.ForeColor = Color.White;
            searchButton.Name = "searchButton";
            searchButton.UseVisualStyleBackColor = false;
            searchButton.Click += SearchButton_Click;
            // 
            // nextMatchButton
            // 
            nextMatchButton.BackColor = Color.Gainsboro;
            nextMatchButton.Cursor = Cursors.Hand;
            resources.ApplyResources(nextMatchButton, "nextMatchButton");
            nextMatchButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            nextMatchButton.ForeColor = Color.FromArgb(15, 23, 42);
            nextMatchButton.Name = "nextMatchButton";
            nextMatchButton.UseVisualStyleBackColor = false;
            nextMatchButton.Click += NextMatchButton_Click;
            // 
            // replaceCurrentButton
            // 
            replaceCurrentButton.BackColor = Color.Gainsboro;
            replaceCurrentButton.Cursor = Cursors.Hand;
            resources.ApplyResources(replaceCurrentButton, "replaceCurrentButton");
            replaceCurrentButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            replaceCurrentButton.ForeColor = Color.FromArgb(15, 23, 42);
            replaceCurrentButton.Name = "replaceCurrentButton";
            replaceCurrentButton.UseVisualStyleBackColor = false;
            replaceCurrentButton.Click += ReplaceCurrentButton_Click;
            // 
            // replaceAllButton
            // 
            replaceAllButton.BackColor = Color.FromArgb(22, 163, 74);
            replaceAllButton.Cursor = Cursors.Hand;
            resources.ApplyResources(replaceAllButton, "replaceAllButton");
            replaceAllButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            replaceAllButton.ForeColor = Color.White;
            replaceAllButton.Name = "replaceAllButton";
            replaceAllButton.UseVisualStyleBackColor = false;
            replaceAllButton.Click += ReplaceAllButton_Click;
            // 
            // editorHelperLabel
            // 
            resources.ApplyResources(editorHelperLabel, "editorHelperLabel");
            editorHelperLabel.ForeColor = Color.FromArgb(133, 133, 133);
            editorHelperLabel.Name = "editorHelperLabel";
            // 
            // validateCharsetButton
            // 
            validateCharsetButton.BackColor = Color.FromArgb(0, 122, 204);
            validateCharsetButton.Cursor = Cursors.Hand;
            validateCharsetButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(validateCharsetButton, "validateCharsetButton");
            validateCharsetButton.ForeColor = Color.White;
            validateCharsetButton.Name = "validateCharsetButton";
            validateCharsetButton.UseVisualStyleBackColor = false;
            validateCharsetButton.Click += ValidateCharsetButton_Click;
            // 
            // remapGroupBox
            // 
            resources.ApplyResources(remapGroupBox, "remapGroupBox");
            remapGroupBox.BackColor = Color.FromArgb(37, 37, 38);
            remapGroupBox.Controls.Add(remapTexturePreviewPictureBox);
            remapGroupBox.Controls.Add(remapSourceLabel);
            remapGroupBox.Controls.Add(remapSourceComboBox);
            remapGroupBox.Controls.Add(remapTargetLabel);
            remapGroupBox.Controls.Add(remapTargetTextBox);
            remapGroupBox.Controls.Add(applyCharRemapButton);
            remapGroupBox.Controls.Add(remapHelperLabel);
            remapGroupBox.Controls.Add(remapLanguageTargetLabel);
            remapGroupBox.Controls.Add(remapLanguageTargetTextBox);
            remapGroupBox.Controls.Add(applyLanguageFlagsButton);
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
            remapGroupBox.ForeColor = Color.FromArgb(0, 122, 204);
            remapGroupBox.Name = "remapGroupBox";
            remapGroupBox.TabStop = false;
            // 
            // remapTexturePreviewPictureBox
            // 
            remapTexturePreviewPictureBox.BackColor = Color.FromArgb(30, 30, 30);
            remapTexturePreviewPictureBox.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(remapTexturePreviewPictureBox, "remapTexturePreviewPictureBox");
            remapTexturePreviewPictureBox.Name = "remapTexturePreviewPictureBox";
            remapTexturePreviewPictureBox.TabStop = false;
            // 
            // remapSourceLabel
            // 
            resources.ApplyResources(remapSourceLabel, "remapSourceLabel");
            remapSourceLabel.ForeColor = Color.Red;
            remapSourceLabel.Name = "remapSourceLabel";
            // 
            // remapSourceComboBox
            // 
            remapSourceComboBox.BackColor = Color.Gainsboro;
            remapSourceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            resources.ApplyResources(remapSourceComboBox, "remapSourceComboBox");
            remapSourceComboBox.FormattingEnabled = true;
            remapSourceComboBox.Name = "remapSourceComboBox";
            remapSourceComboBox.SelectedIndexChanged += RemapSourceComboBox_SelectedIndexChanged;
            // 
            // remapTargetLabel
            // 
            resources.ApplyResources(remapTargetLabel, "remapTargetLabel");
            remapTargetLabel.ForeColor = Color.FromArgb(133, 133, 133);
            remapTargetLabel.Name = "remapTargetLabel";
            // 
            // remapTargetTextBox
            // 
            remapTargetTextBox.BackColor = Color.Gainsboro;
            remapTargetTextBox.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(remapTargetTextBox, "remapTargetTextBox");
            remapTargetTextBox.Name = "remapTargetTextBox";
            remapTargetTextBox.TextChanged += RemapTargetTextBox_TextChanged;
            // 
            // applyCharRemapButton
            // 
            applyCharRemapButton.BackColor = SystemColors.GrayText;
            applyCharRemapButton.Cursor = Cursors.Hand;
            resources.ApplyResources(applyCharRemapButton, "applyCharRemapButton");
            applyCharRemapButton.FlatAppearance.BorderSize = 0;
            applyCharRemapButton.ForeColor = Color.White;
            applyCharRemapButton.Name = "applyCharRemapButton";
            applyCharRemapButton.UseVisualStyleBackColor = false;
            applyCharRemapButton.Click += ApplyCharRemapButton_Click;
            // 
            // remapHelperLabel
            // 
            resources.ApplyResources(remapHelperLabel, "remapHelperLabel");
            remapHelperLabel.ForeColor = Color.FromArgb(133, 133, 133);
            remapHelperLabel.Name = "remapHelperLabel";
            // 
            // remapLanguageTargetLabel
            // 
            resources.ApplyResources(remapLanguageTargetLabel, "remapLanguageTargetLabel");
            remapLanguageTargetLabel.ForeColor = Color.FromArgb(133, 133, 133);
            remapLanguageTargetLabel.Name = "remapLanguageTargetLabel";
            // 
            // remapLanguageTargetTextBox
            // 
            remapLanguageTargetTextBox.BackColor = Color.Gainsboro;
            remapLanguageTargetTextBox.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(remapLanguageTargetTextBox, "remapLanguageTargetTextBox");
            remapLanguageTargetTextBox.Name = "remapLanguageTargetTextBox";
            remapLanguageTargetTextBox.TextChanged += RemapLanguageTargetTextBox_TextChanged;
            // 
            // applyLanguageFlagsButton
            // 
            applyLanguageFlagsButton.BackColor = SystemColors.GrayText;
            applyLanguageFlagsButton.Cursor = Cursors.Hand;
            resources.ApplyResources(applyLanguageFlagsButton, "applyLanguageFlagsButton");
            applyLanguageFlagsButton.FlatAppearance.BorderSize = 0;
            applyLanguageFlagsButton.ForeColor = Color.White;
            applyLanguageFlagsButton.Name = "applyLanguageFlagsButton";
            applyLanguageFlagsButton.UseVisualStyleBackColor = false;
            applyLanguageFlagsButton.Click += ApplyLanguageFlagsButton_Click;
            // 
            // currentGlyphHeaderLabel
            // 
            resources.ApplyResources(currentGlyphHeaderLabel, "currentGlyphHeaderLabel");
            currentGlyphHeaderLabel.ForeColor = Color.FromArgb(255, 128, 0);
            currentGlyphHeaderLabel.Name = "currentGlyphHeaderLabel";
            // 
            // currentGlyphHintLabel
            // 
            resources.ApplyResources(currentGlyphHintLabel, "currentGlyphHintLabel");
            currentGlyphHintLabel.ForeColor = Color.FromArgb(133, 133, 133);
            currentGlyphHintLabel.Name = "currentGlyphHintLabel";
            // 
            // newCharacterHeaderLabel
            // 
            resources.ApplyResources(newCharacterHeaderLabel, "newCharacterHeaderLabel");
            newCharacterHeaderLabel.ForeColor = Color.FromArgb(255, 128, 0);
            newCharacterHeaderLabel.Name = "newCharacterHeaderLabel";
            // 
            // newCharacterHintLabel
            // 
            resources.ApplyResources(newCharacterHintLabel, "newCharacterHintLabel");
            newCharacterHintLabel.ForeColor = Color.FromArgb(133, 133, 133);
            newCharacterHintLabel.Name = "newCharacterHintLabel";
            // 
            // newCharacterLabel
            // 
            resources.ApplyResources(newCharacterLabel, "newCharacterLabel");
            newCharacterLabel.ForeColor = Color.FromArgb(133, 133, 133);
            newCharacterLabel.Name = "newCharacterLabel";
            // 
            // newCharacterTextBox
            // 
            newCharacterTextBox.BackColor = Color.Gainsboro;
            newCharacterTextBox.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(newCharacterTextBox, "newCharacterTextBox");
            newCharacterTextBox.Name = "newCharacterTextBox";
            newCharacterTextBox.TextChanged += NewCharacterTextBox_TextChanged;
            // 
            // newCharacterLanguageLabel
            // 
            resources.ApplyResources(newCharacterLanguageLabel, "newCharacterLanguageLabel");
            newCharacterLanguageLabel.ForeColor = Color.FromArgb(133, 133, 133);
            newCharacterLanguageLabel.Name = "newCharacterLanguageLabel";
            // 
            // newCharacterLanguageTextBox
            // 
            newCharacterLanguageTextBox.BackColor = Color.Gainsboro;
            newCharacterLanguageTextBox.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(newCharacterLanguageTextBox, "newCharacterLanguageTextBox");
            newCharacterLanguageTextBox.Name = "newCharacterLanguageTextBox";
            newCharacterLanguageTextBox.TextChanged += NewCharacterLanguageTextBox_TextChanged;
            // 
            // selectNewGlyphButton
            // 
            selectNewGlyphButton.BackColor = Color.Goldenrod;
            selectNewGlyphButton.Cursor = Cursors.Hand;
            selectNewGlyphButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectNewGlyphButton, "selectNewGlyphButton");
            selectNewGlyphButton.ForeColor = Color.White;
            selectNewGlyphButton.Name = "selectNewGlyphButton";
            selectNewGlyphButton.UseVisualStyleBackColor = false;
            selectNewGlyphButton.Click += SelectNewGlyphButton_Click;
            // 
            // createNewCharacterButton
            // 
            createNewCharacterButton.BackColor = Color.FromArgb(22, 163, 74);
            createNewCharacterButton.Cursor = Cursors.Hand;
            resources.ApplyResources(createNewCharacterButton, "createNewCharacterButton");
            createNewCharacterButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            createNewCharacterButton.ForeColor = Color.White;
            createNewCharacterButton.Name = "createNewCharacterButton";
            createNewCharacterButton.UseVisualStyleBackColor = false;
            createNewCharacterButton.Click += CreateNewCharacterButton_Click;
            // 
            // updateSelectedGlyphButton
            // 
            updateSelectedGlyphButton.BackColor = Color.FromArgb(0, 122, 204);
            updateSelectedGlyphButton.Cursor = Cursors.Hand;
            resources.ApplyResources(updateSelectedGlyphButton, "updateSelectedGlyphButton");
            updateSelectedGlyphButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            updateSelectedGlyphButton.ForeColor = Color.White;
            updateSelectedGlyphButton.Name = "updateSelectedGlyphButton";
            updateSelectedGlyphButton.UseVisualStyleBackColor = false;
            updateSelectedGlyphButton.Click += UpdateSelectedGlyphButton_Click;
            // 
            // removeCharacterButton
            // 
            removeCharacterButton.BackColor = Color.FromArgb(185, 28, 28);
            removeCharacterButton.Cursor = Cursors.Hand;
            removeCharacterButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(removeCharacterButton, "removeCharacterButton");
            removeCharacterButton.ForeColor = Color.White;
            removeCharacterButton.Name = "removeCharacterButton";
            removeCharacterButton.UseVisualStyleBackColor = false;
            removeCharacterButton.Click += RemoveCharacterButton_Click;
            // 
            // remapSectionDividerPanel
            // 
            remapSectionDividerPanel.BackColor = Color.FromArgb(226, 232, 240);
            resources.ApplyResources(remapSectionDividerPanel, "remapSectionDividerPanel");
            remapSectionDividerPanel.Name = "remapSectionDividerPanel";
            // 
            // newCharacterSelectionLabel
            // 
            resources.ApplyResources(newCharacterSelectionLabel, "newCharacterSelectionLabel");
            newCharacterSelectionLabel.ForeColor = Color.FromArgb(255, 128, 0);
            newCharacterSelectionLabel.Name = "newCharacterSelectionLabel";
            // 
            // selectionAdjustLabel
            // 
            resources.ApplyResources(selectionAdjustLabel, "selectionAdjustLabel");
            selectionAdjustLabel.ForeColor = Color.FromArgb(133, 133, 133);
            selectionAdjustLabel.Name = "selectionAdjustLabel";
            // 
            // selectionAdjustStepTextBox
            // 
            selectionAdjustStepTextBox.BackColor = Color.Gainsboro;
            selectionAdjustStepTextBox.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(selectionAdjustStepTextBox, "selectionAdjustStepTextBox");
            selectionAdjustStepTextBox.Name = "selectionAdjustStepTextBox";
            selectionAdjustStepTextBox.TextChanged += SelectionAdjustStepTextBox_TextChanged;
            // 
            // selectionWidthDecreaseButton
            // 
            selectionWidthDecreaseButton.BackColor = Color.White;
            selectionWidthDecreaseButton.Cursor = Cursors.Hand;
            selectionWidthDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionWidthDecreaseButton, "selectionWidthDecreaseButton");
            selectionWidthDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionWidthDecreaseButton.Name = "selectionWidthDecreaseButton";
            selectionWidthDecreaseButton.Tag = "width-";
            selectionWidthDecreaseButton.UseVisualStyleBackColor = false;
            selectionWidthDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionWidthIncreaseButton
            // 
            selectionWidthIncreaseButton.BackColor = Color.White;
            selectionWidthIncreaseButton.Cursor = Cursors.Hand;
            selectionWidthIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionWidthIncreaseButton, "selectionWidthIncreaseButton");
            selectionWidthIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionWidthIncreaseButton.Name = "selectionWidthIncreaseButton";
            selectionWidthIncreaseButton.Tag = "width+";
            selectionWidthIncreaseButton.UseVisualStyleBackColor = false;
            selectionWidthIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionHeightDecreaseButton
            // 
            selectionHeightDecreaseButton.BackColor = Color.White;
            selectionHeightDecreaseButton.Cursor = Cursors.Hand;
            selectionHeightDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionHeightDecreaseButton, "selectionHeightDecreaseButton");
            selectionHeightDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionHeightDecreaseButton.Name = "selectionHeightDecreaseButton";
            selectionHeightDecreaseButton.Tag = "height-";
            selectionHeightDecreaseButton.UseVisualStyleBackColor = false;
            selectionHeightDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionHeightIncreaseButton
            // 
            selectionHeightIncreaseButton.BackColor = Color.White;
            selectionHeightIncreaseButton.Cursor = Cursors.Hand;
            selectionHeightIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionHeightIncreaseButton, "selectionHeightIncreaseButton");
            selectionHeightIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionHeightIncreaseButton.Name = "selectionHeightIncreaseButton";
            selectionHeightIncreaseButton.Tag = "height+";
            selectionHeightIncreaseButton.UseVisualStyleBackColor = false;
            selectionHeightIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionLeftDecreaseButton
            // 
            selectionLeftDecreaseButton.BackColor = Color.White;
            selectionLeftDecreaseButton.Cursor = Cursors.Hand;
            selectionLeftDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionLeftDecreaseButton, "selectionLeftDecreaseButton");
            selectionLeftDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionLeftDecreaseButton.Name = "selectionLeftDecreaseButton";
            selectionLeftDecreaseButton.Tag = "left-";
            selectionLeftDecreaseButton.UseVisualStyleBackColor = false;
            selectionLeftDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionLeftIncreaseButton
            // 
            selectionLeftIncreaseButton.BackColor = Color.White;
            selectionLeftIncreaseButton.Cursor = Cursors.Hand;
            selectionLeftIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionLeftIncreaseButton, "selectionLeftIncreaseButton");
            selectionLeftIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionLeftIncreaseButton.Name = "selectionLeftIncreaseButton";
            selectionLeftIncreaseButton.Tag = "left+";
            selectionLeftIncreaseButton.UseVisualStyleBackColor = false;
            selectionLeftIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionRightDecreaseButton
            // 
            selectionRightDecreaseButton.BackColor = Color.White;
            selectionRightDecreaseButton.Cursor = Cursors.Hand;
            selectionRightDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionRightDecreaseButton, "selectionRightDecreaseButton");
            selectionRightDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionRightDecreaseButton.Name = "selectionRightDecreaseButton";
            selectionRightDecreaseButton.Tag = "right-";
            selectionRightDecreaseButton.UseVisualStyleBackColor = false;
            selectionRightDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionRightIncreaseButton
            // 
            selectionRightIncreaseButton.BackColor = Color.White;
            selectionRightIncreaseButton.Cursor = Cursors.Hand;
            selectionRightIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionRightIncreaseButton, "selectionRightIncreaseButton");
            selectionRightIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionRightIncreaseButton.Name = "selectionRightIncreaseButton";
            selectionRightIncreaseButton.Tag = "right+";
            selectionRightIncreaseButton.UseVisualStyleBackColor = false;
            selectionRightIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionTopDecreaseButton
            // 
            selectionTopDecreaseButton.BackColor = Color.White;
            selectionTopDecreaseButton.Cursor = Cursors.Hand;
            selectionTopDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionTopDecreaseButton, "selectionTopDecreaseButton");
            selectionTopDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionTopDecreaseButton.Name = "selectionTopDecreaseButton";
            selectionTopDecreaseButton.Tag = "top-";
            selectionTopDecreaseButton.UseVisualStyleBackColor = false;
            selectionTopDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionTopIncreaseButton
            // 
            selectionTopIncreaseButton.BackColor = Color.White;
            selectionTopIncreaseButton.Cursor = Cursors.Hand;
            selectionTopIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionTopIncreaseButton, "selectionTopIncreaseButton");
            selectionTopIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionTopIncreaseButton.Name = "selectionTopIncreaseButton";
            selectionTopIncreaseButton.Tag = "top+";
            selectionTopIncreaseButton.UseVisualStyleBackColor = false;
            selectionTopIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionBottomDecreaseButton
            // 
            selectionBottomDecreaseButton.BackColor = Color.White;
            selectionBottomDecreaseButton.Cursor = Cursors.Hand;
            selectionBottomDecreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionBottomDecreaseButton, "selectionBottomDecreaseButton");
            selectionBottomDecreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionBottomDecreaseButton.Name = "selectionBottomDecreaseButton";
            selectionBottomDecreaseButton.Tag = "bottom-";
            selectionBottomDecreaseButton.UseVisualStyleBackColor = false;
            selectionBottomDecreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // selectionBottomIncreaseButton
            // 
            selectionBottomIncreaseButton.BackColor = Color.White;
            selectionBottomIncreaseButton.Cursor = Cursors.Hand;
            selectionBottomIncreaseButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(selectionBottomIncreaseButton, "selectionBottomIncreaseButton");
            selectionBottomIncreaseButton.ForeColor = Color.FromArgb(15, 23, 42);
            selectionBottomIncreaseButton.Name = "selectionBottomIncreaseButton";
            selectionBottomIncreaseButton.Tag = "bottom+";
            selectionBottomIncreaseButton.UseVisualStyleBackColor = false;
            selectionBottomIncreaseButton.Click += SelectionAdjustButton_Click;
            // 
            // resetSelectionToGlyphButton
            // 
            resetSelectionToGlyphButton.BackColor = Color.CornflowerBlue;
            resetSelectionToGlyphButton.Cursor = Cursors.Hand;
            resetSelectionToGlyphButton.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            resources.ApplyResources(resetSelectionToGlyphButton, "resetSelectionToGlyphButton");
            resetSelectionToGlyphButton.ForeColor = Color.White;
            resetSelectionToGlyphButton.Name = "resetSelectionToGlyphButton";
            resetSelectionToGlyphButton.UseVisualStyleBackColor = false;
            resetSelectionToGlyphButton.Click += ResetSelectionToGlyphButton_Click;
            // 
            // remapGlyphZoomPictureBox
            // 
            remapGlyphZoomPictureBox.BackColor = Color.FromArgb(30, 30, 30);
            remapGlyphZoomPictureBox.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(remapGlyphZoomPictureBox, "remapGlyphZoomPictureBox");
            remapGlyphZoomPictureBox.Name = "remapGlyphZoomPictureBox";
            remapGlyphZoomPictureBox.TabStop = false;
            // 
            // remapTexturePreviewLabel
            // 
            remapTexturePreviewLabel.BackColor = Color.Wheat;
            resources.ApplyResources(remapTexturePreviewLabel, "remapTexturePreviewLabel");
            remapTexturePreviewLabel.ForeColor = Color.FromArgb(133, 133, 133);
            remapTexturePreviewLabel.Name = "remapTexturePreviewLabel";
            // 
            // navigationGroupBox
            // 
            resources.ApplyResources(navigationGroupBox, "navigationGroupBox");
            navigationGroupBox.BackColor = Color.FromArgb(37, 37, 38);
            navigationGroupBox.Controls.Add(navigationSummaryLabel);
            navigationGroupBox.Controls.Add(eventTreeView);
            navigationGroupBox.ForeColor = Color.FromArgb(0, 122, 204);
            navigationGroupBox.Name = "navigationGroupBox";
            navigationGroupBox.TabStop = false;
            // 
            // navigationSummaryLabel
            // 
            resources.ApplyResources(navigationSummaryLabel, "navigationSummaryLabel");
            navigationSummaryLabel.ForeColor = Color.FromArgb(133, 133, 133);
            navigationSummaryLabel.Name = "navigationSummaryLabel";
            // 
            // eventTreeView
            // 
            resources.ApplyResources(eventTreeView, "eventTreeView");
            eventTreeView.BackColor = Color.FromArgb(71, 85, 105);
            eventTreeView.BorderStyle = BorderStyle.FixedSingle;
            eventTreeView.ForeColor = Color.White;
            eventTreeView.HideSelection = false;
            eventTreeView.Name = "eventTreeView";
            eventTreeView.AfterSelect += EventTreeView_AfterSelect;
            // 
            // editorGroupBox
            // 
            resources.ApplyResources(editorGroupBox, "editorGroupBox");
            editorGroupBox.BackColor = Color.FromArgb(37, 37, 38);
            editorGroupBox.Controls.Add(selectedEntryLabel);
            editorGroupBox.Controls.Add(editorPreviewPictureBox);
            editorGroupBox.Controls.Add(textTextBox);
            editorGroupBox.Controls.Add(validateCharsetButton);
            editorGroupBox.Controls.Add(editorHelperLabel);
            editorGroupBox.ForeColor = Color.FromArgb(0, 122, 204);
            editorGroupBox.Name = "editorGroupBox";
            editorGroupBox.TabStop = false;
            // 
            // selectedEntryLabel
            // 
            resources.ApplyResources(selectedEntryLabel, "selectedEntryLabel");
            selectedEntryLabel.ForeColor = Color.FromArgb(133, 133, 133);
            selectedEntryLabel.Name = "selectedEntryLabel";
            // 
            // editorPreviewPictureBox
            // 
            resources.ApplyResources(editorPreviewPictureBox, "editorPreviewPictureBox");
            editorPreviewPictureBox.BackColor = Color.Gainsboro;
            editorPreviewPictureBox.BorderStyle = BorderStyle.FixedSingle;
            editorPreviewPictureBox.Controls.Add(baselinePanel);
            editorPreviewPictureBox.Name = "editorPreviewPictureBox";
            editorPreviewPictureBox.TabStop = false;
            // 
            // baselinePanel
            // 
            resources.ApplyResources(baselinePanel, "baselinePanel");
            baselinePanel.BackColor = Color.FromArgb(0, 122, 204);
            baselinePanel.Cursor = Cursors.SizeNS;
            baselinePanel.Name = "baselinePanel";
            baselinePanel.MouseDown += baselinePanel_MouseDown;
            baselinePanel.MouseMove += baselinePanel_MouseMove;
            baselinePanel.MouseUp += baselinePanel_MouseUp;
            // 
            // textTextBox
            // 
            textTextBox.AcceptsReturn = true;
            textTextBox.AcceptsTab = true;
            resources.ApplyResources(textTextBox, "textTextBox");
            textTextBox.BackColor = Color.FromArgb(71, 85, 105);
            textTextBox.BorderStyle = BorderStyle.FixedSingle;
            textTextBox.ForeColor = Color.Yellow;
            textTextBox.Name = "textTextBox";
            textTextBox.TextChanged += TextTextBox_TextChanged;
            // 
            // statusStrip
            // 
            statusStrip.BackColor = Color.White;
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { statusToolStripStatusLabel });
            resources.ApplyResources(statusStrip, "statusStrip");
            statusStrip.Name = "statusStrip";
            // 
            // statusToolStripStatusLabel
            // 
            statusToolStripStatusLabel.Name = "statusToolStripStatusLabel";
            resources.ApplyResources(statusToolStripStatusLabel, "statusToolStripStatusLabel");
            // 
            // validationErrorProvider
            // 
            validationErrorProvider.ContainerControl = this;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            Controls.Add(editorGroupBox);
            Controls.Add(navigationGroupBox);
            Controls.Add(remapGroupBox);
            Controls.Add(searchGroupBox);
            Controls.Add(fileGroupBox);
            Controls.Add(headerPanel);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
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
            editorPreviewPictureBox.ResumeLayout(false);
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
        private ToolStripMenuItem fecharMcdToolStripMenuItem = null!;
        private ToolStripMenuItem exportarCsvToolStripMenuItem = null!;
        private ToolStripMenuItem importarCsvToolStripMenuItem = null!;
        private ToolStripMenuItem ferramentasToolStripMenuItem = null!;
        private ToolStripMenuItem datArchiveToolStripMenuItem = null!;
        private ToolStripMenuItem sobreToolStripMenuItem = null!;
        private ToolStripLabel languageToolStripLabel = null!;
        private ToolStripComboBox languageToolStripComboBox = null!;
        private Panel headerPanel = null!;
        private GroupBox fileGroupBox = null!;
        private Button loadButton = null!;
        private Button saveButton = null!;
        private Button closeButton = null!;
        private Label loadedFileValueLabel = null!;
        private GroupBox searchGroupBox = null!;
        private Label searchFieldLabel = null!;
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
        private Label selectedEntryLabel = null!;
        private TextBox textTextBox = null!;
        private Label editorHelperLabel = null!;
        private PictureBox editorPreviewPictureBox = null!;
        private Panel baselinePanel = null!;
        private StatusStrip statusStrip = null!;
        private ToolStripStatusLabel statusToolStripStatusLabel = null!;
        private ErrorProvider validationErrorProvider = null!;
        };
}

#nullable restore
