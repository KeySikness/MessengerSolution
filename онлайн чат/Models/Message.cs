using System;

namespace ChatClient.Models   // ✅ добавлено namespace
{
    public enum MessageType
    {
        Text = 0,
        Image = 1,
        Document = 2
    }

    public class Message
    {
        public Guid Id { get; set; }
        public Guid DialogId { get; set; }
        public int SenderId { get; set; }

        public string Text { get; set; } = "";
        public MessageType Type { get; set; } = MessageType.Text;

        public bool IsEdited { get; set; }
        public bool IsDeleted { get; set; }    // 🔹 добавил для SignalR
        public DateTime CreatedAt { get; set; }
        public MessageStatus Status { get; set; } // 🔹 добавил для SignalR

        public override string ToString()
        {
            string time = CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
            return $"[{time}] {Text}" + (IsEdited ? " (изменено)" : "");
        }
    }

    public enum MessageStatus
    {
        Sent = 0,
        Delivered = 1,
        Read = 2
    }
}
