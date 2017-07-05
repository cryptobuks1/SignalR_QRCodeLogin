using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using NLog;

namespace SignalR_QRCodeLogin
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
    }

    public class MessageDetail
    {
        public string UserName { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
    }

    public class ChatHub : Hub
    {
        static Dictionary<string, UserDetail> ConnectedUsers = new Dictionary<string, UserDetail>();
        static List<MessageDetail> CurrentMessage = new List<MessageDetail>();
        static Logger Log = LogManager.GetCurrentClassLogger();

        //登入，連線。
        public void Connect(string userName)
        {
            string id = Context.ConnectionId;
            
            if (!ConnectedUsers.ContainsKey(id))
            {
                ConnectedUsers.Add(id, new UserDetail() { ConnectionId = id, UserName = userName });
                
                // send to caller
                Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected(id, userName);

                Log.Trace("{0}登入系統", userName);
            }
        }

        //聊天內容的「公告」，散佈給所有人。
        public void SendMessageToAll(string message)
        {
            string id = Context.ConnectionId;

            if (ConnectedUsers.ContainsKey(id))
            {
                UserDetail CurrentUser = ConnectedUsers[id];

                // store last 100 messages in cache
                MessageDetail NewMessage = AddMessageinCache(CurrentUser.UserName, message);

                // Broad cast message
                Clients.All.messageReceived(NewMessage);

                Log.Trace("{0}發送訊息{1}", CurrentUser.UserName, message);
            }
        }

        //離線。
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            string id = Context.ConnectionId;

            if (ConnectedUsers.ContainsKey(id))
            {
                UserDetail CurrentUser = ConnectedUsers[id];

                //有人離線，會出現提示。
                Clients.All.onUserDisconnected(id, CurrentUser.UserName);

                ConnectedUsers.Remove(id);

                Log.Trace("{0}離開系統", CurrentUser.UserName);
            }

            return base.OnDisconnected(stopCalled);
        }

        //傳送最近訊息紀錄
        private MessageDetail AddMessageinCache(string userName, string message)
        {
            MessageDetail NewMessage = new MessageDetail { UserName = userName, Time = DateTime.Now, Message = message };
            CurrentMessage.Add(NewMessage);

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);

            return NewMessage;
        }
    }
}