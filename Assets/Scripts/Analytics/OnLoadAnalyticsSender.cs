using System.Collections.Generic;
using UI.Scripts;

namespace Analytics
{
    public class OnLoadAnalyticsSender: UiComponent
    {
        public List<InEditorEvent> events;
        
        protected override void Initialize() { }

        protected async void Start()
        {
            foreach (var inEditorEvent in events)
            {
                IAnalyticsEventSender analyticsEventSenderSender = inEditorEvent.GetAnalyticsEventSender();
                await analyticsEventSenderSender.SendToAnalytics(inEditorEvent);
            }
        }
    }
}