using DevToDev.Analytics;
using UI.Scripts;
using UnityEngine.UI;

namespace Analytics
{
    public class AnalyticsTester: UiComponent
    {
        private Button _button;

        protected override void Initialize()
        {
            _button = GetComponent<Button>();
            InitializeButtonClick();
        }

        private void InitializeButtonClick()
        {
            if (_button)
            {
                _button.onClick.AddListener(SendAnalyticsEvent);
            }
        }

        private void SendAnalyticsEvent()
        {
            var parameters = new DTDCustomEventParameters();
            parameters.Add(key: "key for string value", value: "string value");
            parameters.Add(key: "key for int value", value: 10);
            parameters.Add(key: "key for bool value", value: true);
            parameters.Add(key: "key for double value", value: 12.5);
            DTDAnalytics.CustomEvent(eventName: "Event name", parameters: parameters);
        }
    }
}