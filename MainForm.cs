using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StarFoxZeroLocalizationTool.Models;
using StarFoxZeroLocalizationTool.Services;

namespace StarFoxZeroLocalizationTool
{
    public partial class MainForm : Form
    {
        private static readonly Color InfoColor = Color.FromArgb(71, 85, 105);
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
        public MainForm()
        {
            InitializeComponent();
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

        private void ConfigureInitialState()
        {
            loadedFileValueLabel.Text = "Nenhum arquivo carregado";
            selectedEntryLabel.Text = "Nenhuma string selecionada";
            navigationSummaryLabel.Text = "Carregue um arquivo MCD para visualizar os eventos.";
            UpdateSearchHelper("Digite um termo e pressione Enter para pesquisar.", InfoColor);
            UpdateEditorHelper("...", InfoColor);
            UpdateRemapVariantDetails(null);
            UpdateLanguageFlagsCurrentValue(null);
            UpdateNewCharacterBaseInfo(null);
            UpdateNewCharacterSelectionInfo();
            UpdateRemapTexturePreview(null);
            UpdateEditorPreview();
            UpdateRemapHelper("Carregue um arquivo para remapear caracteres do charset.", InfoColor);
            statusToolStripStatusLabel.Text = "Pronto";
            UpdateUiState(false);
        }

        private void UpdateUiState(bool hasLoadedFile)
        {
            saveButton.Enabled = hasLoadedFile;
            salvarToolStripMenuItem.Enabled = hasLoadedFile;
            exportarCsvToolStripMenuItem.Enabled = hasLoadedFile;
            importarCsvToolStripMenuItem.Enabled = hasLoadedFile;
            searchTextBox.Enabled = hasLoadedFile;
            caseSensitiveCheckBox.Enabled = hasLoadedFile;
            replaceTextBox.Enabled = hasLoadedFile;
            searchButton.Enabled = hasLoadedFile;
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
                Filter = "MCD Files (*.mcd)|*.mcd|All Files (*.*)|*.*",
                Title = "Abrir arquivo MCD"
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
                statusToolStripStatusLabel.Text = $"Arquivo carregado: {Path.GetFileName(ofd.FileName)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao carregar o arquivo: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                statusToolStripStatusLabel.Text = "Falha ao carregar o arquivo.";
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
                Filter = "MCD Files (*.mcd)|*.mcd|All Files (*.*)|*.*",
                Title = "Salvar arquivo MCD",
                FileName = _currentFilePath ?? "output.mcd"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                McdIO.WriteMcd(_currentMcd, sfd.FileName);
                statusToolStripStatusLabel.Text = $"Arquivo salvo: {Path.GetFileName(sfd.FileName)}";

                MessageBox.Show(
                    "Arquivo salvo com sucesso.",
                    "Salvar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao salvar o arquivo: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                statusToolStripStatusLabel.Text = "Falha ao salvar o arquivo.";
            }
        }

        private void ExportCsvButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            var suggestedFileName = string.IsNullOrWhiteSpace(_currentFilePath)
                ? "mcd_strings.csv"
                : Path.GetFileNameWithoutExtension(_currentFilePath) + "_strings.csv";

            using var sfd = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                Title = "Exportar strings do MCD para CSV",
                FileName = suggestedFileName
            };

            if (sfd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                McdTextExchangeService.ExportToCsv(_currentMcd, sfd.FileName, _currentFilePath);
                statusToolStripStatusLabel.Text = $"CSV exportado: {Path.GetFileName(sfd.FileName)}";
                MessageBox.Show(
                    this,
                    "Strings exportadas com sucesso para CSV.",
                    "Exportar CSV",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                statusToolStripStatusLabel.Text = "Falha ao exportar CSV.";
                MessageBox.Show(
                    this,
                    $"Erro ao exportar o CSV: {ex.Message}",
                    "Erro",
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
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                Title = "Importar strings de CSV para o MCD carregado"
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

                var summary = $"Importacao concluida. Aplicadas: {result.AppliedEntries}/{result.TotalImportedEntries}. "
                            + $"Exatas: {result.ExactMatches}. Fallback por ordem: {result.IndexFallbackMatches}. "
                            + $"Sem traducao: {result.SkippedEmptyTranslatedRows}. "
                            + $"Nao encontradas: {result.UnmatchedEntries}.";

                statusToolStripStatusLabel.Text = summary;
                MessageBox.Show(
                    this,
                    summary,
                    "Importar CSV",
                    MessageBoxButtons.OK,
                    result.UnmatchedEntries == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                statusToolStripStatusLabel.Text = "Falha ao importar CSV.";
                MessageBox.Show(
                    this,
                    $"Erro ao importar o CSV: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void OpenGtxDdsToolMenuItem_Click(object? sender, EventArgs e)
        {
            using var toolForm = new GtxDdsToolForm();
            toolForm.ShowDialog(this);
        }

        private void OpenGtxRawEditorMenuItem_Click(object? sender, EventArgs e)
        {
            using var toolForm = new GtxRawTextureEditorForm();
            toolForm.ShowDialog(this);
        }

        private void SearchTextBox_TextChanged(object? sender, EventArgs e)
        {
            ClearSearchResults();
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                validationErrorProvider.SetError(searchTextBox, string.Empty);

                if (_currentMcd != null)
                {
                    UpdateSearchHelper("Digite um termo e pressione Enter para pesquisar.", InfoColor);
                }

                return;
            }

            validationErrorProvider.SetError(searchTextBox, string.Empty);
            UpdateSearchHelper("Pressione Enter ou clique em Pesquisar.", InfoColor);
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
                UpdateSearchHelper("Modo da pesquisa atualizado.", InfoColor);
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
                UpdateSearchHelper("Pesquise um termo com resultados antes de substituir.", WarningColor);
                statusToolStripStatusLabel.Text = "Substituicao indisponivel sem resultados.";
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
                    UpdateSearchHelper("Nenhum resultado valido permaneceu para substituir.", WarningColor);
                    statusToolStripStatusLabel.Text = "Nenhuma substituicao aplicada.";
                    return;
                }

                match = _searchMatches[Math.Max(_currentMatchIndex, 0)];
                entry = GetStringEntry(match.Tag);
            }

            var updatedText = entry.Text.Remove(match.Index, searchText.Length).Insert(match.Index, replacementText);
            SetStringEntryText(match.Tag, updatedText);

            PerformSearch(showEmptyValidation: false, focusFirstMatch: true);
            UpdateSearchHelper("Ocorrencia atual substituida com sucesso.", SuccessColor);
            statusToolStripStatusLabel.Text = "Substituicao da ocorrencia atual concluida.";
        }

        private void ReplaceAllButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (!EnsureSearchResultsForReplacement())
            {
                UpdateSearchHelper("Pesquise um termo com resultados antes de substituir.", WarningColor);
                statusToolStripStatusLabel.Text = "Substituicao indisponivel sem resultados.";
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
                UpdateSearchHelper("Nenhuma ocorrencia encontrada para substituir.", WarningColor);
                statusToolStripStatusLabel.Text = "Nenhuma substituicao aplicada.";
                return;
            }

            RefreshUiAfterTextImport(previousSelection);
            UpdateSearchHelper(
                $"{totalReplacements} ocorrencia(s) substituida(s) em {changedStrings} string(s).",
                SuccessColor);
            statusToolStripStatusLabel.Text =
                $"Substituicao concluida: {totalReplacements} ocorrencia(s) em {changedStrings} string(s).";
        }

        private void PerformSearch(bool showEmptyValidation = true, bool focusFirstMatch = true)
        {
            if (_currentMcd == null)
            {
                UpdateSearchHelper("Carregue um arquivo MCD antes de pesquisar.", WarningColor);
                statusToolStripStatusLabel.Text = "Pesquisa indisponivel sem arquivo carregado.";
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
                UpdateSearchHelper("Nenhum resultado encontrado para o termo informado.", WarningColor);
                statusToolStripStatusLabel.Text = "Pesquisa concluida sem resultados.";
                return;
            }

            UpdateSearchHelper($"{_searchMatches.Count} ocorrencia(s) encontrada(s).", SuccessColor);
            statusToolStripStatusLabel.Text = $"Pesquisa concluida: {_searchMatches.Count} resultado(s).";

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
                validationErrorProvider.SetError(searchTextBox, "Informe um texto para pesquisar.");
                UpdateSearchHelper("O campo de pesquisa marcado com * e obrigatorio para buscar.", WarningColor);
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
                UpdateRemapHelper("Selecione um caractere de origem valido para o remapeamento.", WarningColor);
                statusToolStripStatusLabel.Text = "Falha ao remapear caractere.";
                return;
            }
            var targetValue = NormalizeSingleTextElement(remapTargetTextBox.Text.Trim());
            var affectedEntry = _currentMcd.Chars.FirstOrDefault(x => x.Id == sourceOption.CharId);

            if (affectedEntry == null)
            {
                UpdateRemapHelper("Nenhuma variante do charset foi encontrada para o caractere selecionado.", WarningColor);
                statusToolStripStatusLabel.Text = "Falha ao remapear caractere.";
                return;
            }

            var overwriteWarning = _currentMcd.Chars.Any(x => x.Char == targetValue && x.Id != sourceOption.CharId);
            if (overwriteWarning)
            {
                var result = MessageBox.Show(
                    $"O caractere '{targetValue}' ja existe no charset. Deseja continuar mesmo assim?",
                    "Confirmar remapeamento",
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
                $"Variante ID {sourceOption.CharId} do caractere '{sourceOption.Value}' remapeada para '{targetValue}'.",
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = $"Remapeamento aplicado: ID {sourceOption.CharId} {sourceOption.Value} -> {targetValue}.";
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
                var eventName = usedEvent?.Name ?? $"Evento {ev.Id}";

                var eventNode = new TreeNode($"{eventName} ({ev.EventID})")
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
                    var paragraphNode = new TreeNode($"Paragrafo {paragraph.Id}")
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

            navigationSummaryLabel.Text = $"Eventos: {_currentMcd.Events.Count}  |  Strings: {totalStrings}";
            eventTreeView.EndUpdate();
        }

        private static string BuildStringNodeText(int stringIndex, string text)
        {
            var preview = string.IsNullOrWhiteSpace(text) ? "(vazio)" : text.Trim();
            if (preview.Length > 36)
            {
                preview = preview[..36] + "...";
            }

            return $"String {stringIndex}: {preview}";
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

                selectedEntryLabel.Text = "Nenhuma string selecionada";
                UpdateEditorHelper("Selecione uma string para editar o texto.", InfoColor);
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
                $"Evento {tag.EventIndex}  |  Paragrafo {tag.ParagraphIndex}  |  String {tag.StringIndex}  |  LanguageFlags {FormatLanguageFlagsReadable(paragraph.LanguageFlags)}";
            ValidateEditorText(entry.Text);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = "String selecionada para edicao.";
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
                UpdateEditorHelper("Todos os caracteres existem no charset/texturas.", SuccessColor);
                return;
            }

            validationErrorProvider.SetError(textTextBox, "Existem caracteres que nao estao disponiveis no charset.");
            UpdateEditorHelper(
                $"Caracteres ausentes no charset/texturas: {string.Join(", ", missingChars)}",
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
                            group.Count())))
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
                    validationErrorProvider.SetError(remapSourceComboBox, "Selecione um caractere existente do charset.");
                    UpdateRemapHelper("Escolha um caractere existente para remapear.", WarningColor);
                }

                applyCharRemapButton.Enabled = false;
                return false;
            }

            if (!TryGetSingleTextElement(remapTargetTextBox.Text.Trim(), out var targetValue))
            {
                if (showError)
                {
                    validationErrorProvider.SetError(remapTargetTextBox, "Informe exatamente um caractere.");
                    UpdateRemapHelper("Digite exatamente um unico caractere de destino, como 'Ã§'.", WarningColor);
                }

                applyCharRemapButton.Enabled = false;
                return false;
            }

            if (targetValue == sourceOption.Value)
            {
                if (showError)
                {
                    validationErrorProvider.SetError(remapTargetTextBox, "O novo caractere precisa ser diferente do atual.");
                    UpdateRemapHelper("Escolha um caractere diferente do existente para aplicar o remapeamento.", WarningColor);
                }

                applyCharRemapButton.Enabled = false;
                return false;
            }

            applyCharRemapButton.Enabled = true;
            UpdateRemapHelper(
                $"A variante {sourceOption.VariantIndex}/{sourceOption.VariantsCount} de '{sourceOption.Value}' (ID {sourceOption.CharId}, U+{sourceOption.CharCode:X4}) sera remapeada para '{targetValue}'.",
                InfoColor);
            return true;
        }

        private void UpdateSearchHelper(string message, Color color)
        {
            searchHelperLabel.Text = message;
            searchHelperLabel.ForeColor = color;
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
                editorPreviewInfoLabel.Text = "Linha azul = baseline. Selecione uma string para visualizar a altura dos glifos.";
                editorPreviewInfoLabel.ForeColor = InfoColor;
                return;
            }

            try
            {
                var previewLetters = McdIO.BuildPreviewLetters(entry.Text, _currentMcd, entry.Letters, paragraph.LanguageFlags);
                var previewBitmap = CreateEditorPreviewBitmap(paragraph, entry, previewLetters);
                ReplaceEditorPreviewImage(previewBitmap);
                editorPreviewInfoLabel.Text = BuildEditorPreviewInfoText(paragraph, entry);
                editorPreviewInfoLabel.ForeColor = Color.FromArgb(37, 99, 235);
            }
            catch (Exception ex)
            {
                ReplaceEditorPreviewImage(CreateEditorPreviewFallbackBitmap(entry.Text));
                editorPreviewInfoLabel.Text = $"Linha azul = baseline. Previa parcial: {ex.Message}";
                editorPreviewInfoLabel.ForeColor = WarningColor;
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
            var info =
                $"Linha azul = baseline | Paragraph Below {paragraph.BelowSpacing:0.##} | String Below {entry.BelowSpacing:0.##} | String Horizontal {entry.HorizontalSpacing:0.##}";

            if (TryGetSelectedTextureGraph(out var selectedEntry, out var selectedGraph))
            {
                info +=
                    $" | Variante atual '{selectedEntry.Char}' ID {selectedEntry.Id}: Below {selectedGraph.BelowSpacing:0.##}, U_A {selectedGraph.Ua:0.##}";

                if (_newGlyphSelectionRect is Rectangle selectionRect && _currentAtlasBitmap != null &&
                    string.Equals(_currentPreviewTextureId, selectedGraph.TextureID, StringComparison.OrdinalIgnoreCase))
                {
                    info += $" | Previa usando selecao {selectionRect.Width}x{selectionRect.Height}";
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
            if (_currentMcd == null || option == null)
            {
                remapVariantDetailsLabel.Text = "Detalhes da variante selecionada aparecerao aqui.";
                remapVariantDetailsLabel.ForeColor = InfoColor;
                return;
            }

            var entry = _currentMcd.Chars.FirstOrDefault(x => x.Id == option.CharId);
            if (entry == null)
            {
                remapVariantDetailsLabel.Text = "Nao foi possivel localizar os detalhes da variante selecionada.";
                remapVariantDetailsLabel.ForeColor = WarningColor;
                return;
            }

            remapVariantDetailsLabel.Text =
                $"Detalhes: ID {entry.Id} | CharCode U+{entry.CharCode:X4} | LanguageFlags {FormatLanguageFlagsReadable(entry.LanguageFlags)} | Index {entry.Index}";
            remapVariantDetailsLabel.ForeColor = Color.FromArgb(37, 99, 235);
        }

        private void UpdateLanguageFlagsCurrentValue(CharRemapOption? option)
        {
            if (_currentMcd == null || option == null)
            {
                remapLanguageCurrentValueLabel.Text = "Nenhum valor.";
                return;
            }

            var entry = _currentMcd.Chars.FirstOrDefault(x => x.Id == option.CharId);
            remapLanguageCurrentValueLabel.Text = entry == null
                ? "Nao encontrado."
                : FormatLanguageFlagsReadable(entry.LanguageFlags);
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
                    validationErrorProvider.SetError(remapLanguageTargetTextBox, "Informe um LanguageFlags valido em decimal ou 0x hexadecimal.");
                    UpdateRemapHelper("Informe o novo LanguageFlags em decimal ou hexadecimal. Ex.: 12 ou 0x000C.", WarningColor);
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
                    validationErrorProvider.SetError(remapLanguageTargetTextBox, "Informe um valor diferente do LanguageFlags atual.");
                    UpdateRemapHelper("Digite um LanguageFlags diferente do atual para aplicar a alteracao.", WarningColor);
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
                $"Novo LanguageFlags preparado: {FormatLanguageFlagsReadable(currentEntry.LanguageFlags)} -> {FormatLanguageFlagsReadable(parsedValue)}.",
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
                UpdateRemapHelper("Selecione uma variante valida antes de alterar o LanguageFlags.", WarningColor);
                statusToolStripStatusLabel.Text = "Falha ao alterar LanguageFlags.";
                return;
            }

            if (!TryParseLanguageFlags(remapLanguageTargetTextBox.Text.Trim(), out var newLanguageFlags))
            {
                UpdateRemapHelper("Nao foi possivel interpretar o LanguageFlags informado.", WarningColor);
                statusToolStripStatusLabel.Text = "Falha ao alterar LanguageFlags.";
                return;
            }

            var affectedEntry = _currentMcd.Chars.FirstOrDefault(x => x.Id == sourceOption.CharId);
            if (affectedEntry == null)
            {
                UpdateRemapHelper("Nenhuma variante do charset foi encontrada para alterar o LanguageFlags.", WarningColor);
                statusToolStripStatusLabel.Text = "Falha ao alterar LanguageFlags.";
                return;
            }

            affectedEntry.LanguageFlags = newLanguageFlags;
            PopulateRemapSourceOptions(sourceOption.CharId);
            UpdateRemapVariantDetails(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateLanguageFlagsCurrentValue(remapSourceComboBox.SelectedItem as CharRemapOption);
            ValidateLanguageFlagsInput(showError: false);

            UpdateRemapHelper(
                $"LanguageFlags da variante ID {sourceOption.CharId} atualizado para {FormatLanguageFlagsReadable(newLanguageFlags)}.",
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = $"LanguageFlags alterado na variante ID {sourceOption.CharId}.";
        }

        private void RemoveCharacterButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (remapSourceComboBox.SelectedItem is not CharRemapOption sourceOption)
            {
                UpdateRemapHelper("Selecione uma variante valida antes de remover um caractere.", WarningColor);
                statusToolStripStatusLabel.Text = "Falha ao remover caractere.";
                return;
            }

            var entry = _currentMcd.Chars.FirstOrDefault(x => x.Id == sourceOption.CharId);
            if (entry == null)
            {
                UpdateRemapHelper("Nao foi possivel localizar a variante selecionada para remocao.", WarningColor);
                statusToolStripStatusLabel.Text = "Falha ao remover caractere.";
                return;
            }

            var usageCount = CountCharacterUsage(entry.Id);
            if (usageCount > 0)
            {
                MessageBox.Show(
                    this,
                    $"A variante ID {entry.Id} ainda esta sendo usada em {usageCount} letra(s) nas strings.{Environment.NewLine}Remapeie ou substitua esse caractere antes de remove-lo.",
                    "Remover caractere",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                statusToolStripStatusLabel.Text = "Remocao bloqueada: caractere ainda em uso.";
                return;
            }

            var confirmation = MessageBox.Show(
                this,
                $"Deseja remover a variante selecionada?{Environment.NewLine}{Environment.NewLine}" +
                $"Caractere: '{entry.Char}'{Environment.NewLine}" +
                $"ID: {entry.Id}{Environment.NewLine}" +
                $"Graph: {entry.Index}{Environment.NewLine}" +
                $"LanguageFlags: {FormatLanguageFlagsReadable(entry.LanguageFlags)}",
                "Confirmar remocao",
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
                $"Variante removida com sucesso: '{sourceOption.Value}' (ID {removedCharId})." +
                (graphStillUsed ? string.Empty : $" Graph {removedGraphId} tambem removido."),
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = $"Caractere removido: ID {removedCharId}.";
        }

        private void SelectNewGlyphButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (_currentAtlasBitmap == null || string.IsNullOrWhiteSpace(_currentPreviewTextureId))
            {
                UpdateRemapHelper("A atlas real precisa estar carregada para selecionar um novo glifo.", WarningColor);
                statusToolStripStatusLabel.Text = "Selecao de glifo indisponivel sem atlas real.";
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
                statusToolStripStatusLabel.Text = "Falha ao cadastrar novo caractere.";
                return;
            }

            if (!TryGetSingleTextElement(newCharacterTextBox.Text.Trim(), out var newCharacter) ||
                !TryParseLanguageFlags(newCharacterLanguageTextBox.Text.Trim(), out var languageFlags) ||
                _newGlyphSelectionRect is not Rectangle glyphRect ||
                !TryGetSelectedTextureGraph(out var baseEntry, out var baseGraph) ||
                _currentAtlasBitmap == null)
            {
                UpdateRemapHelper("Nao foi possivel montar os dados do novo caractere.", WarningColor);
                statusToolStripStatusLabel.Text = "Falha ao cadastrar novo caractere.";
                return;
            }

            var duplicateExists = _currentMcd.Chars.Any(x => x.Char == newCharacter && x.LanguageFlags == languageFlags);
            if (duplicateExists)
            {
                var confirmation = MessageBox.Show(
                    this,
                    $"Ja existe pelo menos uma entrada para '{newCharacter}' com LanguageFlags {languageFlags}.{Environment.NewLine}Deseja continuar mesmo assim?",
                    "Cadastrar caractere",
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
                $"Novo caractere '{newCharacter}' cadastrado com ID {newCharId}, graph {newGraphId} e TextureID {baseGraph.TextureID}.",
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = $"Novo caractere cadastrado: '{newCharacter}' (ID {newCharId}).";
        }

        private void UpdateSelectedGlyphButton_Click(object? sender, EventArgs e)
        {
            if (_currentMcd == null)
            {
                return;
            }

            if (!ValidateSelectedGlyphUpdate(showError: true))
            {
                statusToolStripStatusLabel.Text = "Falha ao atualizar o glifo da variante.";
                return;
            }

            if (_newGlyphSelectionRect is not Rectangle glyphRect ||
                !TryGetSelectedTextureGraph(out var entry, out var graph) ||
                _currentAtlasBitmap == null)
            {
                UpdateRemapHelper("Nao foi possivel obter os dados necessarios para atualizar o glifo atual.", WarningColor);
                statusToolStripStatusLabel.Text = "Falha ao atualizar o glifo da variante.";
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
                    ? $"Glifo da variante ID {entry.Id} atualizado com nova area da atlas. Como o graph antigo era compartilhado, foi criado o graph {targetGraph.Id} so para essa variante."
                    : $"Glifo da variante ID {entry.Id} atualizado com a nova selecao da atlas.",
                SuccessColor);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = $"Glifo atualizado na variante ID {entry.Id}.";
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
                    remapTexturePreviewLabel.Text = "A posicao da letra na textura aparecera aqui.";
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
                    remapTexturePreviewLabel.Text = "Nao foi possivel localizar a area dessa letra na textura.";
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

                        remapTexturePreviewLabel.Text =
                            $"TextureID: {graph.TextureID}  |  Atlas real carregada{Environment.NewLine}" +
                            $"Arquivo: {Path.GetFileName(atlasInfo!.WtaPath)} + {Path.GetFileName(atlasInfo.WtpPath)}{Environment.NewLine}" +
                            $"Area UV: U {graph.U1:0.0000} -> {graph.U2:0.0000} | V {graph.V1:0.0000} -> {graph.V2:0.0000}{Environment.NewLine}" +
                            $"Area em pixels: X {glyphRect.X}, Y {glyphRect.Y}, L {glyphRect.Width}, A {glyphRect.Height}{Environment.NewLine}" +
                            $"Indice do graph no charset: {entry.Index}{Environment.NewLine}" +
                            $"Esquerda: atlas completa com a letra marcada e a nova selecao em verde. Direita: zoom simples do glifo.";
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
                remapTexturePreviewLabel.Text =
                    $"TextureID: {graph.TextureID}{Environment.NewLine}" +
                    $"Nao foi possivel carregar a atlas real: {atlasError}{Environment.NewLine}" +
                    $"Area UV: U {graph.U1:0.0000} -> {graph.U2:0.0000} | V {graph.V1:0.0000} -> {graph.V2:0.0000}{Environment.NewLine}" +
                    $"Tamanho normalizado: {fallbackWidthNorm:0.0000} x {fallbackHeightNorm:0.0000}{Environment.NewLine}" +
                    $"Indice do graph no charset: {entry.Index}{Environment.NewLine}" +
                    $"A atlas real nao foi carregada.";
                remapTexturePreviewLabel.ForeColor = WarningColor;
                UpdateNewCharacterSelectionInfo();
            }
            catch (Exception ex)
            {
                ReplaceCurrentAtlasBitmap(null, null);
                ReplaceRemapTexturePreviewImage(null);
                ReplaceRemapGlyphZoomImage(null);
                remapTexturePreviewLabel.Text =
                    $"Falha ao montar a pre-visualizacao da textura.{Environment.NewLine}{ex.Message}";
                remapTexturePreviewLabel.ForeColor = WarningColor;
                UpdateNewCharacterSelectionInfo();
            }
        }

        private void UpdateNewCharacterBaseInfo(CharRemapOption? option)
        {
            if (_currentMcd == null || option == null)
            {
                newCharacterBaseInfoLabel.Text = "Variante base/atual: selecione uma variante para editar ou herdar TextureID e metricas.";
                return;
            }

            var entry = _currentMcd.Chars.FirstOrDefault(x => x.Id == option.CharId);
            var graph = entry != null
                ? _currentMcd.CharGraphs.FirstOrDefault(x => x.Id == entry.Index)
                : null;

            if (entry == null || graph == null)
            {
                newCharacterBaseInfoLabel.Text = "Variante base/atual: nao foi possivel localizar a variante selecionada.";
                return;
            }

            if (string.IsNullOrWhiteSpace(newCharacterLanguageTextBox.Text))
            {
                newCharacterLanguageTextBox.Text = entry.LanguageFlags.ToString(CultureInfo.InvariantCulture);
            }

            newCharacterBaseInfoLabel.Text =
                $"Variante base/atual: '{entry.Char}' | ID {entry.Id} | Graph {graph.Id} | TextureID {graph.TextureID} | " +
                $"Tamanho {graph.Width:0.##} x {graph.Height:0.##} | Below {graph.BelowSpacing:0.##} | Horizontal {graph.HorizontalSpacing:0.##}";
        }

        private void UpdateNewCharacterSelectionInfo()
        {
            if (_currentMcd == null)
            {
                newCharacterSelectionLabel.Text = "Selecao do glifo: Escolha uma variante para atualizar ou usar como base.";
                selectNewGlyphButton.Text = "Selecionar glifo";
                updateSelectedGlyphButton.Enabled = false;
                UpdateSelectionAdjustButtonsState();
                return;
            }

            if (_currentAtlasBitmap == null || string.IsNullOrWhiteSpace(_currentPreviewTextureId))
            {
                newCharacterSelectionLabel.Text = "Selecao do glifo: a atlas real precisa estar carregada para marcar a area.";
                selectNewGlyphButton.Text = "Selecionar glifo";
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
                var suffix = _newGlyphSelectionMode ? " | modo de selecao ativo" : string.Empty;
                newCharacterSelectionLabel.Text =
                    $"Selecao do glifo: X {rect.X}, Y {rect.Y}, L {rect.Width}, A {rect.Height} | " +
                    $"UV {u1:0.0000},{v1:0.0000} -> {u2:0.0000},{v2:0.0000}{suffix}";
            }
            else if (_newGlyphSelectionMode)
            {
                newCharacterSelectionLabel.Text = "Selecao do glifo: arraste com o botao esquerdo sobre a atlas da esquerda para marcar a area que sera usada.";
            }
            else
            {
                newCharacterSelectionLabel.Text = "Selecao do glifo: clique em 'Selecionar glifo' e arraste na atlas para atualizar o glifo atual ou cadastrar um novo caractere.";
            }

            selectNewGlyphButton.Text = _newGlyphSelectionMode ? "Selecionando..." : "Selecionar glifo";
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
                    UpdateRemapHelper("Selecione uma variante valida para usar como base do novo caractere.", WarningColor);
                }

                return false;
            }

            if (!TryGetSingleTextElement(newCharacterTextBox.Text.Trim(), out var newCharacter))
            {
                createNewCharacterButton.Enabled = false;
                if (showError)
                {
                    validationErrorProvider.SetError(newCharacterTextBox, "Informe exatamente um caractere.");
                    UpdateRemapHelper("Digite exatamente um unico caractere para cadastrar.", WarningColor);
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
                    validationErrorProvider.SetError(newCharacterLanguageTextBox, "Informe um LanguageFlags valido em decimal ou 0x hexadecimal.");
                    UpdateRemapHelper("Informe o LanguageFlags do novo caractere em decimal ou hexadecimal.", WarningColor);
                }

                return false;
            }

            if (_currentAtlasBitmap == null || string.IsNullOrWhiteSpace(_currentPreviewTextureId))
            {
                createNewCharacterButton.Enabled = false;
                if (showError)
                {
                    UpdateRemapHelper("A atlas real precisa estar carregada para cadastrar o novo caractere.", WarningColor);
                }

                return false;
            }

            if (_newGlyphSelectionRect is not Rectangle selectionRect || selectionRect.Width <= 0 || selectionRect.Height <= 0)
            {
                createNewCharacterButton.Enabled = false;
                if (showError)
                {
                    UpdateRemapHelper("Marque na atlas a area livre onde o novo glifo foi desenhado.", WarningColor);
                }

                return false;
            }

            createNewCharacterButton.Enabled = true;
            UpdateRemapHelper(
                $"Novo caractere pronto: '{newCharacter}' | LanguageFlags {FormatLanguageFlagsReadable(languageFlags)} | " +
                $"Base '{baseEntry.Char}' (TextureID {baseGraph.TextureID}) | area {selectionRect.Width}x{selectionRect.Height}.",
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
                    UpdateRemapHelper("A atlas real precisa estar carregada para atualizar o glifo da variante selecionada.", WarningColor);
                }

                return false;
            }

            if (_newGlyphSelectionRect is not Rectangle selectionRect || selectionRect.Width <= 0 || selectionRect.Height <= 0)
            {
                updateSelectedGlyphButton.Enabled = false;
                if (showError)
                {
                    UpdateRemapHelper("Marque uma area na atlas para substituir o glifo da variante selecionada.", WarningColor);
                }

                return false;
            }

            updateSelectedGlyphButton.Enabled = true;
            if (showError)
            {
                UpdateRemapHelper(
                    $"Atualizacao pronta para a variante ID {entry.Id}: graph {graph.Id} sera apontado para a area {selectionRect.Width}x{selectionRect.Height} selecionada na atlas.",
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
                    validationErrorProvider.SetError(selectionAdjustStepTextBox, "Informe um passo em pixels.");
                }

                return false;
            }

            if (!int.TryParse(selectionAdjustStepTextBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var step) || step <= 0)
            {
                if (showError)
                {
                    validationErrorProvider.SetError(selectionAdjustStepTextBox, "Use um numero inteiro positivo.");
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
            graphics.Clear(Color.White);
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
                    "Selecione uma variante com textura valida antes de exportar o DDS.",
                    "Exportar DDS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var suggestedName = string.IsNullOrWhiteSpace(_currentFilePath)
                ? $"texture_{graph.TextureID}.dds"
                : $"{Path.GetFileNameWithoutExtension(_currentFilePath)}_{graph.TextureID}.dds";

            using var saveDialog = new SaveFileDialog
            {
                Filter = "DDS Files (*.dds)|*.dds|All Files (*.*)|*.*",
                Title = "Exportar textura selecionada para DDS",
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
                    $"Falha ao exportar o DDS.{Environment.NewLine}{error}",
                    "Exportar DDS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                statusToolStripStatusLabel.Text = "Falha ao exportar DDS da textura.";
                return;
            }

            statusToolStripStatusLabel.Text = $"DDS exportado: {Path.GetFileName(saveDialog.FileName)}";
            MessageBox.Show(
                this,
                "Textura exportada com sucesso para DDS.",
                "Exportar DDS",
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
                    "Selecione uma variante com textura valida antes de reimportar o DDS.",
                    "Reimportar DDS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using var openDialog = new OpenFileDialog
            {
                Filter = "DDS Files (*.dds)|*.dds|All Files (*.*)|*.*",
                Title = "Selecionar DDS editado para reimportar"
            };

            if (openDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var confirmation = MessageBox.Show(
                this,
                $"O DDS sera reimportado na textura {graph.TextureID} ao lado do arquivo MCD atual.{Environment.NewLine}Isso sobrescreve o conteudo correspondente no WTP. Deseja continuar?",
                "Reimportar DDS",
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
                    $"Falha ao reimportar o DDS editado.{Environment.NewLine}{error}",
                    "Reimportar DDS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                statusToolStripStatusLabel.Text = "Falha ao reimportar DDS da textura.";
                return;
            }

            InvalidateEditorPreviewAtlas(graph.TextureID);
            UpdateRemapTexturePreview(remapSourceComboBox.SelectedItem as CharRemapOption);
            UpdateEditorPreview();
            statusToolStripStatusLabel.Text = $"DDS reimportado para a textura {graph.TextureID}.";
            MessageBox.Show(
                this,
                $"DDS reimportado com sucesso para a textura {graph.TextureID}.{Environment.NewLine}Variante atual: ID {entry.Id}.",
                "Reimportar DDS",
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
                ? "nenhum bit ativo"
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
                        count += str.Letters.Count(letter => letter.Code == charId);
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

            statusToolStripStatusLabel.Text = $"Resultado {matchIndex + 1} de {_searchMatches.Count}.";
            UpdateSearchHelper(
                $"Resultado {matchIndex + 1} de {_searchMatches.Count}. Use Proximo para navegar.",
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

        private sealed record CharRemapOption(string Value, int CharId, int CharCode, int VariantIndex, int VariantsCount)
        {
            public override string ToString()
            {
                var codeLabel = CharCode > 0
                    ? $"U+{CharCode:X4}"
                    : "sem codigo";

                return $"{Value} | variante {VariantIndex}/{VariantsCount} | ID {CharId} | {codeLabel}";
            }
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
