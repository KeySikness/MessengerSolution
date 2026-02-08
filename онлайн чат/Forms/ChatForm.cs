using ChatClient.Controls;
using ChatClient.Models;
using ChatClient.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class ChatForm : Form
    {
        private HubConnection _connection;
        private Dictionary<int, ContactItem> _contacts = new Dictionary<int, ContactItem>();
        private int _currentUserId;
        private Guid _currentDialogId;
        private ContactItem _currentContact;

        private NotificationManager _notificationManager;
        private string _currentUsername = "";

        public ChatForm(int userId)
        {
            InitializeComponent();
            _currentUserId = userId;
            Connect();
        }

        private async void Connect()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:5072/chat?userId={_currentUserId}")
                .WithAutomaticReconnect()
                .Build();

            await InitNotifications();

            _connection.On<JsonElement>("ReceiveMessage", msg =>
            {
                this.Invoke(new Action(() =>
                {
                    bool isDeleted = TryGetBool(msg, "IsDeleted", false);
                    if (isDeleted) return;

                    Guid id = Guid.Parse(TryGetString(msg, "Id", Guid.Empty.ToString()));
                    Guid dialogId = Guid.Parse(TryGetString(msg, "DialogId", Guid.Empty.ToString()));
                    int senderId = TryGetInt(msg, "SenderId", 0);
                    string text = TryGetString(msg, "Text", "");
                    int typeInt = TryGetInt(msg, "Type", 0);
                    int statusInt = TryGetInt(msg, "Status", 0);
                    bool isEdited = TryGetBool(msg, "IsEdited", false);
                    DateTime createdAt = TryGetDateTime(msg, "CreatedAt", DateTime.UtcNow);
                    string senderNick = TryGetString(msg, "Nickname", "Unknown");

                    if (dialogId != _currentDialogId)
                    {
                        if (senderId != _currentUserId)
                        {
                            _notificationManager?.ShowNotification(
                                $"Новое сообщение от {senderNick}",
                                text.Length > 50 ? text.Substring(0, 50) + "..." : text,
                                dialogId.ToString()
                            );
                        }
                        return;
                    }

                    MessageType type = (MessageType)typeInt;
                    MessageStatus status = (MessageStatus)statusInt;

                    string time = createdAt.ToLocalTime().ToString("HH:mm dd.MM");
                    string display;

                    switch (type)
                    {
                        case MessageType.Image:
                            display = $"[{time}] {senderNick}: [Изображение]";
                            break;
                        case MessageType.Document:
                            display = $"[{time}] {senderNick}: [Документ]";
                            break;
                        default:
                            display = $"[{time}] {senderNick}: {text}";
                            break;
                    }

                    string statusText;
                    if (status == MessageStatus.Sent)
                        statusText = "(отправлено)";
                    else if (status == MessageStatus.Delivered)
                        statusText = "(доставлено)";
                    else if (status == MessageStatus.Read)
                        statusText = "(прочитано)";
                    else
                        statusText = "";

                    if (senderId == _currentUserId)
                        display += " " + statusText;

                    listMessages.Items.Add(new ChatMessage
                    {
                        Id = id,
                        SenderId = senderId,
                        DisplayText = display,
                        Url = text,
                        Type = type,
                        IsEdited = isEdited,
                        IsDeleted = false,
                        Status = status
                    });
                }));
            });

            _connection.On<Guid, int>("MessageStatusUpdated", (msgId, status) =>
            {
                this.Invoke(new Action(() =>
                {
                    for (int i = 0; i < listMessages.Items.Count; i++)
                    {
                        ChatMessage msg = listMessages.Items[i] as ChatMessage;
                        if (msg == null || msg.Id != msgId) continue;

                        msg.Status = (MessageStatus)status;
                        string baseText = msg.DisplayText;
                        int idx = baseText.LastIndexOf("(");
                        if (idx > 0)
                            baseText = baseText.Substring(0, idx).TrimEnd();

                        string statusTextUpdate;
                        if (status == (int)MessageStatus.Sent)
                            statusTextUpdate = "(отправлено)";
                        else if (status == (int)MessageStatus.Delivered)
                            statusTextUpdate = "(доставлено)";
                        else if (status == (int)MessageStatus.Read)
                            statusTextUpdate = "(прочитано)";
                        else
                            statusTextUpdate = "";

                        msg.DisplayText = baseText + " " + statusTextUpdate;
                        listMessages.Refresh();
                        break;
                    }
                }));
            });

            _connection.On<int, string, Guid>("NewDialog", (userId, nickname, dialogId) =>
            {
                this.Invoke(new Action(async () =>
                {
                    if (_contacts.ContainsKey(userId)) return;

                    if (userId == _currentUserId)
                        return;

                    ContactItem contact = new ContactItem
                    {
                        UserId = userId,
                        Username = nickname,
                        DialogId = dialogId,
                        Status = UserStatus.Offline
                    };

                    try
                    {
                        var userInfo = await _connection.InvokeAsync<JsonElement>("GetUserProfile", userId);
                        string avatarPath = TryGetString(userInfo, "avatar", "");
                        int statusInt = TryGetInt(userInfo, "status", 0);

                        contact.Status = (UserStatus)statusInt;

                        if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
                        {
                            try
                            {
                                using (var img = Image.FromFile(avatarPath))
                                {
                                    contact.Avatar = new Bitmap(img);
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }

                    _contacts[userId] = contact;
                    listContacts.Items.Add(contact);
                }));
            });

            _connection.On<Guid, string>("MessageEdited", (msgId, newText) =>
            {
                this.Invoke(new Action(() =>
                {
                    for (int i = 0; i < listMessages.Items.Count; i++)
                    {
                        ChatMessage msg = listMessages.Items[i] as ChatMessage;
                        if (msg != null && msg.Id == msgId)
                        {
                            msg.Url = newText;
                            msg.IsEdited = true;

                            int idx = msg.DisplayText.LastIndexOf("(");
                            string statusPart = idx > 0 ? msg.DisplayText.Substring(idx) : "";
                            msg.DisplayText = newText + " (ред.) " + statusPart;
                            listMessages.Refresh();
                            break;
                        }
                    }
                }));
            });

            _connection.On<Guid>("MessageDeleted", msgId =>
            {
                this.Invoke(new Action(() =>
                {
                    for (int i = 0; i < listMessages.Items.Count; i++)
                    {
                        ChatMessage msg = listMessages.Items[i] as ChatMessage;
                        if (msg != null && msg.Id == msgId)
                        {
                            listMessages.Items.RemoveAt(i);
                            break;
                        }
                    }
                }));
            });

            _connection.On<int, string, string, string>("UserProfileUpdated", (userId, username, avatar, bio) =>
            {
                this.Invoke(new Action(() =>
                {
                    if (_contacts.ContainsKey(userId))
                    {
                        ContactItem contact = _contacts[userId];
                        contact.Username = username;

                        if (!string.IsNullOrEmpty(avatar) && File.Exists(avatar))
                        {
                            try
                            {
                                using (var img = Image.FromFile(avatar))
                                {
                                    contact.Avatar = new Bitmap(img);
                                }
                            }
                            catch { }
                        }

                        listContacts.Refresh();

                        if (_currentContact?.UserId == userId)
                        {
                            dialogInfoPanel.SetContact(contact);
                        }
                    }

                    if (userId == _currentUserId)
                    {
                        _currentUsername = username;
                        this.Text = $"Онлайн чат - {username}";
                    }
                }));
            });

            _connection.On<int, int>("UserStatusChanged", (userId, status) =>
            {
                this.Invoke(new Action(() =>
                {
                    if (_contacts.ContainsKey(userId))
                    {
                        ContactItem contact = _contacts[userId];
                        contact.Status = (UserStatus)status;
                        listContacts.Refresh();

                        if (_currentContact?.UserId == userId)
                        {
                            dialogInfoPanel.UpdateStatus((UserStatus)status);
                        }
                    }
                }));
            });

            try
            {
                await _connection.StartAsync();
                await _connection.InvokeAsync("GetMyDialogs", _currentUserId);
                await _connection.InvokeAsync("ChangeUserStatus", _currentUserId, (int)UserStatus.Online);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task InitNotifications()
        {
            try
            {
                var result = await _connection.InvokeAsync<JsonElement>("GetUserProfile", _currentUserId);

                string settingsJson = TryGetString(result, "notificationSettings", "");
                var settings = NotificationSettings.FromJson(settingsJson);

                _notificationManager = new NotificationManager(settings);

                _currentUsername = TryGetString(result, "username", "User");
                this.Text = $"Онлайн чат - {_currentUsername}";
            }
            catch
            {
                _notificationManager = new NotificationManager(new NotificationSettings());
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text) || _currentDialogId == Guid.Empty)
                return;

            await _connection.InvokeAsync("SendMessage",
                _currentDialogId,
                _currentUserId,
                txtMessage.Text.Trim(),
                MessageType.Text);

            txtMessage.Clear();
        }

        private async void btnSendFile_Click(object sender, EventArgs e)
        {
            if (_currentDialogId == Guid.Empty)
                return;

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dlg.FileName;
            string fileName = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filePath).ToLower();

            MessageType type = (ext == ".png" || ext == ".jpg" || ext == ".jpeg" ||
                               ext == ".gif" || ext == ".bmp")
                ? MessageType.Image
                : MessageType.Document;

            using (HttpClient client = new HttpClient())
            using (FileStream fs = File.OpenRead(filePath))
            using (MultipartFormDataContent form = new MultipartFormDataContent())
            {
                form.Add(new StreamContent(fs), "file", fileName);
                HttpResponseMessage response = await client.PostAsync(
                    "http://localhost:5072/upload", form);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Ошибка загрузки файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fileUrl = await response.Content.ReadAsStringAsync();
                await _connection.InvokeAsync("SendMessage",
                    _currentDialogId,
                    _currentUserId,
                    fileUrl,
                    type);
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            ChatMessage msg = listMessages.SelectedItem as ChatMessage;
            if (msg == null || msg.SenderId != _currentUserId || msg.Type != MessageType.Text)
                return;

            string newText = DialogHelper.ShowInput("Редактировать сообщение",
                "Введите текст", msg.Url);

            if (string.IsNullOrWhiteSpace(newText))
                return;

            await _connection.InvokeAsync("EditMessage", msg.Id, newText);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            ChatMessage msg = listMessages.SelectedItem as ChatMessage;
            if (msg == null || msg.SenderId != _currentUserId)
                return;

            if (MessageBox.Show("Удалить сообщение?", "Подтверждение",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            await _connection.InvokeAsync("DeleteMessage", msg.Id);
        }

        private async void listMessages_DoubleClick(object sender, EventArgs e)
        {
            ChatMessage msg = listMessages.SelectedItem as ChatMessage;
            if (msg == null)
                return;

            if (msg.Type == MessageType.Image)
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] data = await client.GetByteArrayAsync(
                        "http://localhost:5072" + msg.Url);

                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        Form imgForm = new Form();
                        imgForm.WindowState = FormWindowState.Maximized;
                        imgForm.BackColor = Color.Black;

                        PictureBox pb = new PictureBox();
                        pb.Dock = DockStyle.Fill;
                        pb.SizeMode = PictureBoxSizeMode.Zoom;
                        pb.Image = Image.FromStream(ms);

                        imgForm.Controls.Add(pb);
                        imgForm.ShowDialog();
                    }
                }
            }
            else if (msg.Type == MessageType.Document)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = Path.GetFileName(msg.Url);

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                using (HttpClient client = new HttpClient())
                {
                    byte[] data = await client.GetByteArrayAsync(
                        "http://localhost:5072" + msg.Url);
                    File.WriteAllBytes(sfd.FileName, data);
                }

                MessageBox.Show("Файл сохранён", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void listContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContactItem contact = listContacts.SelectedItem as ContactItem;
            if (contact == null)
                return;

            _currentDialogId = contact.DialogId;
            _currentContact = contact;

            listMessages.Items.Clear();

            dialogInfoPanel.SetContact(contact);

            _notificationManager?.SetActiveDialog(_currentDialogId.ToString());

            await _connection.InvokeAsync("JoinDialog", _currentDialogId);
            await _connection.InvokeAsync("MarkAsRead", _currentDialogId);
        }

        private async void btnAddContact_Click(object sender, EventArgs e)
        {
            AddContactForm form = new AddContactForm();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string otherUserLogin = form.Login;

                Guid dialogId = await _connection.InvokeAsync<Guid>("AddContact", _currentUserId, otherUserLogin);

                var otherUserInfo = await _connection.InvokeAsync<JsonElement>("GetUserProfileByLogin", otherUserLogin);

                int otherUserId = TryGetInt(otherUserInfo, "id", -1);

                if (otherUserId == -1)
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                System.Threading.Tasks.Task.Delay(500).Wait();

                if (_contacts.ContainsKey(otherUserId))
                {
                    _currentDialogId = dialogId;
                    _currentContact = _contacts[otherUserId];

                    listMessages.Items.Clear();
                    dialogInfoPanel.SetContact(_currentContact);

                    _notificationManager?.SetActiveDialog(_currentDialogId.ToString());

                    await _connection.InvokeAsync("JoinDialog", dialogId);
                }
            }
            catch (HubException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления контакта: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            ProfileForm profileForm = new ProfileForm(_connection, _currentUserId);

            profileForm.UsernameChanged += (newUsername) =>
            {
                _currentUsername = newUsername;
                this.Text = $"Онлайн чат - {newUsername}";
            };

            profileForm.ShowDialog();
        }

        private void listMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (_connection != null && _connection.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("ChangeUserStatus", _currentUserId, (int)UserStatus.Offline);
                    await _connection.StopAsync();
                }
            }
            catch { }

            base.OnFormClosing(e);
        }

        public class ChatMessage
        {
            public Guid Id;
            public int SenderId;
            public string DisplayText = "";
            public string Url = "";
            public MessageType Type;
            public bool IsEdited;
            public bool IsDeleted;
            public MessageStatus Status;

            public override string ToString()
            {
                return DisplayText;
            }
        }

        private static bool TryGetProperty(JsonElement e, string name, out JsonElement value)
        {
            if (string.IsNullOrEmpty(name))
            {
                value = default(JsonElement);
                return false;
            }

            if (e.TryGetProperty(name, out value))
                return true;

            string camel = char.ToLowerInvariant(name[0]).ToString() + name.Substring(1);
            return e.TryGetProperty(camel, out value);
        }

        private static bool TryGetBool(JsonElement e, string name, bool def)
        {
            return TryGetProperty(e, name, out JsonElement p) &&
                   (p.ValueKind == JsonValueKind.True || p.ValueKind == JsonValueKind.False)
                ? p.GetBoolean()
                : def;
        }

        private static int TryGetInt(JsonElement e, string name, int def)
        {
            return TryGetProperty(e, name, out JsonElement p) &&
                   p.ValueKind == JsonValueKind.Number
                ? p.GetInt32()
                : def;
        }

        private static string TryGetString(JsonElement e, string name, string def)
        {
            return TryGetProperty(e, name, out JsonElement p) &&
                   p.ValueKind == JsonValueKind.String
                ? p.GetString()
                : def;
        }

        private static DateTime TryGetDateTime(JsonElement e, string name, DateTime def)
        {
            return TryGetProperty(e, name, out JsonElement p) &&
                   p.ValueKind == JsonValueKind.String
                ? p.GetDateTime()
                : def;
        }

        public static class DialogHelper
        {
            public static string ShowInput(string title, string prompt, string defaultText)
            {
                Form form = new Form();
                form.Width = 400;
                form.Height = 150;
                form.Text = title;

                TextBox textBox = new TextBox();
                textBox.Left = 10;
                textBox.Top = 20;
                textBox.Width = 360;
                textBox.Text = defaultText;

                Button ok = new Button();
                ok.Text = "OK";
                ok.Left = 200;
                ok.Top = 50;
                ok.Width = 80;
                ok.DialogResult = DialogResult.OK;

                Button cancel = new Button();
                cancel.Text = "Cancel";
                cancel.Left = 290;
                cancel.Top = 50;
                cancel.Width = 80;
                cancel.DialogResult = DialogResult.Cancel;

                form.Controls.Add(textBox);
                form.Controls.Add(ok);
                form.Controls.Add(cancel);
                form.AcceptButton = ok;
                form.CancelButton = cancel;

                return form.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }
    }
}