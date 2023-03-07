using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Analytics
{
    [CustomPropertyDrawer(typeof(InEditorEvent))]
    public class InEditorEventPropertyDrawer: PropertyDrawer
    {
        private PropertyField 
            typeProperty,
            tutorialLevelNumberProperty,
            levelNumberProperty,
            currencyNameProperty,
            currencyAmountProperty,
            sourceProperty,
            accrualTypeProperty,
            purchaseIdProperty,
            purchaseTypeProperty,
            purchaseAmountProperty,
            purchaseCurrencyProperty,
            purchasePriceProperty,
            purchaseResourcesProperty,
            orderIdProperty,
            currencyCodeProperty,
            priceProperty,
            productIdProperty,
            eventNameProperty,
            customEventParametersProperty;

        private VisualElement container;
        
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            container = new VisualElement();
            var typeSerializedProperty = property.FindPropertyRelative("type");

            typeProperty = new PropertyField(typeSerializedProperty);
            tutorialLevelNumberProperty = new PropertyField(property.FindPropertyRelative("tutorialLevelNumber"));
            levelNumberProperty = new PropertyField(property.FindPropertyRelative("levelNumber"));
            currencyNameProperty = new PropertyField(property.FindPropertyRelative("currencyName"));
            currencyAmountProperty = new PropertyField(property.FindPropertyRelative("currencyAmount"));
            sourceProperty = new PropertyField(property.FindPropertyRelative("source"));
            accrualTypeProperty = new PropertyField(property.FindPropertyRelative("accrualType"));
            purchaseIdProperty = new PropertyField(property.FindPropertyRelative("purchaseId"));
            purchaseTypeProperty = new PropertyField(property.FindPropertyRelative("purchaseType"));
            purchaseAmountProperty = new PropertyField(property.FindPropertyRelative("purchaseAmount"));
            purchaseCurrencyProperty = new PropertyField(property.FindPropertyRelative("purchaseCurrency"));
            purchasePriceProperty = new PropertyField(property.FindPropertyRelative("purchasePrice"));
            purchaseResourcesProperty = new PropertyField(property.FindPropertyRelative("resources"));
            orderIdProperty = new PropertyField(property.FindPropertyRelative("orderId"));
            currencyCodeProperty = new PropertyField(property.FindPropertyRelative("currencyCode"));
            priceProperty = new PropertyField(property.FindPropertyRelative("price"));
            productIdProperty = new PropertyField(property.FindPropertyRelative("productId"));
            eventNameProperty = new PropertyField(property.FindPropertyRelative("eventName"));
            customEventParametersProperty = new PropertyField(property.FindPropertyRelative("customEventParameters"));
            
            // typeProperty.BindProperty(typeSerializedProperty);
            // typeProperty.RegisterCallback<ChangeEvent<EventType>>(b =>
            // {
            //     Debug.Log("In callback");
            //     OnShowChange(b.newValue);
            // });
            container.Add(typeProperty);

            // EventType typeValue = (EventType)typeSerializedProperty.enumValueIndex;
            OnShowChange((EventType)typeSerializedProperty.enumValueIndex);
            return container;
        }
    
    
        private void OnShowChange(EventType eventType)
        {
            if (container.childCount > 1)
            {
                container.Remove(tutorialLevelNumberProperty);
                container.Remove(levelNumberProperty);
                container.Remove(currencyNameProperty);
                container.Remove(currencyAmountProperty);
                container.Remove(sourceProperty);
                container.Remove(accrualTypeProperty);
                container.Remove(purchaseIdProperty);
                container.Remove(purchaseTypeProperty);
                container.Remove(purchaseAmountProperty);
                container.Remove(purchaseCurrencyProperty);
                container.Remove(purchasePriceProperty);
                container.Remove(purchaseResourcesProperty);
                container.Remove(orderIdProperty);
                container.Remove(currencyCodeProperty);
                container.Remove(priceProperty);
                container.Remove(productIdProperty);
                container.Remove(eventNameProperty);
                container.Remove(customEventParametersProperty);
            }
            switch (eventType)
            {
                case EventType.Tutorial:
                    container.Add(tutorialLevelNumberProperty);
                    break;
                case EventType.LevelUp:
                    container.Add(levelNumberProperty);
                    break;
                case EventType.CurrentBalance:
                    break;
                case EventType.CurrencyAccrual:
                    container.Add(currencyNameProperty);
                    container.Add(currencyAmountProperty);
                    container.Add(sourceProperty);
                    container.Add(accrualTypeProperty);
                    break;
                case EventType.VirtualCurrencyPayment:
                    container.Add(purchaseIdProperty);
                    container.Add(purchaseTypeProperty);
                    container.Add(purchaseAmountProperty);
                    container.Add(purchaseCurrencyProperty);
                    container.Add(purchasePriceProperty);
                    container.Add(purchaseResourcesProperty);
                    break;
                case EventType.RealCurrencyPayment:
                    container.Add(orderIdProperty);
                    container.Add(currencyCodeProperty);
                    container.Add(priceProperty);
                    container.Add(productIdProperty);
                    break;
                case EventType.CustomEvent:
                    container.Add(eventNameProperty);
                    container.Add(customEventParametersProperty);
                    break;
            }
        }
    }
}
