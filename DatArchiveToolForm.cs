using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using StarFoxZeroLocalizationTool.Services;

namespace StarFoxZeroLocalizationTool;

public sealed partial class DatArchiveToolForm : Form
{
    private readonly DatArchiveService _service = new();

    public DatArchiveToolForm()
    {
        InitializeComponent();
        
        Log("StarFox Zero Tools GUI initialized.");
        Log("Ready to process files.");
    }

    private void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        txtLog.AppendText($"[{timestamp}] {message}\r\n");
        txtLog.SelectionStart = txtLog.TextLength;
        txtLog.ScrollToCaret();
    }

    private void BtnExtract_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Filter = "DAT files (*.dat, *.dtt, *.eff, *.evn)|*.dat;*.dtt;*.eff;*.evn|All files (*.*)|*.*",
            Title = "Select DAT/DTT Archive to Extract"
        };

        var projectRoot = FindProjectRoot();
        var datFolder = Path.Combine(projectRoot, "Arquivo Dat");
        if (Directory.Exists(datFolder))
        {
            ofd.InitialDirectory = datFolder;
        }

        if (ofd.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var filePath = ofd.FileName;
        Log($"Selected DAT File: {filePath}");

        try
        {
            var dir = Path.GetDirectoryName(filePath) ?? string.Empty;
            var baseName = Path.GetFileNameWithoutExtension(filePath);
            var ext = Path.GetExtension(filePath);
            var suffix = ext.Replace('.', '_');
            var outDir = Path.Combine(dir, baseName + suffix);

            Log($"Extracting to: {outDir}");
            var manifest = _service.Extract(filePath, outDir);

            Log($"SUCCESS: Extracted {manifest.Entries.Count} file(s) successfully.");
        }
        catch (Exception ex)
        {
            Log($"ERROR: {ex.Message}");
            MessageBox.Show(this, $"Erro ao extrair arquivo: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnRepack_Click(object? sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog
        {
            Description = "Select an extracted directory (e.g. *_dat) to repackage"
        };

        var projectRoot = FindProjectRoot();
        var datFolder = Path.Combine(projectRoot, "Arquivo Dat");
        if (Directory.Exists(datFolder))
        {
            fbd.SelectedPath = datFolder;
        }

        if (fbd.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var folderPath = fbd.SelectedPath;
        Log($"Selected DAT Folder: {folderPath}");

        var metaDir = Path.Combine(folderPath, ".metadata");
        if (!Directory.Exists(metaDir))
        {
            Log("ERROR: Folder is missing .metadata subfolder. This folder was not extracted by this tool.");
            MessageBox.Show(this, "Esta pasta não contém a subpasta .metadata necessária.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var extension = ".dat";
        var manifestPath = Path.Combine(metaDir, "manifest.json");
        if (File.Exists(manifestPath))
        {
            try
            {
                var manifestText = File.ReadAllText(manifestPath);
                using var doc = JsonDocument.Parse(manifestText);
                if (doc.RootElement.TryGetProperty("SourcePath", out var sourcePathProp))
                {
                    var sourcePath = sourcePathProp.GetString();
                    if (!string.IsNullOrWhiteSpace(sourcePath))
                    {
                        extension = Path.GetExtension(sourcePath);
                    }
                }
            }
            catch
            {
                // Fallback
            }
        }

        var folderName = Path.GetFileName(folderPath);
        var suffix = extension.Replace('.', '_');
        var suggestedName = folderName;
        if (folderName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
        {
            suggestedName = folderName.Substring(0, folderName.Length - suffix.Length);
        }
        suggestedName += extension;

        using var sfd = new SaveFileDialog
        {
            Filter = $"DAT files (*{extension})|*{extension}|All files (*.*)|*.*",
            Title = "Save Repackaged DAT/DTT",
            FileName = suggestedName
        };

        if (Directory.Exists(datFolder))
        {
            sfd.InitialDirectory = datFolder;
        }

        if (sfd.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var outputPath = sfd.FileName;
        Log($"Repackaging to: {outputPath}");

        try
        {
            _service.Repack(folderPath, outputPath);
            Log("SUCCESS: Repackaged successfully.");
        }
        catch (Exception ex)
        {
            Log($"ERROR: {ex.Message}");
            MessageBox.Show(this, $"Erro ao reempacotar pasta: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string FindProjectRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null)
        {
            var candidate = Path.Combine(current.FullName, "McdEditor");
            if (Directory.Exists(candidate))
            {
                return current.FullName;
            }
            current = current.Parent;
        }
        return AppContext.BaseDirectory;
    }
}
