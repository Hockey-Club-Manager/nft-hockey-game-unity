using System;
using DevToDev.Analytics;
using UnityEngine;

namespace Analytics
{
    public class AnalyticsSingleton: MonoBehaviour
    {
        public static AnalyticsSingleton Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

            void Start()
            { 
#if UNITY_ANDROID
                DTDAnalytics.Initialize("bbe4dfe6-bed2-0e6f-aa00-97a523aebcee");
#elif UNITY_IOS
                DTDAnalytics.Initialize("bbe4dfe6-bed2-0e6f-aa00-97a523aebcee");
#elif UNITY_WEBGL
                DTDAnalytics.Initialize("bbe4dfe6-bed2-0e6f-aa00-97a523aebcee");
#elif UNITY_STANDALONE_WIN
                DTDAnalytics.Initialize("bbe4dfe6-bed2-0e6f-aa00-97a523aebcee");
#elif UNITY_STANDALONE_OSX
                DTDAnalytics.Initialize("bbe4dfe6-bed2-0e6f-aa00-97a523aebcee");
#elif UNITY_WSA
                DTDAnalytics.Initialize("bbe4dfe6-bed2-0e6f-aa00-97a523aebcee");
#endif
            }
        }
}