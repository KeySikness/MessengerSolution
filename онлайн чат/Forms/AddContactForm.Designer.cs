namespace ChatClient
{
    partial class AddContactForm
    {
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Button btnOk;

        private void InitializeComponent()
        {
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(12, 12);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(260, 22);
            this.txtLogin.TabIndex = 0;
            this.txtLogin.TextChanged += new System.EventHandler(this.txtLogin_TextChanged);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 45);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Добавить";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // AddContactForm
            // 
            this.ClientSize = new System.Drawing.Size(290, 85);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.btnOk);
            this.Name = "AddContactForm";
            this.Text = "Добавить контакт";
            this.Load += new System.EventHandler(this.AddContactForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
