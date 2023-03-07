using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Analytics.Events;
using DevToDev.Analytics;
using Near;
using NearClientUnity;
using NearClientUnity.Utilities;

namespace Analytics
{
    [Serializable]
    public class InEditorEvent
    {
        public EventType type;
        
        // Tutorial
        public int tutorialLevelNumber;
        
        // LevelUp
        public int levelNumber;
        
        // LevelUp, CurrentBalance
        public CustomDictionaryElement<string, long>[] balance;
        
        // CurrencyAccrual
        public string currencyName; 
        public int currencyAmount;
        public string source;
        public DTDAccrualType accrualType;

        // VirtualCurrencyPayment
        public string purchaseType;
        public int purchaseAmount;
        public CustomDictionaryElement<string, int>[] resources;
        
        // RealCurrencyPayment
        public string orderId;
        public string currencyCode;
        public double price;
        public string productId;
        
        // CustomEvent
        public string eventName;
        public CustomEventParameter[] customEventParameters;

        public string GetPurchaseId()
        {
            return Guid.NewGuid().ToString();
        }

        public string GetOrderId()
        {
            return Guid.NewGuid().ToString();
        }
        
        public async Task<Dictionary<string, long>> GetBalance()
        {
            Dictionary<string, long> balance = new();
            AccountState accountState = await NearPersistentManager.Instance.GetAccountState();
            balance.Add("NEAR", (long)NearUtils.FormatNearAmount(UInt128.Parse(accountState.Amount)));
            return balance;
        }

        public Dictionary<string, int> GetResources()
        {
            return Utils.ElementsListToDict(resources);
        }

        public DTDCustomEventParameters GetCustomEventParameters()
        {
            return Utils.CustomEventParametersToDTD(customEventParameters);
        }

        public IAnalyticsEventSender GetAnalyticsEventSender()
        {
            return type switch
            {
                EventType.Tutorial => new TutorialEventSender(),
                EventType.LevelUp => new LevelUpEventSender(),
                EventType.CurrentBalance => new CurrentBalanceEventSender(),
                EventType.CurrencyAccrual => new CurrencyAccrualEventSender(),
                EventType.VirtualCurrencyPayment => new VirtualCurrencyPaymentEventSender(),
                EventType.RealCurrencyPayment => new RealCurrencyPaymentEventSender(),
                EventType.CustomEvent => new CustomEventSender(),
                _ => throw new ApplicationException("Unknown event type: " + type)
            };
        }
    }
}