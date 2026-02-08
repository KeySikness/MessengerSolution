using System;
using System.Drawing;

namespace ChatClient.Models
{
    public class ContactItem
    {
        public int UserId { get; set; }
        public Guid DialogId { get; set; }
        public string Username { get; set; } = "";
        public string AvatarUrl { get; set; } = "";
        public UserStatus Status { get; set; } = UserStatus.Offline;
        public Image Avatar { get; set; } // Загруженное изображение

        public override string ToString()
        {
            return Username;
        }

        // Получить цвет статуса
        public Color GetStatusColor()
        {
            switch (Status)
            {
                case UserStatus.Online:
                    return Color.LimeGreen;
                case UserStatus.DoNotDisturb:
                    return Color.Red;
                case UserStatus.Offline:
                default:
                    return Color.Gray;
            }
        }

        // Получить текст статуса
        public string GetStatusText()
        {
            switch (Status)
            {
                case UserStatus.Online:
                    return "В сети";
                case UserStatus.DoNotDisturb:
                    return "Не беспокоить";
                case UserStatus.Offline:
                default:
                    return "Не в сети";
            }
        }
    }
}