using System.Threading.Tasks;
using DevToDev.Analytics;

namespace Analytics.Events
{
    public class RealCurrencyPaymentEventSender: IAnalyticsEventSender
    {
        public async Task SendToAnalytics(InEditorEvent inEditorEvent)
        {
            DTDAnalytics.RealCurrencyPayment(
                inEditorEvent.orderId,
                inEditorEvent.price,
                inEditorEvent.productId,
                inEditorEvent.currencyCode);
        }
    }
}