namespace ChatServer.Api.Models
{
    public enum UserStatus
    {
        Offline = 0,
        Online = 1,
        DoNotDisturb = 2
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";

        // 🆕 Новые поля для профиля
        public string Avatar { get; set; } = "";
        public UserStatus Status { get; set; } = UserStatus.Offline;
        public string Bio { get; set; } = "";

        // 🆕 Настройки уведомлений (JSON строка)
        public string NotificationSettings { get; set; } = "{\"sound\":true,\"banner\":true,\"enabled\":true}";
    }
}