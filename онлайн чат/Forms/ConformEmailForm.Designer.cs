namespace онлайн_чат
{
    partial class ConfirmEmailForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnConfirm;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(25, 20);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(260, 16);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Введите код, который пришёл на email:";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(28, 50);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(250, 22);
            this.txtCode.TabIndex = 1;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(28, 90);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(120, 30);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "Подтвердить";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // ConfirmEmailForm
            // 
            this.ClientSize = new System.Drawing.Size(320, 150);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.btnConfirm);
            this.Name = "ConfirmEmailForm";
            this.Text = "Подтверждение email";
            this.Load += new System.EventHandler(this.ConfirmEmailForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
