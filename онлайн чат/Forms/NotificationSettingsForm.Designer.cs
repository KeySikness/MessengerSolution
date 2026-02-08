namespace ChatClient
{
    partial class NotificationSettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpNotifications = new System.Windows.Forms.GroupBox();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.chkSound = new System.Windows.Forms.CheckBox();
            this.chkBanner = new System.Windows.Forms.CheckBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpNotifications.SuspendLayout();
            this.SuspendLayout();

            // grpNotifications
            this.grpNotifications.Controls.Add(this.chkEnabled);
            this.grpNotifications.Controls.Add(this.chkSound);
            this.grpNotifications.Controls.Add(this.chkBanner);
            this.grpNotifications.Controls.Add(this.lblInfo);
            this.grpNotifications.Location = new System.Drawing.Point(12, 12);
            this.grpNotifications.Size = new System.Drawing.Size(360, 180);
            this.grpNotifications.Text = "Настройки уведомлений";

            // chkEnabled
            this.chkEnabled.Location = new System.Drawing.Point(15, 30);
            this.chkEnabled.Size = new System.Drawing.Size(300, 24);
            this.chkEnabled.Text = "Включить уведомления";
            this.chkEnabled.Checked = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);

            // chkSound
            this.chkSound.Location = new System.Drawing.Point(35, 60);
            this.chkSound.Size = new System.Drawing.Size(280, 24);
            this.chkSound.Text = "Звуковые уведомления";
            this.chkSound.Checked = true;

            // chkBanner
            this.chkBanner.Location = new System.Drawing.Point(35, 90);
            this.chkBanner.Size = new System.Drawing.Size(280, 24);
            this.chkBanner.Text = "Баннеры (системные уведомления)";
            this.chkBanner.Checked = true;

            // lblInfo
            this.lblInfo.Location = new System.Drawing.Point(15, 125);
            this.lblInfo.Size = new System.Drawing.Size(330, 40);
            this.lblInfo.Text = "Умные уведомления: уведомления не будут\nпоказываться, если вы активны в диалоге.";
            this.lblInfo.ForeColor = System.Drawing.Color.Gray;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(190, 205);
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(290, 205);
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            // Form
            this.ClientSize = new System.Drawing.Size(394, 250);
            this.Controls.Add(this.grpNotifications);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Настройки уведомлений";
            this.Load += new System.EventHandler(this.NotificationSettingsForm_Load);
            this.grpNotifications.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox grpNotifications;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.CheckBox chkSound;
        private System.Windows.Forms.CheckBox chkBanner;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}