using System;

namespace ChatServer.Api.Models
{
    public enum MessageStatus
    {
        Sent = 0,
        Delivered = 1,
        Read = 2
    }

    public enum MessageType
    {
        Text = 0,
        Image = 1,
        Document = 2
    }

    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid DialogId { get; set; }
        public int SenderId { get; set; }

        public string Text { get; set; } = "";
        public MessageType Type { get; set; } = MessageType.Text;

        public bool IsEdited { get; set; } = false;
        public bool IsDeleted { get; set; } = false; // 🔹 для удаления
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public MessageStatus Status { get; set; } = MessageStatus.Sent;
    }

}
