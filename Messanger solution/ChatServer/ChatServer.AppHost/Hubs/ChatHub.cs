using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ChatServer.AppHost.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<Guid, List<Message>> _dialogs = new();

        public async Task JoinDialog(Guid dialogId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, dialogId.ToString());
        }

        public async Task SendMessage(Guid dialogId, Message message)
        {
            if (!_dialogs.ContainsKey(dialogId))
                _dialogs[dialogId] = new List<Message>();

            _dialogs[dialogId].Add(message);

            await Clients.Group(dialogId.ToString())
                .SendAsync("ReceiveMessage", message);
        }

        public async Task EditMessage(Guid dialogId, Guid messageId, string newText)
        {
            var msg = _dialogs[dialogId].FirstOrDefault(m => m.Id == messageId);
            if (msg == null) return;

            msg.Text = newText;
            msg.IsEdited = true;

            await Clients.Group(dialogId.ToString())
                .SendAsync("MessageEdited", messageId, newText);
        }

        public async Task DeleteMessage(Guid dialogId, Guid messageId)
        {
            var msg = _dialogs[dialogId].FirstOrDefault(m => m.Id == messageId);
            if (msg == null) return;

            _dialogs[dialogId].Remove(msg);

            await Clients.Group(dialogId.ToString())
                .SendAsync("MessageDeleted", messageId);
        }
    }
}
