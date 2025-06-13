using Microsoft.AspNetCore.SignalR;

namespace MusicSemesterTask.Web.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNewTrackNotification(string artistName, string trackTitle)
        {
            await Clients.All.SendAsync("ReceiveTrackUpdate", artistName, trackTitle);
        }
    }
}