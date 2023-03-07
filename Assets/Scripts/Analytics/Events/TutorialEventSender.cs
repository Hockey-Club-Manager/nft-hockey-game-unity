using System.Threading.Tasks;
using DevToDev.Analytics;

namespace Analytics.Events
{
    public class TutorialEventSender: IAnalyticsEventSender
    {
        public async Task SendToAnalytics(InEditorEvent inEditorEvent)
        {
            DTDAnalytics.Tutorial(inEditorEvent.tutorialLevelNumber);
        }
    }
}