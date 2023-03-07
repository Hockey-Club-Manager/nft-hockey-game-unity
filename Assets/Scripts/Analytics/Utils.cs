using System.Collections.Generic;
using DevToDev.Analytics;
using Unity.VisualScripting;

namespace Analytics
{
    [System.Serializable]
    public class CustomDictionaryElement<K, V>
    {
        public K key;
        public V value;
    }
    
    [System.Serializable]
    public class StringLongDictionary
    {
        public string key;
        public long value;
    }

    [System.Serializable]
    public class StringIntDictionary
    {
        public string key;
        public int value;
    }

    [System.Serializable]
    public class CustomEventParameter // TODO: write editor for this
    {
        public string key;
        public long integerValue;
        public string stringValue;
        public bool booleanValue;
        public double floatValue;
    }
    
    public class Utils
    {
        public static Dictionary<TK, TV> ElementsListToDict<TK, TV>(CustomDictionaryElement<TK, TV>[] elements)
        {
            var dict = new Dictionary<TK, TV>();
            foreach (var stringLongDictionaryElement in elements)
            {
                dict[stringLongDictionaryElement.key] = stringLongDictionaryElement.value;
            }

            return dict;
        }

        public static DTDCustomEventParameters CustomEventParametersToDTD(CustomEventParameter[] elements)
        {
            var parameters = new DTDCustomEventParameters();
            foreach (var element in elements)
            {
                if (element.integerValue != 0)
                {
                    parameters.Add(element.key, element.integerValue);
                    continue;
                }
                if (!string.IsNullOrEmpty(element.stringValue))
                {
                    parameters.Add(element.key, element.stringValue);
                    continue;
                }

                if (element.booleanValue)
                {
                    parameters.Add(element.key, element.booleanValue);
                    continue;
                }

                if (element.floatValue != 0)
                {
                    parameters.Add(element.key, element.floatValue);
                }
            }

            return parameters;
        }
    }
}