namespace онлайн_чат
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblMiddleName;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblPassword;

        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtMiddleName;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtPassword;

        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            const int left = 20;
            const int width = 260;
            const int labelHeight = 16;
            const int boxHeight = 22;
            int y = 15;

            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();

            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();

            this.lblMiddleName = new System.Windows.Forms.Label();
            this.txtMiddleName = new System.Windows.Forms.TextBox();

            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();

            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();

            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();

            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();

            this.btnRegister = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // Имя
            this.lblFirstName.Text = "Имя:";
            this.lblFirstName.Location = new System.Drawing.Point(left, y);
            this.lblFirstName.Size = new System.Drawing.Size(width, labelHeight);
            y += labelHeight + 4;

            this.txtFirstName.Location = new System.Drawing.Point(left, y);
            this.txtFirstName.Size = new System.Drawing.Size(width, boxHeight);
            y += boxHeight + 12;

            // Фамилия
            this.lblLastName.Text = "Фамилия:";
            this.lblLastName.Location = new System.Drawing.Point(left, y);
            this.lblLastName.Size = new System.Drawing.Size(width, labelHeight);
            y += labelHeight + 4;

            this.txtLastName.Location = new System.Drawing.Point(left, y);
            this.txtLastName.Size = new System.Drawing.Size(width, boxHeight);
            y += boxHeight + 12;

            // Отчество
            this.lblMiddleName.Text = "Отчество:";
            this.lblMiddleName.Location = new System.Drawing.Point(left, y);
            this.lblMiddleName.Size = new System.Drawing.Size(width, labelHeight);
            y += labelHeight + 4;

            this.txtMiddleName.Location = new System.Drawing.Point(left, y);
            this.txtMiddleName.Size = new System.Drawing.Size(width, boxHeight);
            y += boxHeight + 12;

            // Ник
            this.lblUsername.Text = "Ник / имя пользователя:";
            this.lblUsername.Location = new System.Drawing.Point(left, y);
            this.lblUsername.Size = new System.Drawing.Size(width, labelHeight);
            y += labelHeight + 4;

            this.txtUsername.Location = new System.Drawing.Point(left, y);
            this.txtUsername.Size = new System.Drawing.Size(width, boxHeight);
            y += boxHeight + 12;

            // Email
            this.lblEmail.Text = "E-mail:";
            this.lblEmail.Location = new System.Drawing.Point(left, y);
            this.lblEmail.Size = new System.Drawing.Size(width, labelHeight);
            y += labelHeight + 4;

            this.txtEmail.Location = new System.Drawing.Point(left, y);
            this.txtEmail.Size = new System.Drawing.Size(width, boxHeight);
            y += boxHeight + 12;

            // Телефон
            this.lblPhone.Text = "Номер телефона:";
            this.lblPhone.Location = new System.Drawing.Point(left, y);
            this.lblPhone.Size = new System.Drawing.Size(width, labelHeight);
            y += labelHeight + 4;

            this.txtPhone.Location = new System.Drawing.Point(left, y);
            this.txtPhone.Size = new System.Drawing.Size(width, boxHeight);
            y += boxHeight + 12;

            // Пароль
            this.lblPassword.Text = "Пароль:";
            this.lblPassword.Location = new System.Drawing.Point(left, y);
            this.lblPassword.Size = new System.Drawing.Size(width, labelHeight);
            y += labelHeight + 4;

            this.txtPassword.Location = new System.Drawing.Point(left, y);
            this.txtPassword.Size = new System.Drawing.Size(width, boxHeight);
            this.txtPassword.PasswordChar = '*';
            y += boxHeight + 18;

            // Кнопки
            this.btnRegister.Text = "Зарегистрироваться";
            this.btnRegister.Location = new System.Drawing.Point(left, y);
            this.btnRegister.Size = new System.Drawing.Size(width, 28);
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            y += 38;

            this.btnBack.Text = "Назад к входу";
            this.btnBack.Location = new System.Drawing.Point(left, y);
            this.btnBack.Size = new System.Drawing.Size(width, 28);
            this.btnBack.Click += (s, e) =>
            {
                new LoginForm().Show();
                this.Hide();
            };

            // Form
            this.ClientSize = new System.Drawing.Size(300, y + 50);
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblFirstName, txtFirstName,
                lblLastName, txtLastName,
                lblMiddleName, txtMiddleName,
                lblUsername, txtUsername,
                lblEmail, txtEmail,
                lblPhone, txtPhone,
                lblPassword, txtPassword,
                btnRegister, btnBack
            });

            this.Text = "Регистрация";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
