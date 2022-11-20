using Lab4.Generator;
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

        private static ulong _p;
        private static ulong _g;

        private static bool _isChatStarted = false;

        static ChatHub()
        {
            var generator = new GeneralValues();
            _p = generator.P;
            _g = generator.GetG();
        }

        public ChatHub() { }

        public async Task Connect(string name)
        {
            if (!_isChatStarted)
            {
                _connectedUsers.Add(new UserInfo()
                {
                    Id = Context.ConnectionId,
                    Name = name
                });

                await Clients.All.SendAsync("UserConnected", name, _p, _g);
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

        public async Task SendKey(ulong key, int count)
        {
            var usersCount = _connectedUsers.Count;
            var currentUserId = Context.ConnectionId;
            var currentUserIndex = _connectedUsers.FindIndex(x => x.Id.Equals(currentUserId));
            if (count + 1 != usersCount)
            {
                await Clients.Client(_connectedUsers[(currentUserIndex + 1) % usersCount].Id).SendAsync("ReceivePublicKeyPart", key, count);
            }
            else
            {
                await Clients.Client(_connectedUsers[(currentUserIndex + 1) % usersCount].Id).SendAsync("ReceiveLastPublicKeyPart", key);
            }
        }

        public async Task SendMessage(string message)
        {
            var currentUserId = Context.ConnectionId;
            var currentUserIndex = _connectedUsers.FindIndex(x => x.Id.Equals(currentUserId));
            foreach (var user in _connectedUsers)
            {
                string name = _connectedUsers[currentUserIndex].Name;
                await Clients.Client(user.Id).SendAsync("ReceiveMessage", message,name);
            }
        }
    }
}
