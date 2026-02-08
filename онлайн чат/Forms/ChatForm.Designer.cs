using ChatClient.Controls;

namespace ChatClient
{
    partial class ChatForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listContacts = new ContactListBox();
            this.dialogInfoPanel = new DialogInfoPanel();
            this.listMessages = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnAddContact = new System.Windows.Forms.Button();
            this.btnSendFile = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnProfile = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.listContacts.FormattingEnabled = true;
            this.listContacts.Location = new System.Drawing.Point(12, 12);
            this.listContacts.Name = "listContacts";
            this.listContacts.Size = new System.Drawing.Size(200, 356);
            this.listContacts.TabIndex = 0;
            this.listContacts.SelectedIndexChanged += new System.EventHandler(this.listContacts_SelectedIndexChanged);

            this.btnAddContact.Location = new System.Drawing.Point(12, 375);
            this.btnAddContact.Name = "btnAddContact";
            this.btnAddContact.Size = new System.Drawing.Size(200, 28);
            this.btnAddContact.TabIndex = 1;
            this.btnAddContact.Text = "Добавить контакт";
            this.btnAddContact.UseVisualStyleBackColor = true;
            this.btnAddContact.Click += new System.EventHandler(this.btnAddContact_Click);

            this.btnProfile.Location = new System.Drawing.Point(12, 410);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(200, 28);
            this.btnProfile.TabIndex = 8;
            this.btnProfile.Text = "Профиль";
            this.btnProfile.UseVisualStyleBackColor = true;
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);

            this.dialogInfoPanel.Location = new System.Drawing.Point(225, 12);
            this.dialogInfoPanel.Name = "dialogInfoPanel";
            this.dialogInfoPanel.Size = new System.Drawing.Size(480, 70);
            this.dialogInfoPanel.TabIndex = 9;

            this.listMessages.FormattingEnabled = true;
            this.listMessages.ItemHeight = 16;
            this.listMessages.Location = new System.Drawing.Point(225, 90);
            this.listMessages.Name = "listMessages";
            this.listMessages.Size = new System.Drawing.Size(480, 220);
            this.listMessages.TabIndex = 2;
            this.listMessages.HorizontalScrollbar = true;
            this.listMessages.SelectedIndexChanged += new System.EventHandler(this.listMessages_SelectedIndexChanged);
            this.listMessages.DoubleClick += new System.EventHandler(this.listMessages_DoubleClick);

            this.txtMessage.Location = new System.Drawing.Point(225, 325);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(370, 22);
            this.txtMessage.TabIndex = 3;
            this.txtMessage.Multiline = true;
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            this.btnSend.Location = new System.Drawing.Point(605, 323);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 23);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            this.btnSendFile.Location = new System.Drawing.Point(605, 352);
            this.btnSendFile.Name = "btnSendFile";
            this.btnSendFile.Size = new System.Drawing.Size(100, 23);
            this.btnSendFile.TabIndex = 5;
            this.btnSendFile.Text = "Файл";
            this.btnSendFile.UseVisualStyleBackColor = true;
            this.btnSendFile.Click += new System.EventHandler(this.btnSendFile_Click);

            this.btnEdit.Location = new System.Drawing.Point(225, 352);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(170, 23);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "Редактировать";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            this.btnDelete.Location = new System.Drawing.Point(405, 352);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(190, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.ClientSize = new System.Drawing.Size(720, 450);
            this.Controls.Add(this.dialogInfoPanel);
            this.Controls.Add(this.btnProfile);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnSendFile);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.listMessages);
            this.Controls.Add(this.btnAddContact);
            this.Controls.Add(this.listContacts);
            this.Name = "ChatForm";
            this.Text = "Онлайн чат";
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private ContactListBox listContacts;
        private DialogInfoPanel dialogInfoPanel;
        private System.Windows.Forms.ListBox listMessages;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnSendFile;
        private System.Windows.Forms.Button btnAddContact;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnProfile;
    }
}