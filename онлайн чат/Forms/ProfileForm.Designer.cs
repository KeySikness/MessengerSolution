namespace ChatClient
{
    partial class ProfileForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabProfile = new System.Windows.Forms.TabPage();
            this.tabSecurity = new System.Windows.Forms.TabPage();

            // Profile Tab
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.btnChangeAvatar = new System.Windows.Forms.Button();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblBio = new System.Windows.Forms.Label();
            this.txtBio = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.btnSaveProfile = new System.Windows.Forms.Button();
            this.btnNotificationSettings = new System.Windows.Forms.Button();

            // Security Tab
            this.grpChangePassword = new System.Windows.Forms.GroupBox();
            this.lblOldPassword = new System.Windows.Forms.Label();
            this.txtOldPassword = new System.Windows.Forms.TextBox();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.btnChangePassword = new System.Windows.Forms.Button();

            this.grpChangeEmail = new System.Windows.Forms.GroupBox();
            this.lblNewEmail = new System.Windows.Forms.Label();
            this.txtNewEmail = new System.Windows.Forms.TextBox();
            this.lblPasswordForEmail = new System.Windows.Forms.Label();
            this.txtPasswordForEmail = new System.Windows.Forms.TextBox();
            this.btnChangeEmail = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabProfile.SuspendLayout();
            this.tabSecurity.SuspendLayout();
            this.grpChangePassword.SuspendLayout();
            this.grpChangeEmail.SuspendLayout();
            this.SuspendLayout();

            // TabControl
            this.tabControl.Controls.Add(this.tabProfile);
            this.tabControl.Controls.Add(this.tabSecurity);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Size = new System.Drawing.Size(460, 480);
            this.tabControl.SelectedIndex = 0;

            // ========== PROFILE TAB ==========
            this.tabProfile.Text = "Профиль";
            this.tabProfile.Controls.Add(this.picAvatar);
            this.tabProfile.Controls.Add(this.btnChangeAvatar);
            this.tabProfile.Controls.Add(this.lblUsername);
            this.tabProfile.Controls.Add(this.txtUsername);
            this.tabProfile.Controls.Add(this.lblBio);
            this.tabProfile.Controls.Add(this.txtBio);
            this.tabProfile.Controls.Add(this.lblStatus);
            this.tabProfile.Controls.Add(this.cmbStatus);
            this.tabProfile.Controls.Add(this.lblEmail);
            this.tabProfile.Controls.Add(this.btnSaveProfile);
            this.tabProfile.Controls.Add(this.btnNotificationSettings);

            // Avatar
            this.picAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picAvatar.Location = new System.Drawing.Point(20, 20);
            this.picAvatar.Size = new System.Drawing.Size(120, 120);
            this.picAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            this.btnChangeAvatar.Location = new System.Drawing.Point(20, 145);
            this.btnChangeAvatar.Size = new System.Drawing.Size(120, 28);
            this.btnChangeAvatar.Text = "Сменить аватар";
            this.btnChangeAvatar.Click += new System.EventHandler(this.btnChangeAvatar_Click);

            // Username
            this.lblUsername.Location = new System.Drawing.Point(160, 20);
            this.lblUsername.Size = new System.Drawing.Size(120, 20);
            this.lblUsername.Text = "Имя пользователя:";

            this.txtUsername.Location = new System.Drawing.Point(160, 43);
            this.txtUsername.Size = new System.Drawing.Size(260, 23);

            // Bio
            this.lblBio.Location = new System.Drawing.Point(160, 75);
            this.lblBio.Size = new System.Drawing.Size(100, 20);
            this.lblBio.Text = "О себе:";

            this.txtBio.Location = new System.Drawing.Point(160, 98);
            this.txtBio.Size = new System.Drawing.Size(260, 60);
            this.txtBio.Multiline = true;

            // Status
            this.lblStatus.Location = new System.Drawing.Point(20, 190);
            this.lblStatus.Size = new System.Drawing.Size(100, 20);
            this.lblStatus.Text = "Статус:";

            this.cmbStatus.Location = new System.Drawing.Point(20, 213);
            this.cmbStatus.Size = new System.Drawing.Size(200, 23);
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.Items.AddRange(new object[] { "Офлайн", "Онлайн", "Не беспокоить" });
            this.cmbStatus.SelectedIndex = 0;
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);

            // Email (read-only)
            this.lblEmail.Location = new System.Drawing.Point(20, 250);
            this.lblEmail.Size = new System.Drawing.Size(400, 20);
            this.lblEmail.Text = "Email: ";

            // Buttons
            this.btnSaveProfile.Location = new System.Drawing.Point(20, 290);
            this.btnSaveProfile.Size = new System.Drawing.Size(150, 30);
            this.btnSaveProfile.Text = "Сохранить профиль";
            this.btnSaveProfile.Click += new System.EventHandler(this.btnSaveProfile_Click);

            this.btnNotificationSettings.Location = new System.Drawing.Point(20, 330);
            this.btnNotificationSettings.Size = new System.Drawing.Size(200, 30);
            this.btnNotificationSettings.Text = "Настройки уведомлений";
            this.btnNotificationSettings.Click += new System.EventHandler(this.btnNotificationSettings_Click);

            // ========== SECURITY TAB ==========
            this.tabSecurity.Text = "Безопасность";
            this.tabSecurity.Controls.Add(this.grpChangePassword);
            this.tabSecurity.Controls.Add(this.grpChangeEmail);

            // Change Password Group
            this.grpChangePassword.Location = new System.Drawing.Point(15, 15);
            this.grpChangePassword.Size = new System.Drawing.Size(410, 180);
            this.grpChangePassword.Text = "Смена пароля";
            this.grpChangePassword.Controls.Add(this.lblOldPassword);
            this.grpChangePassword.Controls.Add(this.txtOldPassword);
            this.grpChangePassword.Controls.Add(this.lblNewPassword);
            this.grpChangePassword.Controls.Add(this.txtNewPassword);
            this.grpChangePassword.Controls.Add(this.lblConfirmPassword);
            this.grpChangePassword.Controls.Add(this.txtConfirmPassword);
            this.grpChangePassword.Controls.Add(this.btnChangePassword);

            this.lblOldPassword.Location = new System.Drawing.Point(15, 25);
            this.lblOldPassword.Size = new System.Drawing.Size(150, 20);
            this.lblOldPassword.Text = "Текущий пароль:";

            this.txtOldPassword.Location = new System.Drawing.Point(15, 48);
            this.txtOldPassword.Size = new System.Drawing.Size(360, 23);
            this.txtOldPassword.PasswordChar = '*';

            this.lblNewPassword.Location = new System.Drawing.Point(15, 78);
            this.lblNewPassword.Size = new System.Drawing.Size(150, 20);
            this.lblNewPassword.Text = "Новый пароль:";

            this.txtNewPassword.Location = new System.Drawing.Point(15, 101);
            this.txtNewPassword.Size = new System.Drawing.Size(360, 23);
            this.txtNewPassword.PasswordChar = '*';

            this.lblConfirmPassword.Location = new System.Drawing.Point(15, 131);
            this.lblConfirmPassword.Size = new System.Drawing.Size(150, 20);
            this.lblConfirmPassword.Text = "Подтвердите пароль:";

            this.txtConfirmPassword.Location = new System.Drawing.Point(15, 154);
            this.txtConfirmPassword.Size = new System.Drawing.Size(250, 23);
            this.txtConfirmPassword.PasswordChar = '*';

            this.btnChangePassword.Location = new System.Drawing.Point(275, 152);
            this.btnChangePassword.Size = new System.Drawing.Size(100, 27);
            this.btnChangePassword.Text = "Изменить";
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);

            // Change Email Group
            this.grpChangeEmail.Location = new System.Drawing.Point(15, 210);
            this.grpChangeEmail.Size = new System.Drawing.Size(410, 140);
            this.grpChangeEmail.Text = "Смена Email";
            this.grpChangeEmail.Controls.Add(this.lblNewEmail);
            this.grpChangeEmail.Controls.Add(this.txtNewEmail);
            this.grpChangeEmail.Controls.Add(this.lblPasswordForEmail);
            this.grpChangeEmail.Controls.Add(this.txtPasswordForEmail);
            this.grpChangeEmail.Controls.Add(this.btnChangeEmail);

            this.lblNewEmail.Location = new System.Drawing.Point(15, 25);
            this.lblNewEmail.Size = new System.Drawing.Size(100, 20);
            this.lblNewEmail.Text = "Новый Email:";

            this.txtNewEmail.Location = new System.Drawing.Point(15, 48);
            this.txtNewEmail.Size = new System.Drawing.Size(360, 23);

            this.lblPasswordForEmail.Location = new System.Drawing.Point(15, 78);
            this.lblPasswordForEmail.Size = new System.Drawing.Size(100, 20);
            this.lblPasswordForEmail.Text = "Пароль:";

            this.txtPasswordForEmail.Location = new System.Drawing.Point(15, 101);
            this.txtPasswordForEmail.Size = new System.Drawing.Size(250, 23);
            this.txtPasswordForEmail.PasswordChar = '*';

            this.btnChangeEmail.Location = new System.Drawing.Point(275, 99);
            this.btnChangeEmail.Size = new System.Drawing.Size(100, 27);
            this.btnChangeEmail.Text = "Изменить";
            this.btnChangeEmail.Click += new System.EventHandler(this.btnChangeEmail_Click);

            // Form
            this.ClientSize = new System.Drawing.Size(484, 504);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Text = "Настройки профиля";
            this.Load += new System.EventHandler(this.ProfileForm_Load);

            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabProfile.ResumeLayout(false);
            this.tabProfile.PerformLayout();
            this.tabSecurity.ResumeLayout(false);
            this.grpChangePassword.ResumeLayout(false);
            this.grpChangePassword.PerformLayout();
            this.grpChangeEmail.ResumeLayout(false);
            this.grpChangeEmail.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabProfile;
        private System.Windows.Forms.TabPage tabSecurity;

        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.Button btnChangeAvatar;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblBio;
        private System.Windows.Forms.TextBox txtBio;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Button btnSaveProfile;
        private System.Windows.Forms.Button btnNotificationSettings;

        private System.Windows.Forms.GroupBox grpChangePassword;
        private System.Windows.Forms.Label lblOldPassword;
        private System.Windows.Forms.TextBox txtOldPassword;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnChangePassword;

        private System.Windows.Forms.GroupBox grpChangeEmail;
        private System.Windows.Forms.Label lblNewEmail;
        private System.Windows.Forms.TextBox txtNewEmail;
        private System.Windows.Forms.Label lblPasswordForEmail;
        private System.Windows.Forms.TextBox txtPasswordForEmail;
        private System.Windows.Forms.Button btnChangeEmail;
    }
}