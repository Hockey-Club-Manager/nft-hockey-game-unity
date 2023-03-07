using System.Collections.Generic;
using System.Threading.Tasks;
using DevToDev.Analytics;
using Near;
using NearClientUnity;
using NearClientUnity.Utilities;
using UnityEngine;

namespace Analytics.Events
{
    public class CurrentBalanceEventSender: IAnalyticsEventSender
    {
        public async Task SendToAnalytics(InEditorEvent inEditorEvent)
        {
            DTDAnalytics.CurrentBalance(await inEditorEvent.GetBalance());
        }
    }
}