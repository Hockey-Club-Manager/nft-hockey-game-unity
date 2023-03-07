using System.Threading.Tasks;
using DevToDev.Analytics;

namespace Analytics.Events
{
    public class CurrencyAccrualEventSender: IAnalyticsEventSender
    {
        public async Task SendToAnalytics(InEditorEvent inEditorEvent)
        {
            DTDAnalytics.CurrencyAccrual(
                inEditorEvent.currencyName,
                inEditorEvent.currencyAmount,
                inEditorEvent.source,
                inEditorEvent.accrualType);
        }
    }
}