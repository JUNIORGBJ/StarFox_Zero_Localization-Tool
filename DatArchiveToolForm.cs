using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using StarFoxZeroLocalizationTool.Localization;
using StarFoxZeroLocalizationTool.Services;

namespace StarFoxZeroLocalizationTool;

public sealed partial class DatArchiveToolForm : Form
{
    private readonly DatArchiveService _service = new();

    public DatArchiveToolForm()
    {
        InitializeComponent();
        LocalizationService.ApplyFormTexts(this);
        
        Log(Loc.Get("DatArchiveToolForm.Log.Initialized"));
        Log(Loc.Get("DatArchiveToolForm.Log.Ready"));
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
            Filter = Loc.Get("DatArchiveToolForm.Dialog.DatFilter"),
            Title = Loc.Get("DatArchiveToolForm.Dialog.SelectDatTitle")
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
        Log(Loc.Format("DatArchiveToolForm.Log.SelectedDatFile", filePath));

        try
        {
            var dir = Path.GetDirectoryName(filePath) ?? string.Empty;
            var baseName = Path.GetFileNameWithoutExtension(filePath);
            var ext = Path.GetExtension(filePath);
            var suffix = ext.Replace('.', '_');
            var outDir = Path.Combine(dir, baseName + suffix);

            Log(Loc.Format("DatArchiveToolForm.Log.ExtractingTo", outDir));
            var manifest = _service.Extract(filePath, outDir);

            Log(Loc.Format("DatArchiveToolForm.Log.ExtractSuccess", manifest.Entries.Count));
        }
        catch (Exception ex)
        {
            Log(Loc.Format("DatArchiveToolForm.Log.Error", ex.Message));
            MessageBox.Show(this, Loc.Format("DatArchiveToolForm.Message.ExtractError", ex.Message), Loc.Get("Common.ErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnRepack_Click(object? sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog
        {
            Description = Loc.Get("DatArchiveToolForm.Dialog.RepackFolderDescription")
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
        Log(Loc.Format("DatArchiveToolForm.Log.SelectedDatFolder", folderPath));

        var metaDir = Path.Combine(folderPath, ".metadata");
        if (!Directory.Exists(metaDir))
        {
            Log(Loc.Get("DatArchiveToolForm.Log.MetadataMissing"));
            MessageBox.Show(this, Loc.Get("DatArchiveToolForm.Message.MetadataMissing"), Loc.Get("Common.ErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Filter = string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                "DAT files (*{0})|*{0}|{1}",
                extension,
                Loc.Get("Common.Filter.AllFiles")),
            Title = Loc.Get("DatArchiveToolForm.Dialog.SaveRepackedTitle"),
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
        Log(Loc.Format("DatArchiveToolForm.Log.RepackingTo", outputPath));

        try
        {
            _service.Repack(folderPath, outputPath);
            Log(Loc.Get("DatArchiveToolForm.Log.RepackSuccess"));
        }
        catch (Exception ex)
        {
            Log(Loc.Format("DatArchiveToolForm.Log.Error", ex.Message));
            MessageBox.Show(this, Loc.Format("DatArchiveToolForm.Message.RepackError", ex.Message), Loc.Get("Common.ErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
