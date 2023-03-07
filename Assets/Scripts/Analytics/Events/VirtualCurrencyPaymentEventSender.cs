using System.Threading.Tasks;
using DevToDev.Analytics;

namespace Analytics.Events
{
    public class VirtualCurrencyPaymentEventSender: IAnalyticsEventSender
    {
        public async Task SendToAnalytics(InEditorEvent inEditorEvent)
        {
            DTDAnalytics.VirtualCurrencyPayment(inEditorEvent.GetPurchaseId(),
                inEditorEvent.purchaseType,
                inEditorEvent.purchaseAmount,
                inEditorEvent.GetResources());
        }
    }
}