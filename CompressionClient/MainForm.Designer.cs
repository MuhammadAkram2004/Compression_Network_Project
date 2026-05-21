namespace CompressionClient
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtFilePath   = new System.Windows.Forms.TextBox();
            this.btnBrowse     = new System.Windows.Forms.Button();
            this.txtServer     = new System.Windows.Forms.TextBox();
            this.txtPort       = new System.Windows.Forms.TextBox();
            this.btnConnect    = new System.Windows.Forms.Button();
            this.btnCompress   = new System.Windows.Forms.Button();
            this.btnSaveAs     = new System.Windows.Forms.Button();
            this.lblFile       = new System.Windows.Forms.Label();
            this.lblServer     = new System.Windows.Forms.Label();
            this.lblPort       = new System.Windows.Forms.Label();
            this.lblResult     = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblFile
            this.lblFile.Text      = "File:";
            this.lblFile.Location  = new System.Drawing.Point(12, 15);
            this.lblFile.AutoSize  = true;

            // txtFilePath
            this.txtFilePath.Location  = new System.Drawing.Point(50, 12);
            this.txtFilePath.Width     = 340;
            this.txtFilePath.ReadOnly  = true;

            // btnBrowse
            this.btnBrowse.Text     = "Browse";
            this.btnBrowse.Location = new System.Drawing.Point(400, 11);
            this.btnBrowse.Width    = 80;
            this.btnBrowse.Click   += new System.EventHandler(this.BtnBrowse_Click);

            // lblServer
            this.lblServer.Text     = "Host:";
            this.lblServer.Location = new System.Drawing.Point(12, 55);
            this.lblServer.AutoSize = true;

            // txtServer
            this.txtServer.Text     = "127.0.0.1";
            this.txtServer.Location = new System.Drawing.Point(50, 52);
            this.txtServer.Width    = 150;

            // lblPort
            this.lblPort.Text     = "Port:";
            this.lblPort.Location = new System.Drawing.Point(215, 55);
            this.lblPort.AutoSize = true;

            // txtPort
            this.txtPort.Text     = "9000";
            this.txtPort.Location = new System.Drawing.Point(250, 52);
            this.txtPort.Width    = 70;

            // btnConnect
            this.btnConnect.Text     = "Connect";
            this.btnConnect.Location = new System.Drawing.Point(340, 51);
            this.btnConnect.Width    = 90;
            this.btnConnect.BackColor = System.Drawing.Color.Blue;
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Click    += new System.EventHandler(this.BtnConnect_Click);

            // btnCompress
            this.btnCompress.Text      = "Send & Compress";
            this.btnCompress.Location  = new System.Drawing.Point(12, 100);
            this.btnCompress.Width     = 150;
            this.btnCompress.Height    = 35;
            this.btnCompress.BackColor = System.Drawing.Color.Green;
            this.btnCompress.ForeColor = System.Drawing.Color.White;
            this.btnCompress.Click    += new System.EventHandler(this.BtnCompress_Click);

            // btnSaveAs
            this.btnSaveAs.Text      = "Save File";
            this.btnSaveAs.Location  = new System.Drawing.Point(180, 100);
            this.btnSaveAs.Width     = 120;
            this.btnSaveAs.Height    = 35;
            this.btnSaveAs.BackColor = System.Drawing.Color.Blue;
            this.btnSaveAs.ForeColor = System.Drawing.Color.White;
            this.btnSaveAs.Click    += new System.EventHandler(this.BtnSaveAs_Click);

            // lblResult
            this.lblResult.Text      = "";
            this.lblResult.Location  = new System.Drawing.Point(12, 150);
            this.lblResult.AutoSize  = true;
            this.lblResult.ForeColor = System.Drawing.Color.Gray;

            // MainForm
            this.ClientSize  = new System.Drawing.Size(500, 190);
            this.Text        = "Compression Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnCompress);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.lblResult);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button  btnBrowse;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button  btnConnect;
        private System.Windows.Forms.Button  btnCompress;
        private System.Windows.Forms.Button  btnSaveAs;
        private System.Windows.Forms.Label   lblFile;
        private System.Windows.Forms.Label   lblServer;
        private System.Windows.Forms.Label   lblPort;
        private System.Windows.Forms.Label   lblResult;
    }
}
