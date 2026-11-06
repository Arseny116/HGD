using Microsoft.AspNetCore.SignalR;
using MiroIntegration.API.WebSock;

namespace MiroIntegration.Services;

public sealed class PdfNotifier(IHubContext<NotificationHub> hubContext)
{
    public Task NotifyPdfReadyAsync(
        Guid userId,
        Guid taskId,
        string FileUrl)
    {
        return hubContext
            .Clients // обращаемся ко всем клиаентам 
            .User(userId.ToString()) // обращение к конкретному юзеру из Claims
            .SendAsync("PdfReady", 
            new
            {
                taskId,
                FileUrl,
                status = "Completed"
            });
    }

    public Task NotifyPdfFailedAsync(
        Guid userId,
        Guid taskId,
        string reason)
    {
        return hubContext
            .Clients
            .User(userId.ToString())
            .SendAsync("PdfFailed", new
            {
                taskId,
                reason,
                status = "Failed"
            });
    }
}