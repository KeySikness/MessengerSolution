using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Drawing;
using System.Text.Json;
using System.Windows.Forms;
using ChatClient.Models;

namespace ChatClient
{
    public partial class ProfileForm : Form
    {
        private readonly HubConnection _connection;
        private readonly int _userId;
        private User _currentUser;

        public event Action<string> UsernameChanged;
        public event Action<string> AvatarChanged;
        public event Action<UserStatus> StatusChanged;

        public ProfileForm(HubConnection connection, int userId)
        {
            InitializeComponent();
            _connection = connection;
            _userId = userId;
        }

        private async void ProfileForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Загружаем профиль с сервера
                var result = await _connection.InvokeAsync<JsonElement>("GetUserProfile", _userId);

                _currentUser = new User
                {
                    Id = result.GetProperty("id").GetInt32(),
                    Username = result.GetProperty("username").GetString() ?? "",
                    FirstName = result.GetProperty("firstName").GetString() ?? "",
                    LastName = result.GetProperty("lastName").GetString() ?? "",
                    Email = result.GetProperty("email").GetString() ?? "",
                    Avatar = result.GetProperty("avatar").GetString() ?? "",
                    Status = (UserStatus)result.GetProperty("status").GetInt32(),
                    Bio = result.GetProperty("bio").GetString() ?? ""
                };

                LoadUserData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки профиля: {ex.Message}");
            }
        }

        private void LoadUserData()
        {
            txtUsername.Text = _currentUser.Username;
            txtBio.Text = _currentUser.Bio;
            lblEmail.Text = $"Email: {_currentUser.Email}";

            // Статус
            cmbStatus.SelectedIndex = (int)_currentUser.Status;

            LoadAvatar(_currentUser.Avatar);
        }

        private void LoadAvatar(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                {
                    using (var img = Image.FromFile(path))
                    {
                        picAvatar.Image = new Bitmap(img);
                    }
                }
                else
                {
                    picAvatar.Image = null;
                }
            }
            catch
            {
                picAvatar.Image = null;
            }
        }

        // ================= СМЕНА АВАТАРА =================
        private async void btnChangeAvatar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                try
                {
                    _currentUser.Avatar = dlg.FileName;
                    LoadAvatar(_currentUser.Avatar);

                    AvatarChanged?.Invoke(_currentUser.Avatar);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка смены аватара: {ex.Message}");
                }
            }
        }

        // ================= СОХРАНЕНИЕ ПРОФИЛЯ =================
        private async void btnSaveProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string newUsername = txtUsername.Text.Trim();
                string newBio = txtBio.Text.Trim();

                if (string.IsNullOrWhiteSpace(newUsername))
                {
                    MessageBox.Show("Имя пользователя не может быть пустым");
                    return;
                }

                await _connection.InvokeAsync(
                    "UpdateProfile",
                    _userId,
                    newUsername,
                    newBio,
                    _currentUser.Avatar
                );

                _currentUser.Username = newUsername;
                _currentUser.Bio = newBio;

                UsernameChanged?.Invoke(newUsername);
                MessageBox.Show("Профиль успешно обновлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // ================= СМЕНА СТАТУСА =================
        private async void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UserStatus newStatus = (UserStatus)cmbStatus.SelectedIndex;

                await _connection.InvokeAsync("ChangeUserStatus", _userId, (int)newStatus);

                _currentUser.Status = newStatus;
                StatusChanged?.Invoke(newStatus);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка смены статуса: {ex.Message}");
            }
        }

        // ================= СМЕНА ПАРОЛЯ =================
        private async void btnChangePassword_Click(object sender, EventArgs e)
        {
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(oldPassword) ||
                string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }

            try
            {
                // Проверяем старый пароль через попытку входа
                await _connection.InvokeAsync("Login", _currentUser.Email, oldPassword);

                // Меняем пароль
                await _connection.InvokeAsync("ResetPassword", _currentUser.Email, newPassword);

                MessageBox.Show("Пароль успешно изменен");
                txtOldPassword.Clear();
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();
            }
            catch
            {
                MessageBox.Show("Неверный текущий пароль");
            }
        }

        // ================= СМЕНА EMAIL =================
        private async void btnChangeEmail_Click(object sender, EventArgs e)
        {
            string newEmail = txtNewEmail.Text.Trim();
            string password = txtPasswordForEmail.Text;

            if (string.IsNullOrWhiteSpace(newEmail) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            try
            {
                await _connection.InvokeAsync("ChangeEmail", _userId, newEmail, password);

                _currentUser.Email = newEmail;
                lblEmail.Text = $"Email: {newEmail}";

                MessageBox.Show("Email успешно изменен");
                txtNewEmail.Clear();
                txtPasswordForEmail.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // ================= НАСТРОЙКИ УВЕДОМЛЕНИЙ =================
        private void btnNotificationSettings_Click(object sender, EventArgs e)
        {
            NotificationSettingsForm form = new NotificationSettingsForm(_connection, _userId);
            form.ShowDialog();
        }
    }
}