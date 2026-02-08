using ChatServer.Api.Data;
using ChatServer.Api.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatServer.Api.Hubs
{
    public class ChatHub : Hub
    {
        public Task<int> Register(string fn, string ln, string mn, string username, string email, string phone, string password)
        {
            int id = Database.RegisterUser(fn, ln, mn, username, email, phone, password);
            if (id == -1) throw new HubException("Пользователь уже существует");
            if (id == -2) throw new HubException("Заполните все обязательные поля");
            if (id == -3) throw new HubException("Неверный формат email");
            if (id == -4) throw new HubException("Неверный формат телефона");
            return Task.FromResult(id);
        }

        public Task<int> Login(string login, string password)
        {
            var user = Database.Login(login, password);
            if (user == null)
            {
                throw new HubException("Неверный email/телефон или пароль");
            }
            return Task.FromResult(user.Id);
        }

        public Task ResetPassword(string login, string newPassword)
        {
            var user = Database.GetUserByEmailOrPhone(login);
            if (user == null) throw new HubException("Пользователь не найден");
            Database.UpdatePassword(user.Id, newPassword);
            return Task.CompletedTask;
        }

        public async Task<Guid> AddContact(int meId, string login)
        {
            var other = Database.GetUserByEmailOrPhone(login);
            if (other == null) throw new HubException("Пользователь не найден");
            if (other.Id == meId) throw new HubException("Нельзя добавить самого себя");

            var dialog = Database.GetOrCreateDialog(meId, other.Id);
            Database.AddSystemMessage(dialog.Id, Database.GetUsername(meId) + " добавил " + other.Username + " в контакты");

            await Clients.Caller.SendAsync("NewDialog", other.Id, other.Username, dialog.Id);

            return dialog.Id;
        }

        public async Task JoinDialog(Guid dialogId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, dialogId.ToString());
            var messages = Database.GetMessages(dialogId);
            int currentUser = GetCurrentUserId();

            foreach (var msg in messages)
            {
                if (msg.IsDeleted) continue;

                string nick = msg.SenderId == 0 ? "System" : Database.GetUsername(msg.SenderId);
                await Clients.Caller.SendAsync("ReceiveMessage", new
                {
                    msg.Id,
                    msg.DialogId,
                    msg.SenderId,
                    Text = msg.Text ?? "",
                    msg.Type,
                    msg.CreatedAt,
                    msg.IsEdited,
                    msg.IsDeleted,
                    Status = (int)msg.Status,
                    Nickname = nick
                });

                if (msg.SenderId != currentUser && msg.Status == MessageStatus.Sent)
                {
                    msg.Status = MessageStatus.Delivered;
                    Database.UpdateMessage(msg);
                    await Clients.Group(dialogId.ToString()).SendAsync(
                        "MessageStatusUpdated",
                        msg.Id,
                        (int)MessageStatus.Delivered
                    );
                }
            }

            await MarkAsRead(dialogId);
        }

        public async Task SendMessage(Guid dialogId, int senderId, string text, MessageType type)
        {
            var msg = new Message
            {
                Id = Guid.NewGuid(),
                DialogId = dialogId,
                SenderId = senderId,
                Text = text,
                Type = type,
                CreatedAt = DateTime.UtcNow,
                Status = MessageStatus.Sent,
                IsEdited = false,
                IsDeleted = false
            };

            Database.AddMessage(msg);
            string nick = Database.GetUsername(senderId);

            await Clients.Group(dialogId.ToString()).SendAsync("ReceiveMessage", new
            {
                Id = msg.Id.ToString(),
                msg.DialogId,
                msg.SenderId,
                Text = msg.Text ?? "",
                Type = (int)msg.Type,
                CreatedAt = msg.CreatedAt,
                IsEdited = msg.IsEdited,
                IsDeleted = msg.IsDeleted,
                Status = (int)msg.Status,
                Nickname = nick
            });
        }

        public async Task EditMessage(Guid messageId, string newText)
        {
            var msg = Database.GetMessageById(messageId);
            if (msg == null) throw new HubException("Сообщение не найдено");
            if (msg.SenderId != GetCurrentUserId()) throw new HubException("Можно редактировать только свои сообщения");

            msg.Text = newText;
            msg.IsEdited = true;
            Database.UpdateMessage(msg);

            await Clients.Group(msg.DialogId.ToString()).SendAsync("MessageEdited", msg.Id, msg.Text);
        }

        public async Task DeleteMessage(Guid messageId)
        {
            var msg = Database.GetMessageById(messageId);
            if (msg == null) throw new HubException("Сообщение не найдено");
            if (msg.SenderId != GetCurrentUserId()) throw new HubException("Можно удалять только свои сообщения");

            msg.IsDeleted = true;
            Database.UpdateMessage(msg);

            await Clients.Group(msg.DialogId.ToString()).SendAsync("MessageDeleted", msg.Id);
        }

        public async Task GetMyDialogs(int meId)
        {
            var dialogs = Database.GetDialogsForUser(meId);
            foreach (var pair in dialogs)
            {
                Guid dialogId = pair.DialogId;
                int otherId = pair.OtherUserId;
                string otherNick = Database.GetUsername(otherId);

                await Clients.Caller.SendAsync("NewDialog", otherId, otherNick, dialogId);
            }
        }

        public async Task MarkAsRead(Guid dialogId)
        {
            int me = GetCurrentUserId();
            var messages = Database.GetMessages(dialogId);

            foreach (var msg in messages)
            {
                if (msg.SenderId != me && msg.Status != MessageStatus.Read)
                {
                    msg.Status = MessageStatus.Read;
                    Database.UpdateMessage(msg);
                    await Clients.Group(dialogId.ToString()).SendAsync(
                        "MessageStatusUpdated",
                        msg.Id,
                        (int)MessageStatus.Read
                    );
                }
            }
        }

        public async Task UpdateProfile(int userId, string username, string bio, string avatar)
        {
            Database.UpdateUserProfile(userId, username, bio, avatar);
            await Clients.All.SendAsync("UserProfileUpdated", userId, username, avatar, bio);
        }

        public async Task ChangeUserStatus(int userId, int status)
        {
            Database.UpdateUserStatus(userId, (UserStatus)status);
            await Clients.All.SendAsync("UserStatusChanged", userId, status);
        }

        public Task<object> GetUserProfile(int userId)
        {
            var user = Database.GetUserById(userId);
            if (user == null) throw new HubException("Пользователь не найден");

            return Task.FromResult<object>(new
            {
                user.Id,
                user.Username,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Avatar,
                Status = (int)user.Status,
                user.Bio,
                user.NotificationSettings
            });
        }

        public Task<object> GetUserProfileByLogin(string login)
        {
            var user = Database.GetUserByEmailOrPhone(login);
            if (user == null) throw new HubException("Пользователь не найден");

            return Task.FromResult<object>(new
            {
                user.Id,
                user.Username,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Avatar,
                Status = (int)user.Status,
                user.Bio,
                user.NotificationSettings
            });
        }

        public Task ChangeEmail(int userId, string newEmail, string password)
        {
            if (!IsValidEmail(newEmail))
                throw new HubException("Неверный формат email");

            Database.UpdateEmail(userId, newEmail, password);
            return Task.CompletedTask;
        }

        public Task UpdateNotificationSettings(int userId, string settings)
        {
            Database.UpdateNotificationSettings(userId, settings);
            return Task.CompletedTask;
        }

        private static bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private int GetCurrentUserId()
        {
            return int.Parse(Context.GetHttpContext().Request.Query["userId"]);
        }
    }
}