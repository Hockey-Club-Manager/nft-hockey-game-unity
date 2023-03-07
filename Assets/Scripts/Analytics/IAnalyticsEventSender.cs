using System.Threading.Tasks;

namespace Analytics
{
    public interface IAnalyticsEventSender
    {
        Task SendToAnalytics(InEditorEvent inEditorEvent);
    }
}