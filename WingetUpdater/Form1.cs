using System.Diagnostics;

namespace WingetUpdater
{
    public partial class Form1 : Form
    {
        private readonly WingetManager _wingetManager = new WingetManager();
        public Form1()
        {
            InitializeComponent();
        }
        public class UpdatableApp
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string Version { get; set; }
            public string AvailableVersion { get; set; }
            public string Source { get; set; }
            public bool IsSelected { get; set; }
        }
        public class WingetManager
        {
            // A list to hold the updatable apps.
            public List<UpdatableApp> UpdatableApps { get; } = new List<UpdatableApp>();

            // This method checks if Winget is installed.
            public bool IsWingetInstalled()
            {
                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "winget.exe",
                            Arguments = "--version",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };
                    process.Start();
                    process.WaitForExit();
                    return process.ExitCode == 0;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    return false;
                }
            }

            // This method runs the "winget upgrade --list" command and parses the output.
            public async Task GetUpdatableApps()
            {
                UpdatableApps.Clear();

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "winget.exe",
                        Arguments = "upgrade",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"Error running winget: {error}");
                    return;
                }

                var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var headerLine = lines.FirstOrDefault(l => l.Contains("Name") && l.Contains("Id") && l.Contains("Version") && l.Contains("Available"));

                if (headerLine != null)
                {
                    int nameIndex = headerLine.IndexOf("Name");
                    int idIndex = headerLine.IndexOf("Id");
                    int versionIndex = headerLine.IndexOf("Version");
                    int availableIndex = headerLine.IndexOf("Available");
                    int sourceIndex = headerLine.IndexOf("Source");

                    foreach (var line in lines.SkipWhile(l => !l.Equals(headerLine)).Skip(2))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        try
                        {
                            string name = line.Substring(nameIndex, idIndex - nameIndex).Trim();
                            string id = line.Substring(idIndex, versionIndex - idIndex).Trim();
                            string version = line.Substring(versionIndex, availableIndex - versionIndex).Trim();
                            string available = line.Substring(availableIndex, sourceIndex - availableIndex).Trim();
                            string source = line.Substring(sourceIndex).Trim();

                            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(available))
                            {
                                UpdatableApps.Add(new UpdatableApp
                                {
                                    Name = name,
                                    Id = id,
                                    Version = version,
                                    AvailableVersion = available,
                                    Source = source,
                                    IsSelected = false
                                });
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }

            // This method handles the update process for a single app.
            public async Task UpdateApp(UpdatableApp app)
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "winget.exe",
                        Arguments = $"upgrade \"{app.Id}\" --accept-package-agreements --accept-source-agreements",
                        UseShellExecute = true,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"Failed to update {app.Name}: {error}");
                }
                else
                {
                    Console.WriteLine($"Successfully updated {app.Name}. Output: {output}");
                    // The UI will handle removing the item after the update.
                }
            }
        }

        private async Task LoadUpdates()
        {
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            stsbar.Text = "Checking for updates...";
            //stsbar.Text = "Checking for updates...";
            lvApps.Items.Clear();

            if (!_wingetManager.IsWingetInstalled())
            {
                stsbar.Text = "Winget is not installed. Please install it to continue.";
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = 100;
                return;
            }

            await _wingetManager.GetUpdatableApps();

            if (_wingetManager.UpdatableApps.Count == 0)
            {
                stsbar.Text = "No updates found.";
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = 100;
            }
            else
            {
                stsbar.Text = $"{_wingetManager.UpdatableApps.Count} updates found.";
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = 100;
                foreach (var app in _wingetManager.UpdatableApps)
                {
                    var item = new ListViewItem(app.Name);
                    item.SubItems.Add(app.Id);
                    item.SubItems.Add(app.Version);
                    item.SubItems.Add(app.AvailableVersion);
                    item.SubItems.Add(app.Source);
                    item.Tag = app; // Store the object for later use
                    lvApps.Items.Add(item);
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var selectedItems = lvApps.CheckedItems.Cast<ListViewItem>().ToList();

            if (selectedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one application to update.", "No Apps Selected");
                return;
            }

            btnUpdate.Enabled = false;
            btnCancel.Enabled = false;

            foreach (var item in selectedItems)
            {
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = lvApps.Items.Count;
                var app = item.Tag as UpdatableApp;
                if (app != null)
                {
                    stsbar.Text = $"Updating {app.Name}...";
                    lvApps.Items.Remove(item); 
                    _wingetManager.UpdateApp(app);
                    toolStripProgressBar1.Value += toolStripProgressBar1.Maximum;
                }
            }

            stsbar.Text = "Update process complete.";
            btnUpdate.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvApps.CheckedItems)
            {
                item.Checked = false;
            }
            stsbar.Text = "Cancelled. Select apps to update.";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _ = LoadUpdates();
        }
    }
}
