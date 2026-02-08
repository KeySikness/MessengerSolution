namespace ChatServer.AppHost.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid DialogId { get; set; }
        public string Text { get; set; }
        public bool IsEdited { get; set; }
    }
}
