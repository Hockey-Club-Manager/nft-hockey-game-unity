using System.Collections.Generic;
using System.Threading.Tasks;
using DevToDev.Analytics;

namespace Analytics.Events
{
    public class LevelUpEventSender: IAnalyticsEventSender

    {
        public async Task SendToAnalytics(InEditorEvent inEditorEvent)
        {
            DTDAnalytics.LevelUp(inEditorEvent.levelNumber, await inEditorEvent.GetBalance());
        }

        public void SendToAnalytics(int levelNumber, Dictionary<string, long> resources)
        {
            DTDAnalytics.LevelUp(levelNumber, resources);
        }
    }
}
