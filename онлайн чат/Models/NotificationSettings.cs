using System.Text.Json;

namespace ChatClient.Models
{
    public class NotificationSettings
    {
        public bool Sound { get; set; } = true;
        public bool Banner { get; set; } = true;
        public bool Enabled { get; set; } = true;

        public static NotificationSettings FromJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<NotificationSettings>(json)
                    ?? new NotificationSettings();
            }
            catch
            {
                return new NotificationSettings();
            }
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}