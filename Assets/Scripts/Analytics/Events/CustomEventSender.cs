using System.Threading.Tasks;
using DevToDev.Analytics;

namespace Analytics.Events
{
    public class CustomEventSender: IAnalyticsEventSender
    {
        public async Task SendToAnalytics(InEditorEvent inEditorEvent)
        {
            DTDAnalytics.CustomEvent(inEditorEvent.eventName,
                inEditorEvent.GetCustomEventParameters());
        }
    }
}