using System;
using System.Media;
using System.Windows.Forms;
using ChatClient.Models;

namespace ChatClient.Services
{
    public class NotificationManager
    {
        private NotificationSettings _settings;
        private string _activeDialogId = "";

        public NotificationManager(NotificationSettings settings)
        {
            _settings = settings;
        }

        public void UpdateSettings(NotificationSettings settings)
        {
            _settings = settings;
        }

        public void SetActiveDialog(string dialogId)
        {
            _activeDialogId = dialogId;
        }

        public void ShowNotification(string title, string message, string dialogId)
        {
            if (!_settings.Enabled)
                return;

            // Умные уведомления: не показывать, если пользователь в активном диалоге
            if (!string.IsNullOrEmpty(_activeDialogId) && _activeDialogId == dialogId)
                return;

            // Звук
            if (_settings.Sound)
            {
                try
                {
                    SystemSounds.Asterisk.Play();
                }
                catch { }
            }

            // Баннер (системное уведомление)
            if (_settings.Banner)
            {
                ShowWindowsNotification(title, message);
            }
        }

        private void ShowWindowsNotification(string title, string message)
        {
            try
            {
                // Используем NotifyIcon для показа уведомления
                NotifyIcon notifyIcon = new NotifyIcon
                {
                    Visible = true,
                    Icon = System.Drawing.SystemIcons.Information,
                    BalloonTipTitle = title,
                    BalloonTipText = message,
                    BalloonTipIcon = ToolTipIcon.Info
                };

                notifyIcon.ShowBalloonTip(3000);

                // Удаляем через 4 секунды
                System.Threading.Tasks.Task.Delay(4000).ContinueWith(t =>
                {
                    try
                    {
                        notifyIcon.Visible = false;
                        notifyIcon.Dispose();
                    }
                    catch { }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка показа уведомления: {ex.Message}");
            }
        }
    }
}