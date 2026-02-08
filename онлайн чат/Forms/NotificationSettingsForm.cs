using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Windows.Forms;
using ChatClient.Models;

namespace ChatClient
{
    public partial class NotificationSettingsForm : Form
    {
        private readonly HubConnection _connection;
        private readonly int _userId;
        private NotificationSettings _settings;

        public NotificationSettingsForm(HubConnection connection, int userId)
        {
            InitializeComponent();
            _connection = connection;
            _userId = userId;
        }

        private async void NotificationSettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                var result = await _connection.InvokeAsync<System.Text.Json.JsonElement>(
                    "GetUserProfile", _userId);

                string settingsJson = result.GetProperty("notificationSettings").GetString() ?? "";
                _settings = NotificationSettings.FromJson(settingsJson);

                LoadSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки настроек: {ex.Message}");
            }
        }

        private void LoadSettings()
        {
            chkEnabled.Checked = _settings.Enabled;
            chkSound.Checked = _settings.Sound;
            chkBanner.Checked = _settings.Banner;

            chkSound.Enabled = _settings.Enabled;
            chkBanner.Enabled = _settings.Enabled;
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            chkSound.Enabled = chkEnabled.Checked;
            chkBanner.Enabled = chkEnabled.Checked;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _settings.Enabled = chkEnabled.Checked;
                _settings.Sound = chkSound.Checked;
                _settings.Banner = chkBanner.Checked;

                await _connection.InvokeAsync(
                    "UpdateNotificationSettings",
                    _userId,
                    _settings.ToJson()
                );

                MessageBox.Show("Настройки сохранены");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}