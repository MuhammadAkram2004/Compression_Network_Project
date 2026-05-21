using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompressionClient
{
    public partial class MainForm : Form
    {
        private byte[]  _compressedData   = null;
        private string  _originalFileName = "";
        private bool    _connected        = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog { Filter = "All Files|*.*" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text  = dlg.FileName;
                _originalFileName = Path.GetFileName(dlg.FileName);
                _compressedData   = null;
                btnSaveAs.Enabled = false;
                UpdateResult($"File selected: {_originalFileName}", Color.Gray);
            }
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            string host = txtServer.Text.Trim();
            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Invalid port.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using var tcp = new TcpClient();
                tcp.Connect(host, port);
                _connected = true;
                btnCompress.Enabled  = true;
                btnConnect.BackColor = Color.Green;
                btnConnect.Text      = "Connected";
                UpdateResult($"Connected to {host}:{port}", Color.Green);
            }
            catch (Exception ex)
            {
                _connected           = false;
                btnConnect.BackColor = Color.Red;
                btnConnect.Text      = "Failed";
                UpdateResult($"Connection failed: {ex.Message}", Color.Red);
            }
        }

        private async void BtnCompress_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;
            string host     = txtServer.Text.Trim();

            if (!File.Exists(filePath))
            { MessageBox.Show("Select a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            { MessageBox.Show("Invalid port.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            btnCompress.Enabled = false;
            UpdateResult("Sending file...", Color.Orange);

            try
            {
                byte[] fileData = await Task.Run(() => File.ReadAllBytes(filePath));

                using TcpClient tcp = new TcpClient();
                await tcp.ConnectAsync(host, port);

                using NetworkStream ns = tcp.GetStream();

                await ns.WriteAsync(BitConverter.GetBytes((long)fileData.Length), 0, 8);
                await ns.WriteAsync(fileData, 0, fileData.Length);
                await ns.FlushAsync();

                byte[] sizeBuf = new byte[8];
                await ReadExactAsync(ns, sizeBuf, 0, 8);
                long compSize = BitConverter.ToInt64(sizeBuf, 0);

                _compressedData = new byte[compSize];
                await ReadExactAsync(ns, _compressedData, 0, (int)compSize);

                double ratio = 100.0 * compSize / fileData.Length;
                UpdateResult(
                    $"Done!  {FormatSize(fileData.Length)}  →  {FormatSize(compSize)}  ({ratio:F1}%)",
                    Color.Green);

                btnSaveAs.Enabled = true;
            }
            catch (Exception ex)
            {
                UpdateResult($"Error: {ex.Message}", Color.Red);
            }
            finally
            {
                btnCompress.Enabled = true;
            }
        }

        private void BtnSaveAs_Click(object sender, EventArgs e)
        {
            if (_compressedData == null)
            { 
                MessageBox.Show("No compressed data to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            using var dlg = new SaveFileDialog
            {
                Filter   = "GZip Files|*.gz|All Files|*.*",
                FileName = _originalFileName + ".gz"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(dlg.FileName, _compressedData);
                MessageBox.Show("Saved successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Save Failed.", "Failes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateResult(string msg, Color color)
        {
            if (lblResult.InvokeRequired) { lblResult.Invoke(new Action(() => UpdateResult(msg, color))); return; }
            lblResult.Text      = msg;
            lblResult.ForeColor = color;
        }

        private string FormatSize(long bytes)
        {
            if (bytes < 1024)           return $"{bytes} B";
            if (bytes < 1024 * 1024)    return $"{bytes / 1024.0:F1} KB";
            return $"{bytes / (1024.0 * 1024):F2} MB";
        }

        private static async Task ReadExactAsync(NetworkStream stream, byte[] buffer, int offset, int count)
        {
            int received = 0;
            while (received < count)
            {
                int n = await stream.ReadAsync(buffer, offset + received, count - received);
                if (n == 0) throw new EndOfStreamException("Connection closed by server.");
                received += n;
            }
        }
    }
}
