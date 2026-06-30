using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StarFoxZeroLocalizationTool.Services;

namespace StarFoxZeroLocalizationTool
{
    public partial class GtxRawTextureEditorForm : Form
    {
        private readonly string _tempDirectory = Path.Combine(Path.GetTempPath(), "StarFoxZeroLocalizationTool", "RawGtxEditor", Guid.NewGuid().ToString("N"));
        private RawR8G8DdsImage? _workingImage;
        private string? _workingDdsPath;
        private string? _currentGtxPath;
        private Bitmap? _compositeBitmap;
        private Bitmap? _channelRBitmap;
        private Bitmap? _channelGBitmap;

        public GtxRawTextureEditorForm()
        {
            InitializeComponent();
            Directory.CreateDirectory(_tempDirectory);
            TryAutoDetectToolPath(showFeedback: false);
            UpdateUiState(false);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            DisposePreviewBitmaps();

            try
            {
                if (Directory.Exists(_tempDirectory))
                {
                    Directory.Delete(_tempDirectory, recursive: true);
                }
            }
            catch
            {
                // Ignore temp cleanup failures.
            }

            base.OnFormClosed(e);
        }

        private void DetectToolButton_Click(object? sender, EventArgs e)
        {
            TryAutoDetectToolPath(showFeedback: true);
        }

        private void BrowseToolButton_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "gtx_extract (*.exe;*.py)|gtx_extract.exe;gtx_extract.py|Executaveis (*.exe)|*.exe|Scripts Python (*.py)|*.py|Todos os arquivos (*.*)|*.*",
                Title = "Selecionar gtx_extract (.exe ou .py)"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                toolPathTextBox.Text = ofd.FileName;
            }
        }

        private void BrowseGtxButton_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "GTX Files (*.gtx)|*.gtx|Todos os arquivos (*.*)|*.*",
                Title = "Selecionar GTX base"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                gtxPathTextBox.Text = ofd.FileName;
                SuggestDefaultOutputPath();
            }
        }

        private void OpenGtxButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateToolPath() || !ValidateGtxPath())
            {
                return;
            }

            try
            {
                ToggleBusyState(true);
                AppendStatus("Abrindo GTX base...");

                var gtxPath = gtxPathTextBox.Text.Trim();
                var analysis = GtxExtractService.AnalyzeGtx(toolPathTextBox.Text.Trim(), gtxPath);
                if (!analysis.Success)
                {
                    throw new InvalidOperationException("Nao foi possivel analisar o GTX informado.");
                }

                if (!analysis.Format.Contains("GX2_SURFACE_FORMAT_TC_R8_G8_UNORM", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        $"Formato nao suportado nesta ferramenta: {analysis.Format}. Esta tela e focada em GTX R8_G8_UNORM de fonte/UI.");
                }

                _workingDdsPath = Path.Combine(_tempDirectory, "working.dds");
                var extractResult = GtxExtractService.ConvertGtxToDds(toolPathTextBox.Text.Trim(), gtxPath, _workingDdsPath);
                if (!extractResult.Success)
                {
                    throw new InvalidOperationException("Falha ao extrair a imagem interna do GTX.\r\n\r\n" + extractResult.Output);
                }

                _workingImage = RawR8G8DdsImage.Load(_workingDdsPath);
                _currentGtxPath = gtxPath;
                SuggestDefaultOutputPath();
                RefreshPreviews();
                UpdateUiState(true);

                AppendStatus($"GTX carregado com sucesso: {Path.GetFileName(gtxPath)}");
                AppendStatus($"Dimensoes: {_workingImage.Width}x{_workingImage.Height}");
                AppendStatus($"Formato: {analysis.Format}");
                AppendStatus($"TileMode: {analysis.TileMode} | Swizzle: {analysis.Swizzle}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Erro ao abrir GTX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendStatus("Erro ao abrir GTX: " + ex.Message);
            }
            finally
            {
                ToggleBusyState(false);
            }
        }

        private void BrowseOutputButton_Click(object? sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "GTX Files (*.gtx)|*.gtx|Todos os arquivos (*.*)|*.*",
                Title = "Salvar GTX editado",
                FileName = Path.GetFileName(outputPathTextBox.Text)
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                outputPathTextBox.Text = sfd.FileName;
            }
        }

        private void SaveGtxButton_Click(object? sender, EventArgs e)
        {
            if (_workingImage == null || _workingDdsPath == null || _currentGtxPath == null)
            {
                return;
            }

            if (!ValidateToolPath() || string.IsNullOrWhiteSpace(outputPathTextBox.Text))
            {
                MessageBox.Show(this, "Informe um arquivo GTX de saida valido.", "Saida invalida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ToggleBusyState(true);
                var modifiedDdsPath = Path.Combine(_tempDirectory, "modified.dds");
                _workingImage.Save(modifiedDdsPath);

                AppendStatus("Salvando GTX editado preservando os parametros do GTX base...");
                var result = GtxExtractService.ConvertDdsToGtxPreservingOriginalContainer(
                    toolPathTextBox.Text.Trim(),
                    modifiedDdsPath,
                    outputPathTextBox.Text.Trim(),
                    GtxColorProfile.AutoFromOriginal,
                    _currentGtxPath);

                AppendStatus(result.Output);
                if (!result.Success)
                {
                    throw new InvalidOperationException("O gtx_extract retornou erro ao salvar o GTX.");
                }

                MessageBox.Show(this, "GTX salvo com sucesso.", "Editor Bruto de GTX", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppendStatus("GTX salvo com sucesso: " + outputPathTextBox.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Erro ao salvar GTX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendStatus("Erro ao salvar GTX: " + ex.Message);
            }
            finally
            {
                ToggleBusyState(false);
            }
        }

        private void ExportChannelRButton_Click(object? sender, EventArgs e)
        {
            ExportChannel(0, "Canal 1");
        }

        private void ImportChannelRButton_Click(object? sender, EventArgs e)
        {
            ImportChannel(0, "Canal 1");
        }

        private void ExportChannelGButton_Click(object? sender, EventArgs e)
        {
            ExportChannel(1, "Canal 2");
        }

        private void ImportChannelGButton_Click(object? sender, EventArgs e)
        {
            ImportChannel(1, "Canal 2");
        }

        private void ExportChannel(int channelIndex, string label)
        {
            if (_workingImage == null)
            {
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "PNG Files (*.png)|*.png",
                Title = $"Exportar {label}",
                FileName = BuildChannelFileName(label)
            };

            if (sfd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                _workingImage.ExportChannelToImage(sfd.FileName, channelIndex);
                AppendStatus($"{label} exportado para: {sfd.FileName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Erro ao exportar canal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ImportChannel(int channelIndex, string label)
        {
            if (_workingImage == null)
            {
                return;
            }

            using var ofd = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.bmp;*.jpg;*.jpeg)|*.png;*.bmp;*.jpg;*.jpeg|Todos os arquivos (*.*)|*.*",
                Title = $"Importar {label}"
            };

            if (ofd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                _workingImage.ImportChannelFromImage(ofd.FileName, channelIndex);
                RefreshPreviews();
                AppendStatus($"{label} importado de: {ofd.FileName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Erro ao importar canal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshPreviews()
        {
            if (_workingImage == null)
            {
                return;
            }

            DisposePreviewBitmaps();

            _compositeBitmap = _workingImage.CreateCompositeBitmap();
            _channelRBitmap = _workingImage.CreateChannelBitmap(0);
            _channelGBitmap = _workingImage.CreateChannelBitmap(1);

            compositePictureBox.Image = _compositeBitmap;
            channelRPictureBox.Image = _channelRBitmap;
            channelGPictureBox.Image = _channelGBitmap;
        }

        private void DisposePreviewBitmaps()
        {
            compositePictureBox.Image = null;
            channelRPictureBox.Image = null;
            channelGPictureBox.Image = null;

            _compositeBitmap?.Dispose();
            _channelRBitmap?.Dispose();
            _channelGBitmap?.Dispose();

            _compositeBitmap = null;
            _channelRBitmap = null;
            _channelGBitmap = null;
        }

        private void TryAutoDetectToolPath(bool showFeedback)
        {
            var detectedPath = GtxExtractService.TryFindExecutable();
            if (!string.IsNullOrWhiteSpace(detectedPath))
            {
                toolPathTextBox.Text = detectedPath;
                if (showFeedback)
                {
                    MessageBox.Show(this, $"Backend do gtx_extract localizado em:\r\n{detectedPath}", "Ferramenta localizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                return;
            }

            if (showFeedback)
            {
                MessageBox.Show(this, "Nenhuma ferramenta externa foi localizada automaticamente. Para GTX R8_G8_UNORM, esta tela agora usa o backend nativo e o campo pode ficar vazio.", "Ferramenta opcional", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SuggestDefaultOutputPath()
        {
            var gtxPath = gtxPathTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(gtxPath))
            {
                return;
            }

            outputPathTextBox.Text = Path.Combine(
                Path.GetDirectoryName(gtxPath) ?? string.Empty,
                Path.GetFileNameWithoutExtension(gtxPath) + "_editado.gtx");
        }

        private void UpdateUiState(bool hasImageLoaded)
        {
            exportChannelRButton.Enabled = hasImageLoaded;
            importChannelRButton.Enabled = hasImageLoaded;
            exportChannelGButton.Enabled = hasImageLoaded;
            importChannelGButton.Enabled = hasImageLoaded;
            saveGtxButton.Enabled = hasImageLoaded;
        }

        private void ToggleBusyState(bool isBusy)
        {
            UseWaitCursor = isBusy;
            detectToolButton.Enabled = !isBusy;
            browseToolButton.Enabled = !isBusy;
            browseGtxButton.Enabled = !isBusy;
            openGtxButton.Enabled = !isBusy;
            browseOutputButton.Enabled = !isBusy;
            exportChannelRButton.Enabled = !isBusy && _workingImage != null;
            importChannelRButton.Enabled = !isBusy && _workingImage != null;
            exportChannelGButton.Enabled = !isBusy && _workingImage != null;
            importChannelGButton.Enabled = !isBusy && _workingImage != null;
            saveGtxButton.Enabled = !isBusy && _workingImage != null;
        }

        private bool ValidateToolPath()
        {
            var toolPath = toolPathTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(toolPath) || GtxExtractService.IsValidToolPath(toolPath))
            {
                return true;
            }

            MessageBox.Show(this, "Se informado, o caminho da ferramenta externa precisa apontar para um backend valido do gtx_extract (.exe ou .py).", "Ferramenta nao localizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private bool ValidateGtxPath()
        {
            var path = gtxPathTextBox.Text.Trim();
            if (File.Exists(path) && string.Equals(Path.GetExtension(path), ".gtx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            MessageBox.Show(this, "Selecione um arquivo GTX valido.", "GTX invalido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private string BuildChannelFileName(string label)
        {
            var baseName = _currentGtxPath != null
                ? Path.GetFileNameWithoutExtension(_currentGtxPath)
                : "canal";

            var suffix = label.Replace(" ", "_").ToLowerInvariant();
            return $"{baseName}_{suffix}.png";
        }

        private void AppendStatus(string message)
        {
            if (string.IsNullOrWhiteSpace(statusTextBox.Text))
            {
                statusTextBox.Text = message;
                return;
            }

            statusTextBox.AppendText(Environment.NewLine + message);
        }
    }
}
