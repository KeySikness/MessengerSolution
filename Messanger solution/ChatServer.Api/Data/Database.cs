using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ChatServer.Api.Models;

namespace ChatServer.Api.Data
{
    public static class Database
    {
        public static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chat.db");

        static Database()
        {
            if (!File.Exists(DbPath))
                SQLiteConnection.CreateFile(DbPath);

            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    MiddleName TEXT,
                    Username TEXT NOT NULL,
                    Email TEXT UNIQUE NOT NULL,
                    Phone TEXT UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    Avatar TEXT DEFAULT '',
                    Status INTEGER DEFAULT 0,
                    Bio TEXT DEFAULT '',
                    NotificationSettings TEXT DEFAULT '{""sound"":true,""banner"":true,""enabled"":true}'
                );

                CREATE TABLE IF NOT EXISTS Dialogs (
                    Id TEXT PRIMARY KEY
                );

                CREATE TABLE IF NOT EXISTS DialogMembers (
                    DialogId TEXT,
                    UserId INTEGER,
                    UNIQUE(DialogId, UserId)
                );

                CREATE TABLE IF NOT EXISTS Messages (
                    Id TEXT PRIMARY KEY,
                    DialogId TEXT,
                    SenderId INTEGER,
                    Text TEXT,
                    Type INTEGER,
                    IsEdited INTEGER,
                    CreatedAt TEXT,
                    Status INTEGER,
                    IsDeleted INTEGER DEFAULT 0
                );";
            cmd.ExecuteNonQuery();
        }

        public static int RegisterUser(string firstName, string lastName, string middleName, string username, string email, string phone, string password)
        {
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            username = username.Trim();
            email = email.Trim();
            phone = NormalizePhone(phone ?? "");

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
                return -2;

            if (!IsValidEmail(email))
                return -3;

            if (!string.IsNullOrEmpty(phone) && !IsValidPhone(phone))
                return -4;

            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();

            using (var check = con.CreateCommand())
            {
                check.CommandText = "SELECT COUNT(*) FROM Users WHERE Email=@e OR Phone=@p";
                check.Parameters.AddWithValue("@e", email);
                check.Parameters.AddWithValue("@p", phone);
                if ((long)check.ExecuteScalar() > 0)
                    return -1;
            }

            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Users (FirstName, LastName, MiddleName, Username, Email, Phone, PasswordHash)
                VALUES (@fn,@ln,@mn,@u,@e,@p,@h);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@fn", firstName);
            cmd.Parameters.AddWithValue("@ln", lastName);
            cmd.Parameters.AddWithValue("@mn", middleName);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@e", email);
            cmd.Parameters.AddWithValue("@p", phone);
            cmd.Parameters.AddWithValue("@h", Hash(password));

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static User Login(string login, string password)
        {
            login = login.Trim();
            string phone = NormalizePhone(login);
            string hash = Hash(password);

            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, FirstName, LastName, MiddleName, Username, Email, Phone, 
                       Avatar, Status, Bio, NotificationSettings
                FROM Users
                WHERE (Email=@l OR Phone=@p) AND PasswordHash=@h";
            cmd.Parameters.AddWithValue("@l", login);
            cmd.Parameters.AddWithValue("@p", phone);
            cmd.Parameters.AddWithValue("@h", hash);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new User
            {
                Id = r.GetInt32(0),
                FirstName = r.GetString(1),
                LastName = r.GetString(2),
                MiddleName = r.IsDBNull(3) ? "" : r.GetString(3),
                Username = r.GetString(4),
                Email = r.GetString(5),
                Phone = r.IsDBNull(6) ? "" : r.GetString(6),
                Avatar = r.IsDBNull(7) ? "" : r.GetString(7),
                Status = r.IsDBNull(8) ? UserStatus.Offline : (UserStatus)r.GetInt32(8),
                Bio = r.IsDBNull(9) ? "" : r.GetString(9),
                NotificationSettings = r.IsDBNull(10) ? "{\"sound\":true,\"banner\":true,\"enabled\":true}" : r.GetString(10)
            };
        }

        public static User GetUserByEmailOrPhone(string login)
        {
            login = login.Trim();
            string phone = NormalizePhone(login);

            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, FirstName, LastName, MiddleName, Username, Email, Phone, 
                       Avatar, Status, Bio, NotificationSettings
                FROM Users
                WHERE Email=@l OR Phone=@p";
            cmd.Parameters.AddWithValue("@l", login);
            cmd.Parameters.AddWithValue("@p", phone);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new User
            {
                Id = r.GetInt32(0),
                FirstName = r.GetString(1),
                LastName = r.GetString(2),
                MiddleName = r.IsDBNull(3) ? "" : r.GetString(3),
                Username = r.GetString(4),
                Email = r.GetString(5),
                Phone = r.IsDBNull(6) ? "" : r.GetString(6),
                Avatar = r.IsDBNull(7) ? "" : r.GetString(7),
                Status = r.IsDBNull(8) ? UserStatus.Offline : (UserStatus)r.GetInt32(8),
                Bio = r.IsDBNull(9) ? "" : r.GetString(9),
                NotificationSettings = r.IsDBNull(10) ? "{\"sound\":true,\"banner\":true,\"enabled\":true}" : r.GetString(10)
            };
        }

        public static User GetUserById(int userId)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, FirstName, LastName, MiddleName, Username, Email, Phone, 
                       Avatar, Status, Bio, NotificationSettings 
                FROM Users WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", userId);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new User
            {
                Id = r.GetInt32(0),
                FirstName = r.GetString(1),
                LastName = r.GetString(2),
                MiddleName = r.IsDBNull(3) ? "" : r.GetString(3),
                Username = r.GetString(4),
                Email = r.GetString(5),
                Phone = r.IsDBNull(6) ? "" : r.GetString(6),
                Avatar = r.IsDBNull(7) ? "" : r.GetString(7),
                Status = r.IsDBNull(8) ? UserStatus.Offline : (UserStatus)r.GetInt32(8),
                Bio = r.IsDBNull(9) ? "" : r.GetString(9),
                NotificationSettings = r.IsDBNull(10) ? "{\"sound\":true,\"banner\":true,\"enabled\":true}" : r.GetString(10)
            };
        }

        public static Dialog GetOrCreateDialog(int user1Id, int user2Id)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();

            using (var check = con.CreateCommand())
            {
                check.CommandText = @"
                    SELECT d.Id FROM Dialogs d
                    INNER JOIN DialogMembers dm1 ON d.Id = dm1.DialogId AND dm1.UserId = @u1
                    INNER JOIN DialogMembers dm2 ON d.Id = dm2.DialogId AND dm2.UserId = @u2";
                check.Parameters.AddWithValue("@u1", user1Id);
                check.Parameters.AddWithValue("@u2", user2Id);

                var result = check.ExecuteScalar();
                if (result != null)
                {
                    return new Dialog { Id = Guid.Parse(result.ToString()) };
                }
            }

            var dialog = new Dialog { Id = Guid.NewGuid() };

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Dialogs (Id) VALUES (@id)";
                cmd.Parameters.AddWithValue("@id", dialog.Id.ToString());
                cmd.ExecuteNonQuery();
            }

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO DialogMembers (DialogId, UserId) VALUES (@did, @uid)";
                cmd.Parameters.AddWithValue("@did", dialog.Id.ToString());
                cmd.Parameters.AddWithValue("@uid", user1Id);
                cmd.ExecuteNonQuery();

                cmd.Parameters["@uid"].Value = user2Id;
                cmd.ExecuteNonQuery();
            }

            return dialog;
        }

        public static List<(Guid DialogId, int OtherUserId)> GetDialogsForUser(int userId)
        {
            var result = new List<(Guid, int)>();

            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT dm1.DialogId, dm2.UserId
                FROM DialogMembers dm1
                INNER JOIN DialogMembers dm2 ON dm1.DialogId = dm2.DialogId AND dm2.UserId != @uid
                WHERE dm1.UserId = @uid";
            cmd.Parameters.AddWithValue("@uid", userId);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                result.Add((Guid.Parse(r.GetString(0)), r.GetInt32(1)));
            }

            return result;
        }

        public static void AddMessage(Message msg)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Messages (Id, DialogId, SenderId, Text, Type, IsEdited, CreatedAt, Status, IsDeleted)
                VALUES (@id, @did, @sid, @t, @tp, @e, @ca, @st, @del)";
            cmd.Parameters.AddWithValue("@id", msg.Id.ToString());
            cmd.Parameters.AddWithValue("@did", msg.DialogId.ToString());
            cmd.Parameters.AddWithValue("@sid", msg.SenderId);
            cmd.Parameters.AddWithValue("@t", msg.Text);
            cmd.Parameters.AddWithValue("@tp", (int)msg.Type);
            cmd.Parameters.AddWithValue("@e", msg.IsEdited ? 1 : 0);
            cmd.Parameters.AddWithValue("@ca", msg.CreatedAt.ToString("o"));
            cmd.Parameters.AddWithValue("@st", (int)msg.Status);
            cmd.Parameters.AddWithValue("@del", msg.IsDeleted ? 1 : 0);
            cmd.ExecuteNonQuery();
        }

        public static void AddSystemMessage(Guid dialogId, string text)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                DialogId = dialogId,
                SenderId = 0,
                Text = text,
                Type = MessageType.Text,
                IsEdited = false,
                CreatedAt = DateTime.UtcNow,
                Status = MessageStatus.Read,
                IsDeleted = false
            };
            AddMessage(msg);
        }

        public static List<Message> GetMessages(Guid dialogId)
        {
            var result = new List<Message>();

            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, DialogId, SenderId, Text, Type, IsEdited, CreatedAt, Status, IsDeleted
                FROM Messages
                WHERE DialogId=@did
                ORDER BY CreatedAt";
            cmd.Parameters.AddWithValue("@did", dialogId.ToString());

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                result.Add(new Message
                {
                    Id = Guid.Parse(r.GetString(0)),
                    DialogId = Guid.Parse(r.GetString(1)),
                    SenderId = r.GetInt32(2),
                    Text = r.GetString(3),
                    Type = (MessageType)r.GetInt32(4),
                    IsEdited = r.GetInt32(5) != 0,
                    CreatedAt = DateTime.Parse(r.GetString(6)),
                    Status = (MessageStatus)r.GetInt32(7),
                    IsDeleted = r.GetInt32(8) != 0
                });
            }

            return result;
        }

        public static Message GetMessageById(Guid messageId)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, DialogId, SenderId, Text, Type, IsEdited, CreatedAt, Status, IsDeleted
                FROM Messages
                WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", messageId.ToString());

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Message
            {
                Id = Guid.Parse(r.GetString(0)),
                DialogId = Guid.Parse(r.GetString(1)),
                SenderId = r.GetInt32(2),
                Text = r.GetString(3),
                Type = (MessageType)r.GetInt32(4),
                IsEdited = r.GetInt32(5) != 0,
                CreatedAt = DateTime.Parse(r.GetString(6)),
                Status = (MessageStatus)r.GetInt32(7),
                IsDeleted = r.GetInt32(8) != 0
            };
        }

        public static void UpdateMessage(Message msg)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                UPDATE Messages
                SET Text=@t, IsEdited=@e, Status=@st, IsDeleted=@d
                WHERE Id=@id";
            cmd.Parameters.AddWithValue("@t", msg.Text);
            cmd.Parameters.AddWithValue("@e", msg.IsEdited ? 1 : 0);
            cmd.Parameters.AddWithValue("@st", (int)msg.Status);
            cmd.Parameters.AddWithValue("@d", msg.IsDeleted ? 1 : 0);
            cmd.Parameters.AddWithValue("@id", msg.Id.ToString());
            cmd.ExecuteNonQuery();
        }

        public static void UpdatePassword(int userId, string newPassword)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE Users SET PasswordHash=@h WHERE Id=@id";
            cmd.Parameters.AddWithValue("@h", Hash(newPassword));
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        public static string GetUsername(int userId)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT Username FROM Users WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", userId);
            object val = cmd.ExecuteScalar();
            return val != null ? val.ToString() : "Unknown";
        }

        public static void UpdateUserProfile(int userId, string username, string bio, string avatar)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"
                UPDATE Users 
                SET Username=@u, Bio=@b, Avatar=@a 
                WHERE Id=@id";
            cmd.Parameters.AddWithValue("@u", username ?? "");
            cmd.Parameters.AddWithValue("@b", bio ?? "");
            cmd.Parameters.AddWithValue("@a", avatar ?? "");
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateUserStatus(int userId, UserStatus status)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE Users SET Status=@s WHERE Id=@id";
            cmd.Parameters.AddWithValue("@s", (int)status);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        public static UserStatus GetUserStatus(int userId)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT Status FROM Users WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", userId);
            object result = cmd.ExecuteScalar();
            return result != null ? (UserStatus)Convert.ToInt32(result) : UserStatus.Offline;
        }

        public static void UpdateEmail(int userId, string newEmail, string password)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();

            using var checkCmd = con.CreateCommand();
            checkCmd.CommandText = "SELECT PasswordHash FROM Users WHERE Id=@id";
            checkCmd.Parameters.AddWithValue("@id", userId);
            string storedHash = checkCmd.ExecuteScalar()?.ToString() ?? "";

            if (storedHash != Hash(password))
                throw new Exception("Неверный пароль");

            using var cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE Users SET Email=@e WHERE Id=@id";
            cmd.Parameters.AddWithValue("@e", newEmail);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateNotificationSettings(int userId, string settings)
        {
            using var con = new SQLiteConnection($"Data Source={DbPath}");
            con.Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE Users SET NotificationSettings=@s WHERE Id=@id";
            cmd.Parameters.AddWithValue("@s", settings);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        private static string NormalizePhone(string phone)
        {
            phone = Regex.Replace(phone ?? "", @"\D", "");
            if (phone.Length == 11 && phone.StartsWith("8"))
                phone = "+7" + phone[1..];
            if (phone.Length == 10)
                phone = "+7" + phone;
            if (!phone.StartsWith("+") && !string.IsNullOrEmpty(phone))
                phone = "+" + phone;
            return phone;
        }

        private static string Hash(string s)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(s)));
        }

        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private static bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\+7\d{10}$");
        }
    }
}