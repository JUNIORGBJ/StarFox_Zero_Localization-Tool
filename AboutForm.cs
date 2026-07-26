using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using StarFoxZeroLocalizationTool.Localization;

namespace StarFoxZeroLocalizationTool
{
    public partial class AboutForm : Form
    {
        private const string ProjectUrl = "https://github.com/JUNIORGBJ/StarFox_Zero_Localization-Tool";

        public AboutForm()
        {
            InitializeComponent();
            LocalizationService.ApplyFormTexts(this, dynamicControlNames: new HashSet<string>(StringComparer.Ordinal)
            {
                nameof(versionValueLabel),
                nameof(projectLinkLabel)
            });

            versionValueLabel.Text = Loc.Format("AboutForm.Version", GetDisplayVersion());
            projectLinkLabel.Text = ProjectUrl;
            projectLinkLabel.Links.Clear();
            projectLinkLabel.Links.Add(0, ProjectUrl.Length, ProjectUrl);
        }

        private static string GetDisplayVersion()
        {
            var informationalVersion = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            if (!string.IsNullOrWhiteSpace(informationalVersion))
            {
                var plusIndex = informationalVersion.IndexOf('+');
                return plusIndex >= 0
                    ? informationalVersion[..plusIndex]
                    : informationalVersion;
            }

            return Application.ProductVersion;
        }

        private void ProjectLinkLabel_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var targetUrl = e.Link?.LinkData?.ToString() ?? ProjectUrl;
                Process.Start(new ProcessStartInfo
                {
                    FileName = targetUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    Loc.Format("AboutForm.Message.OpenProjectLinkFailed", Environment.NewLine, ex.Message),
                    Loc.Get("AboutForm.Dialog.Title"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void OkButton_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
