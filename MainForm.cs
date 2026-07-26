using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StarFoxZeroLocalizationTool.Localization;
using StarFoxZeroLocalizationTool.Models;
using StarFoxZeroLocalizationTool.Services;

namespace StarFoxZeroLocalizationTool
{
    public partial class MainForm : Form
    {
        private static readonly Color InfoColor = Color.FromArgb(133, 133, 133);
        private static readonly Color SuccessColor = Color.FromArgb(22, 163, 74);
        private static readonly Color WarningColor = Color.FromArgb(185, 28, 28);

        private McdFile? _currentMcd;
        private string? _currentFilePath;
        private readonly List<SearchMatch> _searchMatches = new();
        private int _currentMatchIndex = -1;
        private bool _suppressEditorEvents;
        private Bitmap? _currentAtlasBitmap;
        private string? _currentPreviewTextureId;
        private Rectangle? _newGlyphSelectionRect;
        private Point _newGlyphSelectionStartPoint;
        private bool _isSelectingNewGlyph;
        private bool _newGlyphSelectionMode;
        private readonly Dictionary<string, Bitmap> _editorPreviewAtlasCache = new(StringComparer.OrdinalIgnoreCase);
        private bool _draggingBaseline = false;
        private int _baselineMouseY = 0;
        private bool _suppressLanguageSelectionEvents;
        private static readonly HashSet<string> DynamicControlNames = new(StringComparer.Ordinal)
        {
            "loadedFileValueLabel",
            "selectedEntryLabel",
            "navigationSummaryLabel",
            "editorHelperLabel",
            "searchHelperLabel",
            "remapHelperLabel",
            "newCharacterSelectionLabel",
            "remapTexturePreviewLabel"
        };
        private static readonly HashSet<string> DynamicToolStripNames = new(StringComparer.Ordinal)
        {
            "languageToolStripComboBox",
            "statusToolStripStatusLabel"
        };

        public MainForm()
        {
            InitializeComponent();
            ApplyLocalizedStaticTexts();
            KeyPreview = true;
            remapTexturePreviewPictureBox.MouseDown += RemapTexturePreviewPictureBox_MouseDown;
            remapTexturePreviewPictureBox.MouseMove += RemapTexturePreviewPictureBox_MouseMove;
            remapTexturePreviewPictureBox.MouseUp += RemapTexturePreviewPictureBox_MouseUp;
            remapGlyphZoomPictureBox.MouseUp += RemapTexturePreviewPictureBox_MouseUp;
            remapTexturePreviewPictureBox.MouseDoubleClick += RemapTexturePreviewPictureBox_MouseDoubleClick;
            remapGlyphZoomPictureBox.MouseDoubleClick += RemapTexturePreviewPictureBox_MouseDoubleClick;
            FormClosed += MainForm_FormClosed;
            ConfigureInitialState();
        }

        private void ApplyLocalizedStaticTexts()
        {
            LocalizationService.ApplyFormTexts(this, DynamicControlNames, DynamicToolStripNames);
            PopulateLanguageSelector();
        }

        private void PopulateLanguageSelector()
        {
            _suppressLanguageSelectionEvents = true;
            languageToolStripComboBox.Items.Clear();

            foreach (var language in LocalizationService.Languages)
            {
                languageToolStripComboBox.Items.Add(new LanguageComboOption(
                    language.Code,
                    GetLanguageDisplayName(language.Code)));
            }

            var selectedIndex = -1;
            for (var i = 0; i < languageToolStripComboBox.Items.Count; i++)
            {
                if (languageToolStripComboBox.Items[i] is LanguageComboOption option &&
                    string.Equals(option.Code, LocalizationService.CurrentLanguageCode, StringComparison.OrdinalIgnoreCase))
                {
                    selectedIndex = i;
                    break;
                }
            }

            languageToolStripComboBox.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
            _suppressLanguageSelectionEvents = false;
        }

        private static string GetLanguageDisplayName(string languageCode)
        {
            return languageCode.Equals("pt-BR", StringComparison.OrdinalIgnoreCase)
                ? Loc.Get("Common.Language.PortugueseBrazil")
                : Loc.Get("Common.Language.English");
        }

        private void ConfigureInitialState()
        {
            loadedFileValueLabel.Text = Loc.Get("MainForm.Init.NoFileLoaded");
            selectedEntryLabel.Text = Loc.Get("MainForm.Init.NoStringSelected");
            navigationSummaryLabel.Text = Loc.Get("MainForm.Init.LoadFileToViewEvents");
            UpdateSearchHelper(Loc.Get("MainForm.Init.SearchHint"), InfoColor);
            UpdateEditorHelper(Loc.Get("MainForm.Init.EditorHint"), InfoColor);
            UpdateRemapVariantDetails(null);
            UpdateLanguageFlagsCurrentValue(null);
            UpdateNewCharacterBaseInfo(null);
            UpdateNewCharacterSelectionInfo();
            UpdateRemapTexturePreview(null);
            UpdateEditorPreview();
            UpdateRemapHelper(Loc.Get("MainForm.Init.RemapHint"), InfoColor);
            statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.Ready");
            UpdateUiState(false);
        }

        private void LanguageToolStripComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_suppressLanguageSelectionEvents || languageToolStripComboBox.SelectedItem is not LanguageComboOption option)
            {
                return;
            }

            if (!LocalizationService.ApplyLanguage(option.Code))
            {
                PopulateLanguageSelector();
                return;
            }

            RefreshLocalizedUi();
        }

        private void RefreshLocalizedUi()
        {
            var selectedTag = textTextBox.Tag is NodeTag tag ? CloneTag(tag) : null;
            var selectedCharId = remapSourceComboBox.SelectedItem is CharRemapOption option ? option.CharId : (int?)null;
            var hasLoadedFile = _currentMcd != null;

            ApplyLocalizedStaticTexts();

            if (!hasLoadedFile)
            {
                ConfigureInitialState();
                return;
            }

            loadedFileValueLabel.Text = Path.GetFileName(_currentFilePath);
            PopulateTreeView();
            PopulateRemapSourceOptions(selectedCharId);
            RefreshUiAfterTextImport(selectedTag);
            UpdateNewCharacterSelectionInfo();
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateEditorPreview();
            UpdateUiState(true);
            statusToolStripStatusLabel.Text = string.IsNullOrWhiteSpace(_currentFilePath)
                ? Loc.Get("MainForm.Status.Ready")
                : Loc.Format("MainForm.Status.FileLoaded", Path.GetFileName(_currentFilePath));
        }

        private void UpdateUiState(bool hasLoadedFile)
        {
            closeButton.Enabled = hasLoadedFile;
            saveButton.Enabled = hasLoadedFile;
            fecharMcdToolStripMenuItem.Enabled = hasLoadedFile;
            salvarToolStripMenuItem.Enabled = hasLoadedFile;
            exportarCsvToolStripMenuItem.Enabled = hasLoadedFile;
            importarCsvToolStripMenuItem.Enabled = hasLoadedFile;
            searchTextBox.Enabled = hasLoadedFile;
            caseSensitiveCheckBox.Enabled = hasLoadedFile;
            replaceTextBox.Enabled = hasLoadedFile;
            searchButton.Enabled = hasLoadedFile;
            validateCharsetButton.Enabled = hasLoadedFile;
            remapSourceComboBox.Enabled = hasLoadedFile;
            remapTargetTextBox.Enabled = hasLoadedFile;
            remapLanguageTargetTextBox.Enabled = hasLoadedFile;
            newCharacterTextBox.Enabled = hasLoadedFile;
            newCharacterLanguageTextBox.Enabled = hasLoadedFile;
            selectNewGlyphButton.Enabled = hasLoadedFile;
            removeCharacterButton.Enabled = hasLoadedFile;
            updateSelectedGlyphButton.Enabled = hasLoadedFile && CanUpdateSelectedGlyph();
            selectionAdjustStepTextBox.Enabled = hasLoadedFile;
            applyCharRemapButton.Enabled = hasLoadedFile && ValidateRemapInput(showError: false);
            applyLanguageFlagsButton.Enabled = hasLoadedFile && ValidateLanguageFlagsInput(showError: false);
            textTextBox.Enabled = hasLoadedFile && textTextBox.Tag is NodeTag tag && tag.Type == NodeType.String;
            UpdateSearchActionButtons();
            ValidateNewCharacterInput(showError: false);
            UpdateSelectionAdjustButtonsState();
        }

        private void LoadButton_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = Loc.Get("MainForm.Dialog.McdFilter"),
                Title = Loc.Get("MainForm.Dialog.OpenMcdTitle")
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                ClearEditorPreviewAtlasCache();
                _currentMcd = McdIO.ReadMcd(ofd.FileName);
                _currentFilePath = ofd.FileName;
                loadedFileValueLabel.Text = Path.GetFileName(ofd.FileName);

                PopulateTreeView();
                PopulateRemapSourceOptions();
                ClearSearchResults();
                ValidateSearchInput(showError: false);
                ValidateRemapInput(showError: false);
                ValidateLanguageFlagsInput(showError: false);
                UpdateUiState(true);
                TrySelectFirstStringNode();
                statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.FileLoaded", Path.GetFileName(ofd.FileName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Loc.Format("MainForm.Message.LoadFileError", ex.Message),
                    Loc.Get("Common.ErrorTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.FileLoadFailed");
            }
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = Loc.Get("MainForm.Dialog.McdFilter"),
                Title = Loc.Get("MainForm.Dialog.SaveMcdTitle"),
                FileName = _currentFilePath ?? Loc.Get("MainForm.Dialog.OutputMcdName")
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                McdIO.WriteMcd(_currentMcd, sfd.FileName);
                statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.FileSaved", Path.GetFileName(sfd.FileName));

                MessageBox.Show(
                    Loc.Get("MainForm.Message.FileSavedSuccess"),
                    Loc.Get("Common.SaveTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Loc.Format("MainForm.Message.SaveFileError", ex.Message),
                    Loc.Get("Common.ErrorTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.FileSaveFailed");
            }
        }

        private void CloseMcdButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            _currentMcd = null;
            _currentFilePath = null;
            validationErrorProvider.Clear();

            ClearSearchResults();
            ClearEditorPreviewAtlasCache();
            ReplaceEditorPreviewImage(null);
            ReplaceRemapTexturePreviewImage(null);
            ReplaceRemapGlyphZoomImage(null);
            ReplaceCurrentAtlasBitmap(null, null);
            ResetNewGlyphSelection(keepSelectionMode: false);

            eventTreeView.BeginUpdate();
            eventTreeView.Nodes.Clear();
            eventTreeView.EndUpdate();

            remapSourceComboBox.BeginUpdate();
            remapSourceComboBox.Items.Clear();
            remapSourceComboBox.SelectedIndex = -1;
            remapSourceComboBox.EndUpdate();

            _suppressEditorEvents = true;
            textTextBox.Tag = null;
            textTextBox.Clear();
            searchTextBox.Clear();
            replaceTextBox.Clear();
            remapTargetTextBox.Clear();
            remapLanguageTargetTextBox.Clear();
            newCharacterTextBox.Clear();
            newCharacterLanguageTextBox.Clear();
            _suppressEditorEvents = false;

            selectionAdjustStepTextBox.Text = "1";
            baselinePanel.Top = 127;

            ConfigureInitialState();
            statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.FileClosed");
        }

        private void ExportCsvButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            var suggestedFileName = string.IsNullOrWhiteSpace(_currentFilePath)
                ? Loc.Get("MainForm.Dialog.DefaultCsvName")
                : Path.GetFileNameWithoutExtension(_currentFilePath) + Loc.Get("MainForm.Dialog.CsvSuffix");

            using var sfd = new SaveFileDialog
            {
                Filter = Loc.Get("MainForm.Dialog.CsvFilter"),
                Title = Loc.Get("MainForm.Dialog.ExportCsvTitle"),
                FileName = suggestedFileName
            };

            if (sfd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                McdTextExchangeService.ExportToCsv(_currentMcd, sfd.FileName, _currentFilePath);
                statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.CsvExported", Path.GetFileName(sfd.FileName));
                MessageBox.Show(
                    this,
                    Loc.Get("MainForm.Message.CsvExportSuccess"),
                    Loc.Get("MainForm.Dialog.ExportCsvCaption"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.CsvExportFailed");
                MessageBox.Show(
                    this,
                    Loc.Format("MainForm.Message.CsvExportError", ex.Message),
                    Loc.Get("Common.ErrorTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ImportCsvButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            using var ofd = new OpenFileDialog
            {
                Filter = Loc.Get("MainForm.Dialog.CsvFilter"),
                Title = Loc.Get("MainForm.Dialog.ImportCsvTitle")
            };

            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var previousSelection = textTextBox.Tag is NodeTag currentTag
                ? CloneTag(currentTag)
                : null;

            try
            {
                var result = McdTextExchangeService.ImportFromCsv(_currentMcd, ofd.FileName);
                RefreshUiAfterTextImport(previousSelection);

                var summary = Loc.Format(
                    "MainForm.Message.ImportSummary",
                    result.AppliedEntries,
                    result.TotalImportedEntries,
                    result.ExactMatches,
                    result.IndexFallbackMatches,
                    result.SkippedEmptyTranslatedRows,
                    result.UnmatchedEntries);

                statusToolStripStatusLabel.Text = summary;
                MessageBox.Show(
                    this,
                    summary,
                    Loc.Get("MainForm.Dialog.ImportCsvCaption"),
                    MessageBoxButtons.OK,
                    result.UnmatchedEntries == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.CsvImportFailed");
                MessageBox.Show(
                    this,
                    Loc.Format("MainForm.Message.CsvImportError", ex.Message),
                    Loc.Get("Common.ErrorTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void OpenDatArchiveToolMenuItem_Click(object? sender, EventArgs e)
        {
            using var toolForm = new DatArchiveToolForm();
            toolForm.ShowDialog(this);
        }

        private void OpenAboutToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using var aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            ClearSearchResults();
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                validationErrorProvider.SetError(searchTextBox, string.Empty);

                if (_currentMcd != null)
                {
                    UpdateSearchHelper(Loc.Get("MainForm.Init.SearchHint"), InfoColor);
                }

                return;
            }

            validationErrorProvider.SetError(searchTextBox, string.Empty);
            UpdateSearchHelper(Loc.Get("MainForm.SearchHint.PressEnterOrSearch"), InfoColor);
        }

        private void ReplaceTextBox_TextChanged(object? sender, EventArgs e)
        {
            UpdateSearchActionButtons();
        }

        private void ReplaceTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReplaceCurrentButton_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void CaseSensitiveCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(searchTextBox.Text) && _searchMatches.Count > 0)
            {
                PerformSearch(showEmptyValidation: false, focusFirstMatch: false);
                UpdateSearchHelper(Loc.Get("MainForm.SearchHint.SearchModeUpdated"), InfoColor);
                return;
            }

            UpdateSearchActionButtons();
        }

        private void SearchTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void SearchButton_Click(object? sender, EventArgs e)
        {
            PerformSearch();
        }

        private void NextMatchButton_Click(object? sender, EventArgs e)
        {
            if (_searchMatches.Count == 0)
            {
                return;
            }

            var nextIndex = _currentMatchIndex + 1;
            if (nextIndex >= _searchMatches.Count)
            {
                nextIndex = 0;
            }

            GotoMatch(nextIndex);
        }

        private void ReplaceCurrentButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (!EnsureSearchResultsForReplacement())
            {
                UpdateSearchHelper(Loc.Get("MainForm.SearchHint.ReplaceRequiresResults"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.ReplaceUnavailableWithoutResults");
                return;
            }

            var matchIndex = _currentMatchIndex >= 0 && _currentMatchIndex < _searchMatches.Count
                ? _currentMatchIndex
                : 0;

            var match = _searchMatches[matchIndex];
            var searchText = searchTextBox.Text.Trim();
            var replacementText = replaceTextBox.Text;
            var entry = GetStringEntry(match.Tag);
            var comparison = GetSearchComparison();

            if (!IsMatchAtIndex(entry.Text, searchText, match.Index, comparison))
            {
                PerformSearch(showEmptyValidation: false, focusFirstMatch: true);
                if (_searchMatches.Count == 0)
                {
                    UpdateSearchHelper(Loc.Get("MainForm.SearchHint.NoValidResultsToReplace"), WarningColor);
                    statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.NoReplacementApplied");
                    return;
                }

                match = _searchMatches[Math.Max(_currentMatchIndex, 0)];
                entry = GetStringEntry(match.Tag);
            }

            var updatedText = entry.Text.Remove(match.Index, searchText.Length).Insert(match.Index, replacementText);
            SetStringEntryText(match.Tag, updatedText);

            PerformSearch(showEmptyValidation: false, focusFirstMatch: true);
            UpdateSearchHelper(Loc.Get("MainForm.SearchHint.CurrentOccurrenceReplaced"), SuccessColor);
            statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.CurrentOccurrenceReplaced");
        }

        private void ReplaceAllButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (!EnsureSearchResultsForReplacement())
            {
                UpdateSearchHelper(Loc.Get("MainForm.SearchHint.ReplaceRequiresResults"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.ReplaceUnavailableWithoutResults");
                return;
            }

            var searchText = searchTextBox.Text.Trim();
            var replacementText = replaceTextBox.Text;
            var comparison = GetSearchComparison();
            var totalReplacements = 0;
            var changedStrings = 0;
            var previousSelection = textTextBox.Tag is NodeTag activeTag ? CloneTag(activeTag) : null;

            for (var evIdx = 0; evIdx < _currentMcd.Events.Count; evIdx++)
            {
                var ev = _currentMcd.Events[evIdx];
                for (var pIdx = 0; pIdx < ev.Paragraphs.Count; pIdx++)
                {
                    var paragraph = ev.Paragraphs[pIdx];
                    for (var sIdx = 0; sIdx < paragraph.Strings.Count; sIdx++)
                    {
                        var entry = paragraph.Strings[sIdx];
                        var updatedText = ReplaceOccurrences(entry.Text, searchText, replacementText, comparison, out var replacements);
                        if (replacements == 0)
                        {
                            continue;
                        }

                        entry.Text = updatedText;
                        totalReplacements += replacements;
                        changedStrings++;
                    }
                }
            }

            if (totalReplacements == 0)
            {
                UpdateSearchHelper(Loc.Get("MainForm.SearchHint.NoOccurrenceFoundToReplace"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.NoReplacementApplied");
                return;
            }

            RefreshUiAfterTextImport(previousSelection);
            UpdateSearchHelper(
                Loc.Format("MainForm.SearchHint.ReplaceAllSummary", totalReplacements, changedStrings),
                SuccessColor);
            statusToolStripStatusLabel.Text =
                Loc.Format("MainForm.Status.ReplaceAllSummary", totalReplacements, changedStrings);
        }

        private void ValidateCharsetButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            var validation = McdIO.ValidateCharsetCoverage(_currentMcd);
            if (validation.Issues.Count == 0)
            {
                UpdateSearchHelper(
                    Loc.Format("MainForm.SearchHint.ValidationNoIssues", validation.TotalStringsChecked),
                    SuccessColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.ValidationNoIssues");
                MessageBox.Show(
                    this,
                    Loc.Format("MainForm.Message.ValidationNoIssuesDialog", Environment.NewLine, validation.TotalStringsChecked),
                    Loc.Get("MainForm.Dialog.ValidateCharsetCaption"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            UpdateSearchHelper(
                Loc.Format("MainForm.SearchHint.ValidationIssuesFound", validation.Issues.Count, validation.AffectedStringsCount),
                WarningColor);
            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.ValidationIssuesFound", validation.Issues.Count);
            ShowTextReportDialog(Loc.Get("Common.Dialog.ReportCharsetTitle"), BuildCharsetValidationReport(validation));
        }

        private void PerformSearch(bool showEmptyValidation = true, bool focusFirstMatch = true)
        {
            if (_currentMcd == null)
            {
                UpdateSearchHelper(Loc.Get("MainForm.SearchHint.LoadFileBeforeSearch"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.SearchUnavailableWithoutFile");
                return;
            }

            if (!ValidateSearchInput(showEmptyValidation))
            {
                ClearSearchResults();
                return;
            }

            var searchText = searchTextBox.Text.Trim();
            var comparison = GetSearchComparison();
            _searchMatches.Clear();
            _currentMatchIndex = -1;

            for (var evIdx = 0; evIdx < _currentMcd.Events.Count; evIdx++)
            {
                var ev = _currentMcd.Events[evIdx];
                for (var pIdx = 0; pIdx < ev.Paragraphs.Count; pIdx++)
                {
                    var paragraph = ev.Paragraphs[pIdx];
                    for (var sIdx = 0; sIdx < paragraph.Strings.Count; sIdx++)
                    {
                        var entry = paragraph.Strings[sIdx];
                        var startIndex = 0;

                        while (startIndex < entry.Text.Length)
                        {
                            var matchIndex = entry.Text.IndexOf(searchText, startIndex, comparison);
                            if (matchIndex < 0)
                            {
                                break;
                            }

                            _searchMatches.Add(new SearchMatch(
                                new NodeTag
                                {
                                    Type = NodeType.String,
                                    EventIndex = evIdx,
                                    ParagraphIndex = pIdx,
                                    StringIndex = sIdx
                                },
                                matchIndex));

                            startIndex = matchIndex + Math.Max(searchText.Length, 1);
                        }
                    }
                }
            }

            UpdateSearchActionButtons();

            if (_searchMatches.Count == 0)
            {
                UpdateSearchHelper(Loc.Get("MainForm.SearchHint.NoResultsFound"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.SearchCompletedWithoutResults");
                return;
            }

            UpdateSearchHelper(Loc.Format("MainForm.SearchHint.ResultsFound", _searchMatches.Count), SuccessColor);
            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.SearchCompletedWithResults", _searchMatches.Count);

            if (focusFirstMatch)
            {
                GotoMatch(0);
            }
        }

        private bool ValidateSearchInput(bool showError)
        {
            if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                validationErrorProvider.SetError(searchTextBox, string.Empty);
                return true;
            }

            if (showError)
            {
                validationErrorProvider.SetError(searchTextBox, Loc.Get("MainForm.Validation.SearchRequired"));
                UpdateSearchHelper(Loc.Get("MainForm.SearchHint.SearchFieldRequired"), WarningColor);
            }

            return false;
        }

        private void RemapSourceComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ResetNewGlyphSelection(keepSelectionMode: false);
            UpdateRemapVariantDetails(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateLanguageFlagsCurrentValue(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateNewCharacterBaseInfo(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateEditorPreview();
            ValidateRemapInput(showError: false);
            ValidateLanguageFlagsInput(showError: false);
            ValidateNewCharacterInput(showError: false);
        }

        private void RemapTargetTextBox_TextChanged(object? sender, EventArgs e)
        {
            ValidateRemapInput(showError: false);
        }

        private void RemapLanguageTargetTextBox_TextChanged(object? sender, EventArgs e)
        {
            ValidateLanguageFlagsInput(showError: false);
        }

        private void NewCharacterTextBox_TextChanged(object? sender, EventArgs e)
        {
            ValidateNewCharacterInput(showError: false);
        }

        private void NewCharacterLanguageTextBox_TextChanged(object? sender, EventArgs e)
        {
            ValidateNewCharacterInput(showError: false);
        }

        private void SelectionAdjustStepTextBox_TextChanged(object? sender, EventArgs e)
        {
            ValidateSelectionAdjustStep(showError: false);
            UpdateSelectionAdjustButtonsState();
        }

        private void SelectionAdjustButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button button || button.Tag is not string action)
            {
                return;
            }

            ApplySelectionAdjustAction(action, showValidationError: true);
        }

        private void ResetSelectionToGlyphButton_Click(object? sender, EventArgs e)
        {
            if (_currentAtlasBitmap == null || !TryGetSelectedTextureGraph(out var _, out var graph))
            {
                return;
            }

            if (!string.Equals(_currentPreviewTextureId, graph.TextureID, StringComparison.OrdinalIgnoreCase))
            {
                UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
            }

            _newGlyphSelectionRect = GetClampedGlyphRectangle(_currentAtlasBitmap.Width, _currentAtlasBitmap.Height, graph);
            UpdateSelectionAfterManualAdjust();
        }

        private void ApplyCharRemapButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null || !ValidateRemapInput(showError: true))
            {
                return;
            }

            if (remapSourceComboBox.SelectedItem is not CharRemapOption sourceOption)
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.SelectValidSource"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.RemapFailed");
                return;
            }
            var targetValue = NormalizeSingleTextElement(remapTargetTextBox.Text.Trim());
            var affectedEntry = _currentMcd.Chars.FirstOrDefault(x => x.Id == sourceOption.CharId);

            if (affectedEntry == null)
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.NoVariantsFound"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.RemapFailed");
                return;
            }

            var overwriteWarning = _currentMcd.Chars.Any(x => x.Char == targetValue && x.Id != sourceOption.CharId);
            if (overwriteWarning)
            {
                var result = MessageBox.Show(
                    Loc.Format("MainForm.Dialog.ConfirmRemapMessage", targetValue),
                    Loc.Get("MainForm.Dialog.ConfirmRemapTitle"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            var targetCode = char.ConvertToUtf32(targetValue, 0);
            affectedEntry.Char = targetValue;
            affectedEntry.CharCode = targetCode;

            PopulateRemapSourceOptions(sourceOption.CharId);
            UpdateRemapVariantDetails(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateLanguageFlagsCurrentValue(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
            ValidateRemapInput(showError: false);
            ValidateLanguageFlagsInput(showError: false);

            if (textTextBox.Enabled && textTextBox.Tag is NodeTag activeTag)
            {
                var activeText = _currentMcd.Events[activeTag.EventIndex].Paragraphs[activeTag.ParagraphIndex].Strings[activeTag.StringIndex].Text;
                ValidateEditorText(activeText);
            }

            UpdateRemapHelper(
                Loc.Format("MainForm.RemapHint.RemapApplied", sourceOption.CharId, sourceOption.Value, targetValue),
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.RemapApplied", sourceOption.CharId, sourceOption.Value, targetValue);
        }

        private void PopulateTreeView()
        {
            eventTreeView.BeginUpdate();
            eventTreeView.Nodes.Clear();

            if (_currentMcd == null)
            {
                eventTreeView.EndUpdate();
                return;
            }

            var totalStrings = 0;

            for (var evIdx = 0; evIdx < _currentMcd.Events.Count; evIdx++)
            {
                var ev = _currentMcd.Events[evIdx];
                var usedEvent = _currentMcd.UsedEvents.FirstOrDefault(u => u.EventID == ev.EventID);
                var eventName = usedEvent?.Name ?? Loc.Format("MainForm.Tree.EventFallback", ev.Id);

                var eventNode = new TreeNode(Loc.Format("MainForm.Tree.EventNode", eventName, ev.EventID))
                {
                    Tag = new NodeTag
                    {
                        Type = NodeType.Event,
                        EventIndex = evIdx
                    }
                };

                for (var pIdx = 0; pIdx < ev.Paragraphs.Count; pIdx++)
                {
                    var paragraph = ev.Paragraphs[pIdx];
                    var paragraphNode = new TreeNode(Loc.Format("MainForm.Tree.ParagraphNode", paragraph.Id))
                    {
                        Tag = new NodeTag
                        {
                            Type = NodeType.Paragraph,
                            EventIndex = evIdx,
                            ParagraphIndex = pIdx
                        }
                    };

                    for (var sIdx = 0; sIdx < paragraph.Strings.Count; sIdx++)
                    {
                        var entry = paragraph.Strings[sIdx];
                        var stringNode = new TreeNode(BuildStringNodeText(sIdx, entry.Text))
                        {
                            Tag = new NodeTag
                            {
                                Type = NodeType.String,
                                EventIndex = evIdx,
                                ParagraphIndex = pIdx,
                                StringIndex = sIdx
                            }
                        };

                        paragraphNode.Nodes.Add(stringNode);
                        totalStrings++;
                    }

                    eventNode.Nodes.Add(paragraphNode);
                }

                eventTreeView.Nodes.Add(eventNode);
            }

            if (eventTreeView.Nodes.Count > 0)
            {
                eventTreeView.Nodes[0].Expand();
            }

            navigationSummaryLabel.Text = Loc.Format("MainForm.Tree.Summary", _currentMcd.Events.Count, totalStrings);
            eventTreeView.EndUpdate();
        }

        private static string BuildStringNodeText(int stringIndex, string text)
        {
            var preview = string.IsNullOrWhiteSpace(text) ? Loc.Get("MainForm.Tree.EmptyString") : text.Trim();
            if (preview.Length > 36)
            {
                preview = preview[..36] + "...";
            }

            return Loc.Format("MainForm.Tree.StringNode", stringIndex, preview);
        }

        private void TrySelectFirstStringNode()
        {
            foreach (TreeNode eventNode in eventTreeView.Nodes)
            {
                foreach (TreeNode paragraphNode in eventNode.Nodes)
                {
                    if (paragraphNode.Nodes.Count > 0)
                    {
                        eventTreeView.SelectedNode = paragraphNode.Nodes[0];
                        paragraphNode.Expand();
                        eventNode.Expand();
                        return;
                    }
                }
            }
        }

        private void EventTreeView_AfterSelect(object? sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is not NodeTag tag || tag.Type != NodeType.String || _currentMcd == null)
            {
                _suppressEditorEvents = true;
                textTextBox.Tag = null;
                textTextBox.Clear();
                textTextBox.Enabled = false;
                _suppressEditorEvents = false;

                selectedEntryLabel.Text = Loc.Get("MainForm.Init.NoStringSelected");
                UpdateEditorHelper(Loc.Get("MainForm.EditorHint.SelectStringToEdit"), InfoColor);
                UpdateEditorPreview();
                validationErrorProvider.SetError(textTextBox, string.Empty);
                return;
            }

            var entry = _currentMcd.Events[tag.EventIndex].Paragraphs[tag.ParagraphIndex].Strings[tag.StringIndex];
            var paragraph = _currentMcd.Events[tag.EventIndex].Paragraphs[tag.ParagraphIndex];

            _suppressEditorEvents = true;
            textTextBox.Tag = tag;
            textTextBox.Text = entry.Text;
            textTextBox.Enabled = true;
            _suppressEditorEvents = false;

            selectedEntryLabel.Text =
                Loc.Format("MainForm.Selection.SelectedEntry", tag.EventIndex, tag.ParagraphIndex, tag.StringIndex, FormatLanguageFlagsReadable(paragraph.LanguageFlags));
            ValidateEditorText(entry.Text);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.StringSelected");
        }

        private void TextTextBox_TextChanged(object? sender, EventArgs e)
        {
            if (_suppressEditorEvents || _currentMcd == null || textTextBox.Tag is not NodeTag tag)
            {
                return;
            }

            var entry = _currentMcd.Events[tag.EventIndex].Paragraphs[tag.ParagraphIndex].Strings[tag.StringIndex];
            entry.Text = textTextBox.Text;
            ValidateEditorText(entry.Text);

            var node = FindNodeByTag(tag);
            if (node != null)
            {
                node.Text = BuildStringNodeText(tag.StringIndex, entry.Text);
            }

            if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                PerformSearch(showEmptyValidation: false, focusFirstMatch: false);
            }

            UpdateEditorPreview();
        }

        private void ValidateEditorText(string text)
        {
            if (_currentMcd == null)
            {
                return;
            }

            var missingChars = McdIO.ValidateTextCharacters(text, _currentMcd.Chars);
            if (missingChars.Count == 0)
            {
                validationErrorProvider.SetError(textTextBox, string.Empty);
                UpdateEditorHelper(Loc.Get("MainForm.EditorHint.AllCharactersAvailable"), SuccessColor);
                return;
            }

            validationErrorProvider.SetError(textTextBox, Loc.Get("MainForm.Validation.CharactersMissing"));
            UpdateEditorHelper(
                Loc.Format("MainForm.EditorHint.CharactersMissing", string.Join(", ", missingChars)),
                WarningColor);
        }

        private void PopulateRemapSourceOptions(int? preferredCharId = null)
        {
            remapSourceComboBox.BeginUpdate();
            remapSourceComboBox.Items.Clear();

            if (_currentMcd != null)
            {
                var options = _currentMcd.Chars
                    .Where(entry => !string.IsNullOrEmpty(entry.Char))
                    .GroupBy(entry => entry.Char)
                    .OrderBy(group => group.Key)
                    .SelectMany(group => group
                        .OrderBy(entry => entry.Id)
                        .Select((entry, index) => new CharRemapOption(
                            entry.Char,
                            entry.Id,
                            entry.CharCode,
                            index + 1,
                            group.Count(),
                            entry.LanguageFlags)))
                    .ToArray();

                remapSourceComboBox.Items.AddRange(options);

                if (options.Length > 0)
                {
                    var selectedOption = preferredCharId.HasValue
                        ? options.FirstOrDefault(option => option.CharId == preferredCharId.Value)
                        : null;

                    remapSourceComboBox.SelectedItem = selectedOption ?? options[0];
                }
            }

            remapSourceComboBox.EndUpdate();
            UpdateRemapVariantDetails(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateLanguageFlagsCurrentValue(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
        }

        private bool ValidateRemapInput(bool showError)
        {
            if (_currentMcd == null)
            {
                validationErrorProvider.SetError(remapSourceComboBox, string.Empty);
                validationErrorProvider.SetError(remapTargetTextBox, string.Empty);
                applyCharRemapButton.Enabled = false;
                return false;
            }

            validationErrorProvider.SetError(remapSourceComboBox, string.Empty);
            validationErrorProvider.SetError(remapTargetTextBox, string.Empty);

            if (remapSourceComboBox.SelectedItem is not CharRemapOption sourceOption)
            {
                if (showError)
                {
                    validationErrorProvider.SetError(remapSourceComboBox, Loc.Get("MainForm.Validation.SelectExistingCharacter"));
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.ChooseExistingCharacter"), WarningColor);
                }

                applyCharRemapButton.Enabled = false;
                return false;
            }

            if (!TryGetSingleTextElement(remapTargetTextBox.Text.Trim(), out var targetValue))
            {
                if (showError)
                {
                    validationErrorProvider.SetError(remapTargetTextBox, Loc.Get("MainForm.Validation.ExactlyOneCharacter"));
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.TypeSingleCharacter"), WarningColor);
                }

                applyCharRemapButton.Enabled = false;
                return false;
            }

            if (targetValue == sourceOption.Value)
            {
                if (showError)
                {
                    validationErrorProvider.SetError(remapTargetTextBox, Loc.Get("MainForm.Validation.CharacterMustDiffer"));
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.CharacterMustDiffer"), WarningColor);
                }

                applyCharRemapButton.Enabled = false;
                return false;
            }

            applyCharRemapButton.Enabled = true;
            UpdateRemapHelper(
                Loc.Format("MainForm.RemapHint.PendingRemap", sourceOption.VariantIndex, sourceOption.VariantsCount, sourceOption.Value, sourceOption.CharId, sourceOption.CharCode, targetValue),
                InfoColor);
            return true;
        }

        private void UpdateSearchHelper(string message, Color color)
        {
            searchHelperLabel.Text = message;
            searchHelperLabel.ForeColor = color;
        }

        private string BuildCharsetValidationReport(McdIO.CharsetValidationResult validation)
        {
            var builder = new StringBuilder();
            builder.AppendLine(Loc.Get("MainForm.Report.Header"));
            builder.AppendLine();
            builder.AppendLine(Loc.Format("MainForm.Report.TotalStrings", validation.TotalStringsChecked));
            builder.AppendLine(Loc.Format("MainForm.Report.AffectedStrings", validation.AffectedStringsCount));
            builder.AppendLine(Loc.Format("MainForm.Report.MissingCharacters", validation.MissingCharacterCount));
            builder.AppendLine(Loc.Format("MainForm.Report.LanguageFlagMismatch", validation.LanguageFlagsMismatchCount));
            builder.AppendLine();

            foreach (var issue in validation.Issues
                         .OrderBy(x => x.EventIndex)
                         .ThenBy(x => x.ParagraphIndex)
                         .ThenBy(x => x.StringIndex)
                         .ThenBy(x => x.Character, StringComparer.Ordinal))
            {
                builder.AppendLine(issue.Kind == McdIO.CharsetValidationIssueKind.MissingCharacter
                    ? Loc.Get("MainForm.Report.IssueMissingCharset")
                    : Loc.Get("MainForm.Report.IssueMissingLanguageFlagVariant"));
                builder.AppendLine(Loc.Format("MainForm.Report.EventLine", issue.EventName, issue.EventId));
                builder.AppendLine(Loc.Format("MainForm.Report.IndicesLine", issue.EventIndex, issue.ParagraphIndex, issue.StringIndex));
                builder.AppendLine(Loc.Format("MainForm.Report.ParagraphLanguageFlags", FormatLanguageFlagsReadable(issue.ParagraphLanguageFlags)));
                builder.AppendLine(Loc.Format("MainForm.Report.CharacterLine", issue.Character));
                if (!string.IsNullOrWhiteSpace(issue.AvailableLanguageFlagsSummary))
                {
                    builder.AppendLine(Loc.Format("MainForm.Report.AvailableInCharset", issue.AvailableLanguageFlagsSummary));
                }

                builder.AppendLine(Loc.Format("MainForm.Report.TextLine", issue.StringText));
                builder.AppendLine(new string('-', 90));
            }

            return builder.ToString().TrimEnd();
        }

        private void ShowTextReportDialog(string title, string content)
        {
            using var dialog = new Form
            {
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = true,
                MinimumSize = new Size(760, 420),
                Size = new Size(980, 620)
            };

            var reportTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false,
                Font = new Font("Consolas", 10F),
                Dock = DockStyle.Fill,
                Text = content
            };

            var copyButton = new Button
            {
                Text = Loc.Get("Common.CopyReport"),
                AutoSize = true,
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(8)
            };
            copyButton.FlatAppearance.BorderSize = 0;
            copyButton.Click += (_, _) =>
            {
                Clipboard.SetText(reportTextBox.Text);
                MessageBox.Show(
                    dialog,
                    Loc.Get("Common.CopyReportSuccess"),
                    Loc.Get("Common.Dialog.ReportCopyTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            };

            var closeButton = new Button
            {
                Text = Loc.Get("Common.CloseButton"),
                AutoSize = true,
                Margin = new Padding(8)
            };
            closeButton.Click += (_, _) => dialog.Close();

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(6),
                WrapContents = false
            };
            buttonPanel.Controls.Add(closeButton);
            buttonPanel.Controls.Add(copyButton);

            dialog.Controls.Add(reportTextBox);
            dialog.Controls.Add(buttonPanel);
            dialog.ShowDialog(this);
        }

        private void UpdateEditorHelper(string message, Color color)
        {
            editorHelperLabel.Text = message;
            editorHelperLabel.ForeColor = color;
        }

        private void UpdateEditorPreview()
        {
            if (_currentMcd == null || !TryGetActiveStringContext(out var paragraph, out var entry))
            {
                ReplaceEditorPreviewImage(null);
                return;
            }

            try
            {
                var previewLetters = McdIO.BuildPreviewLetters(entry.Text, _currentMcd, entry.Letters, paragraph.LanguageFlags);
                var previewBitmap = CreateEditorPreviewBitmap(paragraph, entry, previewLetters);
                ReplaceEditorPreviewImage(previewBitmap);
            }
            catch (Exception)
            {
                ReplaceEditorPreviewImage(CreateEditorPreviewFallbackBitmap(entry.Text));
            }
        }

        private bool TryGetActiveStringContext(out Paragraph paragraph, out StringEntry entry)
        {
            paragraph = null!;
            entry = null!;

            if (_currentMcd == null || textTextBox.Tag is not NodeTag tag || tag.Type != NodeType.String)
            {
                return false;
            }

            paragraph = _currentMcd.Events[tag.EventIndex].Paragraphs[tag.ParagraphIndex];
            entry = paragraph.Strings[tag.StringIndex];
            return true;
        }

        private string BuildEditorPreviewInfoText(Paragraph paragraph, StringEntry entry)
        {
            var info = Loc.Format(
                "MainForm.EditorPreview.Info",
                paragraph.BelowSpacing,
                entry.BelowSpacing,
                entry.HorizontalSpacing);

            if (TryGetSelectedTextureGraph(out var selectedEntry, out var selectedGraph))
            {
                info += Loc.Format(
                    "MainForm.EditorPreview.CurrentVariantInfo",
                    selectedEntry.Char,
                    selectedEntry.Id,
                    selectedGraph.BelowSpacing,
                    selectedGraph.Ua);

                if (_newGlyphSelectionRect is Rectangle selectionRect && _currentAtlasBitmap != null &&
                    string.Equals(_currentPreviewTextureId, selectedGraph.TextureID, StringComparison.OrdinalIgnoreCase))
                {
                    info += Loc.Format("MainForm.EditorPreview.SelectionOverride", selectionRect.Width, selectionRect.Height);
                }
            }

            return info;
        }

        private void UpdateRemapHelper(string message, Color color)
        {
            remapHelperLabel.Text = message;
            remapHelperLabel.ForeColor = color;
        }

        private void UpdateRemapVariantDetails(CharRemapOption? option)
        {
            _ = option;
        }

        private void UpdateLanguageFlagsCurrentValue(CharRemapOption? option)
        {
            _ = option;
        }

        private bool ValidateLanguageFlagsInput(bool showError)
        {
            if (_currentMcd == null || remapSourceComboBox.SelectedItem is not CharRemapOption)
            {
                validationErrorProvider.SetError(remapLanguageTargetTextBox, string.Empty);
                applyLanguageFlagsButton.Enabled = false;
                return false;
            }

            var rawValue = remapLanguageTargetTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                validationErrorProvider.SetError(remapLanguageTargetTextBox, string.Empty);
                applyLanguageFlagsButton.Enabled = false;
                return false;
            }

            if (!TryParseLanguageFlags(rawValue, out var parsedValue))
            {
                if (showError)
                {
                    validationErrorProvider.SetError(remapLanguageTargetTextBox, Loc.Get("MainForm.Validation.InvalidLanguageFlags"));
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.TypeLanguageFlags"), WarningColor);
                }

                applyLanguageFlagsButton.Enabled = false;
                return false;
            }

            var currentEntry = _currentMcd.Chars.FirstOrDefault(x => x.Id == ((CharRemapOption)remapSourceComboBox.SelectedItem).CharId);
            if (currentEntry == null)
            {
                applyLanguageFlagsButton.Enabled = false;
                return false;
            }

            if (currentEntry.LanguageFlags == parsedValue)
            {
                if (showError)
                {
                    validationErrorProvider.SetError(remapLanguageTargetTextBox, Loc.Get("MainForm.Validation.LanguageFlagsMustDiffer"));
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.LanguageFlagsMustDiffer"), WarningColor);
                }
                else
                {
                    validationErrorProvider.SetError(remapLanguageTargetTextBox, string.Empty);
                }

                applyLanguageFlagsButton.Enabled = false;
                return false;
            }

            validationErrorProvider.SetError(remapLanguageTargetTextBox, string.Empty);
            applyLanguageFlagsButton.Enabled = true;
            UpdateRemapHelper(
                Loc.Format("MainForm.RemapHint.PendingLanguageFlags", FormatLanguageFlagsReadable(currentEntry.LanguageFlags), FormatLanguageFlagsReadable(parsedValue)),
                InfoColor);
            return true;
        }

        private void ApplyLanguageFlagsButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null || !ValidateLanguageFlagsInput(showError: true))
            {
                return;
            }

            if (remapSourceComboBox.SelectedItem is not CharRemapOption sourceOption)
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.SelectValidVariantForLanguageFlags"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.LanguageFlagsFailed");
                return;
            }

            if (!TryParseLanguageFlags(remapLanguageTargetTextBox.Text.Trim(), out var newLanguageFlags))
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.InvalidLanguageFlagsInput"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.LanguageFlagsFailed");
                return;
            }

            var affectedEntry = _currentMcd.Chars.FirstOrDefault(x => x.Id == sourceOption.CharId);
            if (affectedEntry == null)
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.NoVariantFoundForLanguageFlags"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.LanguageFlagsFailed");
                return;
            }

            affectedEntry.LanguageFlags = newLanguageFlags;
            PopulateRemapSourceOptions(sourceOption.CharId);
            UpdateRemapVariantDetails(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateLanguageFlagsCurrentValue(remapSourceComboBox.SelectedItem as CharRemapOption);
            ValidateLanguageFlagsInput(showError: false);

            UpdateRemapHelper(
                Loc.Format("MainForm.RemapHint.LanguageFlagsApplied", sourceOption.CharId, FormatLanguageFlagsReadable(newLanguageFlags)),
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.LanguageFlagsApplied", sourceOption.CharId);
        }

        private void RemoveCharacterButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (remapSourceComboBox.SelectedItem is not CharRemapOption sourceOption)
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.SelectValidVariantToRemove"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.RemoveCharacterFailed");
                return;
            }

            var entry = _currentMcd.Chars.FirstOrDefault(x => x.Id == sourceOption.CharId);
            if (entry == null)
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.CouldNotFindVariantToRemove"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.RemoveCharacterFailed");
                return;
            }

            var usageCount = CountCharacterUsage(entry.Id);
            if (usageCount > 0)
            {
                MessageBox.Show(
                    this,
                    Loc.Format("MainForm.Dialog.RemoveCharacterInUse", entry.Id, usageCount, Environment.NewLine),
                    Loc.Get("MainForm.Dialog.RemoveCharacterCaption"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.RemoveCharacterBlocked");
                return;
            }

            var confirmation = MessageBox.Show(
                this,
                Loc.Format("MainForm.Dialog.ConfirmRemovalMessage", Environment.NewLine, entry.Char, entry.Id, entry.Index, FormatLanguageFlagsReadable(entry.LanguageFlags)),
                Loc.Get("MainForm.Dialog.ConfirmRemovalTitle"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            var removedCharId = entry.Id;
            var removedGraphId = entry.Index;
            _currentMcd.Chars.Remove(entry);

            foreach (var character in _currentMcd.Chars.Where(x => x.Id > removedCharId))
            {
                character.Id--;
            }

            foreach (var ev in _currentMcd.Events)
            {
                foreach (var paragraph in ev.Paragraphs)
                {
                    foreach (var str in paragraph.Strings)
                    {
                        foreach (var letter in str.Letters)
                        {
                            if (letter.Code > removedCharId && letter.Code < 0x8000)
                            {
                                letter.Code--;
                            }
                        }
                    }
                }
            }

            var graphStillUsed = _currentMcd.Chars.Any(x => x.Index == removedGraphId);
            if (!graphStillUsed)
            {
                var graph = _currentMcd.CharGraphs.FirstOrDefault(x => x.Id == removedGraphId);
                if (graph != null)
                {
                    _currentMcd.CharGraphs.Remove(graph);
                    foreach (var currentGraph in _currentMcd.CharGraphs.Where(x => x.Id > removedGraphId))
                    {
                        currentGraph.Id--;
                    }

                    foreach (var character in _currentMcd.Chars.Where(x => x.Index > removedGraphId))
                    {
                        character.Index--;
                    }
                }
            }

            ResetNewGlyphSelection(keepSelectionMode: false);
            PopulateRemapSourceOptions();
            ValidateRemapInput(showError: false);
            ValidateLanguageFlagsInput(showError: false);
            ValidateNewCharacterInput(showError: false);

            UpdateRemapHelper(
                Loc.Format(
                    "MainForm.RemapHint.CharacterRemoved",
                    sourceOption.Value,
                    removedCharId,
                    graphStillUsed ? string.Empty : Loc.Format("MainForm.RemapHint.CharacterRemovedGraphSuffix", removedGraphId)),
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.CharacterRemoved", removedCharId);
        }

        private void SelectNewGlyphButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (_currentAtlasBitmap == null || string.IsNullOrWhiteSpace(_currentPreviewTextureId))
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.AtlasRequiredForGlyphSelection"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.GlyphSelectionUnavailable");
                return;
            }

            _newGlyphSelectionMode = !_newGlyphSelectionMode;
            _isSelectingNewGlyph = false;
            UpdateNewCharacterSelectionInfo();
            ValidateNewCharacterInput(showError: false);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
        }

        private void CreateNewCharacterButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (!ValidateNewCharacterInput(showError: true))
            {
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.NewCharacterFailed");
                return;
            }

            if (!TryGetSingleTextElement(newCharacterTextBox.Text.Trim(), out var newCharacter) ||
                !TryParseLanguageFlags(newCharacterLanguageTextBox.Text.Trim(), out var languageFlags) ||
                _newGlyphSelectionRect is not Rectangle glyphRect ||
                !TryGetSelectedTextureGraph(out var baseEntry, out var baseGraph) ||
                _currentAtlasBitmap == null)
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.CouldNotBuildNewCharacter"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.NewCharacterFailed");
                return;
            }

            var duplicateExists = _currentMcd.Chars.Any(x => x.Char == newCharacter && x.LanguageFlags == languageFlags);
            if (duplicateExists)
            {
                var confirmation = MessageBox.Show(
                    this,
                    Loc.Format("MainForm.Dialog.DuplicateCharacterMessage", newCharacter, languageFlags, Environment.NewLine),
                    Loc.Get("MainForm.Dialog.RegisterCharacterTitle"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }
            }

            var atlasWidth = _currentAtlasBitmap.Width;
            var atlasHeight = _currentAtlasBitmap.Height;
            var newGraphId = _currentMcd.CharGraphs.Count;
            var newCharId = _currentMcd.Chars.Count;

            var newGraph = new CharGraph
            {
                Id = newGraphId,
                TextureID = baseGraph.TextureID,
                U1 = (float)glyphRect.Left / atlasWidth,
                V1 = (float)glyphRect.Top / atlasHeight,
                U2 = (float)glyphRect.Right / atlasWidth,
                V2 = (float)glyphRect.Bottom / atlasHeight,
                Width = glyphRect.Width,
                Height = glyphRect.Height,
                Ua = baseGraph.Ua,
                BelowSpacing = baseGraph.BelowSpacing,
                HorizontalSpacing = baseGraph.HorizontalSpacing
            };

            var newEntry = new CharEntry
            {
                Id = newCharId,
                Char = newCharacter,
                CharCode = char.ConvertToUtf32(newCharacter, 0),
                LanguageFlags = languageFlags,
                Index = newGraphId
            };

            _currentMcd.CharGraphs.Add(newGraph);
            _currentMcd.Chars.Add(newEntry);

            var createdRect = glyphRect;
            _newGlyphSelectionMode = false;
            _isSelectingNewGlyph = false;

            PopulateRemapSourceOptions(newCharId);
            _newGlyphSelectionRect = createdRect;
            UpdateNewCharacterSelectionInfo();
            ValidateNewCharacterInput(showError: false);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);

            UpdateRemapHelper(
                Loc.Format("MainForm.RemapHint.NewCharacterCreated", newCharacter, newCharId, newGraphId, baseGraph.TextureID),
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.NewCharacterCreated", newCharacter, newCharId);
        }

        private void UpdateSelectedGlyphButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (!ValidateSelectedGlyphUpdate(showError: true))
            {
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.UpdateGlyphFailed");
                return;
            }

            if (_newGlyphSelectionRect is not Rectangle glyphRect ||
                !TryGetSelectedTextureGraph(out var entry, out var graph) ||
                _currentAtlasBitmap == null)
            {
                UpdateRemapHelper(Loc.Get("MainForm.RemapHint.CouldNotPrepareGlyphUpdate"), WarningColor);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.UpdateGlyphFailed");
                return;
            }

            var atlasWidth = _currentAtlasBitmap.Width;
            var atlasHeight = _currentAtlasBitmap.Height;
            var graphUsageCount = _currentMcd.Chars.Count(x => x.Index == graph.Id);
            var targetGraph = graph;
            var createdNewGraph = false;

            if (graphUsageCount > 1)
            {
                targetGraph = new CharGraph
                {
                    Id = _currentMcd.CharGraphs.Count,
                    TextureID = graph.TextureID,
                    U1 = graph.U1,
                    V1 = graph.V1,
                    U2 = graph.U2,
                    V2 = graph.V2,
                    Width = graph.Width,
                    Height = graph.Height,
                    Ua = graph.Ua,
                    BelowSpacing = graph.BelowSpacing,
                    HorizontalSpacing = graph.HorizontalSpacing
                };

                _currentMcd.CharGraphs.Add(targetGraph);
                entry.Index = targetGraph.Id;
                createdNewGraph = true;
            }

            targetGraph.TextureID = _currentPreviewTextureId ?? graph.TextureID;
            targetGraph.U1 = (float)glyphRect.Left / atlasWidth;
            targetGraph.V1 = (float)glyphRect.Top / atlasHeight;
            targetGraph.U2 = (float)glyphRect.Right / atlasWidth;
            targetGraph.V2 = (float)glyphRect.Bottom / atlasHeight;
            targetGraph.Width = glyphRect.Width;
            targetGraph.Height = glyphRect.Height;

            PopulateRemapSourceOptions(entry.Id);
            _newGlyphSelectionMode = false;
            _isSelectingNewGlyph = false;
            _newGlyphSelectionRect = glyphRect;
            UpdateNewCharacterBaseInfo(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateNewCharacterSelectionInfo();
            ValidateNewCharacterInput(showError: false);
            ValidateSelectedGlyphUpdate(showError: false);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);

            UpdateRemapHelper(
                createdNewGraph
                    ? Loc.Format("MainForm.RemapHint.GlyphUpdatedWithNewGraph", entry.Id, targetGraph.Id)
                    : Loc.Format("MainForm.RemapHint.GlyphUpdated", entry.Id),
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.GlyphUpdated", entry.Id);
        }

        private void UpdateRemapTexturePreview(CharRemapOption? option)
        {
            try
            {
                if (_currentMcd == null || option == null)
                {
                    ReplaceCurrentAtlasBitmap(null, null);
                    ReplaceRemapTexturePreviewImage(null);
                    ReplaceRemapGlyphZoomImage(null);
                    remapTexturePreviewLabel.Text = Loc.Get("MainForm.TexturePreview.Empty");
                    remapTexturePreviewLabel.ForeColor = InfoColor;
                    UpdateNewCharacterSelectionInfo();
                    return;
                }

                var entry = _currentMcd.Chars.FirstOrDefault(x => x.Id == option.CharId);
                var graph = entry != null
                    ? _currentMcd.CharGraphs.FirstOrDefault(x => x.Id == entry.Index)
                    : null;

                if (entry == null || graph == null)
                {
                    ReplaceCurrentAtlasBitmap(null, null);
                    ReplaceRemapTexturePreviewImage(null);
                    ReplaceRemapGlyphZoomImage(null);
                    remapTexturePreviewLabel.Text = Loc.Get("MainForm.TexturePreview.NotFound");
                    remapTexturePreviewLabel.ForeColor = WarningColor;
                    UpdateNewCharacterSelectionInfo();
                    return;
                }

                if (TextureAtlasPreviewService.TryLoadAtlasBitmap(_currentFilePath, graph.TextureID, out var atlasBitmap, out var atlasInfo, out var atlasError)
                    && atlasBitmap != null)
                {
                    using (atlasBitmap)
                    {
                        ReplaceCurrentAtlasBitmap((Bitmap)atlasBitmap.Clone(), graph.TextureID);
                        var glyphRect = GetClampedGlyphRectangle(atlasBitmap.Width, atlasBitmap.Height, graph);
                        var selectionRect = TryGetValidCurrentSelection(atlasBitmap.Size);
                        ReplaceRemapTexturePreviewImage(CreateTextureAtlasOverlayBitmap(atlasBitmap, glyphRect, selectionRect));
                        ReplaceRemapGlyphZoomImage(CreateGlyphZoomBitmap(atlasBitmap, selectionRect ?? glyphRect));

                        remapTexturePreviewLabel.Text = Loc.Format(
                            "MainForm.TexturePreview.Loaded",
                            graph.TextureID,
                            Environment.NewLine,
                            Path.GetFileName(atlasInfo!.WtaPath),
                            Path.GetFileName(atlasInfo.WtpPath),
                            graph.U1,
                            graph.U2,
                            graph.V1,
                            graph.V2,
                            glyphRect.X,
                            glyphRect.Y,
                            glyphRect.Width,
                            glyphRect.Height,
                            entry.Index);
                    }

                    remapTexturePreviewLabel.ForeColor = InfoColor;
                    UpdateNewCharacterSelectionInfo();
                    return;
                }

                ReplaceCurrentAtlasBitmap(null, null);
                ReplaceRemapTexturePreviewImage(CreateTextureAtlasPreviewBitmap(graph));
                ReplaceRemapGlyphZoomImage(null);
                var fallbackWidthNorm = Math.Max(0f, graph.U2 - graph.U1);
                var fallbackHeightNorm = Math.Max(0f, graph.V2 - graph.V1);
                remapTexturePreviewLabel.Text = Loc.Format(
                    "MainForm.TexturePreview.Fallback",
                    graph.TextureID,
                    Environment.NewLine,
                    atlasError,
                    graph.U1,
                    graph.U2,
                    graph.V1,
                    graph.V2,
                    fallbackWidthNorm,
                    fallbackHeightNorm,
                    entry.Index);
                remapTexturePreviewLabel.ForeColor = WarningColor;
                UpdateNewCharacterSelectionInfo();
            }
            catch (Exception ex)
            {
                ReplaceCurrentAtlasBitmap(null, null);
                ReplaceRemapTexturePreviewImage(null);
                ReplaceRemapGlyphZoomImage(null);
                remapTexturePreviewLabel.Text =
                    Loc.Format("MainForm.TexturePreview.Error", Environment.NewLine, ex.Message);
                remapTexturePreviewLabel.ForeColor = WarningColor;
                UpdateNewCharacterSelectionInfo();
            }
        }

        private void UpdateNewCharacterBaseInfo(CharRemapOption? option)
        {
            if (_currentMcd == null || option == null)
            {
                return;
            }

            var entry = _currentMcd.Chars.FirstOrDefault(x => x.Id == option.CharId);
            var graph = entry != null
                ? _currentMcd.CharGraphs.FirstOrDefault(x => x.Id == entry.Index)
                : null;

            if (entry == null || graph == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(newCharacterLanguageTextBox.Text))
            {
                newCharacterLanguageTextBox.Text = entry.LanguageFlags.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void UpdateNewCharacterSelectionInfo()
        {
            if (_currentMcd == null)
            {
                newCharacterSelectionLabel.Text = Loc.Get("MainForm.Selection.NoVariantChosen");
                selectNewGlyphButton.Text = Loc.Get("MainForm.Selection.SelectGlyphButton");
                updateSelectedGlyphButton.Enabled = false;
                UpdateSelectionAdjustButtonsState();
                return;
            }

            if (_currentAtlasBitmap == null || string.IsNullOrWhiteSpace(_currentPreviewTextureId))
            {
                newCharacterSelectionLabel.Text = Loc.Get("MainForm.Selection.AtlasRequired");
                selectNewGlyphButton.Text = Loc.Get("MainForm.Selection.SelectGlyphButton");
                updateSelectedGlyphButton.Enabled = false;
                UpdateSelectionAdjustButtonsState();
                return;
            }

            if (_newGlyphSelectionRect is Rectangle rect)
            {
                var u1 = (float)rect.Left / _currentAtlasBitmap.Width;
                var v1 = (float)rect.Top / _currentAtlasBitmap.Height;
                var u2 = (float)rect.Right / _currentAtlasBitmap.Width;
                var v2 = (float)rect.Bottom / _currentAtlasBitmap.Height;
                var suffix = _newGlyphSelectionMode ? Loc.Get("MainForm.Selection.ActiveModeSuffix") : string.Empty;
                newCharacterSelectionLabel.Text = Loc.Format(
                    "MainForm.Selection.ActiveArea",
                    rect.X,
                    rect.Y,
                    rect.Width,
                    rect.Height,
                    u1,
                    v1,
                    u2,
                    v2,
                    suffix);
            }
            else if (_newGlyphSelectionMode)
            {
                newCharacterSelectionLabel.Text = Loc.Get("MainForm.Selection.DragToMark");
            }
            else
            {
                newCharacterSelectionLabel.Text = Loc.Get("MainForm.Selection.ClickAndDrag");
            }

            selectNewGlyphButton.Text = _newGlyphSelectionMode
                ? Loc.Get("MainForm.Selection.SelectingGlyphButton")
                : Loc.Get("MainForm.Selection.SelectGlyphButton");
            updateSelectedGlyphButton.Enabled = CanUpdateSelectedGlyph();
            UpdateSelectionAdjustButtonsState();
        }

        private bool ValidateNewCharacterInput(bool showError)
        {
            if (_currentMcd == null)
            {
                validationErrorProvider.SetError(newCharacterTextBox, string.Empty);
                validationErrorProvider.SetError(newCharacterLanguageTextBox, string.Empty);
                createNewCharacterButton.Enabled = false;
                return false;
            }

            validationErrorProvider.SetError(newCharacterTextBox, string.Empty);
            validationErrorProvider.SetError(newCharacterLanguageTextBox, string.Empty);

            if (!TryGetSelectedTextureGraph(out var baseEntry, out var baseGraph))
            {
                createNewCharacterButton.Enabled = false;
                if (showError)
                {
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.SelectBaseVariant"), WarningColor);
                }

                return false;
            }

            if (!TryGetSingleTextElement(newCharacterTextBox.Text.Trim(), out var newCharacter))
            {
                createNewCharacterButton.Enabled = false;
                if (showError)
                {
                    validationErrorProvider.SetError(newCharacterTextBox, Loc.Get("MainForm.Validation.ExactlyOneCharacter"));
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.TypeSingleCharacterForNewEntry"), WarningColor);
                }

                return false;
            }

            if (_currentMcd.Chars.Any(x => x.Id != baseEntry.Id && x.Char == newCharacter && x.Index == baseGraph.Id))
            {
                // Permitido, mas mantem a validacao apenas informativa via helper quando necessario.
            }

            if (!TryParseLanguageFlags(newCharacterLanguageTextBox.Text.Trim(), out var languageFlags))
            {
                createNewCharacterButton.Enabled = false;
                if (showError)
                {
                    validationErrorProvider.SetError(newCharacterLanguageTextBox, Loc.Get("MainForm.Validation.NewCharacterLanguageFlags"));
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.NewCharacterLanguageFlags"), WarningColor);
                }

                return false;
            }

            if (_currentAtlasBitmap == null || string.IsNullOrWhiteSpace(_currentPreviewTextureId))
            {
                createNewCharacterButton.Enabled = false;
                if (showError)
                {
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.AtlasRequiredForNewCharacter"), WarningColor);
                }

                return false;
            }

            if (_newGlyphSelectionRect is not Rectangle selectionRect || selectionRect.Width <= 0 || selectionRect.Height <= 0)
            {
                createNewCharacterButton.Enabled = false;
                if (showError)
                {
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.SelectFreeArea"), WarningColor);
                }

                return false;
            }

            createNewCharacterButton.Enabled = true;
            UpdateRemapHelper(
                Loc.Format("MainForm.RemapHint.NewCharacterReady", newCharacter, FormatLanguageFlagsReadable(languageFlags), baseEntry.Char, baseGraph.TextureID, selectionRect.Width, selectionRect.Height),
                InfoColor);
            return true;
        }

        private bool ValidateSelectedGlyphUpdate(bool showError)
        {
            if (_currentMcd == null || !TryGetSelectedTextureGraph(out var entry, out var graph))
            {
                updateSelectedGlyphButton.Enabled = false;
                return false;
            }

            if (_currentAtlasBitmap == null || string.IsNullOrWhiteSpace(_currentPreviewTextureId))
            {
                updateSelectedGlyphButton.Enabled = false;
                if (showError)
                {
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.AtlasRequiredForGlyphUpdate"), WarningColor);
                }

                return false;
            }

            if (_newGlyphSelectionRect is not Rectangle selectionRect || selectionRect.Width <= 0 || selectionRect.Height <= 0)
            {
                updateSelectedGlyphButton.Enabled = false;
                if (showError)
                {
                    UpdateRemapHelper(Loc.Get("MainForm.RemapHint.SelectAreaForGlyphUpdate"), WarningColor);
                }

                return false;
            }

            updateSelectedGlyphButton.Enabled = true;
            if (showError)
            {
                UpdateRemapHelper(
                    Loc.Format("MainForm.RemapHint.GlyphUpdateReady", entry.Id, graph.Id, selectionRect.Width, selectionRect.Height),
                    InfoColor);
            }

            return true;
        }

        private void ResetNewGlyphSelection(bool keepSelectionMode)
        {
            _newGlyphSelectionRect = null;
            _isSelectingNewGlyph = false;
            if (!keepSelectionMode)
            {
                _newGlyphSelectionMode = false;
            }

            UpdateNewCharacterSelectionInfo();
        }

        private bool ValidateSelectionAdjustStep(bool showError)
        {
            validationErrorProvider.SetError(selectionAdjustStepTextBox, string.Empty);
            if (string.IsNullOrWhiteSpace(selectionAdjustStepTextBox.Text))
            {
                if (showError)
                {
                    validationErrorProvider.SetError(selectionAdjustStepTextBox, Loc.Get("MainForm.Validation.SelectionStepRequired"));
                }

                return false;
            }

            if (!int.TryParse(selectionAdjustStepTextBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var step) || step <= 0)
            {
                if (showError)
                {
                    validationErrorProvider.SetError(selectionAdjustStepTextBox, Loc.Get("MainForm.Validation.SelectionStepPositiveInteger"));
                }

                return false;
            }

            return true;
        }

        private void UpdateSelectionAdjustButtonsState()
        {
            var enabled =
                _currentMcd != null &&
                _currentAtlasBitmap != null &&
                _newGlyphSelectionRect is Rectangle rect &&
                rect.Width > 0 &&
                rect.Height > 0 &&
                ValidateSelectionAdjustStep(showError: false);

            selectionWidthDecreaseButton.Enabled = enabled;
            selectionWidthIncreaseButton.Enabled = enabled;
            selectionHeightDecreaseButton.Enabled = enabled;
            selectionHeightIncreaseButton.Enabled = enabled;
            selectionLeftDecreaseButton.Enabled = enabled;
            selectionLeftIncreaseButton.Enabled = enabled;
            selectionRightDecreaseButton.Enabled = enabled;
            selectionRightIncreaseButton.Enabled = enabled;
            selectionTopDecreaseButton.Enabled = enabled;
            selectionTopIncreaseButton.Enabled = enabled;
            selectionBottomDecreaseButton.Enabled = enabled;
            selectionBottomIncreaseButton.Enabled = enabled;
            resetSelectionToGlyphButton.Enabled =
                _currentMcd != null &&
                _currentAtlasBitmap != null &&
                TryGetSelectedTextureGraph(out var _, out var graph) &&
                string.Equals(_currentPreviewTextureId, graph.TextureID, StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateSelectionAfterManualAdjust()
        {
            UpdateNewCharacterSelectionInfo();
            ValidateNewCharacterInput(showError: false);
            ValidateSelectedGlyphUpdate(showError: false);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateEditorPreview();
        }

        private bool ApplySelectionAdjustAction(string action, bool showValidationError)
        {
            if (_currentAtlasBitmap == null || _newGlyphSelectionRect is not Rectangle currentRect || !TryGetSelectionAdjustStep(showValidationError, out var step))
            {
                return false;
            }

            var adjustedRect = AdjustSelectionRect(currentRect, _currentAtlasBitmap.Size, action, step);
            if (adjustedRect == currentRect)
            {
                return false;
            }

            _newGlyphSelectionRect = adjustedRect;
            UpdateSelectionAfterManualAdjust();
            return true;
        }

        private bool TryGetSelectionAdjustStep(bool showError, out int step)
        {
            step = 0;
            if (!ValidateSelectionAdjustStep(showError))
            {
                return false;
            }

            return int.TryParse(selectionAdjustStepTextBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out step) && step > 0;
        }

        private static Rectangle AdjustSelectionRect(Rectangle rect, Size atlasSize, string action, int step)
        {
            var left = rect.Left;
            var top = rect.Top;
            var right = rect.Right;
            var bottom = rect.Bottom;

            switch (action)
            {
                case "width-":
                {
                    var shrinkLeft = Math.Min(step / 2, Math.Max(0, (right - left - 1) / 2));
                    var shrinkRight = Math.Min(step - shrinkLeft, Math.Max(0, right - left - 1 - shrinkLeft));
                    left += shrinkLeft;
                    right -= shrinkRight;
                    break;
                }
                case "width+":
                    left -= step / 2;
                    right += step - (step / 2);
                    break;
                case "height-":
                {
                    var shrinkTop = Math.Min(step / 2, Math.Max(0, (bottom - top - 1) / 2));
                    var shrinkBottom = Math.Min(step - shrinkTop, Math.Max(0, bottom - top - 1 - shrinkTop));
                    top += shrinkTop;
                    bottom -= shrinkBottom;
                    break;
                }
                case "height+":
                    top -= step / 2;
                    bottom += step - (step / 2);
                    break;
                case "left-":
                    left += step;
                    break;
                case "left+":
                    left -= step;
                    break;
                case "right-":
                    right -= step;
                    break;
                case "right+":
                    right += step;
                    break;
                case "top-":
                    top += step;
                    break;
                case "top+":
                    top -= step;
                    break;
                case "bottom-":
                    bottom -= step;
                    break;
                case "bottom+":
                    bottom += step;
                    break;
                default:
                    return rect;
            }

            left = Math.Clamp(left, 0, Math.Max(0, atlasSize.Width - 1));
            top = Math.Clamp(top, 0, Math.Max(0, atlasSize.Height - 1));
            right = Math.Clamp(right, left + 1, atlasSize.Width);
            bottom = Math.Clamp(bottom, top + 1, atlasSize.Height);
            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (TryHandleSelectionShortcut(keyData))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool TryHandleSelectionShortcut(Keys keyData)
        {
            if (!CanHandleSelectionShortcut())
            {
                return false;
            }

            var keyCode = keyData & Keys.KeyCode;
            var modifiers = keyData & Keys.Modifiers;
            string? action = null;

            switch (keyCode)
            {
                case Keys.Left:
                    action = modifiers == Keys.Shift ? "right-" : modifiers == Keys.None ? "left+" : null;
                    break;
                case Keys.Right:
                    action = modifiers == Keys.Shift ? "left-" : modifiers == Keys.None ? "right+" : null;
                    break;
                case Keys.Up:
                    action = modifiers == Keys.Shift ? "bottom-" : modifiers == Keys.None ? "top+" : null;
                    break;
                case Keys.Down:
                    action = modifiers == Keys.Shift ? "top-" : modifiers == Keys.None ? "bottom+" : null;
                    break;
            }

            return action != null && ApplySelectionAdjustAction(action, showValidationError: false);
        }

        private bool CanHandleSelectionShortcut()
        {
            if (!remapGroupBox.ContainsFocus || _currentAtlasBitmap == null || _newGlyphSelectionRect is not Rectangle)
            {
                return false;
            }

            var focusedControl = GetDeepestActiveControl(this);
            return focusedControl is not TextBoxBase && focusedControl is not ComboBox;
        }

        private static Control? GetDeepestActiveControl(ContainerControl parent)
        {
            var activeControl = parent.ActiveControl;
            while (activeControl is ContainerControl container && container.ActiveControl != null)
            {
                activeControl = container.ActiveControl;
            }

            return activeControl;
        }

        private bool CanUpdateSelectedGlyph()
        {
            return _currentMcd != null
                && _currentAtlasBitmap != null
                && !string.IsNullOrWhiteSpace(_currentPreviewTextureId)
                && _newGlyphSelectionRect is Rectangle rect
                && rect.Width > 0
                && rect.Height > 0
                && TryGetSelectedTextureGraph(out var _, out var _);
        }

        private void ReplaceCurrentAtlasBitmap(Bitmap? image, string? textureId)
        {
            if (!string.Equals(_currentPreviewTextureId, textureId, StringComparison.OrdinalIgnoreCase))
            {
                _newGlyphSelectionRect = null;
                _isSelectingNewGlyph = false;
                _newGlyphSelectionMode = false;
            }

            var oldBitmap = _currentAtlasBitmap;
            _currentAtlasBitmap = image;
            _currentPreviewTextureId = textureId;
            oldBitmap?.Dispose();
        }

        private Rectangle? TryGetValidCurrentSelection(Size atlasSize)
        {
            if (_newGlyphSelectionRect is not Rectangle rect)
            {
                return null;
            }

            var bounds = new Rectangle(Point.Empty, atlasSize);
            rect.Intersect(bounds);
            if (rect.Width <= 0 || rect.Height <= 0)
            {
                _newGlyphSelectionRect = null;
                return null;
            }

            _newGlyphSelectionRect = rect;
            return rect;
        }

        private void ReplaceRemapTexturePreviewImage(Bitmap? image)
        {
            var oldImage = remapTexturePreviewPictureBox.Image;
            remapTexturePreviewPictureBox.Image = image;
            oldImage?.Dispose();
        }

        private void ReplaceRemapGlyphZoomImage(Bitmap? image)
        {
            var oldImage = remapGlyphZoomPictureBox.Image;
            remapGlyphZoomPictureBox.Image = image;
            oldImage?.Dispose();
        }

        private void ReplaceEditorPreviewImage(Bitmap? image)
        {
            var oldImage = editorPreviewPictureBox.Image;
            editorPreviewPictureBox.Image = image;
            oldImage?.Dispose();
        }

        private void baselinePanel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _draggingBaseline = true;
                _baselineMouseY = e.Y;
                baselinePanel.Cursor = Cursors.SizeNS;
            }
        }

        private void baselinePanel_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_draggingBaseline)
            {
                int newY = baselinePanel.Top + e.Y - _baselineMouseY;
                // Constrain within the PictureBox
                if (newY < 0)
                    newY = 0;
                if (newY > editorPreviewPictureBox.Height - baselinePanel.Height)
                    newY = editorPreviewPictureBox.Height - baselinePanel.Height;
                baselinePanel.Top = newY;
            }
        }

        private void baselinePanel_MouseUp(object? sender, MouseEventArgs e)
        {
            _draggingBaseline = false;
            baselinePanel.Cursor = Cursors.Default;
        }


        private void MainForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            ClearEditorPreviewAtlasCache();
            ReplaceEditorPreviewImage(null);
            ReplaceRemapTexturePreviewImage(null);
            ReplaceRemapGlyphZoomImage(null);
            ReplaceCurrentAtlasBitmap(null, null);
        }

        private void ClearEditorPreviewAtlasCache()
        {
            foreach (var bitmap in _editorPreviewAtlasCache.Values)
            {
                bitmap.Dispose();
            }

            _editorPreviewAtlasCache.Clear();
        }

        private void InvalidateEditorPreviewAtlas(string? textureId = null)
        {
            if (string.IsNullOrWhiteSpace(textureId))
            {
                ClearEditorPreviewAtlasCache();
                return;
            }

            if (_editorPreviewAtlasCache.Remove(textureId, out var bitmap))
            {
                bitmap.Dispose();
            }
        }

        private Bitmap? GetEditorPreviewAtlasBitmap(string textureId, out string error)
        {
            error = string.Empty;
            if (_editorPreviewAtlasCache.TryGetValue(textureId, out var cachedBitmap))
            {
                return cachedBitmap;
            }

            if (!TextureAtlasPreviewService.TryLoadAtlasBitmap(_currentFilePath, textureId, out var atlasBitmap, out _, out error) ||
                atlasBitmap == null)
            {
                return null;
            }

            using (atlasBitmap)
            {
                cachedBitmap = (Bitmap)atlasBitmap.Clone();
            }

            _editorPreviewAtlasCache[textureId] = cachedBitmap;
            return cachedBitmap;
        }

        private Bitmap CreateEditorPreviewBitmap(Paragraph paragraph, StringEntry entry, List<Letter> previewLetters)
        {
            var previewWidth = Math.Max(1, editorPreviewPictureBox.ClientSize.Width);
            var previewHeight = Math.Max(1, editorPreviewPictureBox.ClientSize.Height);
            var bitmap = new Bitmap(previewWidth, previewHeight);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Gainsboro);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var baselineY = previewHeight - 8f;
            using var baselinePen = new Pen(Color.FromArgb(37, 99, 235), 1.5f);
            graphics.DrawLine(baselinePen, 8f, baselineY, previewWidth - 8f, baselineY);

            var currentX = 12f + entry.Ua + paragraph.HorizontalSpacing + entry.HorizontalSpacing;
            var drawLimit = previewWidth - 18f;
            var ellipsisDrawn = false;

            using var placeholderFont = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            using var placeholderTextBrush = new SolidBrush(Color.FromArgb(30, 41, 59));
            using var placeholderBorderPen = new Pen(Color.FromArgb(148, 163, 184), 1f);
            using var placeholderFillBrush = new SolidBrush(Color.FromArgb(241, 245, 249));

            foreach (var letter in previewLetters)
            {
                if (currentX >= drawLimit)
                {
                    ellipsisDrawn = true;
                    break;
                }

                if (letter.Code == 0x8001)
                {
                    currentX += Math.Max(8f, letter.PositionOffset != 0 ? Math.Abs(letter.PositionOffset) : 10f);
                    continue;
                }

                if (letter.Code > 0 && letter.Code < 0x8000)
                {
                    var charEntry = _currentMcd!.Chars.FirstOrDefault(x => x.Id == letter.Code);
                    var graph = charEntry != null
                        ? _currentMcd.CharGraphs.FirstOrDefault(x => x.Id == charEntry.Index)
                        : null;

                    if (charEntry != null && graph != null)
                    {
                        var drawX = currentX + graph.Ua;
                        var effectiveWidth = graph.Width;
                        var effectiveHeight = graph.Height;
                        var atlasBitmap = GetEditorPreviewAtlasBitmap(graph.TextureID, out _);
                        var glyphRect = atlasBitmap != null
                            ? GetClampedGlyphRectangle(atlasBitmap.Width, atlasBitmap.Height, graph)
                            : Rectangle.Empty;

                        if (TryGetPreviewSelectionOverride(charEntry, graph, out var overrideAtlasBitmap, out var overrideRect))
                        {
                            atlasBitmap = overrideAtlasBitmap;
                            glyphRect = overrideRect;
                            effectiveWidth = overrideRect.Width;
                            effectiveHeight = overrideRect.Height;
                        }

                        var drawY = baselineY - effectiveHeight + graph.BelowSpacing;
                        var advance = Math.Max(4f, graph.HorizontalSpacing > 0f ? graph.HorizontalSpacing : effectiveWidth);

                        if (atlasBitmap != null)
                        {
                            graphics.DrawImage(
                                atlasBitmap,
                                new RectangleF(drawX, drawY, effectiveWidth, effectiveHeight),
                                glyphRect,
                                GraphicsUnit.Pixel);
                        }
                        else
                        {
                            var fallbackRect = new RectangleF(drawX, baselineY - 14f, Math.Max(12f, effectiveWidth), 14f);
                            graphics.FillRectangle(placeholderFillBrush, fallbackRect);
                            graphics.DrawRectangle(placeholderBorderPen, fallbackRect.X, fallbackRect.Y, fallbackRect.Width, fallbackRect.Height);
                            TextRenderer.DrawText(
                                graphics,
                                charEntry.Char,
                                placeholderFont,
                                Rectangle.Round(fallbackRect),
                                Color.FromArgb(30, 41, 59),
                                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                        }

                        currentX += advance;
                        continue;
                    }
                }

                var tokenText = letter.Code == 0x8003
                    ? $"[{letter.PositionOffset}]"
                    : "?";
                var tokenSize = TextRenderer.MeasureText(tokenText, placeholderFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                var tokenRect = new RectangleF(currentX, baselineY - 14f, tokenSize.Width + 8f, 14f);
                graphics.FillRectangle(placeholderFillBrush, tokenRect);
                graphics.DrawRectangle(placeholderBorderPen, tokenRect.X, tokenRect.Y, tokenRect.Width, tokenRect.Height);
                TextRenderer.DrawText(
                    graphics,
                    tokenText,
                    placeholderFont,
                    Rectangle.Round(tokenRect),
                    Color.FromArgb(30, 41, 59),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                currentX += tokenRect.Width + 2f;
            }

            if (ellipsisDrawn)
            {
                TextRenderer.DrawText(
                    graphics,
                    "...",
                    placeholderFont,
                    new Rectangle((int)Math.Max(0f, drawLimit - 18f), (int)(baselineY - 16f), 18, 16),
                    Color.FromArgb(100, 116, 139),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }

            return bitmap;
        }

        private bool TryGetPreviewSelectionOverride(CharEntry charEntry, CharGraph graph, out Bitmap? atlasBitmap, out Rectangle glyphRect)
        {
            atlasBitmap = null;
            glyphRect = Rectangle.Empty;

            if (_currentAtlasBitmap == null || _newGlyphSelectionRect is not Rectangle selectionRect || !TryGetSelectedTextureGraph(out var selectedEntry, out var _))
            {
                return false;
            }

            if (selectedEntry.Id != charEntry.Id || !string.Equals(_currentPreviewTextureId, graph.TextureID, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            atlasBitmap = _currentAtlasBitmap;
            glyphRect = selectionRect;
            return true;
        }

        private Bitmap CreateEditorPreviewFallbackBitmap(string text)
        {
            var previewWidth = Math.Max(1, editorPreviewPictureBox.ClientSize.Width);
            var previewHeight = Math.Max(1, editorPreviewPictureBox.ClientSize.Height);
            var bitmap = new Bitmap(previewWidth, previewHeight);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            var baselineY = previewHeight - 8f;
            using var baselinePen = new Pen(Color.FromArgb(37, 99, 235), 1.5f);
            graphics.DrawLine(baselinePen, 8f, baselineY, previewWidth - 8f, baselineY);

            using var font = new Font("Segoe UI", 9f, FontStyle.Regular);
            TextRenderer.DrawText(
                graphics,
                text,
                font,
                new Rectangle(12, Math.Max(0, (int)baselineY - 18), previewWidth - 24, 18),
                Color.FromArgb(30, 41, 59),
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

            return bitmap;
        }

        private void RemapTexturePreviewPictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (!_newGlyphSelectionMode || e.Button != MouseButtons.Left || _currentAtlasBitmap == null)
            {
                return;
            }

            if (!TryMapPictureBoxPointToImagePoint(remapTexturePreviewPictureBox, _currentAtlasBitmap.Size, e.Location, out var imagePoint))
            {
                return;
            }

            _isSelectingNewGlyph = true;
            _newGlyphSelectionStartPoint = imagePoint;
            _newGlyphSelectionRect = new Rectangle(imagePoint.X, imagePoint.Y, 1, 1);
            UpdateNewCharacterSelectionInfo();
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
        }

        private void RemapTexturePreviewPictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (!_newGlyphSelectionMode || !_isSelectingNewGlyph || _currentAtlasBitmap == null)
            {
                return;
            }

            if (!TryMapPictureBoxPointToImagePoint(remapTexturePreviewPictureBox, _currentAtlasBitmap.Size, e.Location, out var imagePoint))
            {
                return;
            }

            _newGlyphSelectionRect = NormalizeRectangle(_newGlyphSelectionStartPoint, imagePoint, _currentAtlasBitmap.Size);
            UpdateNewCharacterSelectionInfo();
            ValidateNewCharacterInput(showError: false);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
        }

        private void RemapTexturePreviewPictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_newGlyphSelectionMode && sender == remapTexturePreviewPictureBox && e.Button == MouseButtons.Left)
            {
                _isSelectingNewGlyph = false;
                UpdateNewCharacterSelectionInfo();
                ValidateNewCharacterInput(showError: false);
                return;
            }

            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            if (!TryGetSelectedTextureGraph(out var _, out var graph))
            {
                MessageBox.Show(
                    this,
                    Loc.Get("MainForm.Dialog.ExportDdsRequiresVariant"),
                    Loc.Get("MainForm.Dialog.ExportDdsTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var suggestedName = string.IsNullOrWhiteSpace(_currentFilePath)
                ? Loc.Format("MainForm.Dialog.DdsSuggestedNameFallback", graph.TextureID)
                : Loc.Format("MainForm.Dialog.DdsSuggestedNameFromFile", Path.GetFileNameWithoutExtension(_currentFilePath), graph.TextureID);

            using var saveDialog = new SaveFileDialog
            {
                Filter = Loc.Get("MainForm.Dialog.DdsFilter"),
                Title = Loc.Get("MainForm.Dialog.ExportDdsPickerTitle"),
                FileName = suggestedName
            };

            if (saveDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            if (!TextureAtlasPreviewService.TryExportTextureToDds(_currentFilePath, graph.TextureID, saveDialog.FileName, out var error))
            {
                MessageBox.Show(
                    this,
                    Loc.Format("MainForm.Message.ExportDdsError", Environment.NewLine, error),
                    Loc.Get("MainForm.Dialog.ExportDdsTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.ExportDdsFailed");
                return;
            }

            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.ExportDdsSuccess", Path.GetFileName(saveDialog.FileName));
            MessageBox.Show(
                this,
                Loc.Get("MainForm.Message.ExportDdsSuccess"),
                Loc.Get("MainForm.Dialog.ExportDdsTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void RemapTexturePreviewPictureBox_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (_newGlyphSelectionMode)
            {
                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (!TryGetSelectedTextureGraph(out var entry, out var graph))
            {
                MessageBox.Show(
                    this,
                    Loc.Get("MainForm.Dialog.ImportDdsRequiresVariant"),
                    Loc.Get("MainForm.Dialog.ImportDdsTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using var openDialog = new OpenFileDialog
            {
                Filter = Loc.Get("MainForm.Dialog.DdsFilter"),
                Title = Loc.Get("MainForm.Dialog.ImportDdsPickerTitle")
            };

            if (openDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var confirmation = MessageBox.Show(
                this,
                Loc.Format("MainForm.Dialog.ImportDdsConfirm", graph.TextureID, Environment.NewLine),
                Loc.Get("MainForm.Dialog.ImportDdsTitle"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            if (!TextureAtlasPreviewService.TryImportTextureFromDds(_currentFilePath, graph.TextureID, openDialog.FileName, out var error))
            {
                MessageBox.Show(
                    this,
                    Loc.Format("MainForm.Message.ImportDdsError", Environment.NewLine, error),
                    Loc.Get("MainForm.Dialog.ImportDdsTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                statusToolStripStatusLabel.Text = Loc.Get("MainForm.Status.ImportDdsFailed");
                return;
            }

            InvalidateEditorPreviewAtlas(graph.TextureID);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.ImportDdsSuccess", graph.TextureID);
            MessageBox.Show(
                this,
                Loc.Format("MainForm.Message.ImportDdsSuccess", graph.TextureID, Environment.NewLine, entry.Id),
                Loc.Get("MainForm.Dialog.ImportDdsTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private bool TryGetSelectedTextureGraph(out CharEntry entry, out CharGraph graph)
        {
            entry = null!;
            graph = null!;

            if (_currentMcd == null || remapSourceComboBox.SelectedItem is not CharRemapOption option)
            {
                return false;
            }

            var currentEntry = _currentMcd.Chars.FirstOrDefault(x => x.Id == option.CharId);
            if (currentEntry == null)
            {
                return false;
            }

            var currentGraph = _currentMcd.CharGraphs.FirstOrDefault(x => x.Id == currentEntry.Index);
            if (currentGraph == null)
            {
                return false;
            }

            entry = currentEntry;
            graph = currentGraph;
            return graph != null;
        }

        private static Bitmap CreateTextureAtlasPreviewBitmap(CharGraph graph)
        {
            const int previewWidth = 208;
            const int previewHeight = 122;
            const int padding = 12;

            var bitmap = new Bitmap(previewWidth, previewHeight);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.FromArgb(248, 250, 252));

            var atlasRect = new Rectangle(padding, padding, previewWidth - (padding * 2), previewHeight - (padding * 2));
            using var atlasBrush = new SolidBrush(Color.White);
            using var atlasBorderPen = new Pen(Color.FromArgb(148, 163, 184), 1f);
            graphics.FillRectangle(atlasBrush, atlasRect);
            graphics.DrawRectangle(atlasBorderPen, atlasRect);

            using var gridPen = new Pen(Color.FromArgb(226, 232, 240), 1f);
            for (var step = 1; step < 4; step++)
            {
                var x = atlasRect.Left + (atlasRect.Width * step / 4f);
                var y = atlasRect.Top + (atlasRect.Height * step / 4f);
                graphics.DrawLine(gridPen, x, atlasRect.Top, x, atlasRect.Bottom);
                graphics.DrawLine(gridPen, atlasRect.Left, y, atlasRect.Right, y);
            }

            var u1 = Clamp01(graph.U1);
            var v1 = Clamp01(graph.V1);
            var u2 = Clamp01(graph.U2);
            var v2 = Clamp01(graph.V2);
            var left = atlasRect.Left + (atlasRect.Width * Math.Min(u1, u2));
            var top = atlasRect.Top + (atlasRect.Height * Math.Min(v1, v2));
            var width = Math.Max(2f, atlasRect.Width * Math.Abs(u2 - u1));
            var height = Math.Max(2f, atlasRect.Height * Math.Abs(v2 - v1));
            var glyphRect = new RectangleF(left, top, width, height);

            using var glyphBrush = new SolidBrush(Color.FromArgb(90, 239, 68, 68));
            using var glyphPen = new Pen(Color.FromArgb(220, 220, 38, 38), 2f);
            graphics.FillRectangle(glyphBrush, glyphRect);
            graphics.DrawRectangle(glyphPen, glyphRect.X, glyphRect.Y, glyphRect.Width, glyphRect.Height);

            using var labelBrush = new SolidBrush(Color.FromArgb(15, 23, 42));
            using var labelFont = new Font("Segoe UI", 8f, FontStyle.Bold);
            graphics.DrawString("0,0", labelFont, labelBrush, atlasRect.Left - 2, atlasRect.Top - 12);
            graphics.DrawString("1,1", labelFont, labelBrush, atlasRect.Right - 22, atlasRect.Bottom + 1);

            return bitmap;
        }

        private static Bitmap CreateTextureAtlasOverlayBitmap(Bitmap atlasBitmap, Rectangle glyphRect, Rectangle? selectionRect = null)
        {
            var bitmap = new Bitmap(atlasBitmap.Width, atlasBitmap.Height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.DrawImageUnscaled(atlasBitmap, 0, 0);

            using var fillBrush = new SolidBrush(Color.FromArgb(70, 239, 68, 68));
            using var glyphPen = new Pen(Color.FromArgb(255, 220, 38, 38), 2f);
            graphics.FillRectangle(fillBrush, glyphRect);
            graphics.DrawRectangle(glyphPen, glyphRect.X, glyphRect.Y, glyphRect.Width, glyphRect.Height);

            if (selectionRect is Rectangle selectedRect)
            {
                using var selectionBrush = new SolidBrush(Color.FromArgb(65, 34, 197, 94));
                using var selectionPen = new Pen(Color.FromArgb(255, 22, 163, 74), 2f);
                graphics.FillRectangle(selectionBrush, selectedRect);
                graphics.DrawRectangle(selectionPen, selectedRect.X, selectedRect.Y, selectedRect.Width, selectedRect.Height);
            }

            return bitmap;
        }

        private static Bitmap CreateGlyphZoomBitmap(Bitmap atlasBitmap, Rectangle glyphRect)
        {
            const int zoomPadding = 10;
            var sourceRect = Rectangle.Inflate(glyphRect, zoomPadding, zoomPadding);
            sourceRect.Intersect(new Rectangle(0, 0, atlasBitmap.Width, atlasBitmap.Height));
            if (sourceRect.Width <= 0 || sourceRect.Height <= 0)
            {
                sourceRect = new Rectangle(0, 0, Math.Min(1, atlasBitmap.Width), Math.Min(1, atlasBitmap.Height));
            }

            var bitmap = new Bitmap(sourceRect.Width, sourceRect.Height);
            using var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.FromArgb(248, 250, 252));
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.PixelOffsetMode = PixelOffsetMode.Half;
            graphics.DrawImage(atlasBitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), sourceRect, GraphicsUnit.Pixel);

            var localGlyphRect = new Rectangle(
                glyphRect.X - sourceRect.X,
                glyphRect.Y - sourceRect.Y,
                glyphRect.Width,
                glyphRect.Height);

            using var glyphPen = new Pen(Color.FromArgb(255, 220, 38, 38), 2f);
            using var glyphBrush = new SolidBrush(Color.FromArgb(60, 239, 68, 68));
            graphics.FillRectangle(glyphBrush, localGlyphRect);
            graphics.DrawRectangle(glyphPen, localGlyphRect);
            return bitmap;
        }

        private static Rectangle GetClampedGlyphRectangle(int atlasWidth, int atlasHeight, CharGraph graph)
        {
            var left = (int)Math.Floor(Clamp01(Math.Min(graph.U1, graph.U2)) * atlasWidth);
            var top = (int)Math.Floor(Clamp01(Math.Min(graph.V1, graph.V2)) * atlasHeight);
            var right = (int)Math.Ceiling(Clamp01(Math.Max(graph.U1, graph.U2)) * atlasWidth);
            var bottom = (int)Math.Ceiling(Clamp01(Math.Max(graph.V1, graph.V2)) * atlasHeight);

            left = Math.Clamp(left, 0, Math.Max(0, atlasWidth - 1));
            top = Math.Clamp(top, 0, Math.Max(0, atlasHeight - 1));
            right = Math.Clamp(right, left + 1, atlasWidth);
            bottom = Math.Clamp(bottom, top + 1, atlasHeight);

            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        private static bool TryMapPictureBoxPointToImagePoint(PictureBox pictureBox, Size imageSize, Point picturePoint, out Point imagePoint)
        {
            imagePoint = Point.Empty;
            if (imageSize.Width <= 0 || imageSize.Height <= 0)
            {
                return false;
            }

            var imageRect = GetZoomImageRectangle(pictureBox.ClientSize, imageSize);
            if (!imageRect.Contains(picturePoint))
            {
                return false;
            }

            var relativeX = (float)(picturePoint.X - imageRect.X) / imageRect.Width;
            var relativeY = (float)(picturePoint.Y - imageRect.Y) / imageRect.Height;
            var pixelX = Math.Clamp((int)Math.Floor(relativeX * imageSize.Width), 0, imageSize.Width - 1);
            var pixelY = Math.Clamp((int)Math.Floor(relativeY * imageSize.Height), 0, imageSize.Height - 1);
            imagePoint = new Point(pixelX, pixelY);
            return true;
        }

        private static Rectangle GetZoomImageRectangle(Size containerSize, Size imageSize)
        {
            if (imageSize.Width <= 0 || imageSize.Height <= 0 || containerSize.Width <= 0 || containerSize.Height <= 0)
            {
                return Rectangle.Empty;
            }

            var ratio = Math.Min((float)containerSize.Width / imageSize.Width, (float)containerSize.Height / imageSize.Height);
            var drawWidth = Math.Max(1, (int)Math.Round(imageSize.Width * ratio));
            var drawHeight = Math.Max(1, (int)Math.Round(imageSize.Height * ratio));
            var offsetX = (containerSize.Width - drawWidth) / 2;
            var offsetY = (containerSize.Height - drawHeight) / 2;
            return new Rectangle(offsetX, offsetY, drawWidth, drawHeight);
        }

        private static Rectangle NormalizeRectangle(Point start, Point end, Size imageSize)
        {
            var left = Math.Min(start.X, end.X);
            var top = Math.Min(start.Y, end.Y);
            var right = Math.Max(start.X, end.X) + 1;
            var bottom = Math.Max(start.Y, end.Y) + 1;

            left = Math.Clamp(left, 0, Math.Max(0, imageSize.Width - 1));
            top = Math.Clamp(top, 0, Math.Max(0, imageSize.Height - 1));
            right = Math.Clamp(right, left + 1, imageSize.Width);
            bottom = Math.Clamp(bottom, top + 1, imageSize.Height);

            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            if (value > 1f)
            {
                return 1f;
            }

            return value;
        }

        private static string FormatLanguageFlagsReadable(int value)
        {
            var activeBits = Enumerable.Range(0, 32)
                .Where(bit => (((uint)value >> bit) & 1u) != 0)
                .Select(bit => $"b{bit}")
                .ToArray();

            var bitsText = activeBits.Length == 0
                ? Loc.Get("MainForm.LanguageFlags.NoneActive")
                : string.Join(", ", activeBits);

            return $"0x{(uint)value:X8} ({value}) [{bitsText}]";
        }

        private static bool TryParseLanguageFlags(string value, out int parsedValue)
        {
            parsedValue = 0;
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var trimmed = value.Trim();
            if (trimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return uint.TryParse(trimmed[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hexValue)
                    && TryConvertToInt32(hexValue, out parsedValue);
            }

            if (uint.TryParse(trimmed, NumberStyles.Integer, CultureInfo.InvariantCulture, out var decimalValue))
            {
                return TryConvertToInt32(decimalValue, out parsedValue);
            }

            return false;
        }

        private static bool TryConvertToInt32(uint value, out int parsedValue)
        {
            parsedValue = 0;
            if (value > int.MaxValue)
            {
                return false;
            }

            parsedValue = (int)value;
            return true;
        }

        private static bool TryGetSingleTextElement(string value, out string textElement)
        {
            textElement = string.Empty;
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var indexes = StringInfo.ParseCombiningCharacters(value);
            if (indexes.Length != 1)
            {
                return false;
            }

            textElement = value;
            return true;
        }

        private static string NormalizeSingleTextElement(string value)
        {
            return TryGetSingleTextElement(value, out var textElement) ? textElement : value;
        }

        private int CountCharacterUsage(int charId)
        {
            if (_currentMcd == null)
            {
                return 0;
            }

            var count = 0;
            foreach (var ev in _currentMcd.Events)
            {
                foreach (var paragraph in ev.Paragraphs)
                {
                    foreach (var str in paragraph.Strings)
                    {
                        foreach (var letter in str.Letters)
                        {
                            if (letter.Code == charId)
                            {
                                count++;
                            }
                        }
                    }
                }
            }

            return count;
        }

        private void UpdateSearchActionButtons()
        {
            var hasLoadedFile = _currentMcd != null;
            var hasSearchText = hasLoadedFile && !string.IsNullOrWhiteSpace(searchTextBox.Text);
            nextMatchButton.Enabled = hasLoadedFile && _searchMatches.Count > 1;
            replaceCurrentButton.Enabled = hasSearchText && _searchMatches.Count > 0;
            replaceAllButton.Enabled = hasSearchText && _searchMatches.Count > 0;
        }

        private bool EnsureSearchResultsForReplacement()
        {
            if (_currentMcd == null || !ValidateSearchInput(showError: true))
            {
                UpdateSearchActionButtons();
                return false;
            }

            if (_searchMatches.Count == 0)
            {
                PerformSearch(showEmptyValidation: false, focusFirstMatch: true);
            }

            return _searchMatches.Count > 0;
        }

        private StringComparison GetSearchComparison()
        {
            return caseSensitiveCheckBox.Checked
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;
        }

        private StringEntry GetStringEntry(NodeTag tag)
        {
            return _currentMcd!.Events[tag.EventIndex].Paragraphs[tag.ParagraphIndex].Strings[tag.StringIndex];
        }

        private void SetStringEntryText(NodeTag tag, string text)
        {
            var entry = GetStringEntry(tag);
            entry.Text = text;

            var node = FindNodeByTag(tag);
            if (node != null)
            {
                node.Text = BuildStringNodeText(tag.StringIndex, entry.Text);
            }

            if (textTextBox.Tag is NodeTag activeTag && SameTag(activeTag, tag))
            {
                _suppressEditorEvents = true;
                textTextBox.Text = entry.Text;
                _suppressEditorEvents = false;
                ValidateEditorText(entry.Text);
                UpdateEditorPreview();
            }
        }

        private static bool SameTag(NodeTag left, NodeTag right)
        {
            return left.Type == right.Type
                && left.EventIndex == right.EventIndex
                && left.ParagraphIndex == right.ParagraphIndex
                && left.StringIndex == right.StringIndex;
        }

        private static bool IsMatchAtIndex(string text, string searchText, int index, StringComparison comparison)
        {
            return index >= 0
                && index + searchText.Length <= text.Length
                && string.Compare(text, index, searchText, 0, searchText.Length, comparison) == 0;
        }

        private static string ReplaceOccurrences(
            string source,
            string searchText,
            string replacementText,
            StringComparison comparison,
            out int replacements)
        {
            replacements = 0;
            if (string.IsNullOrEmpty(searchText))
            {
                return source;
            }

            var builder = new StringBuilder();
            var startIndex = 0;

            while (startIndex < source.Length)
            {
                var matchIndex = source.IndexOf(searchText, startIndex, comparison);
                if (matchIndex < 0)
                {
                    break;
                }

                builder.Append(source, startIndex, matchIndex - startIndex);
                builder.Append(replacementText);
                startIndex = matchIndex + searchText.Length;
                replacements++;
            }

            if (replacements == 0)
            {
                return source;
            }

            builder.Append(source, startIndex, source.Length - startIndex);
            return builder.ToString();
        }

        private void ClearSearchResults()
        {
            _searchMatches.Clear();
            _currentMatchIndex = -1;
            UpdateSearchActionButtons();
        }

        private void GotoMatch(int matchIndex)
        {
            if (matchIndex < 0 || matchIndex >= _searchMatches.Count || _currentMcd == null)
            {
                return;
            }

            _currentMatchIndex = matchIndex;
            var match = _searchMatches[matchIndex];
            var node = FindNodeByTag(match.Tag);

            if (node != null)
            {
                node.EnsureVisible();
                eventTreeView.SelectedNode = node;
            }

            textTextBox.Focus();
            textTextBox.Select(match.Index, searchTextBox.Text.Trim().Length);
            textTextBox.ScrollToCaret();

            statusToolStripStatusLabel.Text = Loc.Format("MainForm.Status.ResultPosition", matchIndex + 1, _searchMatches.Count);
            UpdateSearchHelper(
                Loc.Format("MainForm.SearchHint.ResultPosition", matchIndex + 1, _searchMatches.Count),
                SuccessColor);
        }

        private TreeNode? FindNodeByTag(NodeTag tag)
        {
            foreach (TreeNode eventNode in eventTreeView.Nodes)
            {
                foreach (TreeNode paragraphNode in eventNode.Nodes)
                {
                    foreach (TreeNode stringNode in paragraphNode.Nodes)
                    {
                        if (stringNode.Tag is NodeTag nodeTag &&
                            nodeTag.EventIndex == tag.EventIndex &&
                            nodeTag.ParagraphIndex == tag.ParagraphIndex &&
                            nodeTag.StringIndex == tag.StringIndex)
                        {
                            return stringNode;
                        }
                    }
                }
            }

            return null;
        }

        private void RefreshUiAfterTextImport(NodeTag? preferredSelection)
        {
            PopulateTreeView();

            var selectedNode = preferredSelection != null
                ? FindNodeByTag(preferredSelection)
                : null;

            if (selectedNode != null)
            {
                selectedNode.EnsureVisible();
                eventTreeView.SelectedNode = selectedNode;
            }
            else
            {
                TrySelectFirstStringNode();
            }

            if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                PerformSearch(showEmptyValidation: false, focusFirstMatch: false);
            }
            else if (textTextBox.Tag is NodeTag activeTag && _currentMcd != null)
            {
                var activeText = _currentMcd.Events[activeTag.EventIndex].Paragraphs[activeTag.ParagraphIndex].Strings[activeTag.StringIndex].Text;
                ValidateEditorText(activeText);
            }
        }

        private static NodeTag CloneTag(NodeTag source)
        {
            return new NodeTag
            {
                Type = source.Type,
                EventIndex = source.EventIndex,
                ParagraphIndex = source.ParagraphIndex,
                StringIndex = source.StringIndex
            };
        }

        private readonly record struct SearchMatch(NodeTag Tag, int Index);

        private sealed record CharRemapOption(string Value, int CharId, int CharCode, int VariantIndex, int VariantsCount, int LanguageFlags)
        {
            public override string ToString()
            {
                var codeLabel = CharCode > 0
                    ? $"U+{CharCode:X4}"
                    : Loc.Get("MainForm.CharRemapOption.NoCode");

                var languageFlagsLabel = FormatLanguageFlagsReadable(LanguageFlags);

                return Loc.Format("MainForm.CharRemapOption.Format", Value, VariantIndex, VariantsCount, CharId, codeLabel, languageFlagsLabel);
            }
        }

        private sealed record LanguageComboOption(string Code, string DisplayName)
        {
            public override string ToString() => DisplayName;
        }
    }

    internal enum NodeType
    {
        Event,
        Paragraph,
        String
    }

    internal sealed class NodeTag
    {
        public NodeType Type { get; set; }

        public int EventIndex { get; set; }

        public int ParagraphIndex { get; set; }

        public int StringIndex { get; set; }
    }
}
