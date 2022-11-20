using Microsoft.AspNetCore.SignalR;
using System.Xml.Linq;

namespace Lab4.Server
{
    public class UserInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class ChatHub : Hub
    {
        private static readonly List<UserInfo> _connectedUsers = new List<UserInfo>();

        private static bool _isChatStarted = false;

        public async Task Connect(string name)
        {
            if (!_isChatStarted)
            {
                _connectedUsers.Add(new UserInfo()
                {
                    Id = Context.ConnectionId,
                    Name = name
                });

                await Clients.All.SendAsync("UserConnected", name);
            }
        }

        public async Task StartChat()
        {
            if (!_isChatStarted)
            {
                _isChatStarted = true;
                await Clients.All.SendAsync("ChatStarted");
            }
        }

        public async Task SendKey(ulong key)
        {
            var usersCount = _connectedUsers.Count;
            var currentUserId = Context.ConnectionId;

            var currentUserIndex = _connectedUsers.FindIndex(x => x.Id.Equals(currentUserId));
        }
    }
}
