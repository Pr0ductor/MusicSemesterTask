using Microsoft.AspNetCore.SignalR;

namespace MusicSemesterTask.Web.Hubs
{
    public class NotificationHub : Hub
    {
        // Пользователь присоединяется к группе артиста при подписке
        public async Task JoinArtistGroup(string artistName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, artistName);
        }

        // Отправка уведомления подписчикам артиста
        public async Task SendNewTrackNotification(string artistName, string trackTitle)
        {
            await Clients.Group(artistName).SendAsync("ReceiveTrackUpdate", artistName, trackTitle);
        }
    }
}