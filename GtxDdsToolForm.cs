using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StarFoxZeroLocalizationTool.Services;

namespace StarFoxZeroLocalizationTool
{
    public partial class GtxDdsToolForm : Form
    {
        private static readonly Color InfoColor = Color.FromArgb(71, 85, 105);
        private static readonly Color SuccessColor = Color.FromArgb(22, 163, 74);
        private static readonly Color WarningColor = Color.FromArgb(185, 28, 28);

        public GtxDdsToolForm()
        {
            InitializeComponent();
            ConfigureForm();
        }

        private void ConfigureForm()
        {
            modeComboBox.Items.AddRange(new object[]
            {
                new ModeOption("GTX -> DDS", ConversionMode.GtxToDds),
                new ModeOption("DDS -> GTX", ConversionMode.DdsToGtx)
            });

            profileComboBox.Items.AddRange(new object[]
            {
                new ProfileOption("Auto (usar GTX base)", GtxColorProfile.AutoFromOriginal),
                new ProfileOption("UNORM", GtxColorProfile.Unorm),
                new ProfileOption("SRGB", GtxColorProfile.Srgb)
            });

            modeComboBox.SelectedIndex = 0;
            profileComboBox.SelectedIndex = 0;
            TryAutoDetectToolPath(showFeedback: false);
            UpdateModeUi();
            ResetAnalysisLabels();
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

        private void DetectToolButton_Click(object? sender, EventArgs e)
        {
            TryAutoDetectToolPath(showFeedback: true);
        }

        private void ModeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateModeUi();
            SuggestDefaultOutputPath(force: false);
        }

        private void BrowseOriginalGtxButton_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "GTX Files (*.gtx)|*.gtx|Todos os arquivos (*.*)|*.*",
                Title = "Selecionar GTX base"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                originalGtxTextBox.Text = ofd.FileName;
            }
        }

        private void AnalyzeOriginalButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateToolPath() || !ValidateOriginalGtxPath(requireForAuto: false))
            {
                return;
            }

            try
            {
                UseWaitCursor = true;
                ToggleActionButtons(false);

                var analysis = GtxExtractService.AnalyzeGtx(toolPathTextBox.Text.Trim(), originalGtxTextBox.Text.Trim());
                ApplyAnalysisResult(analysis);
                executionLogTextBox.Text = analysis.ToolOutput;

                if (!analysis.Success)
                {
                    MessageBox.Show(
                        this,
                        "Nao foi possivel analisar o GTX informado. Verifique o arquivo e a saida do log.",
                        "Analise GTX",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                advisoryLabel.ForeColor = WarningColor;
                advisoryLabel.Text = ex.Message;
                executionLogTextBox.Text = ex.ToString();
                MessageBox.Show(this, ex.Message, "Erro na analise", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleActionButtons(true);
                UseWaitCursor = false;
            }
        }

        private void BrowseInputFileButton_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = SelectedMode == ConversionMode.GtxToDds
                    ? "GTX Files (*.gtx)|*.gtx|Todos os arquivos (*.*)|*.*"
                    : "DDS Files (*.dds)|*.dds|Todos os arquivos (*.*)|*.*",
                Title = SelectedMode == ConversionMode.GtxToDds
                    ? "Selecionar arquivo GTX"
                    : "Selecionar arquivo DDS"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                inputFileTextBox.Text = ofd.FileName;
                SuggestDefaultOutputPath(force: true);
            }
        }

        private void BrowseOutputFileButton_Click(object? sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = SelectedMode == ConversionMode.GtxToDds
                    ? "DDS Files (*.dds)|*.dds|Todos os arquivos (*.*)|*.*"
                    : "GTX Files (*.gtx)|*.gtx|Todos os arquivos (*.*)|*.*",
                Title = SelectedMode == ConversionMode.GtxToDds
                    ? "Salvar arquivo DDS"
                    : "Salvar arquivo GTX",
                FileName = Path.GetFileName(SuggestedOutputPath)
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                outputFileTextBox.Text = sfd.FileName;
            }
        }

        private void FillDefaultOutputButton_Click(object? sender, EventArgs e)
        {
            SuggestDefaultOutputPath(force: true);
        }

        private void InputFileTextBox_TextChanged(object? sender, EventArgs e)
        {
            SuggestDefaultOutputPath(force: false);
        }

        private void ConvertButton_Click(object? sender, EventArgs e)
        {
            if (!ValidateToolPath() || !ValidateInputAndOutput())
            {
                return;
            }

            try
            {
                UseWaitCursor = true;
                ToggleActionButtons(false);

                GtxCommandResult result;
                if (SelectedMode == ConversionMode.GtxToDds)
                {
                    result = GtxExtractService.ConvertGtxToDds(
                        toolPathTextBox.Text.Trim(),
                        inputFileTextBox.Text.Trim(),
                        outputFileTextBox.Text.Trim());
                }
                else
                {
                    var profile = SelectedProfile;
                    if (!ValidateOriginalGtxPath(requireForAuto: true))
                    {
                        return;
                    }

                    result = GtxExtractService.ConvertDdsToGtxPreservingOriginalContainer(
                        toolPathTextBox.Text.Trim(),
                        inputFileTextBox.Text.Trim(),
                        outputFileTextBox.Text.Trim(),
                        profile,
                        originalGtxTextBox.Text.Trim());
                }

                executionLogTextBox.Text = result.Output;
                if (result.Success)
                {
                    advisoryLabel.ForeColor = SuccessColor;
                    advisoryLabel.Text = $"Conversao concluida com sucesso. Arquivo gerado: {Path.GetFileName(outputFileTextBox.Text)}";
                    MessageBox.Show(
                        this,
                        "Conversao concluida com sucesso.",
                        "GTX / DDS",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    advisoryLabel.ForeColor = WarningColor;
                    advisoryLabel.Text = "O gtx_extract retornou erro. Consulte o log completo abaixo.";
                    MessageBox.Show(
                        this,
                        "O gtx_extract retornou erro. Consulte o log completo abaixo.",
                        "GTX / DDS",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                advisoryLabel.ForeColor = WarningColor;
                advisoryLabel.Text = ex.Message;
                executionLogTextBox.Text = ex.ToString();
                MessageBox.Show(this, ex.Message, "Erro na conversao", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleActionButtons(true);
                UseWaitCursor = false;
            }
        }

        private void TryAutoDetectToolPath(bool showFeedback)
        {
            var detectedPath = GtxExtractService.TryFindExecutable();
            if (!string.IsNullOrWhiteSpace(detectedPath))
            {
                toolPathTextBox.Text = detectedPath;
                if (showFeedback)
                {
                    MessageBox.Show(
                        this,
                        $"Backend do gtx_extract localizado em:\n{detectedPath}",
                        "Ferramenta localizada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                return;
            }

            if (showFeedback)
            {
                MessageBox.Show(
                    this,
                    "Nenhuma ferramenta externa foi localizada automaticamente. Para GTX R8_G8_UNORM do jogo, o app agora usa o backend nativo e este campo pode ficar vazio.",
                    "Localizacao automatica",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void UpdateModeUi()
        {
            var isDdsToGtx = SelectedMode == ConversionMode.DdsToGtx;
            profileComboBox.Enabled = isDdsToGtx;
            profileLabel.Enabled = isDdsToGtx;
            originalGtxTextBox.Enabled = true;
            browseOriginalGtxButton.Enabled = true;
            analyzeOriginalButton.Enabled = true;
            inputFileLabel.Text = isDdsToGtx ? "DDS de entrada" : "GTX de entrada";
            outputFileLabel.Text = isDdsToGtx ? "GTX de saida" : "DDS de saida";
            conversionHintLabel.Text = isDdsToGtx
                ? "Ao recriar um GTX a partir de DDS, use sempre um GTX base. A conversao herda tileMode, swizzle e o perfil SRGB/UNORM, e agora preserva tambem o container bruto do GTX original."
                : "Na extracao GTX -> DDS, o arquivo original pode ser analisado para consulta do formato e dos avisos.";
        }

        private void SuggestDefaultOutputPath(bool force)
        {
            if (string.IsNullOrWhiteSpace(inputFileTextBox.Text))
            {
                return;
            }

            if (!File.Exists(inputFileTextBox.Text.Trim()))
            {
                return;
            }

            if (!force && !string.IsNullOrWhiteSpace(outputFileTextBox.Text))
            {
                return;
            }

            outputFileTextBox.Text = SuggestedOutputPath;
        }

        private string SuggestedOutputPath
        {
            get
            {
                var inputPath = inputFileTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(inputPath))
                {
                    return SelectedMode == ConversionMode.GtxToDds ? "output.dds" : "output.gtx";
                }

                var targetExtension = SelectedMode == ConversionMode.GtxToDds ? ".dds" : ".gtx";
                return Path.ChangeExtension(inputPath, targetExtension) ?? $"output{targetExtension}";
            }
        }

        private ConversionMode SelectedMode =>
            modeComboBox.SelectedItem is ModeOption option ? option.Value : ConversionMode.GtxToDds;

        private GtxColorProfile SelectedProfile =>
            profileComboBox.SelectedItem is ProfileOption option ? option.Value : GtxColorProfile.AutoFromOriginal;

        private bool ValidateToolPath()
        {
            var toolPath = toolPathTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(toolPath) || GtxExtractService.IsValidToolPath(toolPath))
            {
                return true;
            }

            MessageBox.Show(
                this,
                "Se informado, o caminho da ferramenta externa precisa apontar para um backend valido do gtx_extract (.exe ou .py).",
                "Ferramenta nao localizada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        private bool ValidateOriginalGtxPath(bool requireForAuto)
        {
            var originalPath = originalGtxTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(originalPath))
            {
                if (!requireForAuto)
                {
                    return false;
                }

                MessageBox.Show(
                    this,
                    "Informe o GTX base para usar o perfil Auto.",
                    "GTX base obrigatorio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (File.Exists(originalPath) && string.Equals(Path.GetExtension(originalPath), ".gtx", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            MessageBox.Show(
                this,
                "O GTX base informado nao foi encontrado ou nao possui extensao .gtx.",
                "GTX base invalido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        private bool ValidateInputAndOutput()
        {
            var inputPath = inputFileTextBox.Text.Trim();
            var outputPath = outputFileTextBox.Text.Trim();

            var expectedInputExtension = SelectedMode == ConversionMode.GtxToDds ? ".gtx" : ".dds";
            var expectedOutputExtension = SelectedMode == ConversionMode.GtxToDds ? ".dds" : ".gtx";

            if (!File.Exists(inputPath) || !string.Equals(Path.GetExtension(inputPath), expectedInputExtension, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    this,
                    $"Selecione um arquivo de entrada valido com extensao {expectedInputExtension}.",
                    "Entrada invalida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(outputPath) || !string.Equals(Path.GetExtension(outputPath), expectedOutputExtension, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    this,
                    $"Informe um arquivo de saida com extensao {expectedOutputExtension}.",
                    "Saida invalida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (SelectedMode == ConversionMode.DdsToGtx && !ValidateOriginalGtxPath(requireForAuto: true))
            {
                return false;
            }

            return true;
        }

        private void ApplyAnalysisResult(GtxAnalysisResult analysis)
        {
            detectedFormatValueLabel.Text = string.IsNullOrWhiteSpace(analysis.Format) ? "-" : analysis.Format;
            detectedTileModeValueLabel.Text = analysis.TileMode?.ToString() ?? "-";
            detectedSwizzleValueLabel.Text = string.IsNullOrWhiteSpace(analysis.Swizzle) ? "-" : analysis.Swizzle;
            detectedComponentValueLabel.Text = string.IsNullOrWhiteSpace(analysis.ComponentSelector) ? "-" : analysis.ComponentSelector;

            advisoryLabel.ForeColor = analysis.HasLosslessRoundtripRisk
                ? WarningColor
                : analysis.Success
                    ? SuccessColor
                    : WarningColor;
            advisoryLabel.Text = analysis.AdvisoryMessage;
        }

        private void ResetAnalysisLabels()
        {
            detectedFormatValueLabel.Text = "-";
            detectedTileModeValueLabel.Text = "-";
            detectedSwizzleValueLabel.Text = "-";
            detectedComponentValueLabel.Text = "-";
            advisoryLabel.ForeColor = InfoColor;
            advisoryLabel.Text = "Selecione um GTX base e clique em 'Analisar GTX base'.";
        }

        private void ToggleActionButtons(bool enabled)
        {
            browseToolButton.Enabled = enabled;
            detectToolButton.Enabled = enabled;
            browseOriginalGtxButton.Enabled = enabled;
            analyzeOriginalButton.Enabled = enabled;
            browseInputFileButton.Enabled = enabled;
            browseOutputFileButton.Enabled = enabled;
            fillDefaultOutputButton.Enabled = enabled;
            convertButton.Enabled = enabled;
        }

        private enum ConversionMode
        {
            GtxToDds,
            DdsToGtx
        }

        private sealed record ModeOption(string Label, ConversionMode Value)
        {
            public override string ToString() => Label;
        }

        private sealed record ProfileOption(string Label, GtxColorProfile Value)
        {
            public override string ToString() => Label;
        }
    }
}
