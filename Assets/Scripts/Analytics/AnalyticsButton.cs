using System.Collections.Generic;
using UI.Scripts;
using UnityEngine.UI;

namespace Analytics
{
    public class AnalyticsButton: UiComponent
    {
        public List<InEditorEvent> events;
        private Button _button;

        protected override void Initialize()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private async void OnClick()
        {
            foreach (var inEditorEvent in events)
            {
                IAnalyticsEventSender analyticsEventSenderSender = inEditorEvent.GetAnalyticsEventSender();
                await analyticsEventSenderSender.SendToAnalytics(inEditorEvent);
            }
        }
    }
}